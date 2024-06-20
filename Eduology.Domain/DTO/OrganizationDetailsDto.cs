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
        public List<Course> Courses { get; set; }
        public List<ApplicationUser> students { get; set; }
        public List<ApplicationUser> instructors { get; set; }
        public List<ApplicationUser> admins { get; set; }

    }
}
