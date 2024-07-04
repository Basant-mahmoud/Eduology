using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IAsignmentServices _assignmentService;
        private readonly IStudentService _studentService;
        private readonly ICourseRepository _courseRepository;

        public SubmissionService(ISubmissionRepository submissionRepository, IAsignmentServices assignmentService, IStudentService studentService, ICourseRepository courseRepository)
        {
            _submissionRepository = submissionRepository;
            _assignmentService = assignmentService;
            _studentService = studentService;
            _courseRepository = courseRepository;
        }

        public async Task<submissionDetailsDto> CreateAsync(submissionDetailsDto submission, string userId, string role)
        {
            bool IsRegistered = await _courseRepository.ISStudentAssignedToCourse(userId, submission.courseId);
            if (!IsRegistered)
            {
                throw new Exception("You Not Registered In This Course");
            }
            var existingSubmission = await _submissionRepository.GetSubmissionByStudentAndAssignmentAsync(userId, submission.AssignmentId);
            if (existingSubmission != null)
            {
                throw new InvalidOperationException("You have already submitted this assignment.");
            }
            var assignment = await _assignmentService.GetByIdAsync(submission.AssignmentId, userId, role);
            if (assignment == null)
            {
                throw new ArgumentException("Invalid assignment ID.");
            }

            if (assignment.Deadline < DateTime.Now)
            {
                throw new InvalidOperationException("The assignment deadline has passed.");
            }

            return await _submissionRepository.CreateAsync(submission, userId);
        }

        public async Task<bool> DeleteAsync(DeleteSubmissionDto deletesubmission, string userId, string role)
        {
            bool IsRegistered = await _courseRepository.ISStudentAssignedToCourse(userId, deletesubmission.CourseId);
            if (!IsRegistered)
            {
                throw new Exception("Not Vaild Operation");
            }

            var assigment = await _assignmentService.GetByIdAsync(deletesubmission.AssigmentId, userId, role);
            var student = await _studentService.GetStudentByIdAsync(userId);
            if (student == null)
            {
                throw new ArgumentException("student ID not exist .");
            }
            if (assigment == null)
            {
                throw new ArgumentException("Invalid assignment ID.");
            }
            if (assigment.Deadline < DateTime.Now)
            {
                throw new InvalidOperationException("Cant delete submmision.");
            }

            var deleted = await _submissionRepository.DeleteAsync(deletesubmission.AssigmentId, userId);
            if (deleted == null)
            {
                throw new Exception("there is no submission");
            }
            return true;
        }




        public async Task<submissionDetailsDto> GetByIdAsync(int id, string userId, string courseId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, courseId);
            if (!IsRegistered)
            {
                throw new Exception("You Not Registered In This Course");
            }
            var submission = await _submissionRepository.GetByIdAsync(id, courseId);
            if (submission == null)
            {
                throw new KeyNotFoundException("Submission not found.");
            }

            return submission;
        }
        public async Task<List<submissionDetailsDto>> GetAllSubmission(string userId, GetAllSubmisionDto submissionDto)
        {
            if (submissionDto == null || string.IsNullOrEmpty(submissionDto.CourseId) || string.IsNullOrEmpty(userId) || submissionDto.AssignmentId == null)
            {
                return new List<submissionDetailsDto>();
            }

            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(userId, submissionDto.CourseId);
            if (!isInstructorAssigned)
            {
                return new List<submissionDetailsDto>();
            }

            var isCourseExist = await _courseRepository.GetByIdAsync(submissionDto.CourseId);
            if (isCourseExist == null)
            {
                return new List<submissionDetailsDto>();
            }

            return await _submissionRepository.GetSubmissionsByCourseAndAssignmentAsync(submissionDto.CourseId, submissionDto.AssignmentId);
        }
        public async Task<bool> IsThereSubmissionByStudentAndAssignmentAsync(IsSubmissionExistDto submissionExistDto, string userId)
        {
            bool IsRegistered = await _courseRepository.ISStudentAssignedToCourse(userId, submissionExistDto.courseId);
            if (!IsRegistered)
            {
                throw new Exception("You Not Registered In This Course");
            }
            var existingSubmission = await _submissionRepository.GetSubmissionByStudentAndAssignmentAsync(userId, submissionExistDto.AssignmentId);
            if (existingSubmission != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AssignGradeAsync(int submissionId, int grade, string userId, string courseid)
        {
            var submission = await _submissionRepository.GetGradeBySubmissionIdAsync(submissionId);
            if (submission == null)
            {
                throw new KeyNotFoundException("Submission not found.");
            }

            bool isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(userId, courseid);
            if (!isInstructorAssigned)
            {
                throw new UnauthorizedAccessException("You are not authorized to grade this submission.");
            }
            var response = await _submissionRepository.GetByIdAsync(submissionId, courseid);
            if (response == null)
            {
                throw new UnauthorizedAccessException("The submission is not related to an assignment in the specified course.");
            }

            return await _submissionRepository.UpdateGradeAsync(submissionId, grade);
        }
        public async Task<List<SubmissionGradeDto>> GetAllGradesAsync(string studentId)
        {
            var courses = await _courseRepository.GetCoursesByStudentIdAsync(studentId);
            if (courses == null || !courses.Any())
            {
                throw new KeyNotFoundException("No courses found for the specified student.");
            }

            return await _submissionRepository.GetAllGradesByStudentAsync(studentId);
        }


    }
}