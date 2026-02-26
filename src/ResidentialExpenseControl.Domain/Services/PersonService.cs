using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(
            IPersonRepository personRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier) : base(notifier)
        {
            _personRepository = personRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Person person)
        {
            if (person == null)
            {
                Notify("Pessoa inválida.");
                return;
            }

            await _personRepository.Add(person);
            await _unitOfWork.Commit();
        }

        public async Task Update(Person person)
        {
            _personRepository.Update(person);
            await _unitOfWork.Commit();
        }

        public async Task Delete(int id)
        {
            var person = await _personRepository.GetById(id);

            if (person == null)
            {
                Notify("Pessoa não encontrada.");
                return;
            }

            _personRepository.Remove(person);
            await _unitOfWork.Commit();
        }
    }
}
