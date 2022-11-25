using EntityFrameworkNet6.Data;
using EntityFrameworkNet6.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

internal class Program
{
    private static readonly FootballLeagueDbContext context = new FootballLeagueDbContext();
    private static async Task Main(string[] args)
    {
        /* ADDS */
        //await AddSingleRecord();
        //await AddSingleRecordV2();
        //await AddLeagueThenTeams();
        //await AddNewTeamsWithLeague();

        /* READS */
        //await SimpleReadAll();
        //await QueryFilters();
        //await FuzzyLogicQueryFilters();
        //await AdditionalExecutionMethods();
        //await AlternativeLinqSyntax();
        //await GetLeagueRecord(2);

        /* UPDATE Operations */
        //await Update_Record();
        //await SimpleUpdateTeam();

        /* DELETE Operations */
        //await SimpleDelete();
        //await DeleteWithRelationship();

        /* Tracking vs NoTracking */
        //await TrackingVsNoTracking();

        /* Adding Records with Relationships */
        //await AddNewTeamsWithLeague();
        //await AddNewTeamsWithLeagueId();
        //await AddNewLeagueWithTeams();

        /* Adding ManyToMany records */
        //await AddNewMatches();

        /* Adding OneToOne records */
        //await AddNewCoach();

        /* Including Related Data - Eager Loading */

        Console.WriteLine("Press any key to end");
        Console.ReadKey();
    }

    private static async Task AddNewCoach()
    {
        var coach1=new Coach { Name="Antonio Conte",TeamId=12};

        await context.AddAsync(coach1);
        await context.SaveChangesAsync();
    }

    private static async Task AddNewMatches()
    {
        var matches = new List<Match> {
            new Match {AwayTeamId= 1,HomeTeamId=2,Date=new DateTime(2021,10,28)},
            new Match {AwayTeamId= 6,HomeTeamId=7,Date=DateTime.Now},
            new Match {AwayTeamId= 10,HomeTeamId=11,Date=DateTime.Now}
            };
        await context.AddRangeAsync(matches);
        await context.SaveChangesAsync();
    }

    private static async Task AddNewLeagueWithTeams()
    {
        var teams = new List<Team>
        {
            new Team { Name="Paris St. Germain"},
            new Team { Name="Marseille"}
        };
        var league = new League { Name = "La Premiershipe", Teams = teams };
        await context.AddAsync(league);
        await context.SaveChangesAsync();
    }

    private static async Task AddNewTeamsWithLeagueId()
    {
        var team = new Team { Name = "Celtic", LeagueId = 8 };
        await context.Teams.AddAsync(team);
        await context.SaveChangesAsync();
    }

    private static async Task TrackingVsNoTracking()
    {
        var withTracking = await context.Teams.FirstOrDefaultAsync(t => t.Id == 6);
        var withNoTracking = await context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == 3);

        withTracking.Name = "Arsenal";
        withNoTracking.Name = "Napoli";

        var entriesBeforeSave = context.ChangeTracker.Entries();

        await context.SaveChangesAsync();

        var entriesAfterSave = context.ChangeTracker.Entries();
    }

    private static async Task DeleteWithRelationship()
    {
        var league = await context.Leagues.FindAsync(7);
        context.Leagues.Remove(league);
        await context.SaveChangesAsync();
    }

    private static async Task SimpleDelete()
    {
        // Get the object
        var league = await context.Leagues.FindAsync(9);
        // Delete the object
        context.Leagues.Remove(league);
        await context.SaveChangesAsync();
    }

    private static async Task SimpleUpdateTeam()
    {
        var team = new Team
        {
            // If Id had been missed off then it would have done an insert
            Id = 5,
            Name = "Tivoli Gardens FC",
            LeagueId = 1
        };
        context.Teams.Update(team);
        await context.SaveChangesAsync();
    }

    private static async Task Update_Record()
    {
        //// Retrieve Record
        var league = await context.Leagues.FindAsync(8);

        //// Update Record
        league.Name = "Scottish Premiership";

        //// Save Record
        // if it's not really a change, then no update happens - clever huh???  THIS IS TRACKING
        await context.SaveChangesAsync();

        await GetLeagueRecord(league.Id);
    }

    private static async Task GetLeagueRecord(int id)
    {
        var league = await context.Leagues.FindAsync(id);
        Console.WriteLine($"{league.Id} - {league.Name}");
    }

    private static async Task AlternativeLinqSyntax()
    {
        // teams is IQueryable here
        //var teams = from i in context.Teams select i;

        // and now it's a List
        //var teams = await (from i in context.Teams select i).ToListAsync();

        Console.WriteLine("Enter Team to search for");
        var team = Console.ReadLine();

        var teams = await (from i in context.Teams
                           where EF.Functions.Like(i.Name, $"%{team}%")
                           select i).ToListAsync();

        foreach (var item in teams)
        {
            Console.WriteLine($"{item.Id} - {item.Name}");
        }
    }

    private static async Task AdditionalExecutionMethods()
    {
        //var l=context.Leagues.Where(q=>q.Name.Contains("A")).FirstOrDefault();
        //var l=await context.Leagues.FirstOrDefaultAsync(q=>q.Name.Contains("A"));
        //Console.WriteLine($"{l.Id} - {l.Name}");

        var leagues = context.Leagues;

        var leaguesSingle = context.Leagues.Where(q => q.Id == 1);

        var list = await context.Leagues.ToListAsync();

        // still gets a list - just 1 item though
        var first = await context.Leagues.FirstAsync();

        // still gets a list - just 1 item though
        var firstOrDefault = await context.Leagues.FirstOrDefaultAsync();

        // expects only one record, and not in a list otherwise it will error
        var single = await leaguesSingle.SingleAsync();

        // expects only one record, and not in a list otherwise it will error
        var singleOrDefault = await leaguesSingle.SingleOrDefaultAsync();

        var count = await context.Leagues.CountAsync();
        var longCount = await context.Leagues.LongCountAsync();
        //var min=await context.Leagues.MinAsync();
        //var max=await context.Leagues.MaxAsync();

        //DbSet method that will execute
        var league = await leagues.FindAsync(1);

    }

    private static async Task FuzzyLogicQueryFilters()
    {
        Console.WriteLine("Enter League to search for");
        var league = Console.ReadLine();
        // simpler LINQ method
        // var leagues = await context.Leagues.Where(q => q.Name.Contains(league)).ToListAsync();
        // alternative method using more SQL-like method
        var leagues = await context.Leagues.Where(q => EF.Functions.Like(q.Name, $"%{league}%")).ToListAsync();
        foreach (var item in leagues)
        {
            Console.WriteLine($"{item.Id} - {item.Name}");
        }
    }

    private static async Task QueryFilters()
    {
        Console.WriteLine("Enter League to search for");
        var league = Console.ReadLine();
        //var leagues = await context.Leagues.Where(q => q.Name == league).ToListAsync();
        var leagues = await context.Leagues.Where(q => q.Name.Equals(league)).ToListAsync();
        foreach (var item in leagues)
        {
            Console.WriteLine($"{item.Id} - {item.Name}");
        }
    }

    private static async Task SimpleReadAll()
    {
        var leagues = await context.Leagues.ToListAsync();
        foreach (var item in leagues)
        {
            Console.WriteLine($"{item.Id} - {item.Name}");
        }
    }

    private static async Task AddNewTeamsWithLeague()
    {
        var league = new League { Name = "Budesliga" };
        var team = new Team { Name = "Bayern Munich", League = league };
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
        League league = new League { Name = "Serie A" };
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