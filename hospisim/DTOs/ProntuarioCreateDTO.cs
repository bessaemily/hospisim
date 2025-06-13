using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class ProntuarioCreateDTO
    {
        [StringLength(50)]
        public string? NumeroProntuario { get; set; }
        
        public DateTime? DataCriacao { get; set; }
        
        public string? Observacoes { get; set; }
        
        [Required]
        public int PacienteId { get; set; }
    }
}