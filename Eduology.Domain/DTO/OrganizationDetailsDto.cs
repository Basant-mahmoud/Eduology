using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class OrganizationDetailsDto
    {
        public int OrganizationID { get; set; }
        public List<CourseDto> Courses { get; set; }
        public List<UserDto> students { get; set; }
        public List<UserDto> instructors { get; set; }
        public List<UserDto> admins { get; set; }

    }
}
