using LightNap.Core.Api;
using LightNap.Core.Financas_.Dto.Request;
using LightNap.Core.Financas_.Dto.Response;

namespace LightNap.Core.Financas_.Interfaces
{
    public interface IFinancasService
    {
        Task<FinancasDto?> GetFinancasAsync(int id);

        Task<PagedResponse<FinancasDto>> SearchFinancasAsync(SearchFinancasDto dto, string applicationUserId);

        Task<FinancasDto> CreateFinancasAsync(CreateFinancasDto dto, string applicationUserId);

        Task<FinancasDto> UpdateFinancasAsync(int id, UpdateFinancasDto dto);

        Task DeleteFinancasAsync(int id);
    }
}
