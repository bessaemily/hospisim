using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hospisim.Data;
using hospisim.Models;
using hospisim.DTOs;

namespace hospisim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AltasHospitalaresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AltasHospitalaresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AltasHospitalares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AltaHospitalarDTO>>> GetAltasHospitalares()
        {
            return await _context.AltasHospitalares
                .Where(a => !a.Excluido) // Filtrar altas excluídas
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .Select(a => new AltaHospitalarDTO
                {
                    Id = a.Id,
                    InternacaoId = a.InternacaoId,
                    Data = a.Data,
                    CondicaoPaciente = a.CondicaoPaciente,
                    InstrucoesPosAlta = a.InstrucoesPosAlta,
                    PacienteId = a.Internacao.PacienteId,
                    NomePaciente = a.Internacao.Paciente.NomeCompleto,
                    DataEntradaInternacao = a.Internacao.DataEntrada,
                    Leito = a.Internacao.Leito,
                    Quarto = a.Internacao.Quarto,
                    Setor = a.Internacao.Setor,
                    MotivoInternacao = a.Internacao.MotivoInternacao
                })
                .ToListAsync();
        }

        // GET: api/AltasHospitalares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AltaHospitalarDTO>> GetAltaHospitalar(int id)
        {
            var altaHospitalar = await _context.AltasHospitalares
                .Where(a => a.Id == id && !a.Excluido) // Verificar se não está excluída
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .FirstOrDefaultAsync();

            if (altaHospitalar == null)
            {
                return NotFound();
            }

            var altaDTO = new AltaHospitalarDTO
            {
                Id = altaHospitalar.Id,
                InternacaoId = altaHospitalar.InternacaoId,
                Data = altaHospitalar.Data,
                CondicaoPaciente = altaHospitalar.CondicaoPaciente,
                InstrucoesPosAlta = altaHospitalar.InstrucoesPosAlta,
                PacienteId = altaHospitalar.Internacao.PacienteId,
                NomePaciente = altaHospitalar.Internacao.Paciente.NomeCompleto,
                DataEntradaInternacao = altaHospitalar.Internacao.DataEntrada,
                Leito = altaHospitalar.Internacao.Leito,
                Quarto = altaHospitalar.Internacao.Quarto,
                Setor = altaHospitalar.Internacao.Setor,
                MotivoInternacao = altaHospitalar.Internacao.MotivoInternacao
            };

            return altaDTO;
        }

        // GET: api/AltasHospitalares/internacao/5
        [HttpGet("internacao/{internacaoId}")]
        public async Task<ActionResult<AltaHospitalarDTO>> GetAltaHospitalarByInternacao(int internacaoId)
        {
            var altaHospitalar = await _context.AltasHospitalares
                .Where(a => a.InternacaoId == internacaoId && !a.Excluido) // Filtrar excluídos
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .FirstOrDefaultAsync();

            if (altaHospitalar == null)
            {
                return NotFound();
            }

            var altaDTO = new AltaHospitalarDTO
            {
                Id = altaHospitalar.Id,
                InternacaoId = altaHospitalar.InternacaoId,
                Data = altaHospitalar.Data,
                CondicaoPaciente = altaHospitalar.CondicaoPaciente,
                InstrucoesPosAlta = altaHospitalar.InstrucoesPosAlta,
                PacienteId = altaHospitalar.Internacao.PacienteId,
                NomePaciente = altaHospitalar.Internacao.Paciente.NomeCompleto,
                DataEntradaInternacao = altaHospitalar.Internacao.DataEntrada,
                Leito = altaHospitalar.Internacao.Leito,
                Quarto = altaHospitalar.Internacao.Quarto,
                Setor = altaHospitalar.Internacao.Setor,
                MotivoInternacao = altaHospitalar.Internacao.MotivoInternacao
            };

            return altaDTO;
        }

        // GET: api/AltasHospitalares/paciente/5
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<AltaHospitalarDTO>>> GetAltasHospitalaresByPaciente(int pacienteId)
        {
            var altasHospitalares = await _context.AltasHospitalares
                .Where(a => a.Internacao.PacienteId == pacienteId && !a.Excluido) // Filtrar excluídos
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .OrderByDescending(a => a.Data)
                .Select(a => new AltaHospitalarDTO
                {
                    Id = a.Id,
                    InternacaoId = a.InternacaoId,
                    Data = a.Data,
                    CondicaoPaciente = a.CondicaoPaciente,
                    InstrucoesPosAlta = a.InstrucoesPosAlta,
                    PacienteId = a.Internacao.PacienteId,
                    NomePaciente = a.Internacao.Paciente.NomeCompleto,
                    DataEntradaInternacao = a.Internacao.DataEntrada,
                    Leito = a.Internacao.Leito,
                    Quarto = a.Internacao.Quarto,
                    Setor = a.Internacao.Setor,
                    MotivoInternacao = a.Internacao.MotivoInternacao
                })
                .ToListAsync();

            if (altasHospitalares == null || !altasHospitalares.Any())
            {
                return NotFound();
            }

            return altasHospitalares;
        }

        // PUT: api/AltasHospitalares/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AltaHospitalarDTO>> PutAltaHospitalar(int id, AltaHospitalarUpdateDTO altaDTO)
        {
            if (id != altaDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            // buscando a alta hospitalar existente
            var altaExistente = await _context.AltasHospitalares
                .Where(a => a.Id == id && !a.Excluido)
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .FirstOrDefaultAsync();
                
            if (altaExistente == null)
            {
                return NotFound("Alta hospitalar não encontrada ou está excluída.");
            }

            // atualizando as propriedades
            if (altaDTO.Data.HasValue)
                altaExistente.Data = altaDTO.Data.Value;
                
            if (altaDTO.CondicaoPaciente != null)
                altaExistente.CondicaoPaciente = altaDTO.CondicaoPaciente;
                
            if (altaDTO.InstrucoesPosAlta != null)
                altaExistente.InstrucoesPosAlta = altaDTO.InstrucoesPosAlta;

            try
            {
                await _context.SaveChangesAsync();
                
                var responseDTO = new AltaHospitalarDTO
                {
                    Id = altaExistente.Id,
                    InternacaoId = altaExistente.InternacaoId,
                    Data = altaExistente.Data,
                    CondicaoPaciente = altaExistente.CondicaoPaciente,
                    InstrucoesPosAlta = altaExistente.InstrucoesPosAlta,
                    PacienteId = altaExistente.Internacao.PacienteId,
                    NomePaciente = altaExistente.Internacao.Paciente.NomeCompleto,
                    DataEntradaInternacao = altaExistente.Internacao.DataEntrada,
                    Leito = altaExistente.Internacao.Leito,
                    Quarto = altaExistente.Internacao.Quarto,
                    Setor = altaExistente.Internacao.Setor,
                    MotivoInternacao = altaExistente.Internacao.MotivoInternacao
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/AltasHospitalares
        [HttpPost]
        public async Task<ActionResult<AltaHospitalarDTO>> PostAltaHospitalar(AltaHospitalarCreateDTO dto)
        {
            // verificando se a iternação existe
            var internacao = await _context.Internacoes
                .Where(i => i.Id == dto.InternacaoId && !i.Excluido)
                .Include(i => i.Paciente)
                .FirstOrDefaultAsync();
                
            if (internacao == null)
            {
                return BadRequest("Internação não encontrada ou está excluída");
            }

            // verificando se  alta já existe
            var altaExistente = await _context.AltasHospitalares
                .Where(a => a.InternacaoId == dto.InternacaoId && !a.Excluido)
                .AnyAsync();
                
            if (altaExistente)
            {
                return BadRequest("Esta internação já possui uma alta registrada");
            }

            // verificando o status da internação
            if (internacao.StatusInternacao == "Finalizada")
            {
                return BadRequest("Não é possível registrar alta para uma internação já finalizada");
            }

            var altaHospitalar = new AltaHospitalar
            {
                InternacaoId = dto.InternacaoId,
                Data = dto.Data == default ? DateTime.Now : dto.Data,
                CondicaoPaciente = dto.CondicaoPaciente,
                InstrucoesPosAlta = dto.InstrucoesPosAlta,
                Excluido = false,
                DataExclusao = null
            };

            // atualizando o status da internação
            internacao.StatusInternacao = "Finalizada";
            _context.Entry(internacao).State = EntityState.Modified;

            // adicionando a alta hospitalar
            _context.AltasHospitalares.Add(altaHospitalar);
            await _context.SaveChangesAsync();

            var altaDTO = new AltaHospitalarDTO
            {
                Id = altaHospitalar.Id,
                InternacaoId = altaHospitalar.InternacaoId,
                Data = altaHospitalar.Data,
                CondicaoPaciente = altaHospitalar.CondicaoPaciente,
                InstrucoesPosAlta = altaHospitalar.InstrucoesPosAlta,
                PacienteId = internacao.PacienteId,
                NomePaciente = internacao.Paciente.NomeCompleto,
                DataEntradaInternacao = internacao.DataEntrada,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                MotivoInternacao = internacao.MotivoInternacao
            };

            return CreatedAtAction("GetAltaHospitalar", new { id = altaHospitalar.Id }, altaDTO);
        }

        // DELETE: api/AltasHospitalares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAltaHospitalar(int id)
        {
            var altaHospitalar = await _context.AltasHospitalares
                .Where(a => a.Id == id && !a.Excluido)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync();
                
            if (altaHospitalar == null)
            {
                return NotFound();
            }

            // mudando o status da internação se a alta for excluída
            var internacao = altaHospitalar.Internacao;
            if (internacao.StatusInternacao == "Finalizada")
            {
                internacao.StatusInternacao = "Em andamento";
                _context.Entry(internacao).State = EntityState.Modified;
            }

            altaHospitalar.Excluido = true;
            altaHospitalar.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/AltasHospitalares/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<AltaHospitalarDTO>>> GetAltasHospitalaresExcluidas()
        {
            return await _context.AltasHospitalares
                .Where(a => a.Excluido) // Somente altas excluídas
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .Select(a => new AltaHospitalarDTO
                {
                    Id = a.Id,
                    InternacaoId = a.InternacaoId,
                    Data = a.Data,
                    CondicaoPaciente = a.CondicaoPaciente,
                    InstrucoesPosAlta = a.InstrucoesPosAlta,
                    PacienteId = a.Internacao.PacienteId,
                    NomePaciente = a.Internacao.Paciente.NomeCompleto,
                    DataEntradaInternacao = a.Internacao.DataEntrada,
                    Leito = a.Internacao.Leito,
                    Quarto = a.Internacao.Quarto,
                    Setor = a.Internacao.Setor,
                    MotivoInternacao = a.Internacao.MotivoInternacao
                })
                .ToListAsync();
        }

        // POST: api/AltasHospitalares/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<AltaHospitalarDTO>> RestaurarAltaHospitalar(int id)
        {
            var altaHospitalar = await _context.AltasHospitalares
                .Where(a => a.Id == id && a.Excluido)
                .Include(a => a.Internacao)
                    .ThenInclude(i => i.Paciente)
                .FirstOrDefaultAsync();
                
            if (altaHospitalar == null)
            {
                return NotFound("Alta hospitalar não encontrada ou não está excluída");
            }

            // verifficando se a  internação não está excluída
            if (altaHospitalar.Internacao.Excluido)
            {
                return BadRequest("Não é possível restaurar a alta hospitalar pois a internação associada está excluída.");
            }

            // mudando o status da internação
            var internacao = altaHospitalar.Internacao;
            internacao.StatusInternacao = "Finalizada";
            _context.Entry(internacao).State = EntityState.Modified;

            // mudando alta
            altaHospitalar.Excluido = false;
            altaHospitalar.DataExclusao = null;
            await _context.SaveChangesAsync();

            var altaDTO = new AltaHospitalarDTO
            {
                Id = altaHospitalar.Id,
                InternacaoId = altaHospitalar.InternacaoId,
                Data = altaHospitalar.Data,
                CondicaoPaciente = altaHospitalar.CondicaoPaciente,
                InstrucoesPosAlta = altaHospitalar.InstrucoesPosAlta,
                PacienteId = altaHospitalar.Internacao.PacienteId,
                NomePaciente = altaHospitalar.Internacao.Paciente.NomeCompleto,
                DataEntradaInternacao = altaHospitalar.Internacao.DataEntrada,
                Leito = altaHospitalar.Internacao.Leito,
                Quarto = altaHospitalar.Internacao.Quarto,
                Setor = altaHospitalar.Internacao.Setor,
                MotivoInternacao = altaHospitalar.Internacao.MotivoInternacao
            };

            return Ok(altaDTO);
        }

        private bool AltaHospitalarExists(int id)
        {
            return _context.AltasHospitalares.Any(e => e.Id == id && !e.Excluido);  
        }
    }
}