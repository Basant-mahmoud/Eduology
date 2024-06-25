using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface ISubmissionService
    {
        public Task<SubmissionDto> CreateAsync(SubmissionDto submission,string userId,string role);
        Task<DeleteSubmissionDto> DeleteAsync(DeleteSubmissionDto deletesubmission, string userId,string role);
        public Task<SubmissionDto> GetByIdAsync(int id,string userId,string role);
        Task<List<SubmissionDto>> GetAllSubmission(string userId, GetAllSubmisionDto SubmissionDto);

    }
}
