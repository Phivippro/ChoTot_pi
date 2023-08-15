using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.MOD
{
    public class SanPham
    {

        public string MSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string LoaiSanPham { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
        public IFormFile file;
    }
    public class danhsachSP
    {
        public string MSanPham { get; set; }
        public string Picture { get; set; }
        public string TenSanPham { get; set; }
        public string LoaiSanPham { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
    }
    public class searchSP
    {
        public string MSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string Picture { get; set; }
        public string LoaiSanPham { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
    }

}

