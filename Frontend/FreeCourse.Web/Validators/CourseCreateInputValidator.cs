using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator :AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(a =>a.Name).NotEmpty().WithMessage("This field must not empty");
            RuleFor(a =>a.Description).NotEmpty().WithMessage("This field must not empty");
            RuleFor(a =>a.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage("This field must not empty");
            RuleFor(a => a.Price).NotEmpty().WithMessage("Price must not empty").ScalePrecision(2, 6).WithMessage("Wrong format");
            RuleFor(a => a.CategoryId).NotEmpty().WithMessage("Category must not be empty");
        }
    }
}
