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
        Task<(bool Success, bool Exists)> AddModuleAsync(ModuleDto module);
        Task<bool> DeleteModuleAsync(ModuleDto module);
        Task<bool> UpdateModuleAsync(UpdateModuleDto moduleDto);

    }
}
