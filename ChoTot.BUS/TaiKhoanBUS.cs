using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoTot.DAL;
using ChoTot.MOD;
using Microsoft.Extensions.Primitives;

namespace ChoTot.BUS
{
    public class TaiKhoanBUS
    {
        public BaseResultMOD Dangnhap(TaiKhoanMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (string.IsNullOrEmpty(item.PhoneNumber))
                {
                    Result.Status = 0;
                    Result.Message = "số điện thoại không được để trống";
                    return Result;
                }
                else if (item.PhoneNumber == "" || item.Password == null)
                {
                    Result.Status = 0;
                    Result.Message = "mật khẩu không được để trống";
                    return Result;
                }
                else
                {
                    var UserLogin = new TaiKhoanDAL().LoginDAL(item);
                    if (UserLogin == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Tài khoản hoặc mật khẩu không đúng";
                    }
                    else if (UserLogin != null)
                    {
                        Result.Status = 1;
                        Result.Message = "Đăng nhập thành công";
                        Result.Data = UserLogin;
                        if (UserLogin != null)
                        {
                            Result.Status = 1;
                            Result.Message = "Đăng nhập thành công";
                            Result.Data = UserLogin;
                            if (UserLogin.role == 1)
                            {
                                Result.Data1 = new TaiKhoanDAL().function(UserLogin.role);
                            }
                            else if (UserLogin.role == 2)
                            {
                                Result.Data1 = new TaiKhoanDAL().function(UserLogin.role);
                            }
                            else if (UserLogin.role == 3)
                            {

                                Result.Data1 = new TaiKhoanDAL().function(UserLogin.role);
                            }
                        }
                    }
                    return Result;
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "Lỗi hệ thống";
                throw;
            }

            return Result;
        }
        public BaseResultMOD DangKytaikhoan(Dangkytaikhoan item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.Name == null || item.Name == "")
                {
                    Result.Status = 0;
                    Result.Message = "Họ tên không được để trống";
                }
                else if (item == null || item.PhoneNumber == null || item.PhoneNumber == "")
                {
                    Result.Status = 0;
                    Result.Message = "Số điện thoại không được để trống";
                }
                else if (item == null || item.Password == null || item.Password == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mật khẩu không được để trống";
                }
                var checktaikhoan = new TaiKhoanDAL().inforTK(item.PhoneNumber);
                if (checktaikhoan != null)
                {
                    Result.Data = 0;
                    Result.Message = "người dùng đã tồn tại";
                }
                else { return new TaiKhoanDAL().RegisterDAL(item); }

            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "điền đủ mọi thông tin";
                throw;
            }
            return Result;
        }
        public BaseResultMOD DanhSachTaiKhoan(int page)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (page > 0) { Result = new TaiKhoanDAL().DanhSachTaiKhoan(page); }
                else
                {
                    Result.Status = 0;
                    Result.Message = "lỗi page";
                }

            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "lỗi hệ thống";
                Result.Data = null;
                throw;
            }
            return Result;
        }
        public BaseResultMOD ChangePass(Changepassword item)
        {
            var Result = new BaseResultMOD();
            if (item.phonenumber == null || item.phonenumber == "")
            {
                Result.Status = 0;
                Result.Message = "Mật khẩu không được để trống";
            }
            else if (item.password == null || item.password == "")
            {
                Result.Status = 0;
                Result.Message = "Mật khẩu không được để trống";
            }
            else if (item.Repassword == null || item.Repassword == "")
            {
                Result.Status = 0;
                Result.Message = "Mật khẩu không được để trống";
            }
            else
            {
                var check = new TaiKhoanDAL().inforTK(item.phonenumber);
                if (check != null && item.password == item.Repassword)
                {

                    return Result = new TaiKhoanDAL().ChangePass(item);
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Số điện thoại bị sai";
                }
            }
            return Result;
        }
    }
}
