using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class AssignmentCreationDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public String CourseId { get; set; }
        public IFormFile File { get; set; }
    }
}
