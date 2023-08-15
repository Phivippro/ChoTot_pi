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

namespace ChoTot.DAL
{
    public class TaiKhoanDAL
    {
        private string strCon = "Data Source=localhost;Initial Catalog=choto;Persist Security Info=True;User ID=dev_user;Password=123456;Trusted_Connection=True";


        public TaiKhoanModel LoginDAL(TaiKhoanMOD item)
        {
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT username, phonenumber, password,role FROM TaiKhoan WHERE phonenumber = @PhoneNumber";
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string hashedPasswordFromDB = reader.GetString(2);
                        if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPasswordFromDB))
                        {
                            var Result = new TaiKhoanModel();
                            Result.username = reader.GetString(0);
                            Result.PhoneNumber = reader.GetString(1);
                            Result.Email= reader.GetString(2);
                            Result.role= reader.GetInt32(3);
                            return Result;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle specific exceptions here or log the error appropriately.
                throw;
            }
            return null;
        }

        public BaseResultMOD RegisterDAL(Dangkytaikhoan item)
        {
            var Result = new BaseResultMOD();
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(item.Password, salt);
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into [TaiKhoan] (UserName,PhoneNumber,Email,Password) values("+ "'" + item.Name + "'"+ ",'" + item.PhoneNumber + "'"+",'" +item.Email + "'" +",'" + hash + "'"+")";
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
                    cmd.CommandType = CommandType.Text;
                    // câu lệnh query thực thi
                    cmd.CommandText="select * from TaiKhoan where phonenumber= '" + Phonenumber + "'";
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
                List<DSChucNang> danhsachchucnang= new List<DSChucNang>();
                using(SqlConnection SQLCon= new SqlConnection(strCon))
                {
                    SQLCon.Open() ; 
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select MChucNang ,TenChucNang,quyen from phanquyen where quyen = @quyen";
                    cmd.Parameters.AddWithValue("@quyen", quyen);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DSChucNang item = new DSChucNang();
                        item.TenChucNang= reader.GetString(1);                       
                        danhsachchucnang.Add(item);
                    }
                    reader.Close();
                }
                result.Status = 1;
                result.Data = danhsachchucnang;
            }
            catch (Exception)
            {

                throw;
            }

            return result;
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
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select username, phonenumber, email from TaiKhoan order by id offset "+startPage+" rows fetch next "+ProductPerPage+" rows only ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    { 
                        TaiKhoanModel model = new TaiKhoanModel();
                        model.username = reader.GetString(0);
                        model.PhoneNumber = reader.GetString(1);
                        model.Email = reader.GetString(2);
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
    }
}

