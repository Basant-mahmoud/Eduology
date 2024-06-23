using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class GetMaterialDto
    {
        
        public string Module { get; set; }
      
        public List<GetFileDto> FileURLs { get; set; } // List of file URLs

        public GetMaterialDto()
        {
            FileURLs = new List<GetFileDto>();
        }
    }
}
