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
                throw new Exception("Invalid Instructor. you are not registered in course");
            if (assignment.Deadline <= DateTime.UtcNow)
                throw new Exception("Deadline must be a future date");
            var _assignment = await _asignmentRepository.CreateAsync(assignment, userId);
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(_assignment));
            }
            return new AssignmentDto
            {
                Id = assignment.Id,
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                File = assignment.File,
                fileURLs = new AssignmentFileDto
                {
                    Title = assignment.fileURLs.Title,
                    URL = assignment.fileURLs.URL,
                },
                Title =assignment.Title,
            };
        }

        public async Task<bool> DeleteAsync(int id, string courseId, string userId,string role)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, courseId);
            if (!IsRegistered)
                throw new Exception("Instructor not registered to course");
            var isDeleted = await _asignmentRepository.DeleteAsync(id);

            if (isDeleted)
            {
                return true;
            }
            var file = await GetByIdAsync(id,userId,role);
            var filePath = file.File.URL;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            throw new Exception("Assignment with this id not found");
        }

        public async Task<Assignment> GetByIdAsync(int id, string userId, string role)
        {
            var assignment = await _asignmentRepository.GetByIdAsync(id);
            if (assignment == null)
                throw new Exception("Assignment not available");

            bool isAssigned = await _courseRepository.IsUserAssignedToCourseAsync(userId, assignment.CourseId, role);
            if (!isAssigned)
                throw new Exception("You are not registered in course");

            return assignment;
        }

        public async Task<Assignment> GetByNameAsync(string name, string userId, string role)
        {
            var assignment = await _asignmentRepository.GetByNameAsync(name);
            if (assignment == null)
                throw new Exception("Assignment not available");
            bool isAssigned = await _courseRepository.IsUserAssignedToCourseAsync(userId, assignment.CourseId, role);
            if (!isAssigned)
                throw new Exception("You are not registered in course");

            return assignment;
        }
        public async Task<Assignment> UpdateAsync(int id, UpdateAssignmemtDto assignment, string userId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, assignment.CourseId);
            if (!IsRegistered)
                throw new Exception("You are not registered in course");
            if (assignment.Deadline <= DateTime.UtcNow)
                throw new Exception("Deadline must be a future date");
            var _assignment = await _asignmentRepository.UpdateAsync(id,assignment);
            if (_assignment == null)
                 throw new Exception("Assignment not found");
            if (_instructorService.GetInstructorByIdAsync(userId).ToString() == null)
            {
                throw new ArgumentException("Invalid InstructorId.");
            }
            var course = _courseRepository.GetByIdAsync(assignment.CourseId);
            if (course == null)
            {
                throw new ArgumentException("Invalid CourseId.");
            }
            return _assignment;
        }
    }
}