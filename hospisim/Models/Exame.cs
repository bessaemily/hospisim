using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Exame
    {
        [Key]
        public int Id { get; set; }

        public int AtendimentoId { get; set; }
        
        [ForeignKey("AtendimentoId")]
        public virtual Atendimento Atendimento { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        public DateTime DataSolicitacao { get; set; }

        public DateTime? DataRealizacao { get; set; }

        public string? Resultado { get; set; }
        
        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}