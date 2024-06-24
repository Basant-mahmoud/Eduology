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
        Task<(bool Success, bool Exists)> AddModuleAsync(string instructorid, ModuleDto moduleDto);
        Task<bool> DeleteModuleAsync(string instructorid, ModuleDto moduleDto);
        Task<bool> UpdateModuleAsync(string instructorid, UpdateModuleDto updatemodule);

    }
}
