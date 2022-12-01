using EntityFrameworkNet6.Data.Configurations.Entities;
using EntityFrameworkNet6.Domain;
using EntityFrameworkNet6.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkNet6.Data
{
    public class FootballLeagueDbContext : AuditableFootballLeagueDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDb;Initial Catalog=FootballLeagueEFCore6",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30),errorNumbersToAdd:null);
                    }
                )
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamsCoachesLeaguesView>().HasNoKey().ToView("TeamsCoachesLeagues");
            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new CoachConfiguration());

            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder
                .Entity<Team>()
                .ToTable("Teams", b => b.IsTemporal());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder modelConfigurationBuilder)
        {
            //modelConfigurationBuilder.Properties<string>().AreUnicode(false).HaveMaxLength(50);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TeamsCoachesLeaguesView> TeamsCoachesLeagues { get; set; }
    }
}