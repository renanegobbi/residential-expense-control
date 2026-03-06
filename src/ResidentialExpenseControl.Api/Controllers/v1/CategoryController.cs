using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentialExpenseControl.Api.Extensions;
using ResidentialExpenseControl.Api.Swagger;
using ResidentialExpenseControl.Api.Swagger.Examples;
using ResidentialExpenseControl.Api.ViewModels.Category;
using ResidentialExpenseControl.Api.ViewModels.People;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Services;
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
    [SwaggerTag("Allows management and consultation of category data.")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryService categoryService,
            INotifier notifier) : base(notifier)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves categories based on the provided search parameters.
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
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SearchCategoryResponseExample))]
        [SwaggerRequestExample(typeof(SearchCategoryViewModel), typeof(SearchCategoryRequestExample))]
        public async Task<IActionResult> SearchCategories([FromBody] SearchCategoryViewModel model)
        {
            try
            {
                var searchCategoryInput = model.ToSearchCategoryInput();

                return new ApiResult(await _categoryService.GetAll(searchCategoryInput));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/get-by-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Category obtained successfully.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetCategoryOutputResponseExample))]
        public async Task<IActionResult> GetCategoryById([FromQuery, SwaggerParameter("Category ID.", Required = true)] int categoryId)
        {
            try
            {
                return new ApiResult(await _categoryService.GetCategory(categoryId));
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <remarks>
        /// Notes:
        /// <ul>
        ///     <li>This endpoint does not require authentication.</li>
        /// </ul>
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/register")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Category successfully registered.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CreateCategoryResponseExample))]
        [SwaggerRequestExample(typeof(CreateCategoryViewModel), typeof(CreateCategoryRequestExample))]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            try
            {
                var category = model.ToCategory();

                var output = await _categoryService.Create(category);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Updates a category in the database.
        /// </summary>
        /// <remarks>Notes:
        /// <ul>
        ///     <li>To access this route, it is not necessary to be logged into the system.</li>
        /// </ul>
        /// </remarks>
        [HttpPut]
        [AllowAnonymous]
        [Route("v{version:apiVersion}/[controller]/update")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Category successfully updated.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UpdateCategoryResponseExample))]
        [SwaggerRequestExample(typeof(UpdateCategoryViewModel), typeof(UpdateCategoryRequestExample))]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryViewModel model)
        {
            try
            {
                var existingCategory = await _categoryService.GetCategory(model.Id);

                if (!existingCategory.Success) { return new ApiResult(existingCategory); }

                var category = model.ToCategory();

                var output = await _categoryService.Update(category);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }

        /// <summary>
        /// Deletes a category from the database.
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
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DeleteCategoryResponseExample))]
        public async Task<IActionResult> DeleteCategory([FromQuery, SwaggerParameter("Category ID.", Required = true)] int id)
        {
            try
            {
                var output = await _categoryService.Delete(id);

                return new ApiResult(output);
            }
            catch (Exception ex)
            {
                return new ApiResult(new Output(false, new string[] { ex?.Message }, null));
            }
        }
    }
}
