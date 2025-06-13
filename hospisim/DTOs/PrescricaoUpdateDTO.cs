using System;

namespace hospisim.DTOs
{
    public class PrescricaoUpdateDTO
    {
        public int Id { get; set; }
        
        public int? ProfissionalId { get; set; }
        
        public string? Medicamento { get; set; }
        
        public string? Dosagem { get; set; }
        
        public string? Frequencia { get; set; }
        
        public string? ViaAdministracao { get; set; }
        
        public DateTime? DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
        
        public string? Observacoes { get; set; }
        
        public string? StatusPrescricao { get; set; }
        
        public string? ReacoesAdversas { get; set; }
    }
}