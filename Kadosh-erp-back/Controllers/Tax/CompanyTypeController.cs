using Application.Dtos.Request;
using Application.Dtos.Request.Tax;
using Application.Dtos.Response;
using Application.Dtos.Response.Tax;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Kadosh_erp.Controllers.Tax
{
    [ApiController]
    [Route("api/tax/[controller]")]
    public class CompanyTypeController : ControllerBase
    {
        private readonly ICrudService<CompanyTypeRequestDto, CompanyTypeResponseDto> _service;
        private readonly IConfigurationProvider _mapperConfig;

        public CompanyTypeController(
            ICrudService<CompanyTypeRequestDto, CompanyTypeResponseDto> service,
            IConfigurationProvider mapperConfig)
        {
            _service = service;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyTypeRequestDto dto)
        {
            await _service.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyTypeRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<DataResult>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<CompanyTypeResponseDto> result, int count) = await _service.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new DataResult { Result = result, Count = count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyTypeResponseDto>> GetById(int id)
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
