using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Eduology.Domain.Models.File;
using Type = Eduology.Domain.Models.Type;
namespace Eduology.Infrastructure.Services
{
    public class MaterialService: IMaterialService
    {
        private readonly IMaterialRepository _matrialRepository;

        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _ModuleRepository;

        public MaterialService(IMaterialRepository matrialRepository, ICourseRepository courseRepository, IModuleRepository moduleRepository)
        {
            _matrialRepository = matrialRepository;
            _courseRepository = courseRepository;
            _ModuleRepository = moduleRepository;
        }
        public async Task<bool> AddMaterialAsync(MaterialDto materialDto)
        {
            if (materialDto == null)
            {
                return false;
            }
            if (materialDto.CourseId == null)
            {
                return false;
            }
            if (materialDto.InstructorId == null)
            {
                return false;
            }
            if (materialDto.FileURLs == null)
            {
                return false;
            }
            if (materialDto.MaterialType == null)
            {
                return false;
            }
            //instructor should be in course to add matrial
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(materialDto.InstructorId, materialDto.CourseId);
            if (!isInstructorAssigned)
            {
                return false;
            }
            var existingType = await _ModuleRepository.GetModuleByNameAsync(materialDto.MaterialType.ToLower());
            if (existingType == null)
            {
             
                return false; 
            }
            var course = await _courseRepository.GetByIdAsync(materialDto.CourseId);
            if (course == null)
            {
                return false;
               
            }
            
            var material = new Material
            {
                Title = materialDto.Title,
                InstructorId = materialDto.InstructorId,
                CourseId = materialDto.CourseId,
                MaterialType = existingType, 
                Files = new List<File>() 
            };

            if (materialDto.FileURLs != null && materialDto.FileURLs.Count > 0)
            {
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
            }
            var success = await _matrialRepository.AddMaterialAsync(material);

            return success;
        }

      
        public async Task<List<MaterialDto>> GetAllMaterialsAsync(string courseId)
        {
            if(courseId == null)
            {
                return null;
            }
            var existing= await _courseRepository.GetByIdAsync(courseId);
            if (existing == null)
            {
                Console.Error.WriteLine($"Type not exist");
                return new List<MaterialDto>();
               
              
            }
            var materials = await _matrialRepository.GetAllMaterialsAsync(courseId);

            var materialDtos = materials.Select(m => new MaterialDto
            {
                Title = m.Title,
                MaterialType = m.MaterialType.Name, 
                InstructorId = m.InstructorId,
                CourseId = m.CourseId,
                FileURLs = m.Files.Select(f => new FileDto
                {
                    URL = f.URL,
                    Title = f.Title
                }).ToList()
            }).ToList();

            return materialDtos;
        }

       
        public async Task<bool> DeleteMatrialAsync(string fileId, string courseId, string materialType)
        {

            if (string.IsNullOrEmpty(fileId))
            {
                return false;   
            }

            if (string.IsNullOrEmpty(courseId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(materialType))
            {
                return false;
            }
            var course = await _courseRepository.GetByIdAsync(courseId);
                var material = await _ModuleRepository.GetModuleByNameAsync(materialType.ToLower());

                if (course == null || material == null)
                {
                    return false; // Course or material doesn't exist
                }
                var success = await _matrialRepository.DeleteMatrialAsync(fileId, courseId, materialType);
                return success;
            
        }
        
    }
}
