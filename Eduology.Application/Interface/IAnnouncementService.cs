﻿using Eduology.Domain.DTO;
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
        Task<IEnumerable<AnnouncementDto>> GetAllAsync();
        Task<AnnouncementDto> GetByIdAsync(int id);
        Task<AnnouncementDto> CreateAsync(AnnouncementDto announcementDto);
        Task DeleteAsync(int id);
    }
}
