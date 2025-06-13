using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        
        public string NomeCompleto { get; set; } = string.Empty;
        
        public string CPF { get; set; } = string.Empty;
        
        public DateTime DataNascimento { get; set; }
        
        public string Sexo { get; set; } = string.Empty;
        
        public string? TipoSanguineo { get; set; }
        
        public string? Telefone { get; set; }
        
        public string? Email { get; set; }
        
        public string? EnderecoCompleto { get; set; }
        
        public string? NumeroCartaoSUS { get; set; }
        
        public string? EstadoCivil { get; set; }
        
        public bool PossuiPlanoSaude { get; set; }
        
        public int Idade => CalcularIdade(DataNascimento);
        
        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            
            if (dataNascimento.Date > hoje.AddYears(-idade))
                idade--;
                
            return idade;
        }
    }
}