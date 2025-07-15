using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Financas_.Extensions;
using LightNap.Core.Financas_.Interfaces;
using LightNap.Core.Financas_.Dto.Request;
using LightNap.Core.Financas_.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Financas_.Services
{
    public class FinancasService(ApplicationDbContext db) : IFinancasService
    {
        public async Task<FinancasDto?> GetFinancasAsync(int id)
        {
            var item = await db.Financas.FindAsync(id);
            return item?.ToDto();
        }

        public async Task<PagedResponse<FinancasDto>> SearchFinancasAsync(SearchFinancasDto dto, string applicationUserId)
        {
            var query = db.Financas.AsQueryable();

            query = query.Where(item => item.ApplicationUserId == applicationUserId);

            if (!string.IsNullOrWhiteSpace(dto.Descricao))
            {
                query = query.Where(item => item.Descricao.Contains(dto.Descricao));
            }

            if (dto.Valor is not null)
            {
                query = query.Where(item => item.Valor == dto.Valor);
            }

            var dataInicio = dto.DataInicio?.Date;
            var dataFinal = dto.DataFinal?.Date.AddDays(1);

            Console.WriteLine($"DataInicio recebida: {dataInicio}");
            Console.WriteLine($"DataFinal recebida: {dataFinal}");

            query = query.Where(item =>
                (!dataInicio.HasValue || item.Data.Date >= dataInicio.Value) &&
                (!dataFinal.HasValue || item.Data.Date < dataFinal.Value)
            );


            if (!string.IsNullOrWhiteSpace(dto.Categoria))
            {
                query = query.Where(item => item.Categoria.Contains(dto.Categoria));
            }

            if (!string.IsNullOrWhiteSpace(dto.Tipo.ToString()))
            {
                query = query.Where(item => item.Tipo == dto.Tipo);
            }

            query = query.OrderBy(item => item.Id);

            int totalCount = await query.CountAsync();

            if (dto.PageNumber > 1)
            {
                query = query.Skip((dto.PageNumber - 1) * dto.PageSize);
            }

            var items = await query
                .Take(dto.PageSize)
                .Select(item => item.ToDto())
                .ToListAsync();

            return new PagedResponse<FinancasDto>(items, dto.PageNumber, dto.PageSize, totalCount);
        }




        public async Task<FinancasDto> CreateFinancasAsync(CreateFinancasDto dto, string applicationUserId)
        {
            if (dto is null)
                throw new UserFriendlyApiException("Dados obrigatórios não fornecidos.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                throw new UserFriendlyApiException("A descrição é obrigatória.");

            if (dto.Descricao.Length > 150)
                throw new UserFriendlyApiException("A descrição não pode ultrapassar 150 caracteres.");

            if (dto.Valor <= 0)
                throw new UserFriendlyApiException("O valor deve ser maior que zero.");

            if (dto.Data == default)
                throw new UserFriendlyApiException("A data é obrigatória.");

            if (!string.IsNullOrWhiteSpace(dto.Categoria) && dto.Categoria.Length > 50)
                throw new UserFriendlyApiException("A categoria não pode ultrapassar 50 caracteres.");

            if (string.IsNullOrWhiteSpace(applicationUserId))
                throw new UserFriendlyApiException("O usuário é obrigatório.");

            var item = dto.ToCreate();

            item.ApplicationUserId = applicationUserId;

            db.Financas.Add(item);
            await db.SaveChangesAsync();

            return item.ToDto();
        }

        public async Task<FinancasDto> UpdateFinancasAsync(int id, UpdateFinancasDto dto)
        {
            if (dto is null)
                throw new UserFriendlyApiException("Dados obrigatórios não fornecidos.");

            var item = await db.Financas.FindAsync(id)
                ?? throw new UserFriendlyApiException("A finança especificada não foi encontrada.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                throw new UserFriendlyApiException("A descrição é obrigatória.");

            if (dto.Descricao.Length > 150)
                throw new UserFriendlyApiException("A descrição não pode ultrapassar 150 caracteres.");

            if (dto.Valor <= 0)
                throw new UserFriendlyApiException("O valor deve ser maior que zero.");

            if (dto.Data == default)
                throw new UserFriendlyApiException("A data é obrigatória.");

            if (!string.IsNullOrWhiteSpace(dto.Categoria) && dto.Categoria.Length > 50)
                throw new UserFriendlyApiException("A categoria não pode ultrapassar 50 caracteres.");

            item.UpdateFromDto(dto);
            await db.SaveChangesAsync();

            return item.ToDto();
        }

        public async Task DeleteFinancasAsync(int id)
        {
            var item = await db.Financas.FindAsync(id);

            if (item is null)
                throw new UserFriendlyApiException("A finança especificada não foi encontrada para exclusão.");

            db.Financas.Remove(item);
            await db.SaveChangesAsync();
        }
    }
}
