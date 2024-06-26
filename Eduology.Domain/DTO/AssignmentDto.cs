using Eduology.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class AssignmentDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public AssignmentFileDto fileURLs { get; set; } = new AssignmentFileDto();
        public String CourseId { get; set; } // Foreign key
        public IFormFile File { get; set; }  

    }
}
