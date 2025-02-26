﻿using DTOs_BE.ServiceOrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/serviceorder")]
    [ApiController]
    public class ServiceOrderController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderService;
         public ServiceOrderController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetServiceOrderById(string id)
        {
            try
            {
                var response = await _serviceOrderService.GetServiceOrderById(id);
                if(response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("GetListOrder")]
        public async Task<IActionResult> GetListServiceOrder()
        {
            try
            {
                var response = await _serviceOrderService.GetListServiceOrder();
                if(response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("CreateServiceOrder")]
        public async Task<IActionResult> CreateServiceOrder(CreateServiceOrderModel model)
        {
            try
            {
                var response = await _serviceOrderService.CreateServiceOrder(model);
                if(response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("RenewServiceorder/{id}")]
        public async Task<IActionResult> RenewServiceorder(UpdateServiceOrderModel model, string id)
        {
            try
            {
                var response = await _serviceOrderService.UpdateServiceOrder(model, id);
                if(response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
