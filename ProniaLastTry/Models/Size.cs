using System.ComponentModel.DataAnnotations;

namespace ProniaLastTry.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(25, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Name { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }


    }
}
