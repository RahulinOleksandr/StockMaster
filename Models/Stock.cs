using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Master.Models
{
    public class Stock
    {
        [Key] public int stockId { get; set; } //primary key


        [Required(ErrorMessage = "ProductId обов'язковий")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public Product? Product { get; set; }

        [Required(ErrorMessage = "WarehouseId обов'язковий")]
        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")] public Warehouse? Warehouse { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Кількість не може бути від'ємною")]
        public int quantity { get; set; }
    }
}
