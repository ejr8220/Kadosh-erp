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
    public class ZoneController : ControllerBase
    {
        private readonly ICrudService<ZoneRequestDto, ZoneResponseDto> _zoneService;
        private readonly AutoMapper.IConfigurationProvider _mapperConfig;

        public ZoneController(
            ICrudService<ZoneRequestDto, ZoneResponseDto> zoneService,
            IConfigurationProvider mapperConfig)
        {
            _zoneService = zoneService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ZoneRequestDto dto)
        {
            await _zoneService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ZoneRequestDto dto)
        {
            await _zoneService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _zoneService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<ZoneResponseDto> result, int count) = await _zoneService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ZoneResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _zoneService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}