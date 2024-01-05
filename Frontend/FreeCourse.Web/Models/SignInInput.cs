using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SigninInput
    {
        [Required(ErrorMessage ="This field is empty")]
        [Display(Name ="Your Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is empty")]
        [Display(Name ="Your Password")]
        public string  Password { get; set; }
        [Required(ErrorMessage = "This field is empty")]
        [Display(Name ="Remember me")]
        public bool IsRememberred { get; set; }
    }
}