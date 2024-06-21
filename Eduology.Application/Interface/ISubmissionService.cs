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
        public Task<SubmissionDto> CreateAsync(SubmissionDto submission);
        public Task<bool> DeleteAsync(int id);
        public Task<SubmissionDto> GetByIdAsync(int id);
    }
}
