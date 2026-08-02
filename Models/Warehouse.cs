using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Stock_Master.Models
{
    public class Warehouse
    {
        [Key] public int warehouseId { get; set; } //primary key
        public string warehouseName { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;

        // stock levels at this warehouse
        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
