using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Person.Database.Context.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Models.Person>
{
    public void Configure(EntityTypeBuilder<Models.Person> builder)
    {
        builder.HasIndex(person => person.Id).IsUnique();
        builder.HasKey(person => person.Id);

        builder.Property(person => person.Id).IsRequired();

        builder.Property(person => person.Name).IsRequired();
    }
}