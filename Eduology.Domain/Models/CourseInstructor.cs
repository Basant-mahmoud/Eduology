using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class CourseInstructor
    {
        public string? InstructorId { get; set; }
        public int CourseId { get; set; }
        public ApplicationUser Instructor { get; set; }
        public Course course { get; set; }


    }
}
