using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
        public File File { get; set; }
        public String CourseId { get; set; } // Foreign key
        public Course? Course { get; set; } // Navigation property
        public string InstructorId { get; set; } // Foreign key
        public ApplicationUser? Instructor { get; set; } // Navigation property

    }
}
