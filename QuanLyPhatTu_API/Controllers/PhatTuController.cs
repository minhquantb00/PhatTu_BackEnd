﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.Requests.PhatTuRequest;
using QuanLyPhatTu_API.Payloads.Responses;
using QuanLyPhatTu_API.Service.Implements;
using QuanLyPhatTu_API.Service.Interfaces;

namespace QuanLyPhatTu_API.Controllers
{
    [ApiController]
    public class PhatTuController : ControllerBase
    {
        private readonly IPhatTuService _iPhatTuService;
        public PhatTuController( IPhatTuService phatTuService)
        {
            _iPhatTuService = phatTuService;
        }
        [HttpGet("/api/phattu/get-all")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayTatCaPhatTu(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayTatCaPhatTu(pageSize, pageNumber));
        }

        [HttpGet("/api/phatu/LayPhatTuTheoChua/{chuaId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoChua(int? chuaId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoChua(chuaId, pageSize, pageNumber));
        }
        [HttpGet("/api/phattu/LayPhatTuTheoEmail")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoEmail(string email)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoEmail(email));
        }
        [HttpGet("/api/phattu/LayPhatTuTheoGioiTinh")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoGioiTinh(string? gioiTinh, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoGioiTinh(gioiTinh, pageSize, pageNumber));
        }
        [HttpGet("/api/phattu/LayPhatTuTheoPhapDanh")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoPhapDanh(string? phapDanh, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoPhapDanh(phapDanh, pageSize, pageNumber));
        }
        [HttpGet("/api/phattu/LayPhatTuTheoTen")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoTen(string? name, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoTen(name, pageSize, pageNumber));
        }
        [HttpGet("/api/phattu/LayPhatTuTheoTrangThai")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayPhatTuTheoTrangThai(bool? status, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iPhatTuService.LayPhatTuTheoTrangThai(status, pageSize, pageNumber));
        }
        [HttpPut("/api/phattu/SuaThongTinPhatTu")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SuaThongTinPhatTu(Request_CapNhatThongTinPhatTu request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!int.TryParse(HttpContext.User.FindFirst("Id")?.Value, out int id))
                {
                    return BadRequest("Id người dùng không hợp lệ");
                }

                var result = await _iPhatTuService.SuaThongTinPhatTu(id, request);

                if (result.Message.ToLower().Contains("Cập nhật thông tin phật tử thành công".ToLower()))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("/api/phattu/XoaPhatTu/{phatTuId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> XoaPhatTu(int phatTuId)
        {
            return Ok(await _iPhatTuService.XoaPhatTu(phatTuId));
        }
        [HttpPut("/api/phattu/ThemPhatTuVaoChua")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ThemPhatTuVaoChua(Request_ThemPhatTuVaoChua request)
        {
            return Ok(await _iPhatTuService.ThemPhatTuVaoChua(request));
        }
    }
}
