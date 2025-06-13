using System;

namespace hospisim.DTOs
{
    public class ExameDTO
    {
        public int Id { get; set; }
        
        public int AtendimentoId { get; set; }
        
        public string Tipo { get; set; } = string.Empty;
        
        public DateTime DataSolicitacao { get; set; }
        
        public DateTime? DataRealizacao { get; set; }
        
        public string? Resultado { get; set; }
        
        public string? NomePaciente { get; set; }
        
        public string? NomeProfissional { get; set; }
        
        public string Status => CalcularStatus();
        
        public string TempoDesdeASolicitacao => CalcularTempoDesdeASolicitacao();
        
        private string CalcularStatus()
        {
            if (DataRealizacao.HasValue && !string.IsNullOrEmpty(Resultado))
                return "Concluído";
            
            if (DataRealizacao.HasValue)
                return "Realizado (aguardando resultado)";
                
            return "Pendente";
        }
        
        private string CalcularTempoDesdeASolicitacao()
        {
            var agora = DateTime.Now;
            var span = agora - DataSolicitacao;
            
            if (span.TotalDays > 30)
                return $"{(int)(span.TotalDays / 30)} mês(es)";
            if (span.TotalDays > 1)
                return $"{(int)span.TotalDays} dia(s)";
            if (span.TotalHours > 1)
                return $"{(int)span.TotalHours} hora(s)";
                
            return $"{(int)span.TotalMinutes} minuto(s)";
        }
    }
}