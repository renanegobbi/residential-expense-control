using Microsoft.Extensions.Logging;
using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Commands.Input;
using ResidentialExpenseControl.Domain.Commands.Output;
using ResidentialExpenseControl.Domain.Extensions;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Utils.Validations;
using System;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Domain.Services
{
    public class TotalsService : BaseService, ITotalsService
    {
        private readonly ITotalsRepository _totalsRepository;
        private readonly ILogger<TotalsService> _logger;

        public TotalsService(
            ITotalsRepository totalsRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<TotalsService> logger) : base(unitOfWork)
        {
            _totalsRepository = totalsRepository;
            _logger = logger;
        }

        public async Task<IOutput> GetTotalsByPeople(TotalsFilterByPersonInput input)
        {
            this.NotifyIfNull(input, "Entrada não informada.");

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            this.AddNotifications(input.Notifications);

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            var result = await _totalsRepository.GetTotalsByPeople(input);

            var totals = result.Item1.Items.ToTotalsOutput(
                result.Item1.TotalIncome,
                result.Item1.TotalExpense
            );

            var totalsSummary = new TotalsSummaryOutput
            {
                TotalIncome = totals.TotalIncome,
                TotalExpense = totals.TotalExpense,
            };

            return new TotalsFilterOutput(totals.Items, totalsSummary, input.OrderBy.ToString(), input.OrderDirection, result.Item2, input.PageIndex, input.PageSize);
        }

        public async Task<IOutput> GetTotalsByCategory(TotalsFilterByCategoryInput input)
        {
            this.NotifyIfNull(input, "Entrada não informada.");

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            this.AddNotifications(input.Notifications);

            if (this.IsInvalid)
                return new Output(false, this.Messages, null);

            var result = await _totalsRepository.GetTotalsByCategory(input);

            var totals = result.Item1.Items.ToTotalsOutput(
                result.Item1.TotalIncome,
                result.Item1.TotalExpense
            );

            var totalsSummary = new TotalsSummaryOutput
            {
                TotalIncome = totals.TotalIncome,
                TotalExpense = totals.TotalExpense,
            };

            return new TotalsFilterOutput(totals.Items, totalsSummary, input.OrderBy.ToString(), input.OrderDirection, result.Item2, input.PageIndex, input.PageSize);
        }
    }
}
