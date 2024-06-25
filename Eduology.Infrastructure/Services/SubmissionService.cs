using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
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

        public SubmissionService(ISubmissionRepository submissionRepository, IAsignmentServices assignmentService,IStudentService studentService, ICourseRepository courseRepository)
        {
            _submissionRepository = submissionRepository;
            _assignmentService = assignmentService;
            _studentService = studentService;
            _courseRepository = courseRepository;
        }

        public async Task<SubmissionDto> CreateAsync(SubmissionDto submission,string userId,string role)
        {
            bool IsRegistered =  await _courseRepository.ISStudentAssignedToCourse(userId,submission.courseId);
            if (!IsRegistered)
            {
                throw new Exception("You Not Registered In This Course");
            }
            var assignment = await _assignmentService.GetByIdAsync(submission.AssignmentId,userId,role);
            if (assignment == null)
            {
                throw new ArgumentException("Invalid assignment ID.");
            }

            if (assignment.Deadline < DateTime.Now)
            {
                throw new InvalidOperationException("The assignment deadline has passed.");
            }

            if (_studentService.GetStudentByIdAsync(userId) == null)
            {
                throw new ArgumentException("Invalid student ID.");
            }
            return await _submissionRepository.CreateAsync(submission,userId);
        }
        public async Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deletesubmission,string userId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, deletesubmission.CourseId);
            if (!IsRegistered)
            {
                throw new Exception("Not Vaild Operation");
            }
            var assigment = await _assignmentService.GetByIdAsync(deletesubmission.AssigmentId,userId, deletesubmission.CourseId);
            var submission= await _submissionRepository.GetByIdAsync(deletesubmission.SubmissionId);
            var student = await _studentService.GetStudentByIdAsync(userId);
            if (student == null)
            {
                throw new ArgumentException("student ID not exist .");
            }
            if (assigment == null)
            {
                throw new ArgumentException("Invalid assignment ID.");

            }
            if (submission == null)
            {
                throw new ArgumentException("Invalid Submmision ID.");

            }
            if (assigment.Deadline < DateTime.Now)
            {
                throw new InvalidOperationException("Cant delete submmision.");
            }
            if (_studentService.GetStudentByIdAsync(userId) == null)
            {
                throw new ArgumentException("Invalid student ID.");
            }
          

            return await _submissionRepository.DeleteAsync(deletesubmission);

        }

      

        public async Task<SubmissionDto> GetByIdAsync(int id, string userId,string courseId)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(userId, courseId);
            if (!IsRegistered)
            {
                throw new Exception("You Not Registered In This Course");
            }
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
            {
                throw new KeyNotFoundException("Submission not found.");
            }

            return submission;
        }
        public async Task<List<SubmissionDto>> GetAllSubmission(string userId, GetAllSubmisionDto submissionDto)
        {
            if (submissionDto == null || string.IsNullOrEmpty(submissionDto.CourseId) || string.IsNullOrEmpty(userId) || submissionDto.AssignmentId == null )
            {
                return new List<SubmissionDto>();
            }

            var isInstructorAssigned = await _courseRepository.IsInstructorAssignedToCourse(userId, submissionDto.CourseId);
            if (!isInstructorAssigned)
            {
                return new List<SubmissionDto>();
            }

            var isCourseExist = await _courseRepository.GetByIdAsync(submissionDto.CourseId);
            if (isCourseExist == null)
            {
                return new List<SubmissionDto>();
            }

            return await _submissionRepository.GetSubmissionsByCourseAndAssignmentAsync(submissionDto.CourseId, submissionDto.AssignmentId);
        }

    }
}
