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
    public class ProntuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProntuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prontuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProntuarioDTO>>> GetProntuarios()
        {
            return await _context.Prontuarios
                .Where(p => !p.Excluido) // Filtrar prontuários excluídos
                .Include(p => p.Paciente)
                .Select(p => new ProntuarioDTO
                {
                    Id = p.Id,
                    NumeroProntuario = p.NumeroProntuario,
                    DataCriacao = p.DataCriacao,
                    Observacoes = p.Observacoes,
                    PacienteId = p.PacienteId,
                    NomePaciente = p.Paciente.NomeCompleto
                })
                .ToListAsync();
        }

        // GET: api/Prontuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProntuarioDTO>> GetProntuario(int id)
        {
            var prontuario = await _context.Prontuarios
                .Where(p => p.Id == id && !p.Excluido) // Verificar se não está excluído
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync();

            if (prontuario == null)
            {
                return NotFound();
            }   

            var prontuarioDTO = new ProntuarioDTO
            {
                Id = prontuario.Id,
                NumeroProntuario = prontuario.NumeroProntuario,
                DataCriacao = prontuario.DataCriacao,
                Observacoes = prontuario.Observacoes,
                PacienteId = prontuario.PacienteId,
                NomePaciente = prontuario.Paciente?.NomeCompleto
            };

            return prontuarioDTO;
        }

        // GET: api/Prontuarios/paciente/5
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<ProntuarioDTO>>> GetProntuariosByPaciente(int pacienteId)
        {
            var prontuarios = await _context.Prontuarios
                .Where(p => p.PacienteId == pacienteId && !p.Excluido) // Filtrar excluídos
                .Include(p => p.Paciente)
                .Select(p => new ProntuarioDTO
                {
                    Id = p.Id,
                    NumeroProntuario = p.NumeroProntuario,
                    DataCriacao = p.DataCriacao,
                    Observacoes = p.Observacoes,
                    PacienteId = p.PacienteId,
                    NomePaciente = p.Paciente.NomeCompleto
                })
                .ToListAsync();

            if (prontuarios == null || !prontuarios.Any())
            {
                return NotFound();
            }

            return prontuarios;
        }
                
        // PUT: api/Prontuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProntuario(int id, ProntuarioUpdateDTO prontuarioDTO)
        {
            var prontuarioExistente = await _context.Prontuarios
                .Where(p => p.Id == id && !p.Excluido)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync();

            if (prontuarioExistente == null)
            {
                return NotFound("Prontuário não encontrado ou está excluído.");
            }

            prontuarioExistente.NumeroProntuario = prontuarioDTO.NumeroProntuario;
            prontuarioExistente.Observacoes = prontuarioDTO.Observacoes;

            try
            {
                await _context.SaveChangesAsync();
                
                var responseDTO = new ProntuarioDTO
                {
                    Id = prontuarioExistente.Id,
                    NumeroProntuario = prontuarioExistente.NumeroProntuario,
                    DataCriacao = prontuarioExistente.DataCriacao,
                    Observacoes = prontuarioExistente.Observacoes,
                    PacienteId = prontuarioExistente.PacienteId,
                    NomePaciente = prontuarioExistente.Paciente?.NomeCompleto
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Prontuarios
        [HttpPost]
        public async Task<ActionResult<ProntuarioDTO>> PostProntuario(ProntuarioCreateDTO dto)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == dto.PacienteId && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (paciente == null)
            {
                return BadRequest("Paciente não encontrado ou está excluído");
            }

            var prontuario = new Prontuario
            {
                NumeroProntuario = string.IsNullOrEmpty(dto.NumeroProntuario) 
                    ? $"PRONT-{DateTime.Now:yyyy}-{await _context.Prontuarios.CountAsync() + 1001}" 
                    : dto.NumeroProntuario,
                    
                DataCriacao = dto.DataCriacao ?? DateTime.Now,
                Observacoes = dto.Observacoes,
                PacienteId = dto.PacienteId,
                Excluido = false,
                DataExclusao = null
            };

            _context.Prontuarios.Add(prontuario);
            await _context.SaveChangesAsync();

            var prontuarioDTO = new ProntuarioDTO
            {
                Id = prontuario.Id,
                NumeroProntuario = prontuario.NumeroProntuario,
                DataCriacao = prontuario.DataCriacao,
                Observacoes = prontuario.Observacoes,
                PacienteId = prontuario.PacienteId,
                NomePaciente = paciente.NomeCompleto
            };

            return CreatedAtAction("GetProntuario", new { id = prontuario.Id }, prontuarioDTO);
        }

        // DELETE: api/Prontuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProntuario(int id)
        {
            var prontuario = await _context.Prontuarios
                .Where(p => p.Id == id && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (prontuario == null)
            {
                return NotFound();
            }
            prontuario.Excluido = true;
            prontuario.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Prontuarios/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<ProntuarioDTO>>> GetProntuariosExcluidos()
        {
            return await _context.Prontuarios
                .Where(p => p.Excluido) // Somente prontuários excluídos
                .Include(p => p.Paciente)
                .Select(p => new ProntuarioDTO
                {
                    Id = p.Id,
                    NumeroProntuario = p.NumeroProntuario,
                    DataCriacao = p.DataCriacao,
                    Observacoes = p.Observacoes,
                    PacienteId = p.PacienteId,
                    NomePaciente = p.Paciente.NomeCompleto
                })
                .ToListAsync();
        }

        // POST: api/Prontuarios/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<ProntuarioDTO>> RestaurarProntuario(int id)
        {
            var prontuario = await _context.Prontuarios
                .Where(p => p.Id == id && p.Excluido)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync();
                
            if (prontuario == null)
            {
                return NotFound("Prontuário não encontrado ou não está excluído");
            }

            prontuario.Excluido = false;
            prontuario.DataExclusao = null;
            await _context.SaveChangesAsync();

            var prontuarioDTO = new ProntuarioDTO
            {
                Id = prontuario.Id,
                NumeroProntuario = prontuario.NumeroProntuario,
                DataCriacao = prontuario.DataCriacao,
                Observacoes = prontuario.Observacoes,
                PacienteId = prontuario.PacienteId,
                NomePaciente = prontuario.Paciente.NomeCompleto
            };

            return Ok(prontuarioDTO);
        }

        private bool ProntuarioExists(int id)
        {
            return _context.Prontuarios.Any(e => e.Id == id && !e.Excluido);
        }
    }
}