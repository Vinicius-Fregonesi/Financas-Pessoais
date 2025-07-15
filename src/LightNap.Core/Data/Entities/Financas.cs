using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightNap.Core.Data.Entities
{
    public enum FinancasTipo
    {
        Receita,
        Despesa
    }
    public class Financas
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "O valor é obrigatório.")]
        public decimal Valor { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        public FinancasTipo Tipo { get; set; }

        [MaxLength(50)]
        public string Categoria { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }


        [ForeignKey("ApplicationUserId")]
        public ApplicationUser Usuario { get; set; }
    }
}
