using CorePerson = Person.Core.Models.Person;
using DbPerson = Person.Database.Models.Person;

namespace Person.Database.Repositories;

public class PersonConverter
{
    public static CorePerson Convert(DbPerson person)
    {
        return new CorePerson(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.Work);
    }
}