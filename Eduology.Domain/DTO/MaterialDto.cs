using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    
        public class MaterialDto
        {
           // public string Title { get; set; }
            public string Module { get; set; }
           // public string InstructorId { get; set; }
            public string CourseId { get; set; }
            public List<FileDto> FileURLs { get; set; } // List of file URLs

            public MaterialDto()
            {
                FileURLs = new List<FileDto>();
            }
        }
    
}
