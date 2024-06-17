using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string InstructorId { get; set; }
        public ICollection<StudentCourse> studentCourses { get; set; }
    }
}
