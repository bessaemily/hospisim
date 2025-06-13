using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class PrescricaoCreateDTO
    {
        [Required]
        public int AtendimentoId { get; set; }
        
        [Required]
        public int ProfissionalId { get; set; }
        
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
    }
}