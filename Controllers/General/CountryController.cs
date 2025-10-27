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
    public class CountryController : ControllerBase
    {
        private readonly ICrudService<CountryRequestDto, CountryResponseDto> _countryService;
        private readonly IConfigurationProvider _mapperConfig;

        public CountryController(
            ICrudService<CountryRequestDto, CountryResponseDto> countryService,
            IConfigurationProvider mapperConfig)
        {
            _countryService = countryService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryRequestDto dto)
        {
            await _countryService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CountryRequestDto dto)
        {
            await _countryService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _countryService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<CountryResponseDto> result, int count) = await _countryService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _countryService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}