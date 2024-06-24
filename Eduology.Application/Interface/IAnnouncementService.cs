using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IAnnouncementService
    {
        Task<AnnouncementDto> GetByIdAsync(string instructorid, int announcementid, string courseid);
        Task<AnnouncementDto> CreateAsync(string instructorid, CreateAnnoncementDto createannouncementDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AnnouncementDto>> GetAnnouncementsByCourseIdAsync(string courseId);
        Task<IEnumerable<AllAnnoncemetDto>> GetAllAnnouncementsForStudentAsync(string studentid);
    }
}
