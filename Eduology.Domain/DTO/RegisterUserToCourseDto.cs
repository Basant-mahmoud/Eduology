using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Eduology.Domain.DTO
{
    public class RegisterUserToCourseDto
    {
       // [Required]
      //  public string InstructorId { get; set; }

        [Required]
        public string CourseCode { get; set; }
    }
}
