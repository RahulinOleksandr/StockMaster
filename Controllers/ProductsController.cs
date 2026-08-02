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
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.productId,
                    p.productName,
                    p.CategoryId,
                    CategoryName = p.Category!.categoryName,
                    p.price
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.productId == id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product newProduct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryExists = await _context.Categories.AnyAsync(c => c.categoryId == newProduct.CategoryId);
            if (!categoryExists) return BadRequest("Обраної категорії не існує");

            newProduct.productId = 0;
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newProduct.productId }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var categoryExists = await _context.Categories.AnyAsync(c => c.categoryId == updated.CategoryId);
            if (!categoryExists) return BadRequest("Обраної категорії не існує");

            product.productName = updated.productName;
            product.CategoryId = updated.CategoryId;
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
