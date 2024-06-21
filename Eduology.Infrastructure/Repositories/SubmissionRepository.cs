using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
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
        private readonly IAsignmentServices _assignmentservice;
        public SubmissionRepository(EduologyDBContext eduologyDBContext, IAsignmentServices asignmentServices)
        {
            Context = eduologyDBContext;
            _assignmentservice = asignmentServices;
        }
        public async Task<SubmissionDto> CreateAsync(SubmissionDto submission)
        {
            Submission _submission = new Submission
            {
                AssignmentId = submission.AssignmentId,
                StudentId = submission.StudentId,
                TimeStamp = DateTime.Now,
                URL = submission.URL,
                Assignment = await _assignmentservice.GetByIdAsync(submission.AssignmentId),
            };
            await Context.submissions.AddAsync(_submission);
            await Context.SaveChangesAsync();
            return new SubmissionDto
            {
                SubmissionId = _submission.SubmissionId,
                StudentId = _submission.StudentId,
                AssignmentId = submission.AssignmentId,
                URL = submission.URL,
            };
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SubmissionDto> GetByIdAsync(int id)
        {
            var submission = await Context.submissions.FindAsync(id);
            if (submission == null)
                return null;
            return new SubmissionDto
            {
                AssignmentId = submission.AssignmentId,
                StudentId = submission.StudentId,
                SubmissionId = submission.SubmissionId,
                URL = submission.URL
            };
        }
    }
}
