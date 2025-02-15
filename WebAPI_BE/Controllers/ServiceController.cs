using AutoMapper;
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
    }
}
