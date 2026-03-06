using ResidentialExpenseControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ResidentialExpenseControl.Api.ViewModels.Totals
{
    public class TotalsFilterByPeopleViewModel
    {
        /// <summary>
        /// Page index to retrieve (starting from 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O índice da página deve ser maior ou igual a 1.")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Number of records to be returned per page
        /// </summary>
        [Range(1, 200, ErrorMessage = "O tamanho da página deve estar entre 1 e 200.")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Property name to sort the retrieved records
        /// </summary>
        [EnumDataType(typeof(TotalsPersonOrderBy), ErrorMessage = "O tipo de ordenação é inválido")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TotalsPersonOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC for ascending; DESC for descending)
        /// </summary>
        [RegularExpression("^(ASC|DESC)$", ErrorMessage = "O sentido de ordenação deve ser ASC ou DESC.")]
        public string? OrderDirection { get; set; }
    }
}
