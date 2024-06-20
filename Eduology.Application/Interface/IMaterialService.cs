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
        Task<(bool Success, bool Exists, Eduology.Domain.Models.Type Type)> AddTypeAsync(MaterialType materialType);
        Task<List<MaterialDto>> GetAllMaterialsAsync(string courseId);
        Task<List<ModuleWithFilesDto>> GetModulesWithFilesAsync(string courseId);
        Task<bool> DeleteFileAsync(string fileId, string courseId, string materialType);
        Task<bool> DeleteModule(string materialType);
    }
}
