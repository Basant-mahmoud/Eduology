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
        Task<bool> AddMaterialAsync(string instructorid, MaterialDto materialDto);
        Task<ICollection<GetMaterialDto>> GetMaterialToInstructorsAsync(string instructorid, CourseUserRequestDto requestDto);
        Task<ICollection<GetMaterialDto>> GetMaterialToStudentAsync(string studentid, CourseUserRequestDto requestDto);
        Task<bool> DeleteFileAsync(string instructorIid, DeleteFileDto deletefile);

    }
}
