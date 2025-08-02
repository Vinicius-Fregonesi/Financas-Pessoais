
using LightNap.Core.Api;
using LightNap.Core.Data.Entities;
using System;

namespace LightNap.Core.Financas_.Dto.Request
{
    public class SearchFinancasDto : PaginationRequestDtoBase
    {
        // TODO: Update to reflect which fields to include for searches.
        public string? Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }

        public FinancasTipo? Tipo { get; set; }
        public string? Categoria { get; set; }


    }
}