using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseDetailsDto
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public ICollection<string> students { get; set; }
        public ICollection<string> Instructors { get; set; }
        public ICollection<AssignmentDto> assignments { get; set; }

    }
}
