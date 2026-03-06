using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class TotalsFilterOutput : Output
    {
        public TotalsFilterOutput(
            IEnumerable<object> data,
            TotalsSummaryOutput summary,
            string orderBy,
            string orderDirection,
            double totalRecords,
            int? pageIndex = null,
            int? pageSize = null)
            : base(
                true,
                new[] { "Consulta executada com sucesso." },
                new QueryResult(pageIndex, pageSize, orderBy, orderDirection, totalRecords, data, summary))
        {

        }

        public class QueryResult
        {
            public int? PageIndex { get; }

            public int? PageSize { get; }

            public string OrderBy { get; }

            public string OrderDirection { get; }

            public double TotalRecords { get; }

            public int? TotalPages { get; }

            public IEnumerable<object> Data { get; }
            public TotalsSummaryOutput Summary { get; }

            public QueryResult(
                int? pageIndex,
                int? pageSize,
                string orderBy,
                string orderDirection,
                double totalRecords,
                IEnumerable<object> data,
                TotalsSummaryOutput summary)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                OrderBy = orderBy;
                OrderDirection = orderDirection;
                TotalRecords = totalRecords;
                Data = data;
                Summary = summary;

                if (pageSize.HasValue)
                {
                    TotalPages = totalRecords % pageSize.Value != 0
                        ? (int)(totalRecords / pageSize.Value) + 1
                        : (int)(totalRecords / pageSize.Value);
                }
            }
        }
    }
}
