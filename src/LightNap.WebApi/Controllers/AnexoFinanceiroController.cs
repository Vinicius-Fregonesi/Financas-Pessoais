using LightNap.Core.AnexoFinanceiro.Interface;
using LightNap.Core.AnexoFinanceiro.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexoFinanceiroController : ControllerBase
    {
        private readonly IAnexosFinanceiroService anexoFinanceiroService;

        public AnexoFinanceiroController(IAnexosFinanceiroService anexoFinanceiroService)
        {
            this.anexoFinanceiroService = anexoFinanceiroService;
        }

        [HttpPost("UploadArquivo")]
        public async Task<IActionResult> UploadArquivo([FromForm] IFormFile arquivo, [FromForm] string financaId)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            if (!int.TryParse(financaId, out int financaIdInt))
                return BadRequest("ID da finança inválido.");

            var anexoFinanceiro = await anexoFinanceiroService.CreateAnexoFinanceiroAsync(arquivo, financaId);

            return Ok(new { Mensagem = "Arquivo enviado com sucesso!", NomedoArquivo = anexoFinanceiro.NomeArquivo });
        }

        [HttpGet("ListarAnexos")]
        public async Task<IActionResult> ListarAnexos([FromQuery] string financasId)
        {
            if (string.IsNullOrEmpty(financasId))
                return BadRequest("FinancasId é obrigatório.");

            var anexos = await anexoFinanceiroService.GetAnexosFinanceirosAsync(financasId);

            return Ok(anexos);
        }

        [HttpGet("DownloadAnexo")]
        public async Task<IActionResult> DownloadAnexo([FromQuery] string caminho)
        {
            var arquivo = await anexoFinanceiroService.DownloadAnexoAsync(caminho);
            if (arquivo == null)
                return NotFound("Arquivo não encontrado.");

            return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeArquivo);
        }

    }

}
