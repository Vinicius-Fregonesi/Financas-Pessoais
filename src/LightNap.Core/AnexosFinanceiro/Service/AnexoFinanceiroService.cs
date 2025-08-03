using LightNap.Core.AnexoFinanceiro.Dto.Response;
using LightNap.Core.AnexoFinanceiro.Interface;
using LightNap.Core.AnexosFinanceiro.Dto.Response;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace LightNap.Core.AnexoFinanceiro.Service
{
    public class AnexosFinanceiroService : IAnexosFinanceiroService
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
                string uploadFolder = Path.Combine("wwwroot", "Uploads", financasId, "Anexos");
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

        public async Task<ICollection<AnexoFinanceiroDto>> GetAnexosFinanceirosAsync(string financasId)
        {
            try
            {
                int id = int.Parse(financasId);

                var anexos = await Task.Run(() => _context.AnexosFinanceiros
                    .Where(a => a.FinancasId == id)
                    .ToList());

                var anexosDto = anexos.Select(a => new AnexoFinanceiroDto
                {
                    NomeArquivo = a.NomeArquivo,
                    Caminho = $"/Uploads/{financasId}/Anexos/{a.NomeArquivo}",
                    TipoArquivo = a.TipoArquivo,
                    DataEnvio = a.DataEnvio
                }).ToList();

                return anexosDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar anexos financeiros no banco de dados.", ex);
            }
        }


        public async Task<ArquivoDownloadDto?> DownloadAnexoAsync(string caminhoRelativo)
        {
            try
            {
                // Caminho absoluto
                var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminhoRelativo.TrimStart('/'));

                if (!File.Exists(caminhoCompleto))
                    return null;

                var stream = new FileStream(caminhoCompleto, FileMode.Open, FileAccess.Read);
                var contentType = MimeTypes.GetMimeType(caminhoCompleto); // Usa biblioteca ou método auxiliar
                var nomeArquivo = Path.GetFileName(caminhoCompleto);

                return new ArquivoDownloadDto
                {
                    Stream = stream,
                    ContentType = contentType,
                    NomeArquivo = nomeArquivo
                };
            }
            catch
            {
                return null;
            }
        }

    }
}
