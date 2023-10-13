namespace QuanLyPhatTu_API.Payloads.DTOs
{
    public class NguoiDungThichBinhLuanBaiVietDTO : BaseDTO
    {
        public int PhatTuId { get; set; }
        public int BinhLuanBaiVietId { get; set; }
        public DateTime ThoiGianThich { get; set; }
        public bool DaXoa { get; set; }
    }
}
