using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNap.Core.AnexoFinanceiro.Dto.Response
{
    public class AnexoFinanceiroDto
    {
        public int Id { get; set; }

        public string NomeArquivo { get; set; }

        public string Caminho { get; set; } // caminho no sistema ou link de armazenamento (ex: S3, Azure Blob)

        public string TipoArquivo { get; set; } 

        public DateTime DataEnvio { get; set; } = DateTime.Now;
        public int FinancasId { get; set; }
    }
}
