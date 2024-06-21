using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class SubmissionDto
    {
        public int SubmissionId { get; set; }
        public string URL { get; set; }
        public string StudentId { get; set; }
        public int AssignmentId { get; set; } 
    }
}
