using FluentValidation;
using FreeCourse.Web.Models.Discount;

namespace FreeCourse.Web.Validators
{
    public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(a => a.Code).NotEmpty().WithMessage("Discount Code must not be empty");
        }
    }
}
