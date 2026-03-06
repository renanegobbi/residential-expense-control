using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentialExpenseControl.Api.Extensions;
using ResidentialExpenseControl.Api.Swagger;
using ResidentialExpenseControl.Api.Swagger.Examples;
using ResidentialExpenseControl.Api.ViewModels.Transaction;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BadRequestApiResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponse))]
    [SwaggerTag("Allows management and consultation of transaction data.")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(
            ITransactionService transactionService,
            INotifier notifier) : base(notifier)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Retrieves transactions based on the provided search parameters.
        /// </summary>
        /// <remarks>
        /// Notes:
        /// <ul>
        ///     <li>This endpoint does not require authentication.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/search")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Query executed successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SearchTransactionResponseExample))]
        [SwaggerRequestExample(typeof(SearchTransactionViewModel), typeof(SearchTransactionRequestExample))]
        public async Task<IActionResult> SearchTransactions([FromBody] SearchTransactionViewModel model)
        {
            try
            {
                var searchTransactionInput = model.ToSearchTransactionInput();

                return new ApiResult(await _transactionService.GetAll(searchTransactionInput));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Gets a transaction by its ID.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/get-by-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction obtained successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetTransactionOutputResponseExample))]
        public async Task<IActionResult> GetTransactionById([FromQuery, SwaggerParameter("Transaction ID.", Required = true)] int transactionId)
        {
            try
            {
                return new ApiResult(await _transactionService.GetTransaction(transactionId));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Creates a new transaction in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/register")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction successfully registered.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CreateTransactionResponseExample))]
        [SwaggerRequestExample(typeof(CreateTransactionViewModel), typeof(CreateTransactionRequestExample))]
        public async Task<IActionResult> CreateTransaction(CreateTransactionViewModel model)
        {
            try
            {
                var transaction = model.ToTransaction();

                var output = await _transactionService.Create(transaction);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Updates a transaction in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPut]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/update")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction successfully updated.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdateTransactionResponseExample))]
        [SwaggerRequestExample(typeof(UpdateTransactionViewModel), typeof(UpdateTransactionRequestExample))]
        public async Task<IActionResult> UpdateTransaction(UpdateTransactionViewModel model)
        {
            try
            {
                var existingTransaction = await _transactionService.GetTransaction(model.Id);

                if (!existingTransaction.Success) { return new ApiResult(existingTransaction); }

                var transaction = model.ToTransaction();

                var output = await _transactionService.Update(transaction);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Deletes a transaction from the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpDelete]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/delete")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Transaction successfully deleted.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DeleteTransactionResponseExample))]
        public async Task<IActionResult> DeleteTransaction([FromQuery, SwaggerParameter("Transaction ID.", Required = true)] int id)
        {
            try
            {
                var output = await _transactionService.Delete(id);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }
    }
}
