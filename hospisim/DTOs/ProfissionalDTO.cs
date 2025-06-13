using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class ProfissionalDTO
    {
        public int Id { get; set; }
        
        public string NomeCompleto { get; set; } = string.Empty;
        
        public string CPF { get; set; } = string.Empty;
        
        public string? Email { get; set; }
        
        public string? Telefone { get; set; }
        
        public string RegistroConselho { get; set; } = string.Empty;
        
        public string? TipoRegistro { get; set; }
        
        public DateTime DataAdmissao { get; set; }
        
        public int CargaHorariaSemanal { get; set; }
        
        public string? Turno { get; set; }
        
        public bool Ativo { get; set; }
        
        public int EspecialidadeId { get; set; }

        public string? NomeEspecialidade { get; set; }

        public int AnosDeServico => CalcularAnosDeServico(DataAdmissao);
        
        private int CalcularAnosDeServico(DateTime dataAdmissao)
        {
            var hoje = DateTime.Today;
            var anos = hoje.Year - dataAdmissao.Year;
            
            if (dataAdmissao.Date > hoje.AddYears(-anos))
                anos--;
                
            return anos;
        }
    }
}