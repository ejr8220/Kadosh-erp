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
    public class UserRoleController : ControllerBase
    {
        private readonly ICrudService<UserRoleRequestDto, UserRoleResponseDto> _userRoleService;
        private readonly IConfigurationProvider _mapperConfig;

        public UserRoleController(
            ICrudService<UserRoleRequestDto, UserRoleResponseDto> userRoleService,
            IConfigurationProvider mapperConfig)
        {
            _userRoleService = userRoleService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRoleRequestDto dto)
        {
            await _userRoleService.AddAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRoleRequestDto dto)
        {
            await _userRoleService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRoleService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<UserRoleResponseDto> result, int count) = await _userRoleService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserRoleResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _userRoleService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}