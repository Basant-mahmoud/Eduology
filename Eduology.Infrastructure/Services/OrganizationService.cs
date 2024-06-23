using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationService(IOrganizationRepository organizationRepository, UserManager<ApplicationUser> userManager)
        {
            _organizationRepository = organizationRepository;
            _userManager = userManager;
        }

        public async Task<List<OrganizationDetailsDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            if (organizations == null || !organizations.Any())
                return new List<OrganizationDetailsDto>(); 

            var organizationDtos = new List<OrganizationDetailsDto>();
            foreach (var organization in organizations)
            {
                var organizationDto = await MapToOrganizationDetailsDtoAsync(organization);
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
                return null;

            return await MapToOrganizationDetailsDtoAsync(organization);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto createOrganizationDto)
        {
            // Check if the email is already registered as a user
            var existingEmail = await _userManager.FindByEmailAsync(createOrganizationDto.Email);
            if (existingEmail != null)
            {
                return null;
            }
            var existingUser = await _userManager.FindByEmailAsync(createOrganizationDto.Name);
            if (existingUser != null)
            {
                return null;
            }

            // Check if the organization already exists
            var existingOrganization = await _organizationRepository.GetByEmailAsync(createOrganizationDto.Email);
            if (existingOrganization != null)
            {
                return null;
            }

            // Validate password and confirm password
            if (createOrganizationDto.Password != createOrganizationDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Password and confirm password do not match!");
            }

            var organization = new Organization
            {
                Name = createOrganizationDto.Name,
                Phone = createOrganizationDto.Phone,
                Email = createOrganizationDto.Email,
                Password = createOrganizationDto.Password,
                ConfirmPassword = createOrganizationDto.ConfirmPassword
            };

            await _organizationRepository.AddAsync(organization);

            return new OrganizationDto
            {
                OrganizationId = organization.OrganizationID,
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
                return false;

            await _organizationRepository.DeleteAsync(id);
            return true;
        }

    private async Task<OrganizationDetailsDto> MapToOrganizationDetailsDtoAsync(Organization organization)
    {
        if (organization == null)
            return null;

        var adminUsers = new List<UserDto>();
        var studentUsers = new List<UserDto>();
        var instructorUsers = new List<UserDto>();

        if (organization.Users != null)
        {
            foreach (var user in organization.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name, 
                    UserName = user.UserName,
                    Email = user.Email
                };

                if (roles.Contains("Admin"))
                    adminUsers.Add(userDto);
                else if (roles.Contains("Student"))
                    studentUsers.Add(userDto);
                else if (roles.Contains("Instructor"))
                    instructorUsers.Add(userDto);
            }
        }

        var coursesDto = new List<CourseDto>();
        if (organization.Courses != null)
        {
            foreach (var course in organization.Courses)
            {
                var courseDto = new CourseDto
                {
                    Name = course.Name,
                    CourseCode = course.CourseCode,
                    Year = course.Year
                };
                coursesDto.Add(courseDto);
            }
        }

        return new OrganizationDetailsDto
        {
            OrganizationID = organization.OrganizationID,
            admins = adminUsers,
            instructors = instructorUsers,
            students = studentUsers,
            Courses = coursesDto,
        };
    }
}
}