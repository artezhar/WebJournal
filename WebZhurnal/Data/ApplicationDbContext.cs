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

            builder.Entity<TeacherGroup>().HasOne(tg => tg.Teacher).WithMany(tg => tg.TeacherGroups).HasForeignKey(tg => tg.TeacherId);
            builder.Entity<TeacherGroup>().HasOne(tg => tg.Group).WithMany(tg => tg.TeacherGroups).HasForeignKey(tg => tg.GroupId);
            builder.Entity<TeacherGroup>().HasKey(tg => new { tg.GroupId, tg.TeacherId });

            builder.Entity<SubjectGroup>().HasOne(tg => tg.Subject).WithMany(tg => tg.SubjectGroups).HasForeignKey(tg => tg.SubjectId);
            builder.Entity<SubjectGroup>().HasOne(tg => tg.Group).WithMany(tg => tg.SubjectGroups).HasForeignKey(tg => tg.GroupId);
            builder.Entity<SubjectGroup>().HasKey(tg => new { tg.GroupId, tg.SubjectId});
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentRate> Rates { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<RateItem> LogItems { get; set; }
    }
}
