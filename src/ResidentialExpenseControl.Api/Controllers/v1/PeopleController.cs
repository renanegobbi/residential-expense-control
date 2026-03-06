using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentialExpenseControl.Api.Extensions;
using ResidentialExpenseControl.Api.Swagger;
using ResidentialExpenseControl.Api.Swagger.Examples;
using ResidentialExpenseControl.Api.ViewModels.People;
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
    [SwaggerTag("Allows management and consultation of people data.")]
    public class PeopleController : BaseController
    {
        private readonly IPersonService _personService;

        public PeopleController(
            IPersonService personService,
            INotifier notifier) : base(notifier)
        {
            _personService = personService;
        }

        /// <summary>
        /// Retrieves people based on the provided search parameters.
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
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SearchPeopleResponseExample))]
        [SwaggerRequestExample(typeof(SearchPeopleViewModel), typeof(SearchPeopleRequestExample))]
        public async Task<IActionResult> SearchPeople([FromBody] SearchPeopleViewModel model)
        {
            try
            {
                var searchPeopleInput = model.ToSearchPeopleInput();

                return new ApiResult(await _personService.GetAll(searchPeopleInput));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Gets a person by its ID.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/get-by-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Person obtained successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetPersonOutputResponseExample))]
        public async Task<IActionResult> GetPersonById([FromQuery, SwaggerParameter("Person ID.", Required = true)] int personId)
        {
            try
            {
                return new ApiResult(await _personService.GetPerson(personId));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Creates a new person in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/register")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Person successfully registered.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CreatePersonResponseExample))]
        [SwaggerRequestExample(typeof(CreatePersonViewModel), typeof(CreatePersonRequestExample))]
        public async Task<IActionResult> CreatePerson(CreatePersonViewModel model)
        {
            try
            {
                var person = model.ToPerson();

                var output = await _personService.Create(person);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Updates a person in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPut]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/update")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Person successfully updated.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdatePersonResponseExample))]
        [SwaggerRequestExample(typeof(UpdatePersonViewModel), typeof(UpdatePersonRequestExample))]
        public async Task<IActionResult> UpdatePerson(UpdatePersonViewModel model)
        {
            try
            {
                var existingPerson = await _personService.GetPerson(model.Id);

                if (!existingPerson.Success) { return new ApiResult(existingPerson); }

                var person = model.ToPerson();

                var output = await _personService.Update(person);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Deletes a person from the database.
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
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DeletePersonResponseExample))]
        public async Task<IActionResult> DeletePerson([FromQuery, SwaggerParameter("Person ID.", Required = true)] int id)
        {
            try
            {
                var output = await _personService.Delete(id);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }
    }
}
