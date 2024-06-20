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
        Task<List<OrganizationDetailsDto>> GetAllOrganizationsAsync();
        Task<OrganizationDetailsDto> GetOrganizationByIdAsync(int id);
        Task<OrganizationDto> CreateOrganizationAsync(OrganizationDto organizationDto);
        Task<bool> DeleteOrganizationAsync(int id);
    }
}
