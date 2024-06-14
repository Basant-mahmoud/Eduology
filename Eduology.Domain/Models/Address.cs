using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Models
{
    public class Address
    {
        public int AddressId { get; set; }
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
