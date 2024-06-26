using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseUserDto
    {
        public string? CourseId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int year { get; set; }
        public ICollection<string> Students { get; set; }
        public ICollection<string> Instructors { get; set; }
    }
}
