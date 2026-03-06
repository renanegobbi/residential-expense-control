using ResidentialExpenseControl.Api.ViewModels.People;
using ResidentialExpenseControl.Api.ViewModels.Transaction;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace ResidentialExpenseControl.Api.Swagger.Examples
{
    #region Search
    public class SearchTransactionRequestExample : IExamplesProvider<SearchTransactionViewModel>
    {
        public SearchTransactionViewModel GetExamples() => new SearchTransactionViewModel
        {
            Id = 1,
            Description = "Pagamento da conta de luz",
            Amount = 150.75m,
            Type = TransactionType.Expense,
            CategoryId = 2,
            PersonId = 1,
            OrderBy = TransactionOrderBy.Id,
            OrderDirection = "ASC",
            PageIndex = 1,
            PageSize = 5
        };
    }

    public class SearchTransactionResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Query executed successfully." },
            Result = new
            {
                PageIndex = 1,
                PageSize = 5,
                OrderBy = TransactionOrderBy.Id.ToString(),
                OrderDirection = "ASC",
                TotalRecords = 1,
                TotalPages = 1,
                Items = new[]
                {
                    new {
                        Id = 1,
                        Description = "Pagamento da conta de luz",
                        Amount = 150.75m,
                        Type = TransactionType.Expense,
                        CategoryId = 2,
                        PersonId = 1
                    }
                }
            }
        };
    }
    #endregion

    #region GetById
    public class GetTransactionOutputResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Transação obtida com sucesso." },
            Result = new[]{
                new TransactionOutput(){
                    Id = 1,
                    Description = "Pagamento da conta de luz",
                    Amount = 150.75m,
                    Type = TransactionType.Expense,
                    CategoryId = 2,
                    PersonId = 1
                }
            }
        };
    }
    #endregion

    #region CreateTransaction
    public class CreateTransactionRequestExample : IExamplesProvider<CreateTransactionViewModel>
    {
        public CreateTransactionViewModel GetExamples() => new CreateTransactionViewModel
        {
            Description = "Pagamento da conta de luz",
            Amount = 150.75m,
            Type = TransactionType.Expense,
            CategoryId = 2,
            PersonId = 1
        };
    }

    public class TransactionOutputExample : TransactionOutput
    {
        public TransactionOutput TransactionCreateExample() => new TransactionOutput()
        {
            Id = 1,
            Description = "Pagamento da conta de luz",
            Amount = 150.75m,
            Type = TransactionType.Expense,
            CategoryId = 2,
            PersonId = 1
        };
    }

    public class CreateTransactionResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Transação cadastrada com sucesso." },
            Result = new TransactionOutputExample().TransactionCreateExample()
        };
    }
    #endregion

    #region UpdateTransaction
    public class UpdateTransactionRequestExample : IExamplesProvider<UpdateTransactionViewModel>
    {
        public UpdateTransactionViewModel GetExamples() => new UpdateTransactionViewModel
        {
            Id = 1,
            Description = "Pagamento da conta de luz",
            Amount = 150.75m,
            Type = TransactionType.Expense,
            CategoryId = 2,
            PersonId = 1
        };
    }

    public class UpdateTransactionResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Transação atualizada com sucesso." },
            Result = new TransactionOutputExample().TransactionCreateExample()
        };
    }
    #endregion

    #region DeleteTransaction
    public class DeleteTransactionResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples() => new Response
        {
            Success = true,
            Messages = new[] { "Transação excluída com sucesso." },
            Result = null
        };
    }
    #endregion
}