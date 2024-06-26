using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class ModuleServicecs : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ICourseRepository _courseRepository;

        public ModuleServicecs(ICourseRepository courseRepository, IModuleRepository moduleRepository)
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<(bool Success, bool Exists)> AddModuleAsync(string instructorid,ModuleDto moduleDto)
        {
            if (moduleDto == null || string.IsNullOrEmpty(moduleDto.Name) || string.IsNullOrEmpty(moduleDto.CourseId)|| string.IsNullOrEmpty(instructorid))
            {
                throw new ValidationException("Name or CourseId and instructorid");
            }

            var courseExists = await _courseRepository.GetByIdAsync(moduleDto.CourseId);
            var instructorExist = await _courseRepository.IsInstructorAssignedToCourse(instructorid, moduleDto.CourseId);
            if (courseExists == null)
            {
                return (false, false);
            }
            if(instructorExist == false)
            {
                return (false, false);
            }

            var moduleExists = await _moduleRepository.GetModuleByNameAsync(moduleDto);
            if (moduleExists != null)
            {
                return (false, true);
            }

            var module = new Domain.Models.Module
            {
                Name = moduleDto.Name,
                courseId = moduleDto.CourseId
            };

            var success = await _moduleRepository.AddModuleAsync(module);
            return (success, false);
        }

        public async Task<bool> DeleteModuleAsync(string instructorid,ModuleDto moduleDto)
        {
            if (string.IsNullOrEmpty(moduleDto.Name) || string.IsNullOrEmpty(moduleDto.CourseId))
            {
                throw new Exception("Module name or course id cant be null");
            }
            var existingCourse = await _courseRepository.GetByIdAsync(moduleDto.CourseId);
            if (existingCourse == null)
            {
                throw new Exception("Course not found.");
            }
            var instructorExist = await _courseRepository.IsInstructorAssignedToCourse(instructorid, moduleDto.CourseId);
            if (instructorExist == false)
            {
                throw new Exception("Instructor is not join to this course.");
            }

            var success = await _moduleRepository.DeleteModuleAsync(moduleDto);
            return success;
        }
        public async Task<bool> UpdateModuleAsync(string instructorid,UpdateModuleDto updatemodule)
        {
            if (updatemodule == null || string.IsNullOrEmpty(updatemodule.Name) || string.IsNullOrEmpty(updatemodule.CourseId))
            {
                return false;
            }
            var instructorExist = await _courseRepository.IsInstructorAssignedToCourse(instructorid, updatemodule.CourseId);
            if (instructorExist == false)
            {
                return false;
            }

            var module = new ModuleDto
            {
                Name = updatemodule.Name.ToLower(),
                CourseId = updatemodule.CourseId
            };
            var existingModule = await _moduleRepository.GetModuleByNameAsync(module);
            if (existingModule == null)
            {
                return false; // Module not found
            }

            
            var success = await _moduleRepository.UpdateModuleAsync(updatemodule);
            return success;
        }
        

    }
}
