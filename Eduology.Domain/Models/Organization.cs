using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }


    }
}
