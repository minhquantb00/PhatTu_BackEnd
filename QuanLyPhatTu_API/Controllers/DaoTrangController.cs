using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.DTOs.ThongKeDaoTrang;
using QuanLyPhatTu_API.Payloads.Requests.DaoTrangRequest;
using QuanLyPhatTu_API.Payloads.Responses;
using QuanLyPhatTu_API.Service.Interfaces;

namespace QuanLyPhatTu_API.Controllers
{
    [ApiController]
    public class DaoTrangController : ControllerBase
    {
        private readonly IDaoTrangService _daoTrangService;
        public DaoTrangController(IDaoTrangService daoTrangService)
        {
            _daoTrangService = daoTrangService;
        }

        [HttpDelete("/api/daotrang/XoaDaoTrang/{daoTrangId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> XoaDaoTrang(int daoTrangId)
        {
            return Ok(await _daoTrangService.XoaDaoTrang(daoTrangId));
        }

        [HttpPost("/api/daotrang/ThemDaoTrang")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> ThemDaoTrang(Request_ThemDaoTrang request)
        {
            return Ok(await _daoTrangService.ThemDaoTrang(request));
        }

        [HttpPut("/api/daotrang/SuaThongTinDaoTrang/{daoTrangId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> SuaThongTinDaoTrang(int daoTrangId, Request_SuaThongTinDaoTrang request)
        {
            return Ok(await _daoTrangService.SuaThongTinDaoTrang(daoTrangId, request));
        }
        [HttpGet("/api/daotrang/ThongKeSoPhatTu")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> ThongKeSoPhatTu()
        {
            return Ok(await _daoTrangService.ThongKeSoPhatTuCuaChuaThamGiaDaoTrang());
        }
        [HttpGet("/api/daotrang/LayTatCaDaoTrang")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> LayTatCaDaoTrang()
        {
            return Ok(await _daoTrangService.LayTatCaDaoTrang());
        }
    }
}
