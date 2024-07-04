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
        public Task<submissionDetailsDto> CreateAsync(submissionDetailsDto submission,string userId,string role);
        public Task<submissionDetailsDto> GetByIdAsync(int id, string userId, string coursId);
        Task<bool> DeleteAsync(DeleteSubmissionDto deletesubmission, string userId,string role);
        Task<List<submissionDetailsDto>> GetAllSubmission(string userId, GetAllSubmisionDto SubmissionDto);
        public Task<bool> IsThereSubmissionByStudentAndAssignmentAsync(IsSubmissionExistDto submissionExistDto,string userId);
        public Task<bool> AssignGradeAsync(int submissionId, int grade, string userId, string courseid);
        public Task<List<SubmissionGradeDto>> GetAllGradesAsync(string studentId);


    }
}
