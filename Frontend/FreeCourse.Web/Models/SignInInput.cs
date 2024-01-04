using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SigninInput
    {
        [Display(Name ="Your Email address")]
        public string Email { get; set; }
        [Display(Name ="Your Password")]
        public string  Password { get; set; }
        [Display(Name ="Remember me")]
        public bool IsRememberred { get; set; }
    }
}