using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class SubmissionDto
    {
        public int AssignmentId { get; set; } 
        public string courseId { get; set; }
        public IFormFile file { get; set; }
    }
}
