using EntityFrameworkNet6.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkNet6.Data
{
    public class FootballLeagueDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDb;Initial Catalog=FootballLeagueEFCore6")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Name }, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                            .HasMany(m => m.HomeMatches)
                            .WithOne(m => m.HomeTeam)
                            .HasForeignKey(m => m.HomeTeamId)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                            .HasMany(m => m.AwayMatches)
                            .WithOne(m => m.AwayTeam)
                            .HasForeignKey(m => m.AwayTeamId)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
    }
}