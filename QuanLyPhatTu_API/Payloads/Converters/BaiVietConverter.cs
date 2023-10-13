﻿using QuanLyPhatTu_API.Entities;
using QuanLyPhatTu_API.Payloads.DTOs;
using QuanLyPhatTu_API.Payloads.Requests.BaiVietRequest;

namespace QuanLyPhatTu_API.Payloads.Converters
{
    public class BaiVietConverter
    {
        public BaiVietDTO EntityToDTO(BaiViet baiViet)
        {
            return new BaiVietDTO
            {
                Id = baiViet.Id,
                LoaiBaiVietId = baiViet.LoaiBaiVietId,
                MoTa = baiViet.MoTa,
                NoiDung = baiViet.NoiDung,
                AnhBaiViet = baiViet.AnhBaiViet,
                SoLuotBinhLuan = baiViet.SoLuotBinhLuan,
                SoLuotThich = baiViet.SoLuotThich,
                ThoiGianDang = baiViet.ThoiGianDang,
                TieuDe = baiViet.TieuDe
            };
        }
        public BaiViet TaoBaiViet(Request_TaoBaiViet request)
        {
            return new BaiViet
            {
                LoaiBaiVietId = request.LoaiBaiVietId,
                NoiDung = request.NoiDung,
                MoTa = request.MoTa,
                TieuDe = request.TieuDe
            };
        }
        public BaiViet SuaBaiViet(BaiViet baiViet, Request_SuaBaiViet request)
        {
            baiViet.LoaiBaiVietId = request.LoaiBaiBietId;
            baiViet.ThoiGianCapNhat = DateTime.Now;
            baiViet.DaXoa = false;
            baiViet.MoTa = request.MoTa;
            baiViet.NoiDung = request.NoiDung;
            baiViet.TieuDe = request.TieuDe;
            return baiViet;


        }
    }
}
