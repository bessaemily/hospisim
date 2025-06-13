using System;

namespace hospisim.DTOs
{
    public class InternacaoDTO
    {
        public int Id { get; set; }
        
        public int PacienteId { get; set; }
        
        public int AtendimentoId { get; set; }
        
        public DateTime DataEntrada { get; set; }
        
        public DateTime? PrevisaoAlta { get; set; }
        
        public string? MotivoInternacao { get; set; }
        
        public string? Leito { get; set; }
        
        public string? Quarto { get; set; }
        
        public string? Setor { get; set; }
        
        public bool PlanoSaudeUtilizado { get; set; }
        
        public string? ObservacoesClinicas { get; set; }
        
        public string? StatusInternacao { get; set; }
        
        public string? NomePaciente { get; set; }
        
        public string? TipoSanguineoPaciente { get; set; }
        
        public DateTime DataHoraAtendimento { get; set; }
        
        public string? NomeProfissional { get; set; }
        
        public int DiasInternado => CalcularDiasInternado();
        
        public bool PossuiAlta => StatusInternacao?.ToLower() == "finalizada";
        
        private int CalcularDiasInternado()
        {
            var dataFim = StatusInternacao?.ToLower() == "finalizada" ? PrevisaoAlta : DateTime.Now;
            return (int)Math.Ceiling((dataFim.GetValueOrDefault(DateTime.Now) - DataEntrada).TotalDays);
        }
    }
}