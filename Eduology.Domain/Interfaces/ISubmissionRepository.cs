using Eduology.Domain.DTO;
using Eduology.Domain.Models;
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
        public Task<Submission> DeleteAsync(int assignmentId,string studentId);
        Task<List<submissionDetailsDto>> GetSubmissionsByCourseAndAssignmentAsync(string courseId, int assignmentId);
        public Task<Submission> GetSubmissionByStudentAndAssignmentAsync(string studentId, int assignmentId);
        public Task<bool> UpdateGradeAsync(int submissionId, int grade);
        public Task<int?> GetGradeBySubmissionIdAsync(int submissionId);
        Task<List<SubmissionGradeDto>> GetAllGradesByStudentAsync(string studentId);


    }
}
