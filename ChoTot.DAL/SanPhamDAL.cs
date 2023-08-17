using ChoTot.MOD;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;

namespace ChoTot.DAL
{
    public class SanPhamDAL
    {
        string strCon = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["ConectionString:choto"];

        SqlConnection SQLCon = null;
        public BaseResultMOD ThemSP(SanPham item, IFormFile file)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                SqlCommand sqlCmd = new SqlCommand();
                if (file.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","upload", file.FileName);

                    using (Stream stream = System.IO.File.Create(path))
                    {
                        file.CopyTo(stream);
                    }
                     Picture = "/upload/" + file.FileName;
                }
                else
                {
                    Picture = "";
                }
               SqlCommand cmd = new SqlCommand();   
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SanPham_Them";
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                cmd.Parameters.AddWithValue("@Picture", Picture);
                cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham);
                cmd.Parameters.AddWithValue("@LoaiSanPham", item.LoaiSanPham);
                cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", item.DonGia);
                SQLCon.Open();
                cmd.ExecuteNonQuery();

                if (SQLCon!= null)
                {
                    Result.Status = 1;
                    Result.Message = "Thêm sản phẩm thành công";
                    Result.Data = 1;
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Thêm sản phẩm thất bại";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Result;
        }
        public BaseResultMOD SuaSP(SanPham editSanPham, IFormFile file)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "upload", file.FileName);

                        using (Stream stream = System.IO.File.Create(path))
                        {
                            file.CopyTo(stream);
                        }
                        Picture = "/upload/" + file.FileName;
                    }
                    else
                    {
                       Picture = "";
                    }
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "SanPham_Update";
                    sqlCmd.Connection = SQLCon;

                    // Use parameters to avoid SQL injection
                    sqlCmd.Parameters.AddWithValue("@Picture", Picture);
                    sqlCmd.Parameters.AddWithValue("@TenSanPham", editSanPham.TenSanPham);
                    sqlCmd.Parameters.AddWithValue("@LoaiSanPham", editSanPham.LoaiSanPham);
                    sqlCmd.Parameters.AddWithValue("@SoLuong", editSanPham.SoLuong);
                    sqlCmd.Parameters.AddWithValue("@DonGia", editSanPham.DonGia);
                    sqlCmd.Parameters.AddWithValue("@MSanPham", editSanPham.MSanPham);
                   sqlCmd.ExecuteNonQuery();
                    if (SQLCon !=null)
                    {
                        Result.Status = 1;
                        Result.Message = "Chỉnh sửa thông tin thành công";
                        Result.Data = 1;
                    }
                    else
                    {
                        Result.Status = -1;
                        Result.Message = "Sản phẩm không tồn tại";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
        public BaseResultMOD XoaSP(string msanpham)
        {
            var Result = new BaseResultMOD();
            try
                {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText= "SanPham_DELETE";
                cmd.Parameters.AddWithValue("@MSanPham", msanpham);
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "xóa dữ liệu thành công";
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "vui lòng kiểm tra lại mã sản phẩm";
                }

                }
                catch (Exception)
                {

                    throw;
                }
               
                return Result;
        }
        public SanPham inforSanPham(string msanpham)
        {
            SanPham item = null;
            try
            {
                if(SQLCon==null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SanPham_ChiTiet";
                cmd.Parameters.AddWithValue("@MSanPham", msanpham);
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new SanPham();
                    item.TenSanPham = reader.GetString(0);               
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }
        public BaseResultMOD GetDanhSachSanPham(PasePagingParams p)
        {
            var Result = new BaseResultMOD();
            List<danhsachSP> productList = new List<danhsachSP>();
            try
            {
               
                using (SqlConnection SQLCon= new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SanPham_DanhSach";
                    cmd.Parameters.AddWithValue("@Keyword", ("'%").ToString() + p.Keyword + ("%'").ToString());
                    cmd.Parameters.AddWithValue("@pagesize", p.PageSize);
                    cmd.Parameters.AddWithValue("@offset", p.Offset);
                    cmd.Parameters.AddWithValue("@limit", p.Limit);
                    cmd.Parameters.AddWithValue("@orderbyname", p.OrderByName);
                    cmd.Parameters.AddWithValue("@orderbyoption", p.OrderByOption);
                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        danhsachSP item = new danhsachSP();
                        item.MSanPham=reader.GetString(0);
                        item.Picture = "localhost:9000/" + reader.GetString(1);
                        item.TenSanPham = reader.GetString(2);
                        item.LoaiSanPham = reader.GetString(3);
                        item.SoLuong = reader.GetInt32(4);
                        item.DonGia =(float)reader.GetDecimal(5);
                        productList.Add(item);
                    }
                    reader.Close();
                }
                Result.Status = 1;
                Result.Data = productList;
            }
            catch (Exception)
            {
                throw;
            }

            return Result;
        }
        public BaseResultMOD SearchByName(string name)
        {
           List<danhsachSP>  danhsachSPsearchbyname = new List<danhsachSP> ();
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strCon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SanPham_Search_by_Name";
                    cmd.Parameters.AddWithValue("@TenSanPham", name);
                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        danhsachSP item = new danhsachSP();
                        item.MSanPham = reader.GetString(0);
                        item.Picture = reader.GetString(1);
                        item.TenSanPham = reader.GetString(2);
                        item.LoaiSanPham = reader.GetString(3);
                        item.SoLuong = reader.GetInt32(4);
                        item.DonGia = (float)reader.GetDecimal(5);
                        danhsachSPsearchbyname.Add(item);
                    }
                    reader.Close();
                }
                Result.Status = 1;
                Result.Data = danhsachSPsearchbyname;
            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }
        
        public BaseResultMOD DanhSachSPbytypeSP(string loaisp,int Page)
        {
            var Result = new BaseResultMOD();
            try
            {
                const int ProductPerPage = 20;
                int startPage = ProductPerPage * (Page - 1);
                List<danhsachSP> DSloaisp = new List<danhsachSP>();
                using(SqlConnection SQLCon = new SqlConnection(strCon)) 
                { 
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand(); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SanPham_DanhSachLoaiSP";
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Parameters.AddWithValue("@startPage", startPage);
                    cmd.Parameters.AddWithValue("@LoaiSanPham", loaisp);
                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        danhsachSP item = new danhsachSP();
                        item.MSanPham = reader.GetString(0);
                        item.Picture = reader.GetString(1);
                        item.TenSanPham = reader.GetString(2);
                        item.LoaiSanPham = reader.GetString(3);
                        item.SoLuong = reader.GetInt32(4);
                        item.DonGia = (float)reader.GetDecimal(5);
                        DSloaisp.Add(item);
                    }
                    reader.Close();
                }
                Result.Status = -1;
                Result.Data = DSloaisp;
            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }
    }

}