using LightNap.Core.AnexoFinanceiro.Dto.Response;
using LightNap.Core.AnexoFinanceiro.Interface;
using LightNap.Core.AnexosFinanceiro.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Exceptions.LightNap.Core.Exceptions;
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
            if (!int.TryParse(financasId, out var idFinanca))
                throw new UserFriendlyApiException("ID da finança inválido.");

            var quantidadeAnexos = _context.AnexosFinanceiros.Count(a => a.FinancasId == idFinanca);
            if (quantidadeAnexos >= 2)
                throw new FileLimitExceededException("Você atingiu o limite máximo de arquivos permitidos.");

            var uploadFolder = Path.Combine("wwwroot", "Uploads", financasId, "Anexos");
            Directory.CreateDirectory(uploadFolder);

            var nomeArquivo = $"{Guid.NewGuid()}_{Path.GetFileName(arquivo.FileName)}";
            var caminhoArquivo = Path.Combine(uploadFolder, nomeArquivo);

            try
            {
                using var stream = new FileStream(caminhoArquivo, FileMode.Create);
                await arquivo.CopyToAsync(stream);
            }
            catch (IOException ioEx)
            {
                throw new UserFriendlyApiException("Erro ao salvar o arquivo no servidor.", ioEx);
            }

            var anexo = new Data.Entities.AnexoFinanceiro
            {
                NomeArquivo = nomeArquivo,
                Caminho = caminhoArquivo,
                TipoArquivo = arquivo.ContentType,
                DataEnvio = DateTime.Now,
                FinancasId = idFinanca
            };

            try
            {
                _context.AnexosFinanceiros.Add(anexo);
                await _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                throw new UserFriendlyApiException("Erro ao salvar o anexo no banco de dados.", dbEx);
            }

            return new AnexoFinanceiroDto
            {
                Id = anexo.Id,
                NomeArquivo = nomeArquivo,
                Caminho = caminhoArquivo,
                TipoArquivo = arquivo.ContentType,
                DataEnvio = anexo.DataEnvio
            };
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
                    Id = a.Id,
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

        public async Task<bool> DeleteAnexoAsync(int anexoId)
        {
            var anexo = await _context.AnexosFinanceiros.FindAsync(anexoId);
            if (anexo == null) return false;

            var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anexo.Caminho.TrimStart('/'));

            if (File.Exists(caminhoCompleto))
                File.Delete(caminhoCompleto);

            _context.AnexosFinanceiros.Remove(anexo);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
