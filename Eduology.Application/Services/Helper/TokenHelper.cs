using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Services.Helper
{
    public static class TokenHelper
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirst("uid")?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
