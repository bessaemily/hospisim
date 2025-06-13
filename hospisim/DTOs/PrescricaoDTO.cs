using System;

namespace hospisim.DTOs
{
    public class PrescricaoDTO
    {
        public int Id { get; set; }
        
        public int AtendimentoId { get; set; }
        
        public int ProfissionalId { get; set; }
        
        public string Medicamento { get; set; } = string.Empty;
        
        public string? Dosagem { get; set; }
        
        public string? Frequencia { get; set; }
        
        public string? ViaAdministracao { get; set; }
        
        public DateTime DataInicio { get; set; }
        
        public DateTime? DataFim { get; set; }
        
        public string? Observacoes { get; set; }
        
        public string? StatusPrescricao { get; set; }
        
        public string? ReacoesAdversas { get; set; }
        
        public string? NomeProfissional { get; set; }
        
        public string? EspecialidadeProfissional { get; set; }
        
        public string? NomePaciente { get; set; }
        
        public DateTime DataAtendimento { get; set; }
        
        public string StatusAtual => CalcularStatusAtual();
        
        public int DiasAtivos => CalcularDiasAtivos();
        
        private string CalcularStatusAtual()
        {
            if (StatusPrescricao?.ToLower() == "cancelada")
                return "Cancelada";
                
            if (DataFim.HasValue && DataFim.Value < DateTime.Now)
                return "Finalizada";
                
            return "Ativa";
        }
        
        private int CalcularDiasAtivos()
        {
            var dataFinal = DataFim.HasValue ? DataFim.Value : DateTime.Now;
            
            if (StatusPrescricao?.ToLower() == "cancelada")
                return 0;
                
            if (dataFinal < DataInicio)
                return 0;
                
            return (int)Math.Ceiling((dataFinal - DataInicio).TotalDays);
        }
    }
}