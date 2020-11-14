using System;
using Microsoft.EntityFrameworkCore;

namespace Matrimony.Backend.Models
{
    public class MatrimonyDBContext: DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=localhost;Database=matrimony;User Id=sa;Password=reallyStrongPwd123;");
        }
    }
}
