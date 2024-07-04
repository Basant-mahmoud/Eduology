using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class SubmissionGradeDto
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public int AssignmentId { get; set; }
        public int Grade { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string AssignmentName { get; set; }
    }
}