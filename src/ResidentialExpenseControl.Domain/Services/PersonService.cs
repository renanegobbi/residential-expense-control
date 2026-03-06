using Microsoft.Extensions.Logging;
using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Entities.Validations;
using ResidentialExpenseControl.Domain.Extensions;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Utils.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonService> _logger;

        public PersonService(
            IPersonRepository personRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<PersonService> logger) : base(unitOfWork)
        {
            _personRepository = personRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _logger =logger;
        }

        public async Task<IOutput> GetAll(SearchPeopleInput input)
        {
            this.NotifyIfNull(input, "Entrada não informada.");

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            this.AddNotifications(input.Notifications);

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            var result = await _personRepository.GetAll(input);

            var itens = result.Item1.Select(x => x.ToPersonOutput());

            return new SearchOutput(itens, input.OrderBy.ToString(), input.OrderDirection, result.Item2, input.PageIndex, input.PageSize);
        }

        public async Task<PersonOutput> GetPersonById(int personId)
        {
            var person = await _personRepository.GetById(personId);

            if (person == null) return null;

            var personOutput = person.ToPersonOutput();

            return personOutput;
        }

        public async Task<IOutput> GetPerson(int personId)
        {
            var output = await GetPersonById(personId);

            return output == null
                ? new Output(false, new[] { "Nenhuma pessoa cadastrada." }, null)
                : new Output(true, new[] { "Pessoa obtida com sucesso." }, output);
        }

        public async Task<IOutput> Create(Person person)
        {
            this.NotifyIfNull(person, "Entrada não informada.");

            NotifyValidationError(new PersonValidation(), person);

            if (HasNotification())
                return new Output(false, GetNotifications().Select(a => a.Message).ToList(), null);

            try
            {
                await _personRepository.Add(person);

                _unitOfWork.Commit();

                var personOutput = await GetPersonById(person.Id);

                _logger.LogInformation($"A pessoa \"{personOutput.Id} - {personOutput.Name}\" foi cadastrada.");

                return new Output(true, new[] { "Pessoa cadastrada com sucesso." }, personOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cadastrar a pessoa {person.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Update(Person person)
        {
            this.NotifyIfNull(person, "Entrada não informada.");

            if (HasNotification())
                return new Output(false, this.Messages, null);

            var existingPerson = await GetPersonById(person.Id);

            NotifyValidationError(new PersonValidation(), person);

            if (HasNotification())
                return new Output(false, this.Messages, null);

            if (existingPerson == null) return new Output(false, new[] { "Nenhuma pessoa encontrada." }, null);

            if (person.Age < 18)
            {
                var hasIncome = await _transactionRepository.HasIncomeByPersonId(person.Id);
                if (hasIncome)
                    return new Output(false, new[] { "Não é possível alterar para menor de idade porque existem receitas cadastradas para esta pessoa." }, null);
            }

            try
            {
                _personRepository.Update(person);

                _unitOfWork.Commit();

                var personOutput = await GetPersonById(person.Id);

                _logger.LogInformation($"A pessoa \"{existingPerson.Id} - {existingPerson.Name}\" foi alterada.");

                return new Output(true, new[] { "Pessoa alterada com sucesso." }, personOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar a pessoa \"{person.Id} - {person.Name}\": {ex.Message}");
                throw;
            }
        }

        public async Task<IOutput> Delete(int id)
        {
            this.NotifyIfLessThan(id, 1, "Id inválido.");

            if (HasNotification())
                return new Output(false, this.Messages, null);

            var existingPerson = await GetPersonById(id);

            if (existingPerson == null) return new Output(false, new[] { "Nenhuma pessoa encontrada" }, null);

            if (HasNotification())
                return new Output(false, this.Messages, null);

            try
            {
                var person = existingPerson.ToPerson();

                _personRepository.Remove(person);

                _unitOfWork.Commit();

                _logger.LogInformation($"A pessoa \"{person.Id} - {person.Name}\" foi removida.");

                return new Output(true, new[] { "Pessoa excluída com sucesso." }, person.ToPersonOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover a pessoa {id}: {ex.Message}");
                throw;
            }
        }

    }
}
