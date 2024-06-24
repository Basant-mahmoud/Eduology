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
using System.Data;
namespace Eduology.Infrastructure.Services
{
    public class AssignmentServices : IAsignmentServices
    {
        private readonly IAssignmentRepository _asignmentRepository;
        private readonly IInstructorService _instructorService;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseService _courseService;
        public AssignmentServices(IAssignmentRepository asignmentRepository, IInstructorService instructorService, ICourseRepository courseRepository, ICourseService courseService)
        {
            _asignmentRepository = asignmentRepository;
            _instructorService = instructorService;
            _courseService = courseService;
            _courseRepository = courseRepository;
        }

        public async Task<AssignmentDto> CreateAsync(AssignmentDto assignment, string userId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, assignment.CourseId);
            if (!IsRegistered)
                throw new Exception("Not Vaild");
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

        public async Task<bool> DeleteAsync(int id, string courseId, string userId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, courseId);
            if (!IsRegistered)
                return false;
            var isDeleted = await _asignmentRepository.DeleteAsync(id);

            if (isDeleted)
            {
                Console.WriteLine($"Assignment with ID {id} deleted successfully.");
                return true;
            }
            return false;
        }

        public async Task<Assignment> GetByIdAsync(int id, string userId,string role)
        {
            var assignment = await _asignmentRepository.GetByIdAsync(id);
            if (assignment == null)
                return null;

            bool isAssigned = await _courseRepository.IsUserAssignedToCourseAsync(userId, assignment.CourseId, role);
            if (!isAssigned)
                return null;

            return assignment;
        }

        public async Task<Assignment> GetByNameAsync(string name, string userId,string role)
        {
            var assignment = await _asignmentRepository.GetByNameAsync(name);
            if (assignment == null)
                return null;

            bool isAssigned = await _courseRepository.IsUserAssignedToCourseAsync(userId, assignment.CourseId, role);
            if (!isAssigned)
                return null;

            return assignment;
        }
        public async Task<bool> UpdateAsync(int id, AssignmentDto assignment, string userId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(assignment.InstructorId, assignment.CourseId);
            if (!IsRegistered)
                return false;
            var _assignment = await _asignmentRepository.UpdateAsync(id, assignment);
            if (_assignment == null)
                return false;
            if (_instructorService.GetInstructorByIdAsync(assignment.InstructorId).ToString() == null)
            {
                throw new ArgumentException("Invalid InstructorId.");
            }
            var course = _courseRepository.GetByIdAsync(assignment.CourseId);
            if (course == null)
            {
                throw new ArgumentException("Invalid CourseId.");
            }
            return true;
        }
        public async Task<List<AssignmentDto>> GetAllAsync(string userId,string role)
        {
            var assignments = await _asignmentRepository.GetAllAsync();
            var assignedAssignments = new List<AssignmentDto>();

            foreach (var assignment in assignments)
            {
                bool isAssigned = await _courseRepository.IsUserAssignedToCourseAsync(userId, assignment.CourseId, role);
                if (isAssigned)
                {
                    assignedAssignments.Add(assignment);
                }
            }

            return assignedAssignments;
        }
    }
}