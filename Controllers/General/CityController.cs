using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Kadosh_erp.Controllers.General
{
    [ApiController]
    [Route("api/general/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICrudService<CityRequestDto, CityResponseDto> _cityService;
        private readonly IConfigurationProvider _mapperConfig;

        public CityController(
            ICrudService<CityRequestDto, CityResponseDto> cityService,
            IConfigurationProvider mapperConfig)
        {
            _cityService = cityService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CityRequestDto dto)
        {
            await _cityService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CityRequestDto dto)
        {
            await _cityService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<CityResponseDto> result, int count) = await _cityService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CityResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _cityService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}