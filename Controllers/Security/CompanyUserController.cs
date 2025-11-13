using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Kadosh_erp.Controllers.Security
{
    [ApiController]
    [Route("api/security/[controller]")]
    public class CompanyUserController : ControllerBase
    {
        private readonly ICrudService<CompanyUserRequestDto, CompanyUserResponseDto> _companyUserService;
        private readonly IConfigurationProvider _mapperConfig;

        public CompanyUserController(
            ICrudService<CompanyUserRequestDto, CompanyUserResponseDto> companyUserService,
            IConfigurationProvider mapperConfig)
        {
            _companyUserService = companyUserService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyUserRequestDto dto)
        {
            await _companyUserService.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUserRequestDto dto)
        {
            await _companyUserService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyUserService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<CompanyUserResponseDto> result, int count) = await _companyUserService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyUserResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _companyUserService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}