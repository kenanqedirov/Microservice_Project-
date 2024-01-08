using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CategoryViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "This field must not empty")]
        public string Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "This field must not empty")]
        public string Name { get; set; }
    }
}
