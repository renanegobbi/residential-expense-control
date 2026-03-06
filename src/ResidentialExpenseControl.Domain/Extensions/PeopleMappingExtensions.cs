using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Domain.Extensions
{
    public static class PeopleMappingExtensions
    {
        
        public static PersonOutput ToPersonOutput(this Person entity)
        {
            if (entity == null) return null;

            return new PersonOutput
            {
                Id = entity.Id,
                Name = entity.Name,
                Age = entity.Age
            };
        }

        public static Person ToPerson(this PersonOutput model)
        {
            if (model == null) return null;

            return new Person
            {
                Id = model.Id,
                Name = model.Name,
                Age = model.Age
            };
        }

    }
}
