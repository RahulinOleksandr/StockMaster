using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Master.Models
{
    public class Product
    {
        [Key] public int productId { get; set; } //primary key
        public string productName { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public decimal price { get; set; }

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
