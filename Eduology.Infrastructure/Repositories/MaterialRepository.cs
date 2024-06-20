using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return false;

            // Add material to course
            course.Materials ??= new List<Material>();
            course.Materials.Add(material);

            // Add files to the context if any
            if (material.Files != null && material.Files.Count > 0)
            {
                foreach (var file in material.Files)
                {
                    _context.Files.Add(file);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
