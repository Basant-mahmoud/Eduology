using Eduology.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using Eduology.Domain.DTO;
using Eduology.Application.Interface;
using Microsoft.EntityFrameworkCore;
namespace Eduology.Infrastructure.Services
{
    public class AssignmentServices : IAsignmentServices
    {
        private readonly IAssignmentRepository _asignmentRepository;
        private readonly IInstructorService _instructorService;
        private readonly ICourseService _courseService; 
        public AssignmentServices(IAssignmentRepository asignmentRepository,IInstructorService instructorService,ICourseService courseService)
        {
            _asignmentRepository = asignmentRepository;
            _instructorService = instructorService;
            _courseService = courseService;
        }

        public async Task<AssignmentDto> CreateAsync(AssignmentDto assignment)
        {
            var _assignment = await _asignmentRepository.CreateAsync(assignment);
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(_assignment));
            }
            if (_instructorService.GetInstructorByIdAsync(assignment.InstructorId).ToString() == null)
            {
                throw new ArgumentException("Invalid InstructorId.");
            }
            return new AssignmentDto
            { 
                Title = assignment.Title,
                AssignmentFile = assignment.AssignmentFile,
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                Id = assignment.Id,
                InstructorId = assignment.InstructorId,
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = await _asignmentRepository.DeleteAsync(id);

            if (isDeleted)
            {
                Console.WriteLine($"Assignment with ID {id} deleted successfully.");
                return true;
            }
            return false;
        }

        public async Task<Assignment> GetByIdAsync(int id)
        {
            var _assignment = await _asignmentRepository.GetByIdAsync(id);
            if (_assignment == null)
                return null;
            return _assignment;
        }

        public async Task<Assignment> GetByNameAsync(string name)
        {
            var _assignment = await _asignmentRepository.GetByNameAsync(name);
            if (_assignment == null)
                return null;
            return _assignment;
        }

        public async Task<Assignment> UpdateAsync(int id, AssignmentDto assignment)
        {
            var _assignment = await _asignmentRepository.UpdateAsync(id,assignment);
            if (_assignment == null)
                return null;
            if (_instructorService.GetInstructorByIdAsync(assignment.InstructorId).ToString() == null)
            {
                throw new ArgumentException("Invalid InstructorId.");
            }
            var course =  _courseService.GetByIdAsync(assignment.CourseId,assignment.InstructorId);
            if (course == null)
            {
                throw new ArgumentException("Invalid CourseId.");
            }
            return _assignment;
        }
        public async Task<List<AssignmentDto>> GetAllAsync()
        {
            var assignments =  await _asignmentRepository.GetAllAsync();
            return assignments;
        }

    }
}
