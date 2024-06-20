using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public string URL { get; set; }
        public int Grade { get; set; }
        public DateTime TimeStamp { get; set; }
        public int AssignmentId { get; set; } //Foreign key
        public Assignment Assignment { get; set; } //Navigation property
        public string StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }

    }
}