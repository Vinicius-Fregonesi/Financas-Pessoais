
using LightNap.Core.Data.Entities;
using System;

namespace LightNap.Core.Financas_.Dto.Response
{
    public class FinancasDto
    {
        // TODO: Finalize which fields to include when returning this item.
		public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public FinancasTipo? Tipo { get; set; }
        public string Categoria { get; set; }

    }
}
