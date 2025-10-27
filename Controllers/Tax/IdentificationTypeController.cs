using Application.Dtos.Request;
using Application.Dtos.Request.Tax;
using Application.Dtos.Response.Tax;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Kadosh_erp.Controllers.Tax
{
    [ApiController]
    [Route("api/tax/[controller]")]
    public class IdentificationTypeController : ControllerBase
    {
        private readonly ICrudService<IdentificationTypeRequestDto, IdentificationTypeResponseDto> _service;
        private readonly IConfigurationProvider _mapperConfig;

        public IdentificationTypeController(
            ICrudService<IdentificationTypeRequestDto, IdentificationTypeResponseDto> service,
            IConfigurationProvider mapperConfig)
        {
            _service = service;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IdentificationTypeRequestDto dto)
        {
            await _service.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] IdentificationTypeRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<IdentificationTypeResponseDto> result, int count) = await _service.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IdentificationTypeResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}