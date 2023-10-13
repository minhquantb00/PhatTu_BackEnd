using QuanLyPhatTu_API.Entities;
using QuanLyPhatTu_API.Payloads.DTOs;

namespace QuanLyPhatTu_API.Payloads.Converters
{
    public class ChuaConverter
    {
        public ChuaDTO EntityToDTO(Chua chua)
        {
            return new ChuaDTO
            {
                Id = chua.Id,
                TenChua = chua.TenChua,
                DiaChi = chua.DiaChi,
                NgayThanhLap = DateTime.Parse(chua.NgayThanhLap.ToShortDateString()),
                NguoiTruTri = chua.NguoiTruTri,
                NgayCapNhat = chua.NgayCapNhat
            };
        }
    }
}
