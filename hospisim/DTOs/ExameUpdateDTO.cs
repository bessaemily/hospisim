using System;

namespace hospisim.DTOs
{
    public class ExameUpdateDTO
    {
        public int Id { get; set; }
        
        public string? Tipo { get; set; }
        
        public DateTime? DataSolicitacao { get; set; }
        
        public DateTime? DataRealizacao { get; set; }
        
        public string? Resultado { get; set; }
        
        public int? AtendimentoId { get; set; }
    }
}