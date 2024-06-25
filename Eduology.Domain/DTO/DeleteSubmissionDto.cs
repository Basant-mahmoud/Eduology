using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class DeleteSubmissionDto
    {
       public int SubmissionId {  get; set; }
       public string CourseId { get; set; }
       public int AssigmentId { get; set; }
    }
}
