using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class submissionDetailsDto
    {
        public int AssignmentId { get; set; }
        public string courseId { get; set; }
        public int SubmissionId { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public DateTime TimeStamp { get; set; }
        public IFormFile file { get; set; }
        public int grade { get; set; }
    }
}
