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
        private readonly ICourseService _courseService;
        public AssignmentRepository(EduologyDBContext context,ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }
        public async Task<AssignmentDto> CreateAsync(AssignmentDto assignmentDto)
        {
            if (assignmentDto == null)
            {
                throw new ArgumentNullException(nameof(assignmentDto));
            }
            var assignment = new Assignment
            {
                Title = assignmentDto.Title,
                CourseId = assignmentDto.CourseId,
                CreatedDate = DateTime.Now,
                InstructorId = assignmentDto.InstructorId,
                Deadline = assignmentDto.Deadline,
                Description = assignmentDto.Description
            };
            if (assignmentDto.AssignmentFile == null)
            {
                throw new ArgumentNullException(nameof(assignmentDto.AssignmentFile), "File information is required.");
            }

            var file = new AssignmentFile
            {
                Title = assignmentDto.AssignmentFile.Title,
                URL = assignmentDto.AssignmentFile.URL,
                Assignment = assignment
            };
            assignment.File = file;

            await _context.Assignments.AddAsync(assignment);
            var course = await _courseService.GetByIdAsync(assignmentDto.CourseId,assignment.InstructorId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {assignmentDto.CourseId} not found.");
            }

            course.assignments ??= new List<Assignment>(); 
            course.assignments.Add(assignment);

            await _courseService.UpdateAsync(course.CourseId, new CourseDto 
            {
                Name = course.Name,
            });
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
        public async Task<Assignment> UpdateAsync(int id, AssignmentDto assignment)
        {
            var _assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
                return null;
            _assignment.InstructorId = assignment.InstructorId;
            _assignment.Description = assignment.Description;
            _assignment.File = new AssignmentFile
            {
                Title = assignment.AssignmentFile.Title,
                URL = assignment.AssignmentFile.URL,
            };
            _assignment.Deadline = assignment.Deadline;
            _assignment.CourseId = assignment.CourseId;
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
        public async Task<List<AssignmentDto>> GetAllAsync()
        {
            var assignments = await _context.Assignments
                .ToListAsync();

            return assignments.Select(a => new AssignmentDto
            {
                Id = a.AssignmentId,
                Title = a.Title,
                CourseId = a.CourseId,
                Deadline = a.Deadline,
                Description = a.Description,
                InstructorId = a.InstructorId,
                AssignmentFile = a.File != null ? new AssignmentFileDto
                {
                    Title = a.File.Title,
                    URL = a.File.URL
                } : null
            }).ToList();
        }

    }
}
