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
        public Task<SubmissionDto> CreateAsync(SubmissionDto submission,string userId);
        public Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deletesubmision);
        public Task<SubmissionDto> GetByIdAsync(int id,string _courseId);
        Task<List<SubmissionDto>> GetSubmissionsByCourseAndAssignmentAsync(string courseId, int assignmentId);

    }
}
