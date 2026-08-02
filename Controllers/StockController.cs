using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Master.Data;
using Stock_Master.Models;

namespace Stock_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly DBContent _context;
        public StockController(DBContent context) => _context = context;

        // displays stock levels along with the product name and warehouse
        // (Include fetches related data rather than requesting each item separately)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var stocks = await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .Select(s => new
                {
                    s.stockId,
                    ProductName = s.Product!.productName,
                    WarehouseName = s.Warehouse!.warehouseName,
                    s.quantity
                })
                .ToListAsync();

            return Ok(stocks);
        }

        // all products in a specific warehouse
        [HttpGet("by-warehouse/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByWarehouse(int warehouseId)
        {
            var stocks = await _context.Stocks
                .Where(s => s.WarehouseId == warehouseId)
                .Include(s => s.Product)
                .Select(s => new { ProductName = s.Product!.productName, s.quantity })
                .ToListAsync();

            return Ok(stocks);
        }

        // which warehouses a particular item is stored in
        [HttpGet("by-product/{productId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByProduct(int productId)
        {
            var stocks = await _context.Stocks
                .Where(s => s.ProductId == productId)
                .Include(s => s.Warehouse)
                .Select(s => new { WarehouseName = s.Warehouse!.warehouseName, s.quantity })
                .ToListAsync();

            return Ok(stocks);
        }

        // Add the item to stock (or update it if it’s already there)
        [HttpPost]
        public async Task<ActionResult<Stock>> AddOrUpdateStock(Stock newStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == newStock.ProductId && s.WarehouseId == newStock.WarehouseId);

            if (existing != null)
            {
                existing.quantity += newStock.quantity; // if there is already an entry, add the quantity
                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            newStock.stockId = 0;
            _context.Stocks.Add(newStock);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), newStock);
        }

        // to determine the exact number
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null) return NotFound();

            stock.quantity = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null) return NotFound();

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
