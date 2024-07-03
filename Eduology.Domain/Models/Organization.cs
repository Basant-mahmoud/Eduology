﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

        public int SubscriptionPlanId { get; set; }   // Foreign Key

        public virtual SubscriptionPlan SubscriptionPlan { get; set; }     // Navigation property


    }
}
