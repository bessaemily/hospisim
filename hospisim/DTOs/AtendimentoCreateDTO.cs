using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class AtendimentoCreateDTO
    {
        [Required]
        public DateTime DataHora { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Local { get; set; }
        
        [Required]
        public int ProntuarioId { get; set; }
        
        [Required]
        public int PacienteId { get; set; }
        
        [Required]
        public int ProfissionalId { get; set; }
    }
}