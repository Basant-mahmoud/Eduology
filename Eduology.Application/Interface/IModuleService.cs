using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IModuleService
    {
        Task<(bool Success, bool Exists, Eduology.Domain.Models.Type Type)> AddTypeAsync(MaterialType materialType);
        Task<List<ModuleWithFilesDto>> GetModulesWithFilesAsync(string courseId);
        Task<bool> DeleteModule(string materialType);
    }
}
