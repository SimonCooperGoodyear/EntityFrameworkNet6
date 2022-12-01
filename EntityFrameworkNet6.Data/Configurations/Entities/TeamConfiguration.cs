using EntityFrameworkNet6.Domain;
using EntityFrameworkNet6.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkNet6.Data.Configurations.Entities
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .HasMany(m => m.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(m => m.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(50);
            // Make this a unique value index - as opposed to just an index
            builder.HasIndex(h => h.Name).IsUnique();

            builder.HasData(
                new Team
                {
                    Id = 20,
                    Name = "Berryfield Wanderers",
                    LeagueId= 20
                }
            );

        }
    }
}
