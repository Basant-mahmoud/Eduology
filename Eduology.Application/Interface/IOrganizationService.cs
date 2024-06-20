using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync();
        Task<OrganizationDto> GetOrganizationByIdAsync(int id);
        Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto organizationDto);
        Task<bool> DeleteOrganizationAsync(int id);
    }
}
