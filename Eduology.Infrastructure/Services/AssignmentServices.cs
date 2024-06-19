using Eduology.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
namespace Eduology.Infrastructure.Services
{
    public class AssignmentServices : IAsignmentServices
    {
        private readonly IAssignmentRepository _asignmentRepository;
        public AssignmentServices(IAssignmentRepository asignmentRepository)
        {
            _asignmentRepository = asignmentRepository;
        }

        public async Task<Assignment> CreateAsync(Assignment assignment)
        {
            var _assignment = await _asignmentRepository.CreateAsync(assignment);
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(_assignment));
            }
            return _assignment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = await _asignmentRepository.DeleteAsync(id);

            if (isDeleted)
            {
                Console.WriteLine($"Assignment with ID {id} deleted successfully.");
                return true;
            }
            return false;
        }

        public async Task<Assignment> GetByIdAsync(int id)
        {
            var _assignment = await _asignmentRepository.GetByIdAsync(id);
            if (_assignment == null)
                return null;
            return _assignment;
        }

        public async Task<Assignment> GetByNameAsync(string name)
        {
            var _assignment = await _asignmentRepository.GetByNameAsync(name);
            if (_assignment == null)
                return null;
            return _assignment;
        }

        public async Task<Assignment> UpdateAsync(int id, Assignment assignment)
        {
            var _assignment = await _asignmentRepository.UpdateAsync(id,assignment);
            if (_assignment == null)
                return null;
            return _assignment;
        }
    }
}
