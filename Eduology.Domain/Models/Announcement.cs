using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAT { get; set; }
        public String CourseId { get; set; } // Foreign key
        public virtual Course Course { get; set; } // Navigation property
        public string InstructorId { get; set; } // Ensure this matches the primary key type in ApplicationUser
        public virtual ApplicationUser Instructor { get; set; }
    }
}
