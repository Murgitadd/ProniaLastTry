using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProniaLastTry.Areas.Admin.ViewModels
{
    public class CreateSlideVM
    {
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(25, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(50, ErrorMessage = "Name should be 1-50 characters long...")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(100, ErrorMessage = "Name should be 1-100 characters long...")]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
