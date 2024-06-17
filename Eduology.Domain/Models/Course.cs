using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Course
    {
        public int  CourseId { get; set; }
        public string Name { get; set; }
        [Required]
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Image {  get; set; }
        public string InstructorId { get; set; } // Ensure this matches the primary key type in ApplicationUser
        public virtual ApplicationUser Instructor { get; set; }
        public virtual ICollection<Material> Materials { get; set; } 
        public virtual ICollection<Announcement> Announcements { get; set; }
        public ICollection<Assignment> Assignments { get; set; } // One-to-many relationship with Assignment
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
