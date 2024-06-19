using Eduology.Application.Interface;
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

        public async Task<Course> CreateAsync(CourseDto courseDto)
        {
            var course = await _courseRepository.CreateAsync(courseDto);
            if (course == null)
            {
                return null;
            }
            return course;
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
        public async Task<bool> AddInstructorToCourseAsync(string instructorId, string courseCode)
        {
            var instructor = await _courseRepository.AddInstructorToCourseAsync(instructorId, courseCode);
            if (instructor == null|| instructor==false)
                return false; 
           return instructor;
        }
    }
}
