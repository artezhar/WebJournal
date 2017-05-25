using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebZhurnal.Models;

namespace WebZhurnal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MaterialGroup>().HasOne(mg => mg.Material).WithMany(mg => mg.MaterialGroups).HasForeignKey(mg => mg.MaterialId);
            builder.Entity<MaterialGroup>().HasOne(mg => mg.Group).WithMany(mg => mg.MaterialGroups).HasForeignKey(mg => mg.GroupId);
            builder.Entity<MaterialGroup>().HasKey(mg => new { mg.GroupId, mg.MaterialId });
            builder.Entity<Material>().HasOne(mat => mat.Subject).WithMany(s => s.Materials).HasForeignKey(mat => mat.SubjectId).IsRequired(false);
            builder.Entity<ApplicationUser>().HasOne(u => u.Group).WithMany(s => s.Users).HasForeignKey(mat => mat.GroupId).IsRequired(false);
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentRate> Rates { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<FileModel> Files { get; set; }
    }
}
