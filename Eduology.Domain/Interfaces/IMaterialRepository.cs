using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Type;

namespace Eduology.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<bool> AddMateriaCourseAsync(Material material);
        Task<(bool Success, bool Exists, Type Type)> AddTypeAsync(Type type);
        Task<Type> GetTypeByNameAsync(string typeName);
        Task<List<Material>> GetAllMaterialsAsync(string courseId);
        Task<List<ModuleWithFilesDto>> ModuleTypesWithFilesAsync(string courseId);
        Task<bool> DeleteFileAsync(string fileId, string courseId, string materialType);
        Task<bool> DeleteModuleAsync(string materialType);
    }
}
