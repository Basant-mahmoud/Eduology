using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        [HttpPost("AddMatrial")]
        public async Task<IActionResult> AddMatrial([FromBody] MaterialDto matrial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.AddMaterialAsync(matrial);
            if (!success)
            {
                return BadRequest(new { message = "Failed to add material." });
            }

            return Ok(new { message = "Material added successfully." });
        }
    }
    
}
