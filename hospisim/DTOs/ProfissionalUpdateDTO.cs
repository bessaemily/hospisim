using System;

namespace hospisim.DTOs
{
    public class ProfissionalUpdateDTO
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string RegistroConselho { get; set; } = string.Empty;
        public string? TipoRegistro { get; set; }
        public DateTime DataAdmissao { get; set; }
        public int CargaHorariaSemanal { get; set; }
        public string? Turno { get; set; }
        public bool Ativo { get; set; }
        public int EspecialidadeId { get; set; }
    }
}