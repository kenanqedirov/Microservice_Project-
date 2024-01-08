using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "This field must not empty")]
        public int Duration { get; set; }
    }
}
