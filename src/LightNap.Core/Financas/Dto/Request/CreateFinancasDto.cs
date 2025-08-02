
using LightNap.Core.Data.Entities;
using System;

namespace LightNap.Core.Financas_.Dto.Request
{

    public class CreateFinancasDto
    {
        // TODO: Update which fields to include when creating this item.
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public FinancasTipo Tipo { get; set; } 
        public string Categoria { get; set; }

    }
}