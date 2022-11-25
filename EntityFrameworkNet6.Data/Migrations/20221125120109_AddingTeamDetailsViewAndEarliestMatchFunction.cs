using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkNet6.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingTeamDetailsViewAndEarliestMatchFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION [dbo].[GetEarliestMatch] (@teamId int)
	                RETURNS datetime
	                BEGIN
		                DECLARE @result datetime
		                SELECT TOP 1 @result=date
		                FROM [dbo].[Matches]
		                order by Date
		                return @result
	                END");
            migrationBuilder.Sql(@"
                        CREATE VIEW [dbo].[TeamsCoachesLeagues]
                        AS
                        SELECT t.Name,c.Name as CoachName,l.Name as LeagueName
                        FROM dbo.Teams as t
                        LEFT OUTER JOIN dbo.Coaches as c
                        ON t.Id=c.TeamId
                        INNER JOIN dbo.Leagues l
                        ON t.LeagueId=l.Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[TeamsCoachesLeagues]");
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[GetEarliestMatch]");
        }
    }
}
