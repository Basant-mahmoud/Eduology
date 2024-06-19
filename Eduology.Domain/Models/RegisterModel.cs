using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{ 
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(50)] 
        public string Role { get; set; }
        [Required] 
        public int OrganizationId { get; set; }
    }
}
