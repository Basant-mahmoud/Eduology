using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
   public interface  IMaterialService
    {
        Task<bool> AddMaterialAsync(MaterialDto MaterialDto);
        Task<ICollection<GetMaterialDto>> GetMaterialToInstructorsAsync(CourseInstructorRequestDto requestDto);
        Task<ICollection<GetMaterialDto>> GetMaterialToStudentAsync(CourseStudentRequestDto requestDto);
        Task<bool> DeleteFileAsync(DeleteFileDto deletefile);

    }
}
