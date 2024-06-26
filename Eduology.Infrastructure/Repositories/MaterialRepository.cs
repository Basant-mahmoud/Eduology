 using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Module;
using Microsoft.EntityFrameworkCore;
using Eduology.Domain.DTO;
using System.Reflection;
using File = Eduology.Domain.Models.File;
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
            try
            {
                var course = await _context.Courses.FindAsync(material.CourseId);
                if (course == null)
                {
                    return false;
                }

                var module = await _context.Modules.FindAsync(material.ModuleId);
                if (module == null || module.courseId != material.CourseId)
                {
                    return false;
                }

                _context.Materials.Add(material);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<File> GetFileByIdAsync(string fileId)
        {
            return await _context.Files
                .Include(f => f.Material)
                .FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
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


