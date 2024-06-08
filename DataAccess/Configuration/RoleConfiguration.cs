﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
              new IdentityRole
              {
                  Name = "Manager",
                  NormalizedName = "MANAGER"
              },
              new IdentityRole
              {
                  Name = "Admin",
                  NormalizedName = "ADMIN"
              }
            );
        }
    }
}
