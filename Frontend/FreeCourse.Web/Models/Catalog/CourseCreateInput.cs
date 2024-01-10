using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        [Display(Name = "Course Name")]
        [Required(ErrorMessage ="This field must not empty")]
        public string Name { get; set; }

        [Display(Name = "Course Description")]
        [Required(ErrorMessage = "This field must not empty")]
        public string Description { get; set; }

        [Display(Name = "Course Price")]
        [Required(ErrorMessage = "This field must not empty")]
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }
        [Required]
        [Display(Name="Course Category")]
        public string CategoryId { get; set; }
        [Display(Name = "Course Photo")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
