using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Master.Models
{
    public class Stock
    {
        [Key] public int stockId { get; set; } //primary key
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public Product? Product { get; set; }


        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")] public Warehouse? Warehouse { get; set; }

        public int quantity { get; set; }
    }
}
