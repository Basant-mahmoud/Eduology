using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseCreationDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
    }
}
