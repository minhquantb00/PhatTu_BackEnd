using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.Requests.BaiVietRequest;
using QuanLyPhatTu_API.Payloads.Responses;
using QuanLyPhatTu_API.Service.Implements;
using QuanLyPhatTu_API.Service.Interfaces;

namespace QuanLyPhatTu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaiVietController : ControllerBase
    {
        private readonly IBaiVietService _iBaiVietService;
        public BaiVietController()
        {
            _iBaiVietService = new BaiVietService();
        }
        [HttpPost("TaoBaiViet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> TaoBaiViet([FromForm] IFormFile file, [FromForm] string data)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            var taoBaiViet =  JsonConvert.DeserializeObject<Request_TaoBaiViet>(data);
            return Ok(await _iBaiVietService.TaoBaiViet(id, file, taoBaiViet));
        }

        [HttpPut("DuyetBaiViet/{baiVietId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> DuyetBaiViet([FromRoute] int baiVietId)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iBaiVietService.DuyetBaiViet(id, baiVietId));
        }

        [HttpPut("SuaBaiViet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SuaBaiViet(int baiVietId, Request_SuaBaiViet request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value!);
            return Ok(await _iBaiVietService.SuaBaiViet(baiVietId, id,  request));
        }

        [HttpDelete("XoaBaiViet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> XoaBaiViet(int baiVietId)
        {
            return Ok(await _iBaiVietService.XoaBaiViet(baiVietId));
        }

        [HttpPut("LayTatCaBaiViet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LayTatCaBaiViet(int pageSize, int pageNumber)
        {
            return Ok(await _iBaiVietService.LayTatCaBaiViet(pageSize, pageNumber));
        }

        [HttpPut("LayBaiVietTheoNguoiDung/{nguoiDungId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayBaiVietTheoNguoiDung([FromRoute] int nguoiDungId, int pageSize, int pageNumber)
        {
            return Ok(await _iBaiVietService.LayBaiVietTheoNguoiDung(nguoiDungId, pageSize, pageNumber));
        }

        [HttpPut("LayBaiVietTheoLoaiBaiViet/{tenLoai}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LayBaiVietTheoLoaiBaiViet([FromRoute]string tenLoai, int pageSize, int pageNumber)
        {
            return Ok(await _iBaiVietService.LayBaiVietTheoLoaiBaiViet(tenLoai, pageSize, pageNumber));
        }
        [HttpGet("LayBaiVietTheoId/{baiVietId}")]
        public async Task<IActionResult> LayBaiVietTheoId(int baiVietId)
        {
            return Ok(await _iBaiVietService.LayBaiVietTheoId(baiVietId));
        }
    }
}
