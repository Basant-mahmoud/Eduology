using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class MaterialService: IMaterialService
    {
        private readonly IMaterialRepository _matrialRepository;


        public MaterialService(IMaterialRepository matrialRepository)
        {
            _matrialRepository = matrialRepository;
        }
        public async Task<bool> AddMaterialAsync(MaterialDto MaterialDto)
        {
            var material = new Material
            {
                Title = MaterialDto.Title,
                //URL = MaterialDto.URL,
                InstructorId = MaterialDto.InstructorId,
                CourseId = MaterialDto.CourseId,
                MaterialType = new Domain.Models.Type { Name = MaterialDto.MaterialType }
            };

            // Add files to the material if provided
            if (MaterialDto.FileURLs != null && MaterialDto.FileURLs.Count > 0)
            {
                // material.Files = new List<Domain.Models.File>();
                foreach (var fileUrl in MaterialDto.FileURLs)
                {
                    var file = new Domain.Models.File
                    {
                        FileId = Guid.NewGuid().ToString(),
                        URL = fileUrl,
                        Title = $"File for {MaterialDto.Title}", // Example: Setting a title for the file
                        MaterialId = material.MaterialId
                    };
                    // material.Files.Add(file);
                }
            }

            // Call repository method to add material
            var success = await _matrialRepository.AddMateriaCourseAsync(material);

            return success;
        }
    }
}
