using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class RegisterStudentToCourseDto
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string CourseCode { get; set; }
    }
}
