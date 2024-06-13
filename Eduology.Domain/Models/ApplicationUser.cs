using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        // to represent one to many relationship between instructor and course
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Material> Materials { get; set; } 



    }
}
