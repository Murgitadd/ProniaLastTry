using System.ComponentModel.DataAnnotations;

namespace ProniaLastTry.Areas.Admin.ViewModels
{
    public class CreateUpdateSizeVM
    {
        [Required(ErrorMessage = "Please Enter Name...")]
        [MaxLength(25, ErrorMessage = "Name should be 1-25 characters long...")]
        public string Name { get; set; }
    }
}
