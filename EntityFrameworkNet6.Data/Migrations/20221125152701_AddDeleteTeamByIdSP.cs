using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkNet6.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteTeamByIdSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DeleteTeamById
	                @teamId int
                AS
                BEGIN
	                Delete from Teams where Id = @teamId
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE PROCEDURE [dbo].[sp_DeleteTeamById]");
        }
    }
}
