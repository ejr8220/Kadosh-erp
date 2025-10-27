using Application.Dtos.Request;
using Application.Dtos.Request.General;
using Application.Dtos.Response;
using Application.Dtos.Response.General;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/general/[controller]")]
    public class ParishController : ControllerBase
    {
        private readonly ICrudService<ParishRequestDto, ParishResponseDto> _parishService;
        private readonly IConfigurationProvider _mapperConfig;

        public ParishController(
            ICrudService<ParishRequestDto, ParishResponseDto> parishService,
            IConfigurationProvider mapperConfig)
        {
            _parishService = parishService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParishRequestDto dto)
        {
            await _parishService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ParishRequestDto dto)
        {
            await _parishService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _parishService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<ParishResponseDto> result, int count) = await _parishService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ParishResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _parishService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
