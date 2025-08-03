using LightNap.Core.AnexoFinanceiro.Dto.Response;
using LightNap.Core.AnexosFinanceiro.Dto.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNap.Core.AnexoFinanceiro.Interface
{
    public interface IAnexosFinanceiroService
    {
        Task<ICollection<AnexoFinanceiroDto>> GetAnexosFinanceirosAsync(string FinancasId);
        Task<AnexoFinanceiroDto> CreateAnexoFinanceiroAsync(IFormFile arquivo, string FinancasId);
        Task<ArquivoDownloadDto?> DownloadAnexoAsync(string caminho);
        Task<bool> DeleteAnexoAsync(int anexoId);

    }
}
