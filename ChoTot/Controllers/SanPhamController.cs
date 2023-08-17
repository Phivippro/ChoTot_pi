using ChoTot.BUS;
using ChoTot.MOD;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ChoTot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : Controller
    {
        [HttpPost]
        [Route("ThemSanPham")]
        public IActionResult ThemSP(IFormFile file, [FromForm] SanPham item)
        {
            if (item == null) return BadRequest();
            if (file.Length > 0)
            {
                var Result = new SanPhamBUS().ThemSP(item, file);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPut]
        [Route("SuaSanPham")]
        public IActionResult SuaSP(IFormFile file,[FromForm] SanPham item)
        {

            if (item == null) return BadRequest();
            if (file.Length > 0)
            {
                var Result = new SanPhamBUS().SuaSp(item, file);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [Route("XoaSanPham")]
        public IActionResult XoaSP(string msanpham)
        {
            if (msanpham == null || msanpham == "") return BadRequest();
            var Result = new SanPhamBUS().XoaSP(msanpham);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPost]
        [Route("DanhSachSanPham")]
        public IActionResult DanhSachSP (PasePagingParams p)
        {
            if (p == null)
            {
                return BadRequest();
            }
            else
            {
                var Result = new SanPhamBUS().DanhSachSP(p);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost]
        [Route("SearchSanPham")]
        public IActionResult SearchSP(string name)
        {
            if (name == null) return BadRequest();           
            var Result = new SanPhamBUS().timkiembyname(name);
            if (Result != null) return Ok(Result);
            else return NotFound();            
        }
        [HttpPost]
        [Route("PhanLoaiSanPham")]
        public IActionResult PhanLoaiSP(string loaiSP,int page)
        {
            if (page<1)
            {
                return BadRequest();
            }
            else
            {
                if (loaiSP == null) return BadRequest();
                var Result = new SanPhamBUS().phanloaisp(loaiSP,page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            
        }
    }
}
