using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class Prontuario
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string NumeroProntuario { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }
        
        public string? Observacoes { get; set; }
        
        public int PacienteId { get; set; }
        
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; } = null!;

        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}