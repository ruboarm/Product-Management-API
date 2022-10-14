using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ApplicationDbContext context, ProductService productService)
        {
            _productService = productService;
        }


        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Products = await _productService.GetProductsAsync();

            return new JsonResult(Products);
        }


        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);

            return new JsonResult(Product);
        }


        // POST api/<ProductsController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Product Product)
        {
            Product? createdProduct = null;
            if (ModelState.IsValid)
            {
                var success = await _productService.CreateProductAsync(Product);

                if (success)
                    createdProduct = await _productService.GetProductByIdAsync(Product.Id);
            }

            return new JsonResult(createdProduct);
        }


        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Product Product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _productService.GetProductByIdAsync(Product.Id);
                    if (existingProduct == null)
                        return new NotFoundResult();

                    // ConcurrencyCheck for "Available" property
                    if (Convert.ToBase64String(existingProduct.RowVersion) != Convert.ToBase64String(Product.RowVersion))
                        return StatusCode(StatusCodes.Status409Conflict,
                            new Response { Status = "Error", Message = "Product 'Availability' has been changed already. Please reload the product and try again!" });

                    var success = await _productService.UpdateProductAsync(Product);
                    if(success)
                        return BadRequest();
                }
                catch (Exception)
                {
                    return new JsonResult(null);
                }
            }

            return new JsonResult(Product);
        }
    }
}
