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
    public class PersonController : ControllerBase
    {
        private readonly ICrudService<PersonRequestDto, PersonResponseDto> _personService;
        private readonly IConfigurationProvider _mapperConfig;

        public PersonController(
            ICrudService<PersonRequestDto, PersonResponseDto> personService,
            IConfigurationProvider mapperConfig)
        {
            _personService = personService;
            _mapperConfig = mapperConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonRequestDto dto)
        {
            await _personService.AddAsync(dto);
            return StatusCode(201); // Created
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonRequestDto dto)
        {
            await _personService.UpdateAsync(id, dto);
            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personService.DeleteAsync(id);
            return NoContent(); // 204
        }

        [HttpPost("filter")]
        public async Task<ActionResult<object>> GetAllFilter([FromBody] CustomDataManagerRequest request)
        {
            (List<PersonResponseDto> result, int count) = await _personService.GetAllFilterAsync(request, _mapperConfig);
            return Ok(new { result, count });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonResponseDto>> GetById(int id)
        {
            try
            {
                var dto = await _personService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}