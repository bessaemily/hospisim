using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Prescricao
    {
        [Key]
        public int Id { get; set; }

        public int AtendimentoId { get; set; }
        
        [ForeignKey("AtendimentoId")]
        public virtual Atendimento Atendimento { get; set; } = null!;

        public int ProfissionalId { get; set; }
        
        [ForeignKey("ProfissionalId")]
        public virtual Profissional Profissional { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Medicamento { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Dosagem { get; set; }

        [StringLength(100)]
        public string? Frequencia { get; set; }

        [StringLength(50)]
        public string? ViaAdministracao { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string? Observacoes { get; set; }

        [StringLength(50)]
        public string? StatusPrescricao { get; set; }

        public string? ReacoesAdversas { get; set; }
        
        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}