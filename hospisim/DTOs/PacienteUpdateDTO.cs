using System;

namespace hospisim.DTOs
{
    public class PacienteUpdateDTO
    {
        public string? NomeCompleto { get; set; }
        public string? CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Sexo { get; set; }
        public string? TipoSanguineo { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? EnderecoCompleto { get; set; }
        public string? NumeroCartaoSUS { get; set; }
        public string? EstadoCivil { get; set; }
        public bool? PossuiPlanoSaude { get; set; }
    }
}