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
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public string InstructorId { get; set; }
        public ICollection<StudentCourse> studentCourses { get; set; }

    }
}
