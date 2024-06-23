using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module = Eduology.Domain.Models.Module;
namespace Eduology.Domain.Interfaces
{
    public interface IModuleRepository
    {
         Task<bool> AddModuleAsync(Module module);
         Task<Module> GetModuleByNameAsync(ModuleDto module);
        Task<bool> DeleteModuleAsync(ModuleDto module);
        Task<bool> UpdateModuleAsync(UpdateModuleDto module);
        Task<List<Module>> GetModulesByCourseIdAsync(string courseId);

    }
}
