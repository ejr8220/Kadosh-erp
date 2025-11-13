using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/general/[controller]")]
    public class MaritalStatusController : ControllerBase
    {
        private readonly ICrudService<MaritalStatusRequestDto, MaritalStatusResponseDto> _maritalStatusService;
        private readonly IConfigurationProvider _mapperConfig;

        public MaritalStatusController(
            ICrudService<MaritalStatusRequestDto, MaritalStatusResponseDto> maritalStatusService,
            IConfigurationProvider mapperConfig)
        {
            _maritalStatusService = maritalStatusService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaritalStatusRequestDto dto)
        {
            await _maritalStatusService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaritalStatusRequestDto dto)
        {
            await _maritalStatusService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _maritalStatusService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<MaritalStatusResponseDto> result, int count) = await _maritalStatusService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaritalStatusResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _maritalStatusService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}