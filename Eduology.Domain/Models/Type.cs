using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Material> Materials { get; set; } 
    }
}
