using DataObjects_BE.Entities;
using DTOs_BE.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services_BE.Interfaces;

namespace WebAPI_BE.ProductController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Tạo sản phẩm mới
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductModel model)
        {
            try
            {
                var result = await _productService.CreateProductAsync(model);
                if (!result)
                    return BadRequest("Tạo sản phẩm không thành công");
                return Ok("Tạo sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa sản phẩm
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductModel model)
        {
            var result = await _productService.DeleteProductAsync(model);
            if (!result)
                return BadRequest("Xóa sản phẩm không thành công");
            return Ok("Xóa sản phẩm thành công");
        }

        // Cập nhật sản phẩm
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(model);
                if (!result)
                    return BadRequest("Cập nhật sản phẩm không thành công");
                return Ok("Cập nhật sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Lấy tất cả sản phẩm
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProduct()
        {
            var result = await _productService.GetAllProductAsync();
            if (result == null)
                return BadRequest("Không tìm thấy sản phẩm");
            return Ok(result);
        }

        // Lấy sản phẩm theo ID
        [HttpGet("get-by-id/{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);
            if (result == null)
                return BadRequest("Không tìm thấy sản phẩm");
            return Ok(result);
        }


    }
}
