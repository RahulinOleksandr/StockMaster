using Stock_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Stock_Master.Data;
using Microsoft.EntityFrameworkCore;

namespace Stock_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly DBContent _context;
        public CategoriesController(DBContent context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category newCategory)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            newCategory.categoryId = 0;
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newCategory.categoryId }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category updated)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            category.categoryName = updated.categoryName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                return BadRequest("Неможливо видалити категорію, доки є товари з цією категорією");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
