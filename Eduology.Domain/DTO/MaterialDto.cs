using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    
        public class MaterialDto
        {
            public string Module { get; set; }
            public string CourseId { get; set; }
            public List<IFormFile> FileURLs { get; set; } // List of file URLs

            public MaterialDto()
            {
                FileURLs = new List<IFormFile>();
            }
        }
    
}
