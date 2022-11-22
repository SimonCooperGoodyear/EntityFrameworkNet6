using EntityFrameworkNet6.Data;
using EntityFrameworkNet6.Domain;

internal class Program
{
    private static readonly FootballLeagueDbContext context=new FootballLeagueDbContext();
    private static async Task Main(string[] args)
    {
        //await AddSingleRecord();
        //await AddSingleRecordV2();
        //await AddLeagueThenTeams();
        await AddLeagueAndTeam();


        Console.WriteLine("Press any key to end");
        Console.ReadKey();
    }

    private static async Task AddLeagueAndTeam()
    {
        var league = new League { Name = "Budesliga" };
        var team = new Team { Name="Bayern Munich",League=league};
        await context.Teams.AddAsync(team);
        await context.SaveChangesAsync();
    }

    static async Task AddSingleRecord()
    {
        await context.Leagues.AddAsync(new League { Name = "Premier League" });
        await context.SaveChangesAsync();
    }

    static async Task AddSingleRecordV2()
    {
        League league = new League();
        league.Name = "La Liga";
        await context.Leagues.AddAsync(league);
        await context.SaveChangesAsync();
    }

    static async Task AddLeagueThenTeams()
    {
        League league=new League { Name="Serie A"};
        await context.Leagues.AddAsync(league);
        await context.SaveChangesAsync();
        await AddTeamWithLeague(league);
    }

    private static async Task AddTeamWithLeague(League league)
    {
        var teams = new List<Team>
        {
            new Team
            {
                Name="Juventus",
                LeagueId=league.Id
            },
            new Team
            {
                Name="AC Milan",
                LeagueId=league.Id
            },
            new Team
            {
                Name="AS Roma",
                League=league
            }
        };
        await context.Teams.AddRangeAsync(teams);
        await context.SaveChangesAsync();
    }
}