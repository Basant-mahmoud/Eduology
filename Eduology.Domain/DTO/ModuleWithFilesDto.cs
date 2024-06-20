using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class ModuleWithFilesDto
    {
   
        public string TypeName { get; set; }
        public List<FileDtoWithId> Files { get; set; }
    }
}
