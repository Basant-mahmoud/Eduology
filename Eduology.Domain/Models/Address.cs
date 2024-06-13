using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    internal class Address
    {
        [Required]
        [MaxLength(100)]
        public String Street { get; set; }
        [Required]
        [MaxLength(100)]
        public String City { get; set; }
        [Required]
        [MaxLength(100)]
        public String Country { get; set; }
    }
}
