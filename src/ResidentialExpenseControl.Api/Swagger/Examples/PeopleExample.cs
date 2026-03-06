using ResidentialExpenseControl.Api.ViewModels.People;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace ResidentialExpenseControl.Api.Swagger.Examples
{
    #region Search
    public class SearchPeopleRequestExample : IExamplesProvider<SearchPeopleViewModel>
    {
        public SearchPeopleViewModel GetExamples() => new SearchPeopleViewModel
        {
            Id = 1,
            Name = "João",
            Age = 30,
            OrderBy = PersonOrderBy.Id,
            OrderDirection = "ASC",
            PageIndex = 1,
            PageSize = 5
        };
    }

    public class PeopleExample 
    { 
        public PersonOutput PersonSearchExample(int id, string name, int age) => 
            new PersonOutput 
            { 
                Id = id, 
                Name = name, 
                Age = age 
            }; 
    }

    public class SearchPeopleExample : SearchOutput 
    { 
        public SearchPeopleExample() : base(new PersonOutput[] 
        { 
            new PeopleExample().PersonSearchExample(1, "João", 30), 
            new PeopleExample().PersonSearchExample(2, "Maria", 25), 
            new PeopleExample().PersonSearchExample(3, "Carlos", 40) }, 
            PersonOrderBy.Id.ToString(), 
            "DESC", 
            1, 
            1, 
            5) 
        { } 
    }

    public class SearchPeopleResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Consulta realizada com sucesso." },
            Result = new SearchPeopleExample().Result
        };
    }
    #endregion

    #region GetById
    public class GetPersonOutputResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Pessoa obtida com sucesso." },
            Result = new[]{
                new PersonOutput(){
                    Id = 1,
                    Name = "João",
                    Age = 30
                }
            }
        };
    }
    #endregion

    #region CreatePerson
    public class CreatePersonRequestExample : IExamplesProvider<CreatePersonViewModel>
    {
        public CreatePersonViewModel GetExamples() => new CreatePersonViewModel
        {
            Name = "João da Silva",
            Age = 40
        };
    }

    public class PersonOutputExample : PersonOutput
    {
        public PersonOutput PersonCreateExample() => new PersonOutput()
        {
            Id = 1,
            Name = "João da Silva"
        };
    }

    public class CreatePersonResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Pessoa cadastrada com sucesso." },
            Result = new PersonOutputExample().PersonCreateExample()
        };
    }
    #endregion

    #region UpdatePerson
    public class UpdatePersonRequestExample : IExamplesProvider<UpdatePersonViewModel>
    {
        public UpdatePersonViewModel GetExamples() => new UpdatePersonViewModel
        {
            Id = 1,
            Name = "João da Silva",
            Age = 40
        };
    }

    public class UpdatePersonResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Pessoa atualizada com sucesso." },
            Result = new PersonOutputExample().PersonCreateExample()
        };
    }
    #endregion

    #region DeletePerson
    public class DeletePersonResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Pessoa excluída com sucesso." },
            Result = null
        };
    }
    #endregion
}
