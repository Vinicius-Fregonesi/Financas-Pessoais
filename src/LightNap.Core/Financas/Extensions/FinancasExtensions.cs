
using LightNap.Core.Data.Entities;
using LightNap.Core.Financas_.Dto.Request;
using LightNap.Core.Financas_.Dto.Response;

namespace LightNap.Core.Financas_.Extensions
{
    public static class FinancasExtensions
    {
        public static Financas ToCreate(this CreateFinancasDto dto)
        {
            // TODO: Update these fields to match the DTO.
            var item = new Financas()
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Data = dto.Data,
                Tipo = dto.Tipo,
                Categoria = dto.Categoria,
            };  
            return item;
        }

        public static FinancasDto ToDto(this Financas item)
        {
            // TODO: Update these fields to match the DTO.
            var dto = new FinancasDto()
            {
                Id = item.Id,
                Descricao = item.Descricao,
                Valor = item.Valor,
                Data = item.Data,
                Tipo = item.Tipo,
                Categoria = item.Categoria,
            };
            return dto;
        }

        public static void UpdateFromDto(this Financas item, UpdateFinancasDto dto)
        {
            // TODO: Update these fields to match the DTO.
            item.Descricao = dto.Descricao;
            item.Valor = dto.Valor;
            item.Data = dto.Data;
            item.Tipo = dto.Tipo;
            item.Categoria = dto.Categoria;
        }
    }
}