using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Stock_Master.Models
{
    public class Warehouse
    {
        [Key] public int warehouseId { get; set; } //primary key

        [Required(ErrorMessage = "Назва складу обов'язкова")]
        [StringLength(100, MinimumLength = 2)]
        public string warehouseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Адреса обов'язкова")]
        [StringLength(200, ErrorMessage = "Адреса не може перевищувати 200 символів")]
        public string address { get; set; } = string.Empty;

        // stock levels at this warehouse
        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
