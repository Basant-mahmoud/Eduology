﻿using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        public Task<AssignmentDto> CreateAsync(AssignmentDto assignment,string instructorId);
        public Task<Assignment> GetByIdAsync(int id);
        public Task<Assignment> GetByNameAsync(String name);
        public Task<Assignment> UpdateAsync(int id,UpdateAssignmemtDto assignment);
        public Task<bool> DeleteAsync(int id);
    }
}
