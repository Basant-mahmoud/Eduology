using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IOrganizationService
    {
        Task<List<OrganizationDetailsDto>> GetAllOrganizationsAsync();
        Task<OrganizationDetailsDto> GetOrganizationByIdAsync(int id);
        Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto createOrganizationDto, int subscriptionPlanId);
        Task<bool> DeleteOrganizationAsync(int id);
        Task<List<UserDto>> GetStudentsByOrganizationIdAsync(int organizationId);
        Task<IEnumerable<UserDto>> GetAllInstructorsToOrganizationAsync(int OrganizationId);
    }
}
