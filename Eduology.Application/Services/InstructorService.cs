using Eduology.Application.Interfaces;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eduology.Application.Services
{
    public class AdminService: IInstructorService
    {
        private readonly IInstructorRepository _InstructorRepository;

        public AdminService(IInstructorRepository InstrucorRepository)
        {
            _InstructorRepository = InstrucorRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllInstructorsAsync()
        {
            return await _InstructorRepository.GetAllInstructorsAsync();
        }
        public async Task<UserDto> GetInstructorByIdAsync(string instructorId)
        {
            return await _InstructorRepository.GetInstructorByIdAsync(instructorId);
        }
        public async Task<UserDto> GetInstructorByNameAsync(string instructorName)
        {
            return await _InstructorRepository.GetInstructorByNameAsync(instructorName);
        }
        public async Task<UserDto> GetInstructorByUserNameAsync(string instructorUserName)
        {
            return await _InstructorRepository.GetInstructorByUserNameAsync(instructorUserName);
        }
        public async Task<bool> DeleteInstructorAsync(string instructorId)
        {
            return await _InstructorRepository.DeleteInstructorAsync(instructorId);
        }

    }
}