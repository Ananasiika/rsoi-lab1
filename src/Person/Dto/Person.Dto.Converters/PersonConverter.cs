using CorePerson = Person.Core.Models.Person;

namespace Person.Dto.Converters;

public class PersonConverter
{
    public static PersonResponse Convert(CorePerson model)
    {
        return new PersonResponse(model.Id,
            model.Name,
            model.Age,
            model.Address,
            model.Work);
    }
}