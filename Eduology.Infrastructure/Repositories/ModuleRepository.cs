using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module = Eduology.Domain.Models.Module;

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
        public async Task<bool> AddModuleAsync(Module module)
        {
            var existingModule = await _context.Modules
                .FirstOrDefaultAsync(m => m.Name.ToLower() == module.Name.ToLower() && m.courseId == module.courseId);

            if (existingModule != null)
            {
                return false;
            }

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<Module> GetModuleByNameAsync(ModuleDto module)
        {
            return await _context.Modules.FirstOrDefaultAsync(t => t.Name.ToLower() == module.Name.ToLower()&&t.courseId==module.CourseId);
        }

        public async Task<bool> DeleteModuleAsync(ModuleDto module)
        {
            var DeltedModule = await _context.Modules
                .FirstOrDefaultAsync(m => m.Name.ToLower() == module.Name.ToLower() && m.courseId == module.CourseId);

            if (module == null)
            {
                return false;
            }
            if (DeltedModule == null)
            {
                return false; 
            }

            _context.Modules.Remove(DeltedModule);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateModuleAsync(UpdateModuleDto module)
        {
            var existingModule = await _context.Modules
                 .FirstOrDefaultAsync(m => m.Name.ToLower() == module.Name.ToLower() && m.courseId == module.CourseId);

            if (existingModule == null)
            {
                return false; // Module not found
            }

            existingModule.Name = module.NewName.ToLower(); // Update other properties as needed

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public async Task<List<Module>> GetModulesByCourseIdAsync(string courseId)
        {
            return await _context.Modules
                .Where(m => m.courseId == courseId)
                .Include(m => m.Materials) 
                .ThenInclude(mat => mat.Files) // Include related files in materials
                .ToListAsync();
        }

    }
 }

