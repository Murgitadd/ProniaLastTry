using System.ComponentModel.DataAnnotations;

namespace ProniaLastTry.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(25, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
