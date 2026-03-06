using ResidentialExpenseControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ResidentialExpenseControl.Api.ViewModels.Transaction
{
    public class SearchTransactionViewModel
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Transaction description (contains)
        /// </summary>
        [MaxLength(400, ErrorMessage = "The transaction description must be at most 400 characters long.")]
        public string? Description { get; set; }

        /// <summary>
        /// Transaction amount (positive number)
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "The transaction amount must be greater than zero.")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// Transaction type (Expense or Income)
        /// </summary>
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? Type { get; set; }

        /// <summary>
        /// Category ID associated with the transaction
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Person ID associated with the transaction
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Page index to retrieve (starting from 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The page index must be greater than or equal to 1.")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Number of records to be returned per page
        /// </summary>
        [Range(1, 200, ErrorMessage = "The page size must be between 1 and 200.")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Property name to sort the retrieved records
        /// </summary>
        [EnumDataType(typeof(TransactionOrderBy), ErrorMessage = "Invalid sort type.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sort direction (ASC for ascending; DESC for descending)
        /// </summary>
        [RegularExpression("^(ASC|DESC)$", ErrorMessage = "The sort direction must be ASC or DESC.")]
        public string? OrderDirection { get; set; }
    }
}
