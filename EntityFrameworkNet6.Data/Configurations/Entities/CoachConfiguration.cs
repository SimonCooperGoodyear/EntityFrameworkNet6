using EntityFrameworkNet6.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkNet6.Data.Configurations.Entities
{
    public class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);
            // A unique multi field index
            builder.HasIndex(h => new { h.Name, h.TeamId }).IsUnique();

            builder.HasData(
                new Coach
                {
                    Id = 20,
                    Name = "Harry Redknapp",
                    TeamId = 20
                },
                new Coach
                {
                    Id = 21,
                    Name = "Billy Nicholson"
                },
                new Coach
                {
                    Id = 22,
                    Name = "Sir Alf Ramsey"
                }
            );

        }
    }
}
