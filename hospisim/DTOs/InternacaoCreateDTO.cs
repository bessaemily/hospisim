using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class InternacaoCreateDTO
    {
        [Required]
        public int PacienteId { get; set; }
        
        [Required]
        public int AtendimentoId { get; set; }
        
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
    }
}