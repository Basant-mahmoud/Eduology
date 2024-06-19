using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class StudentCourse
    {
            public string StudentId { get; set; }
            public ApplicationUser Student { get; set; }
            public String CourseId { get; set; }
            public Course Course { get; set; }
    }
}
