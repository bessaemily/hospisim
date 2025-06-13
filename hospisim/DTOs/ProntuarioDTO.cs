using System;

namespace hospisim.DTOs
{
    public class ProntuarioDTO
    {
        public int Id { get; set; }
        
        public string NumeroProntuario { get; set; } = string.Empty;
        
        public DateTime DataCriacao { get; set; }
        
        public string? Observacoes { get; set; }
        
        public int PacienteId { get; set; }
        
        public string? NomePaciente { get; set; }
        
        public int DiasDesdeAbertura => CalcularDiasDesdeAbertura(DataCriacao);
        
        private int CalcularDiasDesdeAbertura(DateTime dataCriacao)
        {
            return (int)(DateTime.Now - dataCriacao).TotalDays;
        }
    }
}