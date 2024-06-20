using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; } // Foreign key
        public virtual Type MaterialType { get; set; } // Navigation property
        public String CourseId { get; set; } // Foreign key
        public virtual Course Course { get; set; } // Navigation property
        public string InstructorId { get; set; }
        public virtual ApplicationUser Instructor { get; set; }

    }
}
