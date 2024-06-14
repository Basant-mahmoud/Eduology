using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class File
    {
        public string FileId { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public int AssignmentId { get; set; } // Foreign key
        public Assignment Assignment { get; set; } // Navigation property
        public int MaterialId { get; set; } // Foreign key
        public Material Material { get; set; } // Navigation property

    }
}
