﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string InstructorId { get; set; }
        public string Image { get; set; }
    }
}
