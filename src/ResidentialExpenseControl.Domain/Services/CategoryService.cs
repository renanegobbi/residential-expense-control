using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork,
            INotifier notifier) : base(notifier)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Category category)
        {
            await _repository.Add(category);
            await _unitOfWork.Commit();
        }
    }
}
