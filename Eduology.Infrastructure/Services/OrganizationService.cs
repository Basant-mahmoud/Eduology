using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
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
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public OrganizationService(IOrganizationRepository organizationRepository, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _organizationRepository = organizationRepository;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
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
            {
                throw new KeyNotFoundException("Organization not found");
            }

            return await MapToOrganizationDetailsDtoAsync(organization);
        }

        public async Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto createOrganizationDto)
        {
            // Check if the email is already registered as a user
            var existingEmail = await _userManager.FindByEmailAsync(createOrganizationDto.Email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("Email is already registered");
            }
            var existingUser = await _userManager.FindByNameAsync(createOrganizationDto.Name);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User name is already taken");
            }

            // Check if the organization already exists
            var existingOrganization = await _organizationRepository.GetByEmailAsync(createOrganizationDto.Email);
            if (existingOrganization != null)
            {
                throw new InvalidOperationException("Organization with the same email already exists");
            }

            // Check if the organization already exists by name
            var existingOrganizationByName = await _organizationRepository.GetByNameAsync(createOrganizationDto.Name);
            if (existingOrganizationByName != null)
            {
                throw new InvalidOperationException("Organization with the same name already exists");
            }

            // Validate password and confirm password
            if (createOrganizationDto.Password != createOrganizationDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Password and confirm password do not match!");
            }

            // Hash the password
            //var hashedPassword = _passwordHasher.HashPassword(null, createOrganizationDto.Password);

            var organization = new Organization
            {
                Name = createOrganizationDto.Name,
                Phone = createOrganizationDto.Phone,
                Email = createOrganizationDto.Email,
                Password = createOrganizationDto.Password,
                ConfirmPassword = createOrganizationDto.ConfirmPassword
            };

            await _organizationRepository.AddAsync(organization);

            var organizationDto = new OrganizationDto
            {
                OrganizationId = organization.OrganizationID,
                Name = organization.Name,
                Phone = organization.Phone,
                Email = organization.Email,
                Password = createOrganizationDto.Password,
                ConfirmPassword = createOrganizationDto.ConfirmPassword
            };

            return organizationDto;
        }

        public async Task<bool> DeleteOrganizationAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                throw new KeyNotFoundException("Organization not found");
            }

            await _organizationRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<UserDto>> GetStudentsByOrganizationIdAsync(int organizationId)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                throw new KeyNotFoundException($"Organization with ID '{organizationId}' not found");
            }

            var studentUsers = await _organizationRepository.GetStudentsByOrganizationIdAsync(organizationId);

            if (studentUsers == null || !studentUsers.Any())
            {
                return new List<UserDto>();
            }

            return studentUsers.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();
        }

        public async Task<IEnumerable<UserDto>> GetAllInstructorsToOrganizationAsync(int organizationId)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                throw new KeyNotFoundException($"Organization with ID '{organizationId}' not found");
            }

            var instructors = await _organizationRepository.GetInstructorToOrganizationIdAsync(organizationId);
            if (instructors == null || !instructors.Any())
            {
                return new List<UserDto>();
            }

            return instructors.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();
            ;
        }

        private async Task<OrganizationDetailsDto> MapToOrganizationDetailsDtoAsync(Organization organization)
        {
            if (organization == null)
            {
                return null;
            }

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
                        OrganizationId = organization.OrganizationID
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
        public async Task<string> GetOrganizationPasswordByIdAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                throw new KeyNotFoundException("Organization not found");
            }

            return organization.Password;
        }

    }
}