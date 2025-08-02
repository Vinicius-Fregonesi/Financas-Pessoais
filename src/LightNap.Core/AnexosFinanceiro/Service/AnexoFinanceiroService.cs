using LightNap.Core.AnexoFinanceiro.Dto.Response;
using LightNap.Core.AnexoFinanceiro.Interface;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace LightNap.Core.AnexosFinanceiro.Service
{
    internal class AnexosFinanceiroService : IAnexosFinanceiroService
    {
        private readonly ApplicationDbContext _context;

        public AnexosFinanceiroService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnexoFinanceiroDto> CreateAnexoFinanceiroAsync(IFormFile arquivo, string financasId)
        {
            try
            {
                // Cria diretório de upload
                string uploadFolder = Path.Combine("Uploads", financasId, "Anexos");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string nomeArquivo = $"{Guid.NewGuid()}_{Path.GetFileName(arquivo.FileName)}";
                string caminhoArquivo = Path.Combine(uploadFolder, nomeArquivo);

                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                // Salvar no banco
                var anexo = new Data.Entities.AnexoFinanceiro
                {
                    NomeArquivo = nomeArquivo,
                    Caminho = caminhoArquivo,
                    TipoArquivo = arquivo.ContentType,
                    DataEnvio = DateTime.Now,
                    FinancasId = int.Parse(financasId)
                };

                _context.AnexosFinanceiros.Add(anexo);
                await _context.SaveChangesAsync();

                // Retorna DTO
                return new AnexoFinanceiroDto
                {
                    NomeArquivo = nomeArquivo,
                    Caminho = caminhoArquivo,
                    TipoArquivo = arquivo.ContentType,
                    DataEnvio = anexo.DataEnvio
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao salvar o anexo no banco de dados.", ex);
            }
        }

        public Task<ICollection<AnexoFinanceiroDto>> GetAnexosFinanceirosAsync(string FinancasId)
        {
            throw new NotImplementedException();
        }
    }
}
