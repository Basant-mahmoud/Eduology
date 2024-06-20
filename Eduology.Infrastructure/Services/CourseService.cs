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

        public async Task<bool> AddMaterialAsync(MaterialDto MaterialDto)
        {
            var material = new Material
            {
                Title = MaterialDto.Title,
                //URL = MaterialDto.URL,
                InstructorId = MaterialDto.InstructorId,
                CourseId = MaterialDto.CourseId,
                MaterialType = new Type { Name = MaterialDto.MaterialType }
            };

            // Add files to the material if provided
            if (MaterialDto.FileURLs != null && MaterialDto.FileURLs.Count > 0)
            {
               // material.Files = new List<Domain.Models.File>();
                foreach (var fileUrl in MaterialDto.FileURLs)
                {
                    var file = new Domain.Models.File
                    {
                        FileId= Guid.NewGuid().ToString(),
                        URL = fileUrl,
                        Title = $"File for {MaterialDto.Title}", // Example: Setting a title for the file
                        MaterialId = material.MaterialId
                    };
                   // material.Files.Add(file);
                }
            }

            // Call repository method to add material
            var success = await _courseRepository.AddMateriaCourseAsync(material);

            return success;
        }
    }
}