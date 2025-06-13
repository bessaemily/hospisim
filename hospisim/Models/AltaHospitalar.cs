using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospisim.Models
{
    public class AltaHospitalar
    {
        [Key]
        public int Id { get; set; }

        public int InternacaoId { get; set; }
        
        [ForeignKey("InternacaoId")]
        public virtual Internacao Internacao { get; set; } = null!;

        [Required]
        public DateTime Data { get; set; }

        [StringLength(255)]
        public string? CondicaoPaciente { get; set; }

        public string? InstrucoesPosAlta { get; set; }
        
        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}