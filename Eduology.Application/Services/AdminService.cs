using Eduology.Application.Interfaces;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eduology.Application.Services
{
    public class AdminService: IAdminService
    {
        private readonly IAdminRepository _AdminRepository;

        public AdminService(IAdminRepository AdminRepository)
        {
            _AdminRepository = AdminRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllInstructorsAsync()
        {
            return await _AdminRepository.GetAllInstructorsAsync();
        }
        public async Task<UserDto> GetInstructorByIdAsync(string instructorId)
        {
            return await _AdminRepository.GetInstructorByIdAsync(instructorId);
        }
        public async Task<UserDto> GetInstructorByNameAsync(string instructorName)
        {
            return await _AdminRepository.GetInstructorByNameAsync(instructorName);
        }
        public async Task<UserDto> GetInstructorByUserNameAsync(string instructorUserName)
        {
            return await _AdminRepository.GetInstructorByUserNameAsync(instructorUserName);
        }
        public async Task<bool> DeleteInstructorAsync(string instructorId)
        {
            return await _AdminRepository.DeleteInstructorAsync(instructorId);
        }

    }
}