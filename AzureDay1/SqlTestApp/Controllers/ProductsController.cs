using Microsoft.AspNetCore.Mvc;
using SqlTestApp.Data;
using SqlTestApp.Models;
using System.Linq;

namespace SqlTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public IActionResult Get() => Ok(_context.Products.ToList());

        // POST: api/products
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
                return BadRequest("Invalid product data.");

            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
    }
}
