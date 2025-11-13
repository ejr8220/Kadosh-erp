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
    public class RolePermissionController : ControllerBase
    {
        private readonly ICrudService<RolePermissionRequestDto, RolePermissionResponseDto> _rolePermissionService;
        private readonly IConfigurationProvider _mapperConfig;

        public RolePermissionController(
            ICrudService<RolePermissionRequestDto, RolePermissionResponseDto> rolePermissionService,
            IConfigurationProvider mapperConfig)
        {
            _rolePermissionService = rolePermissionService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolePermissionRequestDto dto)
        {
            await _rolePermissionService.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RolePermissionRequestDto dto)
        {
            await _rolePermissionService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rolePermissionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<RolePermissionResponseDto> result, int count) = await _rolePermissionService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RolePermissionResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _rolePermissionService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}