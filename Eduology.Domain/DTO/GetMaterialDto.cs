using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class GetMaterialDto
    {
        public int MaterialId { get; set; } // Add MaterialId
        public string Title { get; set; }
        public string MaterialType { get; set; }
        public string InstructorId { get; set; }
        public string CourseId { get; set; }
        public List<GetFileDto> FileURLs { get; set; } // List of file URLs

        public GetMaterialDto()
        {
            FileURLs = new List<GetFileDto>();
        }
    }
}
