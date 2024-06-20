using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Type;
using Microsoft.EntityFrameworkCore;
using Eduology.Domain.DTO;
namespace Eduology.Infrastructure.Repositories
{
    public class MaterialRepository: IMaterialRepository
    {
        private readonly EduologyDBContext _context;
        public MaterialRepository(EduologyDBContext context)
        {
            _context = context;
        }
         public async Task<bool> AddMateriaCourseAsync(Material material)
        {
            var course = await _context.Courses.FindAsync(material.CourseId);
            if (course == null)
            {
                Console.Error.WriteLine($"course repo  not exist");
                return false; 
            }

            course.Materials ??= new List<Material>();
            course.Materials.Add(material);

            if (material.Files != null && material.Files.Count > 0)
            {
                foreach (var file in material.Files)
                {
                    _context.Files.Add(file);
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Success, bool Exists, Type Type)> AddTypeAsync(Type type)
        {
            var existingType = await _context.MaterialTypes
                .FirstOrDefaultAsync(t => t.Name.ToLower() == type.Name.ToLower());

            if (existingType != null)
            {
                return (false, true, null);
            }

            _context.MaterialTypes.Add(type);
            await _context.SaveChangesAsync();
            return (true, false, type);
        }
        public async Task<Type> GetTypeByNameAsync(string typeName)
        {
            return await _context.MaterialTypes
                .FirstOrDefaultAsync(t => t.Name.ToLower() == typeName.ToLower());
        }
        public async Task<List<Material>> GetAllMaterialsAsync(string courseId)
        {
            return await _context.Materials
                .Include(m => m.MaterialType)
                .Include(m => m.Files)
                .Where(m => m.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<List<ModuleWithFilesDto>> ModuleTypesWithFilesAsync(string courseId)
        {
            var typesWithFiles = await _context.Materials
                .Where(m => m.CourseId == courseId)
                .Include(m => m.MaterialType)
                .Select(m => new ModuleWithFilesDto
                {
                    TypeName = m.MaterialType.Name,
                    Files = m.Files.Select(f => new FileDtoWithId
                    {
                        FileId = f.FileId,
                        URL = f.URL,
                        Title = f.Title
                    }).ToList()
                })
                .ToListAsync();

            return typesWithFiles;
        }
    }
    }


