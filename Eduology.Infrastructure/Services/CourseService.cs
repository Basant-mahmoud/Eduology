using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduology.Infrastructure.Repositories;
using Type = Eduology.Domain.Models.Type;
using Eduology.Application.Services.Interface;
namespace Eduology.Infrastructure.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorService _instructorService;
        public CourseService(ICourseRepository courseRepository,IInstructorService instructorService,IStudentService studentService)
        {
            _courseRepository = courseRepository;
            _instructorService = instructorService;
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
        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync(string userID, string courseId)
        {
            bool isEnrolledStudent = await _courseRepository.IStudentAssignedToCourse(userID, courseId);
            bool isEnrolledInstructor = await _courseRepository.IsInstructorAssignedToCourse(userID, courseId);

            if (!isEnrolledStudent && !isEnrolledInstructor)
            {
                return Enumerable.Empty<CourseDetailsDto>();
            }

            var courses = await _courseRepository.GetAllAsync();
            if (courses == null)
                return Enumerable.Empty<CourseDetailsDto>();

            var userCourses = courses.Where(c =>
                c.StudentCourses.Any(sc => sc.StudentId == userID) ||
                c.CourseInstructors.Any(ci => ci.InstructorId == userID)).ToList();

            return userCourses.Select(c => new CourseDetailsDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                CourseCode = c.CourseCode,
                Instructors = c.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                students = c.StudentCourses.Select(sc => sc.Student.Name).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateAsync(String id, CourseDto course)
        {
            bool IsRegistered = await _courseRepository.IsInstructorAssignedToCourse(course.InstructorId, id);
            if (!IsRegistered)
                return false;
            var updatedCourse = await _courseRepository.UpdateAsync(id, course);
            if (updatedCourse == null)
                return false;
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var course = await _courseRepository.DeleteAsync(id);
            if (course == null)
                return false;
            return true;
        }

        public async Task<CourseDetailsDto> GetByIdAsync(string ID, string UserID)
        {
            bool isEnrolledStudent = await _courseRepository.IStudentAssignedToCourse(UserID, ID);
            bool isEnrolledInstructor = await _courseRepository.IsInstructorAssignedToCourse(UserID,ID);

            if (!isEnrolledStudent && !isEnrolledInstructor)
            {
                return null;
            }

            var course = await _courseRepository.GetByIdAsync(ID);
            if (course == null)
                return null;
            return course;
        }

        public async Task<CourseDetailsDto> GetByNameAsync(string name, string UserID,string CourseId)
        {
            bool isEnrolledStudent = await _courseRepository.IStudentAssignedToCourse(UserID, CourseId);
            bool isEnrolledInstructor = await _courseRepository.IsInstructorAssignedToCourse(UserID, CourseId);

            if (!isEnrolledStudent && !isEnrolledInstructor)
            {
                return null;
            }

            var course = await _courseRepository.GetByNameAsync(name);
            if (course == null)
                return null;
            return course;
        }
    }
}