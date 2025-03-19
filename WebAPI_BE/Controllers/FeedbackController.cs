using DataObjects_BE.Entities;
using DTOs_BE.FeedbackDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet("get-feedback-by-id/{id}")]
        public async Task<IActionResult> GetFeedbackById(string id)
        {
          
            var response = await _feedbackService.GetFeedbackById(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);

        }
        [HttpGet("get-list-feedback")]
        public async Task<IActionResult> GetListFeedback()
        {
            var response = await _feedbackService.GetListFeedBack();
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("get-list-feedback-by-childId/{childId}")]
        public async Task<IActionResult>GetListFeedbackByChildId(string childId)
        {
            var response =await _feedbackService.ListFeedbackByChildId(childId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("get-list-feedback-by-doctorId/{doctorId}")]
        public async Task<IActionResult>GetListFeedbackByDoctorId(string doctorId) 
        {
            var response = await _feedbackService.ListFeedbackByDoctorId(doctorId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);

        }
        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = _feedbackService.CreateFeedback(model);
                if(response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("update-feedback/{id}")]
        public async Task<IActionResult> UpdateFeedback([FromBody] UpdateFeedbackModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _feedbackService.UpdateFeedback(model, id);
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
        [HttpPut("change-active-feedback/{id}")]
        public async Task<IActionResult> ChangeActiveFeedback(string id)
        {
            var response = await _feedbackService.ChangeActiveOfFeedback(id);
            if(response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        
    }
}
