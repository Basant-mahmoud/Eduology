using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<UserDto> GetStudentByIdAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }
            return student;
        }
        public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            return students;
        }

        public async Task<bool> UpdateStudentAsync(string studentId, UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Email))
            {
                return false;
            }

            var success = await _studentRepository.UpdateStudentAsync(studentId, userDto);
            if (!success)
            {
                return false;
            }
            return success;
        }
        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            var success = await _studentRepository.DeleteStudentAsync(studentId);
            if (!success)
            {
                return false;
            }
            return success;
        }
        public async Task<bool> RegisterToCourseAsync(string studentId, string courseCode)
        {
            var student = await _studentRepository.RegisterToCourseAsync(studentId, courseCode);
            if (student == null || student == false)
                return false;
            return student;
        }
        public async Task<List<CourseUserDto>> GetAllCourseToSpecificStudentAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return null;
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }

            var courses = await _studentRepository.GetAllCourseToSpecificStudentAsync(studentId);
            if (courses == null || !courses.Any())
            {
                return new List<CourseUserDto>();
            }

            var courseDtos = courses.Select(course => new CourseUserDto
            {
                Name = student.Name,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                year = course.year
            }).ToList();

            return courseDtos;
        }
    }
}