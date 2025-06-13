using System.Collections.Generic;

namespace hospisim.DTOs
{
    public class EspecialidadeDTO
    {
        public int Id { get; set; }
        
        public string Nome { get; set; } = string.Empty;
        
        public string? Descricao { get; set; }
        
        public int QuantidadeProfissionais { get; set; }
    }
}