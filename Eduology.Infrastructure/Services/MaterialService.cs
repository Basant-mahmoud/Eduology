using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using File = Eduology.Domain.Models.File;
using Type = Eduology.Domain.Models.Module;
namespace Eduology.Infrastructure.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _matrialRepository;

        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _moduleRepository;

        public MaterialService(IMaterialRepository matrialRepository, ICourseRepository courseRepository, IModuleRepository moduleRepository)
        {
            _matrialRepository = matrialRepository;
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
        }
        public async Task<bool> AddMaterialAsync(string instructorid,MaterialDto materialDto)
        {
            if (materialDto == null ||
                string.IsNullOrEmpty(materialDto.CourseId) ||
                string.IsNullOrEmpty(instructorid) ||
                string.IsNullOrEmpty(materialDto.Module) ||
                materialDto.FileURLs == null || materialDto.FileURLs.Count == 0)
            {
                return false;
            }

            // Check if the instructor is assigned to the course
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(instructorid, materialDto.CourseId);
            if (!isInstructorAssigned)
            {
                return false;
            }

            // Get the module by name within the course
            var moduleDto = new ModuleDto
            {
                CourseId = materialDto.CourseId,
                Name = materialDto.Module
            };
            var existingModule = await _moduleRepository.GetModuleByNameAsync(moduleDto);
            if (existingModule == null)
            {
                return false; 
            }

            // Create a new Material entity
            var material = new Material
            {
                InstructorId = instructorid,
                CourseId = materialDto.CourseId,
                ModuleId = existingModule.ModuleId, 
                Files = new List<File>()
            };

            // Add files to the material
            foreach (var fileDto in materialDto.FileURLs)
            {
                var file = new File
                {
                    FileId = Guid.NewGuid().ToString(),
                    URL = fileDto.URL,
                    Title = $"File for {fileDto.Title}",
                    MaterialId = material.MaterialId
                };
                material.Files.Add(file);
            }

            // Call repository method to add the material
            var success = await _matrialRepository.AddMaterialAsync(material);

            return success;
        }

        public async Task<ICollection<GetMaterialDto>> GetMaterialToInstructorsAsync(string instructorid,CourseUserRequestDto requestDto)
        {
            if (requestDto == null ||
                string.IsNullOrEmpty(requestDto.CourseId) ||
                string.IsNullOrEmpty(instructorid))
            {
                return null;
            }

            // Check if the instructor is assigned to the course
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(instructorid, requestDto.CourseId);
            if (!isInstructorAssigned)
            {
                return null;
            }

            // Get all modules for the course
            var modules = await _moduleRepository.GetModulesByCourseIdAsync(requestDto.CourseId);
            if (modules == null || !modules.Any())
            {
                return new List<GetMaterialDto>();
            }

            var moduleWithMaterialsList = new List<GetMaterialDto>();

            foreach (var module in modules)
            {
                // Aggregate files for the current module
                var files = module.Materials.SelectMany(m => m.Files).Select(f => new GetFileDto
                {
                    FileId = f.FileId,
                    Title = f.Title,
                    URL = f.URL
                }).ToList();

                // Check if the module already exists in the list
                var existingModule = moduleWithMaterialsList.FirstOrDefault(m => m.Module == module.Name);
                if (existingModule != null)
                {
                    // Add files to the existing module
                    existingModule.FileURLs.AddRange(files);
                }
                else
                {
                    var moduleDto = new GetMaterialDto
                    {  
                        Module = module.Name,
                        FileURLs = files
                    };
                    moduleWithMaterialsList.Add(moduleDto);
                }
            }

            return moduleWithMaterialsList;
        }
        /// ////////////////////////////////
        public async Task<ICollection<GetMaterialDto>> GetMaterialToStudentAsync(string studentid, CourseUserRequestDto requestDto)
        {
            if (requestDto == null ||
                string.IsNullOrEmpty(requestDto.CourseId) ||
                string.IsNullOrEmpty(studentid))
            {
                return null;
            }

            // Check if the instructor is assigned to the course
            var isStudentAssigned = await _courseRepository.IStudentAssignedToCourse(studentid, requestDto.CourseId);
            if (!isStudentAssigned)
            {
                return null;
            }

            // Get all modules for the course
            var modules = await _moduleRepository.GetModulesByCourseIdAsync(requestDto.CourseId);
            if (modules == null || !modules.Any())
            {
                return new List<GetMaterialDto>();
            }

            var moduleWithMaterialsList = new List<GetMaterialDto>();

            foreach (var module in modules)
            {
                // Aggregate files for the current module
                var files = module.Materials.SelectMany(m => m.Files).Select(f => new GetFileDto
                {
                    FileId = f.FileId,
                    Title = f.Title,
                    URL = f.URL
                }).ToList();

                // Check if the module already exists in the list
                var existingModule = moduleWithMaterialsList.FirstOrDefault(m => m.Module == module.Name);
                if (existingModule != null)
                {
                    // Add files to the existing module
                    existingModule.FileURLs.AddRange(files);
                }
                else
                {
                    var moduleDto = new GetMaterialDto
                    {
                        Module = module.Name,
                        FileURLs = files
                    };
                    moduleWithMaterialsList.Add(moduleDto);
                }
            }

            return moduleWithMaterialsList;
        }
        public async Task<bool> DeleteFileAsync(string instructorIid,DeleteFileDto deletefile)
        {
            if (deletefile == null ||
                string.IsNullOrEmpty(deletefile.courseId) ||
                string.IsNullOrEmpty(instructorIid) || string.IsNullOrEmpty(deletefile.Module)|| string.IsNullOrEmpty(deletefile.fileId))
            {
                return false;
            }
            // Check if the instructor is assigned to the course
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(instructorIid, deletefile.courseId);
            if (!isInstructorAssigned)
            {
                return false;
            }
            var iscourseexist = _courseRepository.GetByIdAsync(deletefile.courseId);
            if (iscourseexist == null) { return false; }
            var success = await _matrialRepository.DeleteMatrialAsync(deletefile);
            return success;
        }

       
    }
}

