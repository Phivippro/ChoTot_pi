using ChoTot.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Threading.Channels;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace ChoTot.DAL
{
    public class TaiKhoanDAL
    {
        string strCon = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["ConectionString:choto"];
        public TaiKhoanModel LoginDAL(TaiKhoanMOD item)
        {
            var result = new TaiKhoanModel();

            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "TaiKhoan_Login"; // Thay thế bằng tên thủ tục của bạn
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string hashedPasswordFromDB = reader.GetString(2);
                        if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPasswordFromDB))
                        {
                            result.Email = reader.GetString(1); // Thay thế bằng tên cột Email trong kết quả trả về
                            result.username = reader.GetString(0); // Thay thế bằng tên cột Username trong kết quả trả về
                            result.role = reader.GetInt32(4); // Thay thế bằng tên cột Role trong kết quả trả về
                            result.PhoneNumber =reader.GetString(3);
                            return result;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ cụ thể ở đây hoặc ghi log lỗi một cách thích hợp.
                throw;
            }
            return null;
        }

        public BaseResultMOD RegisterDAL(Dangkytaikhoan item)
        {
            var Result = new BaseResultMOD();
            string hash = BCrypt.Net.BCrypt.HashPassword(item.Password);
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "TaiKhoan_Insert";
                    cmd.Parameters.AddWithValue("@UserName", item.Name);
                    cmd.Parameters.AddWithValue("@Email", item.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Password", hash);
                    cmd.Parameters.AddWithValue("@role", 1);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                }
                Result.Status = 1;
                Result.Message = "Đăng ký thành công";

            }
            catch (Exception ex)
            {
                // Handle specific exceptions here or log the error appropriately.
                throw;
            }
            return Result;
        }
        public Dangkytaikhoan inforTK(String Phonenumber)
        {
            Dangkytaikhoan item = null;
            try
            {
                // tạo query kết nối
                using(SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open() ;
                    // tạo commad
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    // câu lệnh query thực thi
                    cmd.CommandText="TaiKhoan_Check";
                    cmd.Parameters.AddWithValue("@PhoneNumber", Phonenumber);
                    //thực thi
                    cmd.Connection = SQLCon;
                    //lấy dữ liệu và trả về kết quả
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        item = new Dangkytaikhoan();
                        item.PhoneNumber = reader.GetString(1);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }
        public BaseResultMOD function(int quyen)
        {
            
            var result = new BaseResultMOD();   
            try
            {
                List<DSChucNang> danhsachchucnang = new List<DSChucNang>();
                using(SqlConnection SQLCon= new SqlConnection(strCon))
                {
                    SQLCon.Open() ; 
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "TaiKhoan_PhanQuyen";
                    cmd.Parameters.AddWithValue("@quyen", quyen);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DSChucNang item =  new DSChucNang();
                        item.TenChucNang= reader.GetString(0);                       
                        danhsachchucnang.Add(item);
                    }
                    reader.Close();
                }
                result.Status = 1;
                result.Data1 = danhsachchucnang;
            }
            catch (Exception)
            {
                throw;
            }

            return result ;
        }
        public BaseResultMOD DanhSachTaiKhoan(int Page)
        {
            var result = new BaseResultMOD();
            try
            {
                List<TaiKhoanModel> ListAccount = new List<TaiKhoanModel>();
                using(SqlConnection SQLCon = new SqlConnection(strCon) ) 
                {
                    const int ProductPerPage = 20;
                    int startPage = ProductPerPage * (Page - 1);
                    SQLCon.Open();                
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "TaiKhoan_Select";
                    cmd.Parameters.AddWithValue("@startPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    { 
                        TaiKhoanModel model = new TaiKhoanModel();
                        model.username = reader.GetString(0);
                        model.PhoneNumber = reader.GetString(1);
                        model.Email = reader.GetString(2);
                        model.role=reader.GetInt32(3);
                        ListAccount.Add(model);
                    }
                    reader.Close();
                }
                result.Status = 1;
                result.Data = ListAccount;

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public BaseResultMOD ChangePass(Changepassword item)
        {
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(item.password, salt);
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "TaiKhoan_ChangPass";
                    cmd.Parameters.AddWithValue("@phonenumber", item.phonenumber);
                    cmd.Parameters.AddWithValue("@Password", salt);
                    cmd.Connection =SQLCon;
                    cmd.ExecuteNonQuery();
                }
                Result.Status = 1;
                Result.Message = "đổi mật khẩu thành công";
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "đổi mật khẩu thất bại";
                throw;
            }
            return Result;
            
        }
       

    }
}

