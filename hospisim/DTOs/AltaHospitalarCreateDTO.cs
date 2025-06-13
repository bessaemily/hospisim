using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class AltaHospitalarCreateDTO
    {
        [Required]
        public int InternacaoId { get; set; }
        
        [Required]
        public DateTime Data { get; set; }
        
        [StringLength(255)]
        public string? CondicaoPaciente { get; set; }
        
        public string? InstrucoesPosAlta { get; set; }
    }
}