using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Eduology.Domain.Models;

namespace Eduology.Infrastructure.Persistence
{
    public class EduologyDBContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Material> Materials { get; set; } // Add this line
        public DbSet<Domain.Models.Type> MaterialTypes { get; set; } // Add this line
        public DbSet<Announcement> Announcements { get; set; } // Add this line
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> submissions { get; set; }
        public DbSet<Domain.Models.File> Files { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CourseInstructor> courseInstructors { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        public EduologyDBContext(DbContextOptions<EduologyDBContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship between ApplicationUser (Admin) and Organization
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.organization)
                .WithOne(o => o.Admin)
                .HasForeignKey<Organization>(o => o.AdminId);
            modelBuilder.Entity<Course>()
            .HasIndex(e => e.CourseCode)
            .IsUnique();

            base.OnModelCreating(modelBuilder);
            // Many-to-Many relationship between Students and Courses
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure many-to-many relationship between Course and ApplicationUser (Instructor)
            modelBuilder.Entity<CourseInstructor>()
               .HasKey(ci => new { ci.CourseId, ci.InstructorId });

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.course)
                .WithMany(c => c.CourseInstructors)
                .HasForeignKey(ci => ci.CourseId);

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.Instructor)
                .WithMany(i => i.CourseInstructors)
                .HasForeignKey(ci => ci.InstructorId);
            // Configure one-to-many relationship between ApplicationUser (Instructor) and Material
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Instructor)
                .WithMany(u => u.Materials)
                .HasForeignKey(m => m.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure one-to-many relationship between ApplicationUser (Instructor) and annuncement
            modelBuilder.Entity<Announcement>()
               .HasOne(m => m.Instructor)
               .WithMany(u => u.Announcements)
               .HasForeignKey(m => m.InstructorId)
               .OnDelete(DeleteBehavior.Cascade);
            // Configure one-to-many relationship between course and annuncement
            modelBuilder.Entity<Course>()
            .HasMany(c => c.Announcements)
            .WithOne(a => a.Course)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
