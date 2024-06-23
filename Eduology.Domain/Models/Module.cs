using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Material> Materials { get; set; } 
        public string courseId { get; set; }
        public virtual  Course Course { get; set; }
    }
}
