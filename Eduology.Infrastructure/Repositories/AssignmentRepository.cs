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
                throw new ArgumentNullException(nameof(assignmentDto));
            }
            var assignment = new Assignment
            {
                Title = assignmentDto.Title,
                CourseId = assignmentDto.CourseId,
                CreatedDate = DateTime.Now,
                Deadline = assignmentDto.Deadline,
                InstructorId = instructorId,
                Description = assignmentDto.Description
            };
            if (assignmentDto.fileURLs == null)
            {
                throw new ArgumentNullException(nameof(assignmentDto.fileURLs), "File information is required.");
            }

            var file = new AssignmentFile
            {
                Title = assignmentDto.fileURLs.Title,
                URL = assignmentDto.fileURLs.URL,
                Assignment = assignment
            };
            assignment.File = file;

            await _context.Assignments.AddAsync(assignment);
            var course = await _courseRepository.GetByIdAsync(assignmentDto.CourseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {assignmentDto.CourseId} not found.");
            }

            course.assignments ??= new List<Assignment>(); 
            course.assignments.Add(assignment);

            await _courseRepository.UpdateAsync(course.CourseId, new CourseDto 
            {
                Name = course.CourseName,
            });
            await _context.SaveChangesAsync();
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
            var _assignment = await _context.Assignments
                                                .Include(a => a.File) 
                                                .FirstOrDefaultAsync(a => a.AssignmentId == id); if (_assignment == null)
                throw new KeyNotFoundException($"Assignment with Id {id} not found.");
            _assignment.Description = assignment.Description;
            if (_assignment.File != null)
            {
                _assignment.File.Title = assignment.fileURLs.Title;
                _assignment.File.URL = assignment.fileURLs.URL;
            }
            else
            {
                _assignment.File = new AssignmentFile
                {
                    Title = assignment.fileURLs.Title,
                    URL = assignment.fileURLs.URL,
                };
            }

            _assignment.Deadline = assignment.Deadline;
            _assignment.CourseId = assignment.CourseId;
             _context.Assignments.Update(_assignment);
            await _context.SaveChangesAsync();

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
                Title = a.Title,
                CourseId = a.CourseId,
                Deadline = a.Deadline,
                Description = a.Description,
                fileURLs = a.File != null ? new AssignmentFileDto
                {
                    Title = a.File.Title,
                    URL = a.File.URL
                } : null
            }).ToList();
        }

    }
}
