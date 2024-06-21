using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseDto
    {
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public int Year { get; set; }
        public string InstructorId { get; set; }
    }
}
