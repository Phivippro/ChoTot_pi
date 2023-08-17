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
        public IActionResult Login([FromBody] TaiKhoanMOD login)
        {
            if (login == null) return BadRequest();
            var Result = new TaiKhoanBUS().Dangnhap(login);
            if (Result != null) return Ok(Result);
            else return BadRequest();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] Dangkytaikhoan item)
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
        [HttpPut]
        [Route("ChangePass")]
        public IActionResult ChangePass([FromBody] Changepassword item)
        {
            if (item == null) return BadRequest();
            var result = new TaiKhoanBUS().ChangePass(item);
            if (result != null) return Ok(result);
            else return NotFound();
        }
    }

}
