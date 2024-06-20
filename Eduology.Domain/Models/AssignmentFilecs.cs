using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class AssignmentFile
    {
        public int AssignmentFileId { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public virtual Assignment Assignment { get; set; }
        public int AssignmentId { get; set; }
    }
}