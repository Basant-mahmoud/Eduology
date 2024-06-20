using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
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


        public MaterialService(IMaterialRepository matrialRepository)
        {
            _matrialRepository = matrialRepository;
        }
        public async Task<bool> AddMaterialAsync(MaterialDto materialDto)
        {
            var material = new Material
            {
                Title = materialDto.Title,
                InstructorId = materialDto.InstructorId,
                CourseId = materialDto.CourseId,
                MaterialType = new Type { Name = materialDto.MaterialType },
                Files = new List<File>() // Initialize list of files
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
    }
}
