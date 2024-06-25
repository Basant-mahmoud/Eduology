using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Module;

namespace Eduology.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<bool> AddMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(DeleteFileDto deletedfile);
    }
}
