using FluentValidation;

namespace Person.Dto.Validators.Validators;

public class PersonRequestValidator : PersonValidatorBase<PersonRequest>
{
    public PersonRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage(PropertyIsEmptyMsg);

        When(model => model.Age is not null, () =>
        {
            RuleFor(model => model.Age)
                .GreaterThan(0).WithMessage(PropertyIsLessOrEqualThanMsg);
        });
    }
}