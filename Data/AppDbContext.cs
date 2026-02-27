
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgSheet.Api.Models;

namespace RpgSheet.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sheet>()
                .HasMany(s => s.Skills)
                .WithOne(sk => sk.Sheet)
                .HasForeignKey(sk => sk.SheetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Skill>().HasIndex(sk => sk.SheetId);
        }
    }
}