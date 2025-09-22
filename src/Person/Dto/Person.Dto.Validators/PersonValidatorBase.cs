using FluentValidation;

namespace Person.Dto.Validators;

public class PersonValidatorBase<T> : AbstractValidator<T>
{
    protected readonly string PropertyIsEmptyMsg = "Поле '{PropertyName}' не может быть пустым!";
    protected readonly string PropertyIsLessOrEqualThanMsg = "Поле '{PropertyName}' со значением: '{PropertyValue}' не может быть меньше или равно {ComparisonValue}!";
}
