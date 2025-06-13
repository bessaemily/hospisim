using System;

namespace hospisim.DTOs
{
    public class InternacaoUpdateDTO
    {
        public int Id { get; set; }
        
        public DateTime? DataEntrada { get; set; }
        
        public DateTime? PrevisaoAlta { get; set; }
        
        public string? MotivoInternacao { get; set; }
        
        public string? Leito { get; set; }
        
        public string? Quarto { get; set; }
        
        public string? Setor { get; set; }
        
        public bool? PlanoSaudeUtilizado { get; set; }
        
        public string? ObservacoesClinicas { get; set; }
        
        public string? StatusInternacao { get; set; }
    }
}