using System;

namespace hospisim.DTOs
{
    public class AltaHospitalarUpdateDTO
    {
        public int Id { get; set; }
        
        public DateTime? Data { get; set; }
        
        public string? CondicaoPaciente { get; set; }
        
        public string? InstrucoesPosAlta { get; set; }
    }
}