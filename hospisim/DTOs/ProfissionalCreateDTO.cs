using System;
using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class ProfissionalCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string CPF { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [Required]
        [StringLength(50)]
        public string RegistroConselho { get; set; } = string.Empty;

        [StringLength(50)]
        public string? TipoRegistro { get; set; }

        public DateTime DataAdmissao { get; set; }

        public int CargaHorariaSemanal { get; set; }

        [StringLength(50)]
        public string? Turno { get; set; }

        public bool Ativo { get; set; } = true;

        [Required]
        public int EspecialidadeId { get; set; }
    }
}