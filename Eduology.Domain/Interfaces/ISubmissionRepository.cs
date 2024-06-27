using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface ISubmissionRepository
    {
        public Task<submissionDetailsDto> CreateAsync(submissionDetailsDto submission,string userId);
        public Task<submissionDetailsDto> GetByIdAsync(int id, string _courseId);
        public Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deletesubmision);
        Task<List<submissionDetailsDto>> GetSubmissionsByCourseAndAssignmentAsync(string courseId, int assignmentId);

    }
}
