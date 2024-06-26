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
      

        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            
                if (string.IsNullOrEmpty(id))
                {
                 throw new Exception("invalid input.");
                    
                }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                 throw new Exception("instructor id not found.");
                

            }

                return instructor;
            
           
        }

        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
 
                if (string.IsNullOrEmpty(name))
                {
                 throw new Exception("invalid input.");
                

            }

                var instructor = await _instructorRepository.GetInstructorByNameAsync(name);
                if (instructor == null)
                {
                throw new Exception("instructor Name not found."); 
                }

                return instructor;
            
        }

        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            
                if (string.IsNullOrEmpty(userName))
                {
                throw new Exception("invalid input.");

            }

                var instructor = await _instructorRepository.GetInstructorByUserNameAsync(userName);
                if (instructor == null)
                {
                throw new Exception("instructor userName not found.");

            }

                return instructor;
            
           
        }

        public async Task<bool> DeleteInstructorAsync(string id)
        {
           
                if (string.IsNullOrEmpty(id))
                {
                throw new Exception("invalid input.");

            }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                throw new Exception("instructor not found."); ;

            }

                return await _instructorRepository.DeleteInstructorAsync(id);
           
        }

        public async Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto)
        {
            
                if (string.IsNullOrEmpty(id))
                {
                throw new Exception(" invalid input.");
            }

                if (updateInstructorDto == null)
                {
                 throw new Exception("you should input data to update it."); 
            }

                if (string.IsNullOrEmpty(updateInstructorDto.Name) || string.IsNullOrEmpty(updateInstructorDto.UserName) || string.IsNullOrEmpty(updateInstructorDto.Email))
                {
                throw new Exception("some field in input is empty."); ;
            }

                var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
                if (instructor == null)
                {
                throw new Exception("instructor not found."); ;
            }

                return await _instructorRepository.UpdateInstructorAsync(id, updateInstructorDto);
            
           
        }

        public async Task<bool> RegisterToCourseAsync(string instructorId, string courseCode)
        {
            
                if (string.IsNullOrEmpty(instructorId) || string.IsNullOrEmpty(courseCode))
                {
                throw new Exception("some field of input is empty"); ; ;
                }

                var instructorExists = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructorExists == null)
                {
                throw new Exception("instructor not found."); ;
            }

            var success = await _instructorRepository.RegisterToCourseAsync(instructorId, courseCode);
                if (!success)
                {
                throw new Exception("instructor should register in course frist."); ;
            }

            return true;
            }
            
        

        public async Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string instructorId)
        {
            
                if (string.IsNullOrEmpty(instructorId))
                {
                throw new Exception("instructor id is empty."); ;
            }

            var instructor = await _instructorRepository.GetInstructorByIdAsync(instructorId);
                if (instructor == null)
                {
                throw new Exception("instructor not found."); ;
            }

            var courses = await _instructorRepository.GetAllCourseToSpecificInstructorAsync(instructorId);
                if (courses == null || !courses.Any())
                {
                    return new List<CourseUserDto>();
                }

                var courseDtos = courses.Select(course => new CourseUserDto
                {
                    CourseId = course.id,
                    Name = instructor.Name,
                    CourseName = course.Name,
                    CourseDescription = course.Description,
                    year = course.Year,
                    Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                    Students = course.StudentCourses.Select(sc => sc.Student.Name).ToList()
                }).ToList();

                return courseDtos;
           
        }

    }
}
