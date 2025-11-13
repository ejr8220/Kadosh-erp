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
    public class UserController : ControllerBase
    {
        private readonly ICrudService<UserRequestDto, UserResponseDto> _userService;
        private readonly IConfigurationProvider _mapperConfig;

        public UserController(
            ICrudService<UserRequestDto, UserResponseDto> userService,
            IConfigurationProvider mapperConfig)
        {
            _userService = userService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequestDto dto)
        {
            await _userService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDto dto)
        {
            await _userService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<UserResponseDto> result, int count) = await _userService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _userService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}