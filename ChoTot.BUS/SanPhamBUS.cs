using ChoTot.DAL;
using ChoTot.MOD;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.BUS
{
    public class SanPhamBUS
    {
        public BaseResultMOD ThemSP(SanPham item, IFormFile file)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.MSanPham == null || item.MSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";
                }
                if (item == null || file == null)
                {
                    Result.Status = 0;
                    Result.Message = "ảnh không được để trống";
                }
                if (item == null || item.TenSanPham == null || item.TenSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên sản phẩm không được để trống";
                }
                if (item == null || item.LoaiSanPham == null || item.LoaiSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không được để trống";
                }
                if (item == null || item.SoLuong < 0)
                {
                    Result.Status = 0;
                    Result.Message = "Số lượng sản phẩm không được để trống";
                }
                if (item == null || item.DonGia <= 0)
                {
                    Result.Status = 0;
                    Result.Message = "Đơn Giá sản phẩm không được để trống";
                }
                else
                {
                    var checksp = new SanPhamDAL().inforSanPham(item.MSanPham);
                    if (checksp != null)
                    {
                        Result.Status = -1;
                        Result.Message = "mã sản phẩm đã tồn tại";
                    }
                    else
                    {
                        Result.Status = 1;
                        Result.Message = "thêm sản phẩm thành công";
                        return new SanPhamDAL().ThemSP(item, file);
                    }
                }               
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "vui lòng điền đủ thông tin";
                throw;
            }
            return Result;
        }
        public BaseResultMOD SuaSp(SanPham item, IFormFile file)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || file == null)
                {
                    Result.Status = 0;
                    Result.Message = "ảnh không được để trống";
                }
                if (item == null || item.TenSanPham == null || item.TenSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên sản phẩm không được để trống";
                }
                if (item == null || item.LoaiSanPham == null || item.LoaiSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không được để trống";
                }
                if (item == null || item.SoLuong < 0)
                {
                    Result.Status = 0;
                    Result.Message = "Số lượng sản phẩm không được để trống";
                }
                if (item == null || item.DonGia <= 0)
                {
                    Result.Status = 0;
                    Result.Message = "Đơn Giá sản phẩm không được để trống";
                }
                else
                {
                    var sua = new SanPhamDAL().inforSanPham(item.MSanPham);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã sản phẩm không đúng";
                        return Result;
                    }
                    else
                    {
                        Result.Status = 1;
                        Result.Message = "xóa thành công";
                        return new SanPhamDAL().SuaSP(item, file);
                    }
                }
                var checksanpham = new SanPhamDAL().inforSanPham(item.MSanPham);
                if (checksanpham == null)
                {
                    Result.Status = 0;
                    Result.Message = "vui lòng kiểm tra mã sản phẩm";
                }
                else { return new SanPhamDAL().SuaSP(item, file); }

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "vui lòng điền đủ thông tin";

                throw;
            }
            return Result;
        }
        public BaseResultMOD XoaSP(string Masanpham)
        {
            var Result = new BaseResultMOD();
            if (Masanpham == null || Masanpham == "")
            {
                Result.Status = 0;
                Result.Message = "vui lòng nhập mã sản phẩm";
                return Result;
            }
            else
            {
                var chitietsanpham = new SanPhamDAL().inforSanPham(Masanpham);
                if (chitietsanpham == null)
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không tồn tại";
                    return Result;
                }
                else
                {
                    return new SanPhamDAL().XoaSP(Masanpham);
                }
            }
            return Result;
        }
        public BaseResultMOD DanhSachSP(PasePagingParams p)
        {
            var Result = new BaseResultMOD();
                try
                {
                if (p == null)
                {
                    Result.Status = 0;
                    Result.Message = "vui lòng nhập số trang";
                }
                else {
                   
                    Result = new SanPhamDAL().GetDanhSachSanPham(p);
                    Result.Status = 1;
                    Result.Message = "lấy danh sách thành công";
                }   
                   
                }
                catch (Exception ex)
                {
                    Result.Status = -1;
                    Result.Data = null;
                    Result.Message = "Lỗi khi lấy danh sách sản phẩm: " + ex.Message;
                }
                return Result;
            
        }
        public BaseResultMOD timkiembyname(string name)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (name == null || name == "")
                {
                    Result.Status = 0;
                    Result.Message = "vui lòng nhập tên sản phẩm";                    
                }
                else
                {
                    var checksp= new SanPhamDAL().SearchByName(name);
                    if(checksp == null)
                    {
                        Result.Status= 0;
                        Result.Message = "Không tìm thấy sản phẩm";
                    }
                    else
                    {
                         Result = checksp;
                        Result.Status = 1;
                        Result.Message = "lấy danh sách thành công";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }
        public BaseResultMOD phanloaisp(string loaisp,int page)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (loaisp == null || loaisp == "")
                {
                    Result.Status = 0;
                    Result.Message = "vui lòng chọn loại sản phẩm";
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = "lấy danh sách thành công";
                    Result = new SanPhamDAL().DanhSachSPbytypeSP(loaisp,page);
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = "Lỗi khi lấy danh sách sản phẩm: " + ex.Message;
                throw;
            }
            return Result;
        }

    }
}
