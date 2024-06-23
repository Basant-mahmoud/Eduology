using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class UpdateModuleDto
    {
       public string Instructorid {  get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string NewName {  get; set; }
    }
}
