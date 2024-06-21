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
        public SubmissionService(ISubmissionRepository submissionRepository, IAsignmentServices assignmentService,IStudentService studentService)
        {
            _submissionRepository = submissionRepository;
            _assignmentService = assignmentService;
            _studentService = studentService;
        }

        public async Task<SubmissionDto> CreateAsync(SubmissionDto submission)
        {
            var assignment = await _assignmentService.GetByIdAsync(submission.AssignmentId);
            if (assignment == null)
            {
                throw new ArgumentException("Invalid assignment ID.");
            }

            if (assignment.Deadline < DateTime.Now)
            {
                throw new InvalidOperationException("The assignment deadline has passed.");
            }

            if (_studentService.GetStudentByIdAsync(submission.StudentId) == null)
            {
                throw new ArgumentException("Invalid student ID.");
            }
            return await _submissionRepository.CreateAsync(submission);
        }
        /// basant 
        public async Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deletesubmission)
        {
            var assigment = await _assignmentService.GetByIdAsync(deletesubmission.AssigmentId);
            var submission= await _submissionRepository.GetByIdAsync(deletesubmission.SubmissionId);
            var student = await _studentService.GetStudentByIdAsync(deletesubmission.StudentId);
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
            if (_studentService.GetStudentByIdAsync(deletesubmission.StudentId) == null)
            {
                throw new ArgumentException("Invalid student ID.");
            }
          

            return await _submissionRepository.DeleteAsync(deletesubmission);

        }

        public async Task<SubmissionDto> GetByIdAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
            {
                throw new KeyNotFoundException("Submission not found.");
            }

            return submission;
        }
    }
}
