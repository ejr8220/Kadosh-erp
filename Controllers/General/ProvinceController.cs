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
    public class ProvinceController : ControllerBase
    {
        private readonly ICrudService<ProvinceRequestDto, ProvinceResponseDto> _provinceService;
        private readonly IConfigurationProvider _mapperConfig;

        public ProvinceController(
            ICrudService<ProvinceRequestDto, ProvinceResponseDto> provinceService,
            IConfigurationProvider mapperConfig)
        {
            _provinceService = provinceService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProvinceRequestDto dto)
        {
            await _provinceService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProvinceRequestDto dto)
        {
            await _provinceService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _provinceService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<ProvinceResponseDto> result, int count) = await _provinceService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProvinceResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _provinceService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}