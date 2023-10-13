using Microsoft.EntityFrameworkCore;
using QuanLyPhatTu_API.Entities;
using QuanLyPhatTu_API.Handle.Email;
using QuanLyPhatTu_API.Handle.HandlePagination;
using QuanLyPhatTu_API.Payloads.Converters;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.Requests.PhatTuRequest;
using QuanLyPhatTu_API.Payloads.Responses;
using QuanLyPhatTu_API.Service.Interfaces;

namespace QuanLyPhatTu_API.Service.Implements
{
    public class PhatTuService : BaseService, IPhatTuService
    {
        private readonly PhatTuConverter _phatTuConverter;
        private readonly ResponseObject<PhatTuDTO> _responseObject;
        public PhatTuService(PhatTuConverter phatTuConverter, ResponseObject<PhatTuDTO> responseObject)
        {
            _phatTuConverter = phatTuConverter;
            _responseObject = responseObject;
        }

        public async Task<PageResult<PhatTuDTO>> LayPhatTuTheoChua(int? chuaId, int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.ChuaId == chuaId).Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<PhatTuDTO>> LayPhatTuTheoEmail(string email)
        {
            var phatTuQuery = await _context.phatTus.FirstOrDefaultAsync(x => x.Email.Equals(email));
            return _responseObject.ResponseSuccess($"Phật tử có email: {email} có thông tin là: ", _phatTuConverter.EntityToDTO(phatTuQuery));
        }

        public async Task<PageResult<PhatTuDTO>> LayPhatTuTheoGioiTinh(string? gioiTinh, int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.GioiTinh.Equals(gioiTinh)).Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<PhatTuDTO>> LayPhatTuTheoPhapDanh(string? phapDanh, int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.PhapDanh.Equals(phapDanh)).Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery,pageSize, pageNumber); 
            return result;
        }

        public async Task<PageResult<PhatTuDTO>> LayPhatTuTheoTen(string? name, int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.HoVaTen.ToLower().Contains(name.ToLower())).Take(pageSize).Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<PhatTuDTO>> LayPhatTuTheoTrangThai(bool? status, int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.DaHoanTuc == status).Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<PhatTuDTO>> LayTatCaPhatTu(int pageSize = 10, int pageNumber = 1)
        {
            var phatTuQuery = _context.phatTus.Where(x => x.IsActive == true)
                .Select(x => _phatTuConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData<PhatTuDTO>(phatTuQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<PhatTuDTO>> SuaThongTinPhatTu(int phatTuId, Request_CapNhatThongTinPhatTu request)
        {
            var phatTu = await _context.phatTus.FirstOrDefaultAsync(x => x.Id == phatTuId);
            try
            {
                phatTu.PhapDanh = request.PhapDanh;
                phatTu.NgayCapNhat = DateTime.Now;
                phatTu.Email = request.Email;
                phatTu.GioiTinh = request.GioiTinh;
                phatTu.NgaySinh = request.NgaySinh;
                if (!Validate.IsValidPhoneNumber(request.SoDienThoai))
                {
                    throw new Exception("Định dạng số điện thoại không hợp lệ");
                }
                else
                {
                    phatTu.SoDienThoai = request.SoDienThoai;
                    _context.phatTus.Update(phatTu);
                    await _context.SaveChangesAsync();
                }
                return _responseObject.ResponseSuccess("Cập nhật thông tin phật tử thành công", _phatTuConverter.EntityToDTO(phatTu));
            }
            catch (Exception ex)
            {
                return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<string> ThemPhatTuVaoChua(Request_ThemPhatTuVaoChua request)
        {
            var phatTu = await _context.phatTus.SingleOrDefaultAsync(x => x.Id == request.PhatTuId);
            if(phatTu is null)
            {
                return "Phật tử không tồn tại";
            }
            var chua = await _context.chuas.SingleOrDefaultAsync(x => x.Id == request.ChuaId);
            if(chua is null)
            {
                return "Chùa không tồn tại";
            }
            phatTu.ChuaId = request.ChuaId;
            phatTu.NgayXuatGia = DateTime.Now;
            phatTu.NgayCapNhat = DateTime.Now;
            _context.phatTus.Update(phatTu);
            await _context.SaveChangesAsync();
            return "THêm phật tử vào chùa thành công";

        }

        public async Task<string> XoaPhatTu(int phatTuId)
        {
            var phatTu = await _context.phatTus.FirstOrDefaultAsync(x => x.Id == phatTuId);
            if(phatTu is null)
            {
                return "Phật tử không tồn tại";
            }
            else
            {
                phatTu.DaHoanTuc = true;
                phatTu.IsActive = false;
                _context.phatTus.Update(phatTu);
                await _context.SaveChangesAsync();
                return "Đã cập nhật trạng thái hoàn tục của phật tử";
            }
        }
    }
}
