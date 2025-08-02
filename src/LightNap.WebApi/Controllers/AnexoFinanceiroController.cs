using LightNap.Core.AnexoFinanceiro.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexoFinanceiroController(IAnexosFinanceiroService anexoFinanceiroService) : ControllerBase
    {
        [HttpPost("UploadArquivo")]
        public async Task<IActionResult> UploadArquivo(IFormFile arquivo, string financaId)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            var anexoFinanceiro = await anexoFinanceiroService.CreateAnexoFinanceiroAsync(arquivo,financaId);
            
            return Ok(new { Mensagem = "Arquivo enviado com sucesso!", NomedoArquivo = anexoFinanceiro.NomeArquivo });
        }
    }
}
