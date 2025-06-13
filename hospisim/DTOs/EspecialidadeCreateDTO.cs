using System.ComponentModel.DataAnnotations;

namespace hospisim.DTOs
{
    public class EspecialidadeCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }
    }
}