using AutoMapper;
using DTOs_BE.ServiceDTOs;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _service;
        private readonly IMapper _mapper;
        public ServiceController(IServiceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpGet("list-service")]
        public async Task<IActionResult> GetListServices()
        {
            var response = await _service.ListService();
            if (response == null )
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("get-service-by-id/{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var response = await _service.GetServiceById(id);
            return Ok(response);
        }
        [HttpPost("create-service")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var serviceResponse = await _service.CreateService(model);
                return Ok(serviceResponse);
            }catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("update-service/{id}")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var serviceResponse = await _service.UpdateService(model, id);
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
