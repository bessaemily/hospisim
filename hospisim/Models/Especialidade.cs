using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hospisim.Models
{
    public class Especialidade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }

        public virtual ICollection<Profissional> Profissionais { get; set; } = new List<Profissional>();

        public bool Excluido { get; set; } = false;
        public DateTime? DataExclusao { get; set; }
    }
}