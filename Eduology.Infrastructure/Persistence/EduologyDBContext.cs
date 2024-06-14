using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public EduologyDBContext(DbContextOptions<EduologyDBContext> options) : base(options)
        { }

    }
}
