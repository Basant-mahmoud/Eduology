using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services_class
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
            return instructors;
        }

        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                throw new KeyNotFoundException("Instructor not found.");
            }
            return instructor;
        }

        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
            var instructor = await _instructorRepository.GetInstructorByNameAsync(name);
            if (instructor == null)
            {
                throw new KeyNotFoundException("Instructor not found.");
            }
            return instructor;
        }

        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            var instructor = await _instructorRepository.GetInstructorByUserNameAsync(userName);
            if (instructor == null)
            {
                throw new KeyNotFoundException("Instructor not found.");
            }
            return instructor;
        }

        public async Task<bool> DeleteInstructorAsync(string id)
        {
            var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                throw new KeyNotFoundException("Instructor not found.");
            }
            return await _instructorRepository.DeleteInstructorAsync(id);
        }

        public async Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto)
        {
            var instructor = await _instructorRepository.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                //throw new KeyNotFoundException("Instructor not found.");
                return false;
            }

            if (string.IsNullOrEmpty(updateInstructorDto.Name) || string.IsNullOrEmpty(updateInstructorDto.UserName) || string.IsNullOrEmpty(updateInstructorDto.Email))
            {
                throw new ArgumentException("Update data is invalid.");
            }

            return await _instructorRepository.UpdateInstructorAsync(id, updateInstructorDto);
        }
        public async Task<bool> RegisterToCourseAsync(string instructorId, string courseCode)
        {
            var instructor = await _instructorRepository.RegisterToCourseAsync(instructorId, courseCode);
            if (instructor == null || instructor == false)
                return false;
            return instructor;
        }

        public async Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string InstructorId)
        {
            var instructor = await _instructorRepository.GetInstructorByIdAsync(InstructorId);
            if (instructor == null)
            {
                return null;
                throw new KeyNotFoundException("Instructor not found.");
            }

            var courses = await _instructorRepository.GetAllCourseToSpecificInstructorAsync(InstructorId);
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

    }
}
