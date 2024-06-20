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
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public ICollection<string> students { get; set; }
        public ICollection<string> Instructors { get; set; }
        public ICollection<Assignment> assignments { get; set; }

    }
}
