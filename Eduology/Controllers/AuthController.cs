using Eduology.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eduology.Application.Utilities;
using Eduology.Application.Services.Interface;
using Eduology.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            // Validate email format
            if (!ValidationHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format");
                return BadRequest(ModelState);
            }

            // Validate model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the register method of AuthService
            var result = await _authService.RegisterAsync(model);

            // Handle authentication result
            if (!result.IsAuthenticated)
            {
                ModelState.AddModelError("RegistrationError", result.Message);
                return BadRequest(ModelState);
            }

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register-from-excel")]
        public async Task<IActionResult> RegisterFromExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var users = new List<RegisterModel>();

            using (var stream = new MemoryStream())
            {

                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {

                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("No worksheet found in Excel file.");

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var model = new RegisterModel
                        {
                            Username = worksheet.Cells[row, 1].Text,
                            Email = worksheet.Cells[row, 2].Text,
                            Password = worksheet.Cells[row, 3].Text,
                            Name = worksheet.Cells[row, 4].Text,
                            OrganizationId = int.Parse(worksheet.Cells[row, 5].Text),
                            Role = worksheet.Cells[row, 6].Text
                        };

                        users.Add(model);
                    }
                }
            }

            var results = new List<AuthModel>();
            foreach (var user in users)
            {
                var result = await _authService.RegisterAsync(user);
                results.Add(result);
            }

            return Ok(results);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addrole")]
        [Authorize] // This endpoint should be authorized
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}