using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentialExpenseControl.Api.Swagger.Examples;
using ResidentialExpenseControl.Api.Swagger;
using ResidentialExpenseControl.Domain.Commands.Output;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Net;
using System.Threading.Tasks;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Api.ViewModels.Totals;
using ResidentialExpenseControl.Api.Extensions;
using ResidentialExpenseControl.Api.ViewModels.Transaction;

namespace ResidentialExpenseControl.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BadRequestApiResponse), (int)HttpStatusCode.BadRequest)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponse))]
    [SwaggerTag("Provides financial totals grouped by person and by category.")]
    public class TotalsController : BaseController
    {
        private readonly ITotalsService _totalsService;

        public TotalsController(
            ITotalsService totalsService,
            INotifier notifier) : base(notifier)
        {
            _totalsService = totalsService;
        }

        /// <summary>
        /// Get financial totals per person.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/by-people")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Totals obtained successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(TotalsByPeopleResponseExample))]
        [SwaggerRequestExample(typeof(TotalsFilterByPeopleViewModel), typeof(TotalsByPeopleRequesttExample))]
        public async Task<IActionResult> GetTotalsByPeople([FromBody] TotalsFilterByPeopleViewModel model)
        {
            try
            {
                var totalsFilterInput = model.ToTotalsFilterInput();

                return new ApiResult(await _totalsService.GetTotalsByPeople(totalsFilterInput));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Get financial totals per category.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/by-category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Totals obtained successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(TotalsByCategoryResponseExample))]
        [SwaggerRequestExample(typeof(TotalsFilterByCategoryViewModel), typeof(TotalsByCategoryRequestExample))]
        public async Task<IActionResult> GetTotalsByCategory(TotalsFilterByCategoryViewModel model)
        {
            try
            {
                var totalsFilterInput = model.ToTotalsFilterInput();

                return new ApiResult(await _totalsService.GetTotalsByCategory(totalsFilterInput));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }
    }
}
