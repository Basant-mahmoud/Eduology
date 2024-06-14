﻿using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
