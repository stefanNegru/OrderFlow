using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Common;
using OrderFlow.Application.Products.Dtos;
using OrderFlow.Application.Products.Exceptions;
using OrderFlow.Application.Products.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet]
        [ProducesResponseType(
        typeof(IReadOnlyList<ProductResponse>),
        StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetAll(
            [FromQuery] ProductQueryParameters parameters, 
            CancellationToken cancellationToken)
        {
            var products = await productService.GetAllAsync(parameters, cancellationToken);

            return Ok(products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(
            typeof(ProductResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var product = await productService.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Product not found",
                    Detail = $"Product with ID '{id}' was not found."
                });
            }
            return Ok(product);
        }

        // POST api/<ProductsController>
        /*[HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductResponse>> Create(
            CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var product = await productService.CreateAsync(request, cancellationToken);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = product.Id },
                    product);
            }
            catch (DuplicateProductSkuException exception)
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Duplicate product SKU",
                    Detail = exception.Message
                });
            }
        }*/
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductResponse>> Create(
            CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var product = await productService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var product = await productService.UpdateAsync(id, request, cancellationToken);

            if (product is null)
                return NotFound();
            return Ok(product);
        }

        [HttpPatch("{id:guid}/deactivate")]
        public async Task<ActionResult<ProductResponse>> Deactivate(Guid id, CancellationToken cancellationToken)
        {
            var product = await productService.DeactivateAsync(id, cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpPatch("{id:guid}/activate")]
        public async Task<ActionResult<ProductResponse>> Activate(Guid id, CancellationToken cancellationToken)
        {
            var product = await productService.ActivateAsync(id, cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }
    }
}
