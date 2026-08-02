using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Master.Models
{
    public class Product
    {
        [Key] public int productId { get; set; } //primary key

        [Required(ErrorMessage = "Назва товару обов'язкова")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Назва має бути від 2 до 100 символів")]
        public string productName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Категорія обов'язкова")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Ціна має бути більше 0 і менше 1 мільйону")]
        public decimal price { get; set; }

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
