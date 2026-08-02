using Stock_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Stock_Master.Data;
using Microsoft.EntityFrameworkCore;


namespace Stock_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WarehousesController : ControllerBase
    {
        private readonly DBContent _context;
        public WarehousesController(DBContent context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Warehouse>>> GetAll()
        {
            return await _context.Warehouses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetById(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();
            return warehouse;
        }

        [HttpPost]
        public async Task<ActionResult<Warehouse>> Create(Warehouse newWarehouse)
        {
            newWarehouse.warehouseId = 0;
            _context.Warehouses.Add(newWarehouse);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newWarehouse.warehouseId }, newWarehouse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Warehouse updated)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            warehouse.warehouseName = updated.warehouseName;
            warehouse.address = updated.address;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}