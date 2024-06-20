using Eduology.Application.Interface;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Eduology.Domain.Models.File;

namespace Eduology.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly EduologyDBContext _context;
        private readonly ICourseService courseService;
        public AssignmentRepository(EduologyDBContext context)
        {
            _context = context;
        }
        public async Task<Assignment> CreateAsync(Assignment assignment)
        {
            var _assignment = new Assignment
            {
                CourseId = assignment.CourseId,
                CreatedDate = DateTime.Now,
                InstructorId = assignment.InstructorId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                
            };
            var __assignment = await _context.Assignments.AddAsync(_assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }
        public async Task<Assignment> GetByIdAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
                return null;
            return assignment;
        }
        public async Task<Assignment> GetByNameAsync(String name)
        {
            var assignment = await _context.Assignments.FindAsync(name);
            if (assignment == null)
                return null;
            return assignment;
        }
        public async Task<Assignment> UpdateAsync(int id,Assignment assignment)
        {
            var _assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
                return null;
            _assignment.InstructorId = assignment.InstructorId;
            _assignment.Instructor = assignment.Instructor;
            _assignment.Submissions = assignment.Submissions;
            _assignment.CreatedDate = assignment.CreatedDate;
            _assignment.Description = assignment.Description;
            _assignment.File =  assignment.File;
            _assignment.Deadline = assignment.Deadline;
            _assignment.AssignmentId = assignment.AssignmentId;
            _assignment.CourseId = assignment.CourseId;
            _assignment.Course = assignment.Course;
            return _assignment;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return false;
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
