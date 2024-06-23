using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class DeleteFileDto
    {
        public string InstructorId { get; set; }
        public string courseId { get; set; }
        public string Module { get; set; }
        public string fileId { get; set; }
    }
}
