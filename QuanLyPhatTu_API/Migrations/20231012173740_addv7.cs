using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyPhatTu_API.Migrations
{
    /// <inheritdoc />
    public partial class addv7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnhBaiViet",
                table: "BaiViet_tbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnhBaiViet",
                table: "BaiViet_tbl");
        }
    }
}
