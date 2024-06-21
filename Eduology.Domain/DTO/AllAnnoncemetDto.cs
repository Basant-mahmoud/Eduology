using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class AllAnnoncemetDto
    {
        public string coursename {  get; set; }
        public string instructorname { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAT { get; set; }
    }
}
