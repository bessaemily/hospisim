using System;

namespace hospisim.DTOs
{
    public class ProntuarioUpdateDTO
    {
        public string NumeroProntuario { get; set; } = string.Empty;
        
        public string? Observacoes { get; set; }
        
    }
}