using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class ModuleServicecs: IModuleService
    {
        private readonly IModuleRepository _ModuleRepository;

        private readonly ICourseRepository _courseRepository;
      

        public ModuleServicecs( ICourseRepository courseRepository, IModuleRepository moduleRepository)
        {
           
            _courseRepository = courseRepository;
            _ModuleRepository = moduleRepository;
        }
        public async Task<(bool Success, bool Exists, Domain.Models.Type Type)> AddModuleAsync(MaterialType materialType)
        {
            if (materialType == null)
            {
                throw new ArgumentException("Module cannot be null.");
            }

            if (string.IsNullOrEmpty(materialType.Name))
            {
                throw new ArgumentException("Module name cannot be null or empty.");
            }
            var type = new Domain.Models.Type
            {
                Name = materialType.Name.ToLower(),
            };

            var (success, exists, createdType) = await _ModuleRepository.AddModuleAsync(type);
            return (success, exists, createdType);
        }
        public async Task<List<ModuleWithFilesDto>> GetAllModulesAsync(string courseId)
        {
            if (courseId == null)
            {
                throw new ArgumentException("Course ID cannot be null or empty.");
            }
            var courseExists = await _courseRepository.GetByIdAsync(courseId);
            if (courseExists == null)
            {
                return new List<ModuleWithFilesDto>();
                throw new ArgumentException("Course Not Exist");
            }

            var typesWithFiles = await _ModuleRepository.GetAllModulesAsync(courseId);
            return typesWithFiles;
        }
        public async Task<bool> DeleteModule(string Module)
        {
            if (string.IsNullOrEmpty(Module))
            {
                throw new ArgumentException("Module cannot be null or empty.");
            }

            var material = await _ModuleRepository.GetModuleByNameAsync(Module.ToLower());

            if (material == null)
            {
                return false;
            }
            var success = await _ModuleRepository.DeleteModuleAsync(Module);
            return success;

        }
    }
}
