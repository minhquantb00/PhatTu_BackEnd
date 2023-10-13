namespace QuanLyPhatTu_API.Payloads.DTOs
{
    public class DonDangKyDTO : BaseDTO
    {
        public DateTime NgayGuiDon { get; set; }
        public DateTime? NgayXuLy { get; set; }
        public int? NguoiXuLy { get; set; }
        public int TrangThaiDonId { get; set; }
        public int DaoTrangId { get; set; }
        public int PhatTuId { get; set; }
    }
}
