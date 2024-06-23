using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllInstructorsAsync()
        {
            
                var instructors = await _instructorRepository.GetAllInstructorsAsync();
                if (instructors == null || !instructors.Any())
                {
                    return new List<UserDto>();
                }
                return instructors;
        }
        public async Task<IEnumerable<UserDto>> GetAllInstructorsToOrganizationAsync(int OrganizationId)
        {

            var instructors = await _instructorRepository.GetInstructorToOrganizationIdAsync(OrganizationId);
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

        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                    
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    return null;
                   
                }

                return instructor;
            
           
        }

        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
 
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                   
                }

                var instructor = await _instructorRepository.GetInstructorByNameAsync(name);
                if (instructor == null)
                {
                    return null;
                }

                return instructor;
            
        }

        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            
                if (string.IsNullOrEmpty(userName))
                {
                    return null;
                    
                }

                var instructor = await _instructorRepository.GetInstructorByUserNameAsync(userName);
                if (instructor == null)
                {
                    return null;
                   
                }

                return instructor;
            
           
        }

        public async Task<bool> DeleteInstructorAsync(string id)
        {
           
                if (string.IsNullOrEmpty(id))
                {
                    return false;
                   
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    return false;
                    
                }

                return await _instructorRepository.DeleteInstructorAsync(id);
           
        }

        public async Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto)
        {
            
                if (string.IsNullOrEmpty(id))
                {
                    return false;
                }

                if (updateInstructorDto == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(updateInstructorDto.Name) || string.IsNullOrEmpty(updateInstructorDto.UserName) || string.IsNullOrEmpty(updateInstructorDto.Email))
                {
                    return false ;
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    return false; 
                }

                return await _instructorRepository.UpdateInstructorAsync(id, updateInstructorDto);
            
           
        }

        public async Task<bool> RegisterToCourseAsync(string instructorId, string courseCode)
        {
            
                if (string.IsNullOrEmpty(instructorId) || string.IsNullOrEmpty(courseCode))
                {
                    return false;
                }

                var instructorExists = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructorExists == null)
                {
                    return false;   
                }

                var success = await _instructorRepository.RegisterToCourseAsync(instructorId, courseCode);
                if (!success)
                {
                    return false;
                }

                return true;
            }
            
        

        public async Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string instructorId)
        {
            
                if (string.IsNullOrEmpty(instructorId))
                {
                return null;
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructor == null)
                {
                return null;
                }

                var courses = await _instructorRepository.GetAllCourseToSpecificInstructorAsync(instructorId);
                if (courses == null || !courses.Any())
                {
                    return new List<CourseUserDto>();
                }

                var courseDtos = courses.Select(course => new CourseUserDto
                {
                    Name = instructor.Name,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    year = course.year
                }).ToList();

                return courseDtos;
           
        }

    }
}
