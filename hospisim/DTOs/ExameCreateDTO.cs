using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class ExameCreateDTO
    {
        [Required]
        public int AtendimentoId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Tipo { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataSolicitacao { get; set; }
        
        public DateTime? DataRealizacao { get; set; }
        
        public string? Resultado { get; set; }
    }
}