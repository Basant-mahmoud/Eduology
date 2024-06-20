using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            if (organizations == null)
                return Enumerable.Empty<OrganizationDto>();

            return organizations.Select(org => MapToOrganizationDto(org));
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return null;
            }
            return MapToOrganizationDto(organization);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto organizationDto)
        {
            var existingOrganization = await _organizationRepository.GetByIdAsync(organizationDto.OrganizationID);
            if (existingOrganization != null)
            {
                return null; 
            }

            var organization = new Organization
            {
                Name = organizationDto.Name,
                Phone = organizationDto.Phone,
                Email = organizationDto.Email,
                Password = organizationDto.Password,
                ConfirmPassword = organizationDto.ConfirmPassword
            };

            await _organizationRepository.AddAsync(organization);

            var createdOrganization = await _organizationRepository.GetByIdAsync(organization.OrganizationID);
            return MapToOrganizationDto(createdOrganization);
        }
        public async Task<bool> DeleteOrganizationAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return false;
            }

            await _organizationRepository.DeleteAsync(id);
            return true;
        }

        private OrganizationDto MapToOrganizationDto(Organization organization)
        {
            if (organization == null)
                return null;

            return new OrganizationDto
            {
                Name = organization.Name,
                Phone = organization.Phone,
                Email = organization.Email,
                Password = organization.Password,
                ConfirmPassword = organization.ConfirmPassword,
                Courses = organization.Courses.ToList(),
                Users = organization.Users.ToList()
            };
        }
    }
}
