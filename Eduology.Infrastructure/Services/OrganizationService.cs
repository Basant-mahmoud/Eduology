using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationService(IOrganizationRepository organizationRepository,UserManager<ApplicationUser> userManager)
        {
            _organizationRepository = organizationRepository;
            _userManager = userManager;
        }

        public async Task<List<OrganizationDetailsDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            if (organizations == null)
                return null;
            var organizationDtos = new List<OrganizationDetailsDto>();
            foreach (var organization in organizations)
            {
                var organizationDto = await MapToOrganizationDtoAsync(organization);
                if (organizationDto != null)
                {
                    organizationDtos.Add(organizationDto);
                }
            }

            return organizationDtos;
        }

        public async Task<OrganizationDetailsDto> GetOrganizationByIdAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return null;
            }
            var _organziation= MapToOrganizationDtoAsync(organization);
            return await _organziation;
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
            if (createdOrganization == null)
                return null;
            return new OrganizationDto
            {
                Name = organization.Name,
                Phone = organization.Phone,
                Email = organization.Email,
                Password = organization.Password,
                ConfirmPassword = organization.ConfirmPassword,
            };
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

        private async Task<OrganizationDetailsDto> MapToOrganizationDtoAsync(Organization organization)
        {
            if (organization == null)
                return null;
            var adminUsers = new List<ApplicationUser>();
            var studentUsers = new List<ApplicationUser>();
            var instructorUsers = new List<ApplicationUser>();
            foreach (var user in organization.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                    adminUsers.Add(user);
                else if(roles.Contains("Student"))
                    studentUsers.Add(user);
                else if (roles.Contains("Instructor"))
                    instructorUsers.Add(user);

            }
            return new OrganizationDetailsDto
            {
               OrganizationID = organization.OrganizationID,
               admins = adminUsers,
               instructors = instructorUsers,
               students = studentUsers,
               Courses = (List<Course>)organization.Courses,
               
            };
        }
    }
}
