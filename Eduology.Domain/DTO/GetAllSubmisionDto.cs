﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.DTO
{
    public class GetAllSubmisionDto
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string CourseId { get; set; }
    }
}