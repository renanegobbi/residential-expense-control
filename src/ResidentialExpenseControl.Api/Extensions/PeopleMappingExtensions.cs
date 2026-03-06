using ResidentialExpenseControl.Api.ViewModels.People;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Api.Extensions
{
    public static class PeopleMappingExtensions
    {
        public static SearchPeopleInput ToSearchPeopleInput(this SearchPeopleViewModel model)
        {
            if (model == null) return null;

            var input = new SearchPeopleInput(
                model.OrderBy,
                model.OrderDirection ?? "ASC",
                model.PageIndex,
                model.PageSize
            )
            {
                Id = model.Id,
                Name = model.Name,
                Age = model.Age,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge
            };

            return input;
        }

        public static Person ToPerson(this CreatePersonViewModel model)
        {
            if (model == null) return null;

            return new Person
            {
                Name = model.Name,
                Age = model.Age
            };
        }

        public static Person ToPerson(this UpdatePersonViewModel model)
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
