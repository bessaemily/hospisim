using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Profissional
    {
        [Key]
        public int Id { get; set; }

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

        public int EspecialidadeId { get; set; }
        
        [ForeignKey("EspecialidadeId")]
        public virtual Especialidade Especialidade { get; set; } = null!;

        public virtual ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();
        public virtual ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
        
        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}
