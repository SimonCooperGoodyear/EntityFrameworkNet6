using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkNet6.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditingUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Audits");
        }
    }
}
