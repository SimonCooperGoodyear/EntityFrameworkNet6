using EntityFrameworkNet6.Domain;
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
    internal class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.HasIndex(h => h.Name);

            builder.HasData(
                new League
                {
                    Id = 20,
                    Name = "Sample League"
                }
            );

        }
    }
}
