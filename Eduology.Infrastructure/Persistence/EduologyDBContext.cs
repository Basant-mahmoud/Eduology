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
        public EduologyDBContext(DbContextOptions<EduologyDBContext> options) : base(options)
        {
        }
    }
}
