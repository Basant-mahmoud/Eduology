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
                throw new ArgumentException("Material DTO cannot be null.");
            }
            if (materialDto.CourseId == null)
            {
                throw new ArgumentException("Course ID cannot be null.");
            }
            if (materialDto.InstructorId == null)
            {
                throw new ArgumentException("Instructor ID cannot be null.");
            }
            if (materialDto.FileURLs == null)
            {
                throw new ArgumentException("File URLs cannot be null.");
            }
            if (materialDto.MaterialType == null)
            {
                throw new ArgumentException("Module cannot be null.");
            }
            //instructor should be in course to add matrial
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(materialDto.InstructorId, materialDto.CourseId);
            if (!isInstructorAssigned)
            {
                throw new ArgumentException($"Instructor with ID '{materialDto.InstructorId}' is not assigned to course '{materialDto.CourseId}'.");
            }
            var existingType = await _ModuleRepository.GetTypeByNameAsync(materialDto.MaterialType.ToLower());
            if (existingType == null)
            {
                throw new ArgumentException("Module cannot be null.");
                Console.Error.WriteLine($"Type not exist");
                return false; 
            }
            var course = await _courseRepository.GetByIdAsync(materialDto.CourseId);
            if (course == null)
            {
                return false;
                throw new ArgumentException(" Course Not Esxist be null.");
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
            var success = await _matrialRepository.AddMateriaCourseAsync(material);

            return success;
        }

      
        public async Task<List<MaterialDto>> GetAllMaterialsAsync(string courseId)
        {
            if(courseId == null)
            {
                throw new ArgumentException("Course ID cannot be null or empty.");
            }
            var existing= await _courseRepository.GetByIdAsync(courseId);
            if (existing == null)
            {
                Console.Error.WriteLine($"Type not exist");
                return new List<MaterialDto>();
                throw new ArgumentException("Course Not Exist");
              
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

       
        public async Task<bool> DeleteFileAsync(string fileId, string courseId, string materialType)
        {

            if (string.IsNullOrEmpty(fileId))
            {
                throw new ArgumentException("File ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(courseId))
            {
                throw new ArgumentException("Course ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(materialType))
            {
                throw new ArgumentException("Material type cannot be null or empty.");
            }
            var course = await _courseRepository.GetByIdAsync(courseId);
                var material = await _ModuleRepository.GetTypeByNameAsync(materialType.ToLower());

                if (course == null || material == null)
                {
                    return false; // Course or material doesn't exist
                }
                var success = await _matrialRepository.DeleteFileAsync(fileId, courseId, materialType);
                return success;
            
        }
        
    }
}
