using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hospisim.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

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

        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }

        public virtual ICollection<Prontuario> Prontuarios { get; set; } = new List<Prontuario>();
        public virtual ICollection<Internacao> Internacoes { get; set; } = new List<Internacao>();
    }
}