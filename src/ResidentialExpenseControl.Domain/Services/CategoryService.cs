using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Entities.Validations;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Utils.Validations;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ResidentialExpenseControl.Domain.Extensions;
using ResidentialExpenseControl.Domain.Commands.Input;

namespace ResidentialExpenseControl.Domain.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<CategoryService> logger) : base(unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IOutput> GetAll(SearchCategoryInput input)
        {
            this.NotifyIfNull(input, "Entrada não informada.");

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            this.AddNotifications(input.Notifications);

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            var result = await _categoryRepository.GetAll(input);

            var itens = result.Item1.Select(x => x.ToCategoryOutput());

            return new SearchOutput(itens, input.OrderBy.ToString(), input.OrderDirection, result.Item2, input.PageIndex, input.PageSize);
        }

        public async Task<CategoryOutput> GetCategoryById(int categoryId)
        {
            var category = await _categoryRepository.GetById(categoryId);

            if (category == null) return null;

            var categoryOutput = category.ToCategoryOutput();

            return categoryOutput;
        }

        public async Task<IOutput> GetCategory(int categoryId)
        {
            var output = await GetCategoryById(categoryId);

            return output == null
                ? new Output(false, new[] { "Nenhuma categoria cadastrada." }, null)
                : new Output(true, new[] { "Categoria obtida com sucesso." }, output);
        }

        public async Task<IOutput> Create(Category category)
        {
            this.NotifyIfNull(category, "Entrada não informada.");

            NotifyValidationError(new CategoryValidation(), category);

            if (HasNotification())
                return new Output(false, GetNotifications().Select(a => a.Message).ToList(), null);

            var alreadyExists = await _categoryRepository.ExistsByDescription(category.Description);
            if (alreadyExists)
                return new Output(false, new[] { "Já existe uma categoria com essa descrição." }, null);

            try
            {
                await _categoryRepository.Add(category);

                _unitOfWork.Commit();

                var categoryOutput = await GetCategoryById(category.Id);

                _logger.LogInformation($"A categoria \"{categoryOutput.Id} - {categoryOutput.Description}\" foi cadastrada.");

                return new Output(true, new[] { "Categoria cadastrada com sucesso." }, categoryOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cadastrar a cateogria {category.Purpose}: {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Update(Category category)
        {
            this.NotifyIfNull(category, "Entrada não informada.");

            if (HasNotification())
                return new Output(false, this.Messages, null);

            var existingCategory = await GetCategoryById(category.Id);

            NotifyValidationError(new CategoryValidation(), category);

            if (HasNotification())
                return new Output(false, GetNotifications().Select(a => a.Message).ToList(), null);

            var existing = await _categoryRepository.GetById(category.Id);
            if (existing == null)
                return new Output(false, new[] { "Nenhuma categoria encontrada." }, null);

            var exists = await _categoryRepository.ExistsOtherWithDescription(category.Description, category.Id);
            if (exists)
                return new Output(false, new[] { "Já existe outra categoria com essa descrição." }, null);

            try
            {
                _categoryRepository.Update(category);

                _unitOfWork.Commit();

                var categoryOutput = await GetCategoryById(category.Id);

                _logger.LogInformation($"A categoria \"{existingCategory.Id} - {existingCategory.Description}\" foi alterada.");

                return new Output(true, new[] { "Categoria alterada com sucesso." }, categoryOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar a categoria \"{category.Id} - {category.Description}\": {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Delete(int id)
        {
            this.NotifyIfLessThan(id, 1, "Id inválido.");

            if (HasNotification())
                return new Output(false, this.Messages, null);

            if (await _transactionRepository.ExistsByCategoryId(id))
                return new Output(false, new[] { "Não é possível excluir a categoria porque existem transações vinculadas a ela." }, null);

            var existingCategory = await GetCategoryById(id);

            if (existingCategory == null) return new Output(false, new[] { "Nenhuma categoria encontrada" }, null);

            if (HasNotification())
                return new Output(false, this.Messages, null);

            try
            {
                var category = existingCategory.ToCategory();

                _categoryRepository.Remove(category);

                _unitOfWork.Commit();

                _logger.LogInformation($"A categoria \"{category.Id} - {category.Description}\" foi removida.");

                return new Output(true, new[] { "Categoria excluída com sucesso." }, category.ToCategoryOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover a categoria {id}: {ex.Message}");
                throw;
            }
        }
    }
}
