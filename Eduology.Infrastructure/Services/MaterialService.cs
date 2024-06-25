﻿using Eduology.Application.Interface;
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
    
        public async Task<bool> AddMaterialAsync(string instructorId, MaterialDto materialDto)
        {
            if (materialDto == null ||
                string.IsNullOrEmpty(materialDto.CourseId) ||
                string.IsNullOrEmpty(instructorId) ||
                string.IsNullOrEmpty(materialDto.Module) ||
                materialDto.FileURLs == null || materialDto.FileURLs.Count == 0)
            {
                return false;
            }

            // Check if the instructor is assigned to the course
            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(instructorId, materialDto.CourseId);
            if (!isInstructorAssigned)
            {
                return false;
            }

            // Get the module by name within the course 
            var moduleDto = new ModuleDto
            {
                CourseId = materialDto.CourseId,
                Name = materialDto.Module.ToLower(),
            };
            var existingModule = await _moduleRepository.GetModuleByNameAsync(moduleDto);
            if (existingModule == null)
            {
                return false;
            }

            var material = new Material
            {
                InstructorId = instructorId,
                CourseId = materialDto.CourseId,
                ModuleId = existingModule.ModuleId,
                Files = new List<File>()
            };

            foreach (var fileDto in materialDto.FileURLs)
            {
                var fileId = Guid.NewGuid().ToString(); 
                var filePath = "/uploads/"; 

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileDto.File.CopyToAsync(stream);
                }

                // Create File entity
                var file = new File
                {
                    FileId = fileId,
                    Title = fileDto.Title,
                    URL = filePath + fileDto.File.FileName, // Adjust URL as per your file storage
                    MaterialId = material.MaterialId // Ensure MaterialId is set correctly
                };

                material.Files.Add(file);
            }

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
            var isStudentAssigned = await _courseRepository.ISStudentAssignedToCourse(studentid, requestDto.CourseId);
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
        public async Task<bool> DeleteFileAsync(string instructorId, DeleteFileDto deleteFile)
        {
            if (deleteFile == null ||
                string.IsNullOrEmpty(deleteFile.courseId) ||
                string.IsNullOrEmpty(instructorId) ||
                string.IsNullOrEmpty(deleteFile.Module) ||
                string.IsNullOrEmpty(deleteFile.fileId))
            {
                return false;
            }

            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(instructorId, deleteFile.courseId);
            if (!isInstructorAssigned)
            {
                return false;
            }

            var success = await _matrialRepository.DeleteMaterialAsync(deleteFile);
            return success;
        }


    }
}

