using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kadosh_erp.Controllers.General
{
    [ApiController]
    [Route("api/general/[controller]")]
    public class ParameterController : ControllerBase
    {
        private readonly IParameterService _parameterService;

        public ParameterController(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }

        [HttpPost("header")]
        public async Task<IActionResult> CreateHeader([FromBody] ParameterHeaderRequestDto request)
        {
            var id = await _parameterService.CreateHeaderAsync(request);
            return Ok(new { id });
        }

        [HttpPost("detail")]
        public async Task<IActionResult> CreateDetail([FromBody] ParameterDetailRequestDto request)
        {
            var id = await _parameterService.CreateDetailAsync(request);
            return Ok(new { id });
        }

        [HttpGet("headers")]
        public async Task<ActionResult<List<ParameterHeaderResponseDto>>> GetHeaders()
        {
            var result = await _parameterService.GetHeadersAsync();
            return Ok(result);
        }

        [HttpGet("resolve")]
        public async Task<ActionResult<ResolvedParameterValueResponseDto>> Resolve(
            [FromQuery] string code,
            [FromQuery] int? companyId)
        {
            var value = await _parameterService.ResolveValueAsync(code, companyId);
            return Ok(new ResolvedParameterValueResponseDto
            {
                Code = code,
                CompanyId = companyId,
                Value = value
            });
        }
    }
}
