using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly EduologyDBContext _context;
        private readonly ICourseRepository _courseRepository;
        public AssignmentRepository(EduologyDBContext context,ICourseRepository courseRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
        }
        public async Task<AssignmentDto> CreateAsync(AssignmentDto assignmentDto,string instructorId)
        {
            if (assignmentDto == null)
            {
                return null;
            }
            var assignment = new Assignment
            {
                Title = assignmentDto.Title,
                CourseId = assignmentDto.CourseId,
                CreatedDate = DateTime.Now,
                Deadline = assignmentDto.Deadline,
                InstructorId = instructorId,
                Description = assignmentDto.Description,
            };
            if (assignmentDto.fileURLs == null)
            {
                return null;
            }

            var file = new AssignmentFile
            {
                Title = assignmentDto.fileURLs.Title,
                URL = assignmentDto.fileURLs.URL,
                Assignment = assignment
            };
            assignment.File = file;

            await _context.Assignments.AddAsync(assignment);
            var course = await _context.Courses.FindAsync(assignmentDto.CourseId);
            course.Assignments.Add(assignment);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            assignmentDto.Id = assignment.AssignmentId;
            return assignmentDto;
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
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Title == name);
            if (assignment == null)
                return null;
            return assignment;
        }
        public async Task<Assignment> UpdateAsync(int id, UpdateAssignmemtDto assignment)
        {
            var _assignment = await _context.Assignments
                                                .Include(a => a.File) 
                                                .FirstOrDefaultAsync(a => a.AssignmentId == id);
            if (_assignment == null)
                return null;
            _assignment.Description = assignment.Description;
            // deadline 
            _assignment.Deadline = assignment.Deadline;
            _assignment.Title = assignment.Title;
             _context.Assignments.Update(_assignment);
            await _context.SaveChangesAsync();

            return _assignment;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(e => e.AssignmentId == id);
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
