using System;

namespace hospisim.DTOs
{
    public class AtendimentoDTO
    {
        public int Id { get; set; }
        
        public DateTime DataHora { get; set; }
        
        public string Tipo { get; set; } = string.Empty;
        
        public string Status { get; set; } = string.Empty;
        
        public string? Local { get; set; }
        
        public int ProntuarioId { get; set; }
        public int PacienteId { get; set; }
        public int ProfissionalId { get; set; }
        
        public string? NumeroProntuario { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeProfissional { get; set; }
        public string? EspecialidadeProfissional { get; set; }
        
        public bool TemInternacao { get; set; }
        public int QtdPrescricoes { get; set; }
        public int QtdExames { get; set; }
        
        public string TempoDecorrido => CalcularTempoDecorrido(DataHora);
        
        private string CalcularTempoDecorrido(DateTime dataHora)
        {
            var agora = DateTime.Now;
            var span = agora - dataHora;
            
            if (span.TotalDays > 365)
                return $"{(int)(span.TotalDays / 365)} ano(s) atrás";
            if (span.TotalDays > 30)
                return $"{(int)(span.TotalDays / 30)} mês(es) atrás";
            if (span.TotalDays > 1)
                return $"{(int)span.TotalDays} dia(s) atrás";
            if (span.TotalHours > 1)
                return $"{(int)span.TotalHours} hora(s) atrás";
            if (span.TotalMinutes > 1)
                return $"{(int)span.TotalMinutes} minuto(s) atrás";
            
            return "Agora";
        }
    }
}