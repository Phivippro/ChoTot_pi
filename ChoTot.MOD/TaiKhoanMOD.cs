using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.MOD
{
    public class TaiKhoanMOD
    {
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
    public class Dangkytaikhoan
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        protected int role { get; set; }

    }
    public class TaiKhoanModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string username { get; set; }
        public int role { get; set; }
    }
    public class DSChucNang 
    {
        public int MChucNang { get; set; } 
        public string TenChucNang { get; set; }
        public int quyen { get; set; }
    }
    public class Changepassword {
        public string phonenumber { get; set; }
        public string password { get; set;}
        public string Repassword { get; set; }

    }
}
