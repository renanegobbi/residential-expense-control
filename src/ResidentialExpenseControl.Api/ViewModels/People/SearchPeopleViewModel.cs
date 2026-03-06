using ResidentialExpenseControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ResidentialExpenseControl.Api.ViewModels.People
{
    public class SearchPeopleViewModel
    {
        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string? Name { get; set; }

        /// <summary>
        /// Idade exata da pessoa
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser maior ou igual a zero.")]
        public int? Age { get; set; }

        /// <summary>
        /// Idade mínima (inclusive)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "A idade mínima deve ser maior ou igual a zero.")]
        public int? MinAge { get; set; }

        /// <summary>
        /// Idade máxima (inclusive)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "A idade máxima deve ser maior ou igual a zero.")]
        public int? MaxAge { get; set; }


        /// <summary>
        /// Index da página que deseja obter (iniciando por 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O índice da página deve ser maior ou igual a 1.")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Quantidade de registros que deverão ser retornados por página
        /// </summary>
        [Range(1, 200, ErrorMessage = "O tamanho da página deve estar entre 1 e 200.")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Nome da propriedade que se deseja ordenar os registros encontrados
        /// </summary>
        [EnumDataType(typeof(PersonOrderBy), ErrorMessage = "O tipo de ordenação é inválido")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PersonOrderBy? OrderBy { get; set; }

        /// <summary>
        /// Sentido da ordenação (ASC para crescente; DESC para decrescente)
        /// </summary>
        [RegularExpression("^(ASC|DESC)$", ErrorMessage = "O sentido de ordenação deve ser ASC ou DESC.")]
        public string? OrderDirection { get; set; }
    }
}
