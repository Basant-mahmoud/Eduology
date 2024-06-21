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
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
