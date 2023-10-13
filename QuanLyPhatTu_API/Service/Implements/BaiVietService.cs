using Microsoft.EntityFrameworkCore;
using QuanLyPhatTu_API.Handle.HandlePagination;
using QuanLyPhatTu_API.Handle.Image;
using QuanLyPhatTu_API.Payloads.Converters;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.Requests.BaiVietRequest;
using QuanLyPhatTu_API.Payloads.Responses;
using QuanLyPhatTu_API.Service.Interfaces;

namespace QuanLyPhatTu_API.Service.Implements
{
    public class BaiVietService : BaseService, IBaiVietService
    {
        private readonly BaiVietConverter _converter;
        private readonly ResponseObject<BaiVietDTO> _responseObjectBaiVietDTO;
        public BaiVietService()
        {
            _converter = new BaiVietConverter();
            _responseObjectBaiVietDTO = new ResponseObject<BaiVietDTO>();
        }
        private string ChuanHoaChuoi(string name)
        {
            name = name.Trim().ToLower();
            while (name.Contains("  "))
            {
                name = name.Replace("  ", " ");
            }
            return name;
        }
        public async Task<PageResult<BaiVietDTO>> LayBaiVietTheoLoaiBaiViet(string? tenLoai, int pageSize = 10, int pageNumber = 1)
        {
            var baiVietQuery = _context.baiViets
                .Where(x => ChuanHoaChuoi(x.LoaiBaiViet.TenLoai).Contains(ChuanHoaChuoi(tenLoai))).Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData<BaiVietDTO>(baiVietQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<BaiVietDTO>> LayBaiVietTheoNguoiDung(int? nguoiDungId, int pageSize = 10, int pageNumber = 1)
        {
            var baiVietQuery = _context.baiViets
                .Where(x => x.Id == nguoiDungId).Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData<BaiVietDTO>(baiVietQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<BaiVietDTO>> LayTatCaBaiViet(int pageSize, int pageNumber)
        {
            var baiVietQuery = _context.baiViets.Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData<BaiVietDTO>(baiVietQuery, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<BaiVietDTO>> SuaBaiViet(int baiVietId, int phatTuId, Request_SuaBaiViet request)
        {
            var baiViet = await _context.baiViets.SingleOrDefaultAsync(x => x.Id == baiVietId);
            if (baiViet == null)
            {
                return _responseObjectBaiVietDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy bài viết", null);
            }
            var phatTu = await _context.phatTus.SingleOrDefaultAsync(x => x.Id == phatTuId);
            var baiVietSua = _converter.SuaBaiViet(baiViet, request);
            baiVietSua.PhatTuId = phatTuId;
            _context.baiViets.Update(baiVietSua);
            await _context.SaveChangesAsync();
            return _responseObjectBaiVietDTO.ResponseSuccess("Sửa bài viết thành công", _converter.EntityToDTO(baiVietSua));
        }

        public async Task<ResponseObject<BaiVietDTO>> TaoBaiViet(int nguoiDangBaiId, IFormFile file, Request_TaoBaiViet request)
        {
            var loaiBaiViet = await _context.loaiBaiViets.SingleOrDefaultAsync(x => x.Id == request.LoaiBaiVietId);
            if (loaiBaiViet == null)
            {
                return _responseObjectBaiVietDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy loại bài viết", null);
            }
            var nguoiDangBai = await _context.phatTus.SingleOrDefaultAsync(x => x.Id == nguoiDangBaiId);
            var baiVietTao = _converter.TaoBaiViet(request);
            baiVietTao.ThoiGianDang = DateTime.MaxValue;
            baiVietTao.PhatTuId = nguoiDangBaiId;
            baiVietTao.NguoiDuyetBaiId = 0;
            baiVietTao.DaXoa = false;
            baiVietTao.SoLuotBinhLuan = 0;
            baiVietTao.SoLuotThich = 0;
            baiVietTao.AnhBaiViet = await HandleUploadImage.Upfile(file);
            baiVietTao.TrangThaiBaiVietId = 1;
            baiVietTao.ThoiGianCapNhat = DateTime.MaxValue;
            baiVietTao.ThoiGianXoa = DateTime.MaxValue;
            await _context.baiViets.AddAsync(baiVietTao);
            await _context.SaveChangesAsync();
            return _responseObjectBaiVietDTO.ResponseSuccess("Tạo bài viết thành công", _converter.EntityToDTO(baiVietTao));
        }

        public async Task<string> XoaBaiViet(int baiVietId)
        {
            var baiViet = await _context.baiViets.SingleOrDefaultAsync(x => x.Id == baiVietId);
            if (baiViet == null)
            {
                return "Không tìm thấy bài viết";
            }
            var nguoiTaoBai = await _context.phatTus.SingleOrDefaultAsync(x => x.Id == baiViet.PhatTuId);
            if(nguoiTaoBai == null)
            {
                return "Không muốn tìm thấy người tạo bài viết";
            }
            baiViet.DaXoa = true;
            _context.baiViets.Update(baiViet);
            await _context.SaveChangesAsync();
            return "Xóa bài viết thành công";
        }

        public async Task<string> DuyetBaiViet(int nguoiDuyetId, int baiVietId)
        {
            var baiViet = await _context.baiViets.SingleOrDefaultAsync(x => x.Id == baiVietId);
            if (baiViet is null)
            {
                return "Bài viết không tồn tại";
            }
            var nguoiDuyet = await _context.phatTus.SingleOrDefaultAsync(x => x.Id == nguoiDuyetId);
            baiViet.ThoiGianDang = DateTime.Now;
            baiViet.TrangThaiBaiVietId = 3;
            baiViet.NguoiDuyetBaiId = nguoiDuyetId;
            _context.baiViets.Update(baiViet);
            await _context.SaveChangesAsync();
            return "Đã duyệt bài thành công";

        }
        public async Task<ResponseObject<BaiVietDTO>> LayBaiVietTheoId(int baiVietId)
        {
            var user = await _context.baiViets.SingleOrDefaultAsync(x => x.Id == baiVietId);
            if(user == null)
            {
                return _responseObjectBaiVietDTO.ResponseError(StatusCodes.Status404NotFound, "Bài viết không tồn tại", null);
            }
            return _responseObjectBaiVietDTO.ResponseSuccess("Lấy bài viết thành công", _converter.EntityToDTO(user));
        }
    }
}
