using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Atendimento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty; // emergência, consulta, internação

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty; // realizado, em andamento, cancelado

        [StringLength(100)]
        public string? Local { get; set; }

        public int ProntuarioId { get; set; }
        
        [ForeignKey("ProntuarioId")]
        public virtual Prontuario Prontuario { get; set; } = null!;

        public int PacienteId { get; set; }
        
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; } = null!;

        public int ProfissionalId { get; set; }
        
        [ForeignKey("ProfissionalId")]
        public virtual Profissional Profissional { get; set; } = null!;

        public virtual ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
        public virtual ICollection<Exame> Exames { get; set; } = new List<Exame>();
        
        public virtual Internacao? Internacao { get; set; }

        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}