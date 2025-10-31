using Application.Dtos.Request;
using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Kadosh_erp.Controllers.Security
{
    [ApiController]
    [Route("api/security/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ICrudService<RoleRequestDto, RoleResponseDto> _roleService;
        private readonly IConfigurationProvider _mapperConfig;

        public RoleController(
            ICrudService<RoleRequestDto, RoleResponseDto> roleService,
            IConfigurationProvider mapperConfig)
        {
            _roleService = roleService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleRequestDto dto)
        {
            await _roleService.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleRequestDto dto)
        {
            await _roleService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<RoleResponseDto> result, int count) = await _roleService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _roleService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}