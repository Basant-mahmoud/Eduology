using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly EduologyDBContext Context;
        private readonly IAssignmentRepository assignmentRepository;
        public SubmissionRepository(EduologyDBContext eduologyDBContext, IAssignmentRepository _assignmentRepository)
        {
            Context = eduologyDBContext;
            assignmentRepository = _assignmentRepository;
        }
        public async Task<submissionDetailsDto> CreateAsync(submissionDetailsDto submission,string userId)
        {
            Submission _submission = new Submission
            {
                AssignmentId = submission.AssignmentId,
                StudentId = userId,
                TimeStamp = DateTime.Now,
                Assignment = await assignmentRepository.GetByIdAsync(submission.AssignmentId),
                Title = submission.Title,
                URL = submission.URL,
            };
            var __submission = await Context.submissions.AddAsync(_submission);
            var assignnment = await assignmentRepository.GetByIdAsync(_submission.AssignmentId);
            assignnment.Submissions.Add(_submission);
            Context.Assignments.Update(assignnment);
            await Context.SaveChangesAsync();
            submission.SubmissionId = _submission.SubmissionId;
            return submission;
        }

        public async Task<submissionDetailsDto> GetByIdAsync(int id, string _courseId)
        {
            var submission = await Context.submissions.FirstOrDefaultAsync(e => e.SubmissionId == id);
            if (submission == null)
                return null;
            return new submissionDetailsDto
            {
                AssignmentId = submission.AssignmentId,
                courseId = _courseId,
                SubmissionId = id,
                TimeStamp = submission.TimeStamp,
                Title = submission.Title,
                URL = submission.URL 
            };
        }
        public async Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deleteSubmissionDto)
        {

            var submission = await Context.submissions.FirstOrDefaultAsync(e => e.SubmissionId == deleteSubmissionDto.SubmissionId);
            if (submission == null)
                return null;

            Context.submissions.Remove(submission);
            await Context.SaveChangesAsync();

            return deleteSubmissionDto;
        }

        public async Task<List<submissionDetailsDto>> GetSubmissionsByCourseAndAssignmentAsync(string courseId, int assignmentId)
        {
            var submissions = await Context.submissions
                .Where(s => s.Assignment.CourseId == courseId && s.AssignmentId == assignmentId)
                .ToListAsync();

            return submissions.Select(s => new submissionDetailsDto
            {
                courseId = courseId,
                AssignmentId = s.AssignmentId,
                URL = s.URL,
                SubmissionId = s.SubmissionId,
                TimeStamp = s.TimeStamp,
                Title = s.Title,
            }).ToList();
        }
    }
}
