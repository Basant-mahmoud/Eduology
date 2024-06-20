using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class DeleteFileDto
    {
        public string fileId { get; set; }
        public string courseId { get; set; }
        public string materialType { get; set; }
    }
}
