using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Eduology.Domain.Models.Type;

namespace Eduology.Infrastructure.Repositories
{
    public class ModuleRepository: IModuleRepository
    {
        //////////
        private readonly EduologyDBContext _context;
        public ModuleRepository(EduologyDBContext context)
        {
            _context = context;
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
        //////////
        public async Task<bool> DeleteModuleAsync(string materialType)
        {
            var module = await _context.MaterialTypes
                .FirstOrDefaultAsync(mt => mt.Name.ToLower() == materialType.ToLower());

            if (module == null)
            {
                return false;
            }

            _context.MaterialTypes.Remove(module);
            await _context.SaveChangesAsync();
            return true;
        }
        /////////
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

        public Task<(bool Success, bool Exists, System.Type Type)> AddTypeAsync(System.Type type)
        {
            throw new NotImplementedException();
        }

      
    }
}
