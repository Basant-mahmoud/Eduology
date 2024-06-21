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
         public async Task<bool> AddMaterialAsync(Material material)
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

        public async Task<List<Material>> GetAllMaterialsAsync(string courseId)
        {
            return await _context.Materials
                .Include(m => m.MaterialType)
                .Include(m => m.Files)
                .Where(m => m.CourseId == courseId)
                .ToListAsync();
        }

       
        public async Task<bool> DeleteMatrialAsync(string fileId, string courseId, string materialType)
        {
            var file = await _context.Files
                .Include(f => f.Material)
                .ThenInclude(m => m.MaterialType)
                .FirstOrDefaultAsync(f => f.FileId == fileId && f.Material.CourseId == courseId && f.Material.MaterialType.Name.ToLower() == materialType.ToLower());

            if (file == null)
            {
                return false; 
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }
        

    }
    }


