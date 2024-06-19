﻿using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduology.Infrastructure.Repositories;
namespace Eduology.Infrastructure.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;


        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course> CreateAsync(CourseCreationDto courseDto)
        {
            // Generate a unique course code
            string courseCode;
            do
            {
                courseCode = GenerateCourseCode();
            } while (await _courseRepository.ExistsByCourseCodeAsync(courseCode));

            var course = new Course
            {
                CourseId = Guid.NewGuid().ToString(),
                Name = courseDto.Name,
                CourseCode = courseCode,
                Year = courseDto.Year,
                OrganizationID = courseDto.OrganizationId 

            };

            await _courseRepository.CreateAsync(course);
            return course;
        }

        private string GenerateCourseCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> DeleteAsync(String id)
        {
            var course = await _courseRepository.DeleteAsync(id);
            if (course == null)
                return false;
            return true;
        }

        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            if (courses == null)
                return Enumerable.Empty<CourseDetailsDto>();
            return courses;
        }

        public async Task<CourseDetailsDto> GetByIdAsync(String id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return null;
            return course;
        }

        public async Task<bool> UpdateAsync(String id, CourseDto course)
        {
            var updatedCourse = await _courseRepository.UpdateAsync(id, course);
            if (updatedCourse == null)
                return false;
            return true;
        }

        public async Task<CourseDetailsDto> GetByNameAsync(string name)
        {
            var course = await _courseRepository.GetByNameAsync(name);
            if (course == null)
                return null;
            return course;
        }

        public async Task<bool> AddMateriaCourseAsync(MaterialDto materialDto)
        {
            var course = await _courseRepository.GetByIdAsync(materialDto.courseId);
            if (course == null)
                return false;

            var material = new Material
            {
                Title = materialDto.Title,
                URL = materialDto.URL,
                InstructorId = materialDto.InstructorId,
                MaterialType = new Domain.Models.Type { Name = materialDto.MatrialType } // Assuming you want to create a new Type if it doesn't exist
            };

            var success = await _courseRepository.AddMateriaCourseAsync(material);
            return success;
        }

       
    }
}