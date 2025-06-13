using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Internacao
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; } = null!;

        public int AtendimentoId { get; set; }
        
        [ForeignKey("AtendimentoId")]
        public virtual Atendimento Atendimento { get; set; } = null!;

        [Required]
        public DateTime DataEntrada { get; set; }
        
        public DateTime? PrevisaoAlta { get; set; }
        
        [StringLength(255)]
        public string? MotivoInternacao { get; set; }
        
        [StringLength(50)]
        public string? Leito { get; set; }
        
        [StringLength(50)]
        public string? Quarto { get; set; }
        
        [StringLength(100)]
        public string? Setor { get; set; }
        
        public bool PlanoSaudeUtilizado { get; set; }
        
        public string? ObservacoesClinicas { get; set; }
        
        [StringLength(50)]
        public string? StatusInternacao { get; set; }
        
        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}