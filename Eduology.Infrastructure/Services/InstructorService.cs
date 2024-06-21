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
            try
            {
                var instructors = await _instructorRepository.GetAllInstructorsAsync();
                if (instructors == null || !instructors.Any())
                {
                    return new List<UserDto>();
                }
                return instructors;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while fetching the instructors.", ex);
            }
        }

        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Instructor ID cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                return instructor;
            }
            catch (Exception ex)
            { 
                throw new ApplicationException("An error occurred while fetching the instructor by ID.", ex);
            }
        }

        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Instructor name cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByNameAsync(name);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                return instructor;
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException("An error occurred while fetching the instructor by name.", ex);
            }
        }

        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException("Instructor username cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByUserNameAsync(userName);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                return instructor;
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException("An error occurred while fetching the instructor by username.", ex);
            }
        }

        public async Task<bool> DeleteInstructorAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Instructor ID cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                return await _instructorRepository.DeleteInstructorAsync(id);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while deleting the instructor.", ex);
            }
        }

        public async Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Instructor ID cannot be null or empty.");
                }

                if (updateInstructorDto == null)
                {
                    throw new ArgumentException("Update data cannot be null.");
                }

                if (string.IsNullOrEmpty(updateInstructorDto.Name) || string.IsNullOrEmpty(updateInstructorDto.UserName) || string.IsNullOrEmpty(updateInstructorDto.Email))
                {
                    throw new ArgumentException("Update data is invalid. Name, UserName, and Email cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                    return false; // Returning false instead of throwing exception to indicate that the instructor was not found.
                }

                return await _instructorRepository.UpdateInstructorAsync(id, updateInstructorDto);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while updating the instructor.", ex);
            }
        }

        public async Task<bool> RegisterToCourseAsync(string instructorId, string courseCode)
        {
            try
            {
                if (string.IsNullOrEmpty(instructorId) || string.IsNullOrEmpty(courseCode))
                {
                    throw new ArgumentException("Instructor ID and Course Code cannot be null or empty.");
                }

                var instructorExists = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructorExists == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                var success = await _instructorRepository.RegisterToCourseAsync(instructorId, courseCode);
                if (!success)
                {
                    throw new InvalidOperationException("Failed to register instructor to course.");
                }

                return true;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while registering the instructor to the course.", ex);
            }
        }

        public async Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string instructorId)
        {
            try
            {
                if (string.IsNullOrEmpty(instructorId))
                {
                    throw new ArgumentException("Instructor ID cannot be null or empty.");
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("Instructor not found.");
                }

                var courses = await _instructorRepository.GetAllCourseToSpecificInstructorAsync(instructorId);
                if (courses == null || !courses.Any())
                {
                    return new List<CourseUserDto>();
                }

                var courseDtos = courses.Select(course => new CourseUserDto
                {
                    InstructorName = instructor.Name,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    year = course.year
                }).ToList();

                return courseDtos;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while fetching courses for the instructor.", ex);
            }
        }
    }
}
