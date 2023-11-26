using ProniaLastTry.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaLastTry.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(25, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide pricing.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please add an order type.")]
        public int CountId { get; set; }
        [Required(ErrorMessage = "Please Enter SKU (what is a sku btw?)")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(100, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please add a category label!")]
        public int? CategoryId { get; set; }
    }
}
