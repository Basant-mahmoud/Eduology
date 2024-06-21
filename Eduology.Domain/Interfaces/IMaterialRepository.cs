﻿using Eduology.Domain.DTO;
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
        Task<bool> AddMaterialAsync(Material material);
        Task<List<Material>> GetAllMaterialsAsync(string courseId);
        Task<bool> DeleteMatrialAsync(string fileId, string courseId, string materialType);
       
    }
}
