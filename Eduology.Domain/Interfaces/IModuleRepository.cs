using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Type;
namespace Eduology.Domain.Interfaces
{
    public interface IModuleRepository
    {
        Task<(bool Success, bool Exists, Type Type)> AddModuleAsync(Type type);
        Task<Type> GetModuleByNameAsync(string typeName);
        Task<bool> DeleteModuleAsync(string materialType);
        Task<List<ModuleWithFilesDto>> GetAllModulesAsync(string courseId);
    }
}
