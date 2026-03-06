using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class SearchOutput : Output
    {
        public SearchOutput(
            IEnumerable<object> data,
            string orderBy,
            string orderDirection,
            double totalRecords,
            int? pageIndex = null,
            int? pageSize = null)
            : base(
                true,
                new[] { "Consulta executada com sucesso." },
                new QueryResult(pageIndex, pageSize, orderBy, orderDirection, totalRecords, data))
        {

        }

        public SearchOutput(IEnumerable<object> data)
            : base(
                true,
                new[] { "Consulta executada com sucesso." },
                new QueryResult(null, null, null, null, data != null ? data.Count() : 0, data))
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

            public QueryResult(
                int? pageIndex,
                int? pageSize,
                string orderBy,
                string orderDirection,
                double totalRecords,
                IEnumerable<object> data)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                OrderBy = orderBy;
                OrderDirection = orderDirection;
                TotalRecords = totalRecords;
                Data = data;

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
