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
        Task<List<MaterialDto>> GetAllMaterialsAsync(string courseId);
        Task<bool> DeleteMatrialAsync(string fileId, string courseId, string materialType);
      
    }
}
