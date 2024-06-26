using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ValidationException("Student ID is required.");
            }
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with id {studentId} not found.");
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
                throw new ValidationException("Name, UserName, and Email are required.");
            }

            var success = await _studentRepository.UpdateStudentAsync(studentId, userDto);
            if (!success)
            {
                throw new InvalidOperationException($"Failed to update student with id {studentId}.");
            }
            return success;
        }
        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            var success = await _studentRepository.DeleteStudentAsync(studentId);
            if (!success)
            {
                throw new KeyNotFoundException($"Student with id {studentId} not found.");
            }
            return success;
        }
        public async Task<bool> RegisterToCourseAsync(string studentId, string courseCode)
        {
            var student = await _studentRepository.RegisterToCourseAsync(studentId, courseCode);
            if (student == null || student == false)
            {
                throw new ValidationException($"Failed to register student with id {studentId} to course {courseCode}.");
            }
            return student;
        }
        public async Task<List<CourseUserDto>> GetAllCourseToSpecificStudentAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ValidationException("Student ID is required.");
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with id {studentId} not found.");
            }

            var courses = await _studentRepository.GetAllCourseToSpecificStudentAsync(studentId);
            if (courses == null || !courses.Any())
            {
                return new List<CourseUserDto>();
            }

            var courseDtos = courses.Select(course => new CourseUserDto
            {
                CourseId = course.id,
                Name = student.Name,
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