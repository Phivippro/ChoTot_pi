using ChoTot.BUS;
using ChoTot.MOD;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChoTot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromForm] TaiKhoanMOD login)
        {
            if (login == null) return BadRequest();
            var Result = new TaiKhoanBUS().Dangnhap(login);
            if (Result != null) return Ok(Result);
            else return BadRequest();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromForm] Dangkytaikhoan item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DangKytaikhoan(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpGet]
        [Route("DanhSachTK")]
        public IActionResult DanhSachTK(int page) 
        {
            if (page < 1)
            {
                return BadRequest();
            }
            else
            {
                var TotalRow = 0;
                var Result = new TaiKhoanBUS().DanhSachTaiKhoan(page);
                Result.TotalRow = TotalRow;
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

    }

}
