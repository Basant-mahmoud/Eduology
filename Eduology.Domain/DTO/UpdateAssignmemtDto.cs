using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class UpdateAssignmemtDto
    {
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Title { get; set; }
        public String CourseId { get; set; }
    }
}
