using System;

namespace hospisim.DTOs
{
    public class AltaHospitalarDTO
    {
        public int Id { get; set; }
        
        public int InternacaoId { get; set; }
        
        public DateTime Data { get; set; }
        
        public string? CondicaoPaciente { get; set; }
        
        public string? InstrucoesPosAlta { get; set; }
        
        public int PacienteId { get; set; }
        
        public string? NomePaciente { get; set; }
        
        public DateTime DataEntradaInternacao { get; set; }
        
        public int DiasInternado => CalcularDiasInternado();
        
        public string? Leito { get; set; }
        
        public string? Quarto { get; set; }
        
        public string? Setor { get; set; }
        
        public string? MotivoInternacao { get; set; }
        
        public int DiasDesdeAlta => CalcularDiasDesdeAlta();
        
        private int CalcularDiasInternado()
        {
            return (int)Math.Ceiling((Data - DataEntradaInternacao).TotalDays);
        }
        
        private int CalcularDiasDesdeAlta()
        {
            return (int)Math.Ceiling((DateTime.Now - Data).TotalDays);
        }
    }
}