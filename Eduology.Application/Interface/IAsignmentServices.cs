﻿using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IAsignmentServices
    {
        public Task<AssignmentDto> CreateAsync(AssignmentDto assignment,string userId);
        public Task<bool> UpdateAsync(int id,AssignmentDto assignment, string userId);
        public Task<bool> DeleteAsync(int id,string courseId, string userId);
        public Task<Assignment> GetByIdAsync(int id, string userId, string role);
        public Task<Assignment> GetByNameAsync(string name, string userId,string role);
        public Task<List<AssignmentDto>> GetAllAsync(string userId,string role);

    }
}
