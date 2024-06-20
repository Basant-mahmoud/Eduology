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

        public MaterialService(IMaterialRepository matrialRepository, ICourseRepository courseRepository)
        {
            _matrialRepository = matrialRepository;
            _courseRepository = courseRepository;
        }
        public async Task<bool> AddMaterialAsync(MaterialDto materialDto)
        {
            // Check if the material type exists
            var existingType = await _matrialRepository.GetTypeByNameAsync(materialDto.MaterialType.ToLower());
            if (existingType == null)
            {
                Console.Error.WriteLine($"Type not exist");
                return false; 
            }

            // Check if the course exists
            var course = await _courseRepository.GetByIdAsync(materialDto.CourseId);
            if (course == null)
            {
                Console.Error.WriteLine($"course not exist");
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

            // Add files to the material if provided
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

            // Call repository method to add material
            var success = await _matrialRepository.AddMateriaCourseAsync(material);

            return success;
        }

        public async Task<(bool Success, bool Exists, Domain.Models.Type Type)> AddTypeAsync(MaterialType materialType)
        {
            var type = new Domain.Models.Type
            {
                Name = materialType.Name.ToLower(),
            };

            var (success, exists, createdType) = await _matrialRepository.AddTypeAsync(type);
            return (success, exists, createdType);
        }
        public async Task<List<MaterialDto>> GetAllMaterialsAsync(string courseId)
        {
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
                MaterialType = m.MaterialType?.Name, // Assuming MaterialType is nullable
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

        public async Task<List<ModuleWithFilesDto>> GetModulesWithFilesAsync(string courseId)
        {
            var courseExists = await _courseRepository.GetByIdAsync(courseId);
            if (courseExists == null)
            {
                return new List<ModuleWithFilesDto>();
            }

            var typesWithFiles = await _matrialRepository.ModuleTypesWithFilesAsync(courseId);
            return typesWithFiles;
        }
    }
}
