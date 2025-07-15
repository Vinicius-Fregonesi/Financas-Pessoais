using System.Security.Claims;
using LightNap.Core.Api;
using LightNap.Core.Financas_.Interfaces;
using LightNap.Core.Financas_.Dto.Request;
using LightNap.Core.Financas_.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancasController(IFinancasService financasService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<FinancasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<FinancasDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFinancas(int id)
        {
            var financa = await financasService.GetFinancasAsync(id);

            if (financa is null)
                return NotFound(ApiResponseDto<FinancasDto>.Fail("Finança não encontrada."));

            return Ok(ApiResponseDto<FinancasDto>.Success(financa));
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDto<PagedResponse<FinancasDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchFinancas([FromBody] SearchFinancasDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponseDto<string>.Fail("Usuário não autenticado."));
            }

            // Passa o userId para o service garantir que busca só as finanças desse usuário
            var result = await financasService.SearchFinancasAsync(dto, userId);
            return Ok(new ApiResponseDto<PagedResponse<FinancasDto>>(result));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<FinancasDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateFinancas([FromBody] CreateFinancasDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponseDto<string>.Fail("Usuário não autenticado."));
            }

            var created = await financasService.CreateFinancasAsync(dto, userId);
            return CreatedAtAction(nameof(GetFinancas), new { id = created.Id }, ApiResponseDto<FinancasDto>.Success(created));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<FinancasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<FinancasDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFinancas(int id, [FromBody] UpdateFinancasDto dto)
        {
            var updated = await financasService.UpdateFinancasAsync(id, dto);

            if (updated is null)
                return NotFound(ApiResponseDto<FinancasDto>.Fail("Finança não encontrada."));

            return Ok(ApiResponseDto<FinancasDto>.Success(updated));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFinancas(int id)
        {
            await financasService.DeleteFinancasAsync(id);
            return NoContent();
        }
    }
}
