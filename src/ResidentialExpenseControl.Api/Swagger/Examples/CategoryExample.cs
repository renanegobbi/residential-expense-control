using ResidentialExpenseControl.Api.ViewModels.Category;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace ResidentialExpenseControl.Api.Swagger.Examples
{
    #region Search
    public class SearchCategoryRequestExample : IExamplesProvider<SearchCategoryViewModel>
    {
        public SearchCategoryViewModel GetExamples() => new SearchCategoryViewModel
        {
            Id = 1,
            Description = "Descrição da categoria.",
            OrderBy = CategoryOrderBy.Id,
            OrderDirection = "ASC",
            PageIndex = 1,
            PageSize = 5
        };
    }

    public class CategoryExample
    {
        public CategoryOutput CategorySearchExample(int id, string description, CategoryPurpose purpose) =>
            new CategoryOutput
            {
                Id = id,
                Description = description,
                Purpose = purpose
            };
    }

    public class SearchCategoryExample : SearchOutput
    {
        public SearchCategoryExample() : base(new CategoryOutput[]
        {
            new CategoryExample().CategorySearchExample(1, "Descrição da categoria 1.", CategoryPurpose.Expense),
            new CategoryExample().CategorySearchExample(2, "Descrição da categoria 2.", CategoryPurpose.Income),
            new CategoryExample().CategorySearchExample(3, "Descrição da categoria 3.", CategoryPurpose.Both) },
            CategoryOrderBy.Id.ToString(),
            "DESC",
            1,
            1,
            5)
        { }
    }

    public class SearchCategoryResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Consulta realizada com sucesso." },
            Result = new SearchCategoryExample().Result
        };
    }
    #endregion

    #region GetById
    public class GetCategoryOutputResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Categoria obtida com sucesso." },
            Result = new[]{
                new CategoryOutput(){
                    Id = 1,
                    Description = "Descrição da categoria.",
                    Purpose = CategoryPurpose.Expense
                }
            }
        };
    }
    #endregion

    #region CreateCategory
    public class CreateCategoryRequestExample : IExamplesProvider<CreateCategoryViewModel>
    {
        public CreateCategoryViewModel GetExamples() => new CreateCategoryViewModel
        {
            Description = "Descrição da categoria.",
        };
    }

    public class CategoryOutputExample : CategoryOutput
    {
        public CategoryOutput CategoryCreateExample() => new CategoryOutput()
        {
            Id = 1,
            Description = "Descrição da categoria."
        };
    }

    public class CreateCategoryResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Categoria cadastrada com sucesso." },
            Result = new CategoryOutputExample().CategoryCreateExample()
        };
    }
    #endregion

    #region UpdateCategory
    public class UpdateCategoryRequestExample : IExamplesProvider<UpdateCategoryViewModel>
    {
        public UpdateCategoryViewModel GetExamples() => new UpdateCategoryViewModel
        {
            Id = 1,
            Description = "Descrição da categoria.",
            CategoryPurpose = CategoryPurpose.Expense
        };
    }

    public class UpdateCategoryResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Categoria atualizada com sucesso." },
            Result = new CategoryOutputExample().CategoryCreateExample()
        };
    }
    #endregion

    #region DeleteCategory
    public class DeleteCategoryResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Categoria excluída com sucesso." },
            Result = null
        };
    }
    #endregion
}
