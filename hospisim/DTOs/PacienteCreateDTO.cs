using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class PacienteCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(20)]
        public string Sexo { get; set; } = string.Empty;

        [StringLength(10)]
        public string? TipoSanguineo { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? EnderecoCompleto { get; set; }

        [StringLength(20)]
        public string? NumeroCartaoSUS { get; set; }

        [StringLength(20)]
        public string? EstadoCivil { get; set; }

        public bool PossuiPlanoSaude { get; set; }
    }
}