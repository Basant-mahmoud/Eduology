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
        public async Task<submissionDetailsDto> CreateAsync(submissionDetailsDto submission, string userId)
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
        public async Task<Submission> DeleteAsync(int assignmentId, string studentId)
        {
            var submission = await Context.submissions
                                          .FirstOrDefaultAsync(e => e.AssignmentId == assignmentId && e.StudentId == studentId);
            if (submission == null)
                return null;

            Context.submissions.Remove(submission);
            await Context.SaveChangesAsync();

            return submission;
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
                grade = s.Grade,
            }).ToList();
        }
        public async Task<Submission> GetSubmissionByStudentAndAssignmentAsync(string studentId, int assignmentId)
        {
            var submission = await Context.submissions
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.AssignmentId == assignmentId);
            if (submission == null) return null;

            return submission;
        }
        public async Task<bool> UpdateGradeAsync(int submissionId, int grade)
        {
            var submission = await Context.submissions.FirstOrDefaultAsync(s => s.SubmissionId == submissionId);
            if (submission == null) return false;

            submission.Grade = grade;
            Context.submissions.Update(submission);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> GetGradeBySubmissionIdAsync(int submissionId)
        {
            var submission = await Context.submissions.FirstOrDefaultAsync(s => s.SubmissionId == submissionId);
            return submission?.Grade;
        }
        public async Task<List<SubmissionGradeDto>> GetAllGradesByStudentAsync(string studentId)
        {
            var grades = await Context.submissions
                .Where(s => s.StudentId == studentId)
                .Select(s => new SubmissionGradeDto
                {
                    CourseId = s.Assignment.CourseId,
                    AssignmentId = s.AssignmentId,
                    Grade = s.Grade,
                    CourseName = s.Assignment.Course.Name,
                    Title = s.Title,
                    URL = s.URL,
                })
                .ToListAsync();

            return grades;
        }

    }
}