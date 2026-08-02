using Stock_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Stock_Master.Data;
using Microsoft.EntityFrameworkCore;


namespace Stock_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly DBContent _context;
        public ProductsController(DBContent context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product newProduct)
        {
            newProduct.productId = 0;
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newProduct.productId }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.productName = updated.productName;
            product.category = updated.category;
            product.price = updated.price;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
