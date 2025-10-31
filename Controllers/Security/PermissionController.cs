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
    public class PermissionController : ControllerBase
    {
        private readonly ICrudService<PermissionRequestDto, PermissionResponseDto> _permissionService;
        private readonly IConfigurationProvider _mapperConfig;

        public PermissionController(
            ICrudService<PermissionRequestDto, PermissionResponseDto> permissionService,
            IConfigurationProvider mapperConfig)
        {
            _permissionService = permissionService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionRequestDto dto)
        {
            await _permissionService.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PermissionRequestDto dto)
        {
            await _permissionService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _permissionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<PermissionResponseDto> result, int count) = await _permissionService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PermissionResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _permissionService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}