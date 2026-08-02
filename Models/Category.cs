using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Stock_Master.Models
{
    public class Category
    {
        [Key]public int categoryId { get; set; }

        [Required(ErrorMessage = "Назва категорії обов'язкова")]
        [StringLength(50, ErrorMessage = "Назва категорії не може перевищувати 50 символів")]
        public string categoryName { get; set; } = string.Empty;


        public List<Product> Products { get; set; } = new List<Product>();
    }
}
