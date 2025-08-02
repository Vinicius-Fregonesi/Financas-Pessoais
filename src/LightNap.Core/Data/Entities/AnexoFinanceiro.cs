using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNap.Core.Data.Entities
{
    public class AnexoFinanceiro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NomeArquivo { get; set; }

        [Required]
        public string Caminho { get; set; } // caminho no sistema ou link de armazenamento (ex: S3, Azure Blob)

        [Required]
        public string TipoArquivo { get; set; } // "application/pdf", "image/jpeg", etc.

        [Required]
        public DateTime DataEnvio { get; set; } = DateTime.Now;

        [Required]
        public int FinancasId { get; set; }

        [ForeignKey("FinancasId")]
        public Financas Financa { get; set; }
    }

}
