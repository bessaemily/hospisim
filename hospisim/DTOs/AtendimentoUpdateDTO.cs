using System;

namespace hospisim.DTOs
{
    public class AtendimentoUpdateDTO
    {
        public int Id { get; set; }
        
        public DateTime? DataHora { get; set; }
        
        public string? Tipo { get; set; }
        
        public string? Status { get; set; }
        
        public string? Local { get; set; }
        
        public int? ProntuarioId { get; set; }
        
        public int? PacienteId { get; set; }
        
        public int? ProfissionalId { get; set; }
    }
}