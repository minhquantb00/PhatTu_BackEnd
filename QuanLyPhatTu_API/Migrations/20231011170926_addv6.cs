using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyPhatTu_API.Migrations
{
    /// <inheritdoc />
    public partial class addv6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DaoTrang_tbl",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DaoTrang_tbl");
        }
    }
}
