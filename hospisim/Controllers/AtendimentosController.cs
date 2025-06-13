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
    public class AtendimentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AtendimentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Atendimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtendimentoDTO>>> GetAtendimentos()
        {
            return await _context.Atendimentos
                .Where(a => !a.Excluido) // Filtrar atendimentos excluídos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .Select(a => new AtendimentoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    Tipo = a.Tipo,
                    Status = a.Status,
                    Local = a.Local,
                    ProntuarioId = a.ProntuarioId,
                    PacienteId = a.PacienteId,
                    ProfissionalId = a.ProfissionalId,
                    NumeroProntuario = a.Prontuario.NumeroProntuario,
                    NomePaciente = a.Paciente.NomeCompleto,
                    NomeProfissional = a.Profissional.NomeCompleto,
                    EspecialidadeProfissional = a.Profissional.Especialidade.Nome,
                    TemInternacao = a.Internacao != null,
                    QtdPrescricoes = a.Prescricoes.Count,
                    QtdExames = a.Exames.Count
                })
                .ToListAsync();
        }

        // GET: api/Atendimentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AtendimentoDTO>> GetAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == id && !a.Excluido) // Verificar se não está excluído
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync();

            if (atendimento == null)
            {
                return NotFound();
            }

            var atendimentoDTO = new AtendimentoDTO
            {
                Id = atendimento.Id,
                DataHora = atendimento.DataHora,
                Tipo = atendimento.Tipo,
                Status = atendimento.Status,
                Local = atendimento.Local,
                ProntuarioId = atendimento.ProntuarioId,
                PacienteId = atendimento.PacienteId,
                ProfissionalId = atendimento.ProfissionalId,
                NumeroProntuario = atendimento.Prontuario.NumeroProntuario,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                NomeProfissional = atendimento.Profissional.NomeCompleto,
                EspecialidadeProfissional = atendimento.Profissional.Especialidade?.Nome,
                TemInternacao = atendimento.Internacao != null,
                QtdPrescricoes = atendimento.Prescricoes.Count,
                QtdExames = atendimento.Exames.Count
            };

            return atendimentoDTO;
        }

        // GET: api/Atendimentos/paciente/5
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<AtendimentoDTO>>> GetAtendimentosByPaciente(int pacienteId)
        {
            var atendimentos = await _context.Atendimentos
                .Where(a => a.PacienteId == pacienteId && !a.Excluido) // Filtrar excluídos
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .OrderByDescending(a => a.DataHora)
                .Select(a => new AtendimentoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    Tipo = a.Tipo,
                    Status = a.Status,
                    Local = a.Local,
                    ProntuarioId = a.ProntuarioId,
                    PacienteId = a.PacienteId,
                    ProfissionalId = a.ProfissionalId,
                    NumeroProntuario = a.Prontuario.NumeroProntuario,
                    NomePaciente = a.Paciente.NomeCompleto,
                    NomeProfissional = a.Profissional.NomeCompleto,
                    EspecialidadeProfissional = a.Profissional.Especialidade.Nome,
                    TemInternacao = a.Internacao != null,
                    QtdPrescricoes = a.Prescricoes.Count,
                    QtdExames = a.Exames.Count
                })
                .ToListAsync();

            if (atendimentos == null || !atendimentos.Any())
            {
                return NotFound();
            }

            return atendimentos;
        }

        // GET: api/Atendimentos/profissional/5
        [HttpGet("profissional/{profissionalId}")]
        public async Task<ActionResult<IEnumerable<AtendimentoDTO>>> GetAtendimentosByProfissional(int profissionalId)
        {
            var atendimentos = await _context.Atendimentos
                .Where(a => a.ProfissionalId == profissionalId && !a.Excluido) // Filtrar excluídos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .OrderByDescending(a => a.DataHora)
                .Select(a => new AtendimentoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    Tipo = a.Tipo,
                    Status = a.Status,
                    Local = a.Local,
                    ProntuarioId = a.ProntuarioId,
                    PacienteId = a.PacienteId,
                    ProfissionalId = a.ProfissionalId,
                    NumeroProntuario = a.Prontuario.NumeroProntuario,
                    NomePaciente = a.Paciente.NomeCompleto,
                    NomeProfissional = a.Profissional.NomeCompleto,
                    EspecialidadeProfissional = a.Profissional.Especialidade.Nome,
                    TemInternacao = a.Internacao != null,
                    QtdPrescricoes = a.Prescricoes.Count,
                    QtdExames = a.Exames.Count
                })
                .ToListAsync();

            if (atendimentos == null || !atendimentos.Any())
            {
                return NotFound();
            }

            return atendimentos;
        }

        // PUT: api/Atendimentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtendimento(int id, AtendimentoUpdateDTO atendimentoDTO)
        {
            if (id != atendimentoDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            // buscando o atendimento 
            var atendimentoExistente = await _context.Atendimentos
                .Where(a => a.Id == id && !a.Excluido)
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync();

            if (atendimentoExistente == null)
            {
                return NotFound("Atendimento não encontrado ou está excluído.");
            }

            // atualizando as propriedades 
            if (atendimentoDTO.DataHora.HasValue)
            {
                atendimentoExistente.DataHora = atendimentoDTO.DataHora.Value;
            }
            
            if (!string.IsNullOrEmpty(atendimentoDTO.Tipo))
            {
                atendimentoExistente.Tipo = atendimentoDTO.Tipo;
            }
            
            if (!string.IsNullOrEmpty(atendimentoDTO.Status))
            {
                atendimentoExistente.Status = atendimentoDTO.Status;
            }
            
            if (atendimentoDTO.Local != null)
            {
                atendimentoExistente.Local = atendimentoDTO.Local;
            }

            if (atendimentoDTO.PacienteId.HasValue && atendimentoExistente.PacienteId != atendimentoDTO.PacienteId.Value)
            {
                var paciente = await _context.Pacientes
                    .Where(p => p.Id == atendimentoDTO.PacienteId.Value && !p.Excluido)
                    .FirstOrDefaultAsync();
                    
                if (paciente == null)
                {
                    return BadRequest($"Paciente com ID {atendimentoDTO.PacienteId.Value} não encontrado ou está excluído.");
                }
                atendimentoExistente.PacienteId = atendimentoDTO.PacienteId.Value;
                atendimentoExistente.Paciente = paciente;
            }

            if (atendimentoDTO.ProfissionalId.HasValue && atendimentoExistente.ProfissionalId != atendimentoDTO.ProfissionalId.Value)
            {
                var profissional = await _context.Profissionais
                    .Include(p => p.Especialidade)
                    .FirstOrDefaultAsync(p => p.Id == atendimentoDTO.ProfissionalId.Value);
                    
                if (profissional == null)
                {
                    return BadRequest($"Profissional com ID {atendimentoDTO.ProfissionalId.Value} não encontrado.");
                }
                atendimentoExistente.ProfissionalId = atendimentoDTO.ProfissionalId.Value;
                atendimentoExistente.Profissional = profissional;
            }

            if (atendimentoDTO.ProntuarioId.HasValue && atendimentoExistente.ProntuarioId != atendimentoDTO.ProntuarioId.Value)
            {
                var prontuario = await _context.Prontuarios
                    .Where(p => p.Id == atendimentoDTO.ProntuarioId.Value && !p.Excluido)
                    .FirstOrDefaultAsync();
                    
                if (prontuario == null)
                {
                    return BadRequest($"Prontuário com ID {atendimentoDTO.ProntuarioId.Value} não encontrado ou está excluído.");
                }
                atendimentoExistente.ProntuarioId = atendimentoDTO.ProntuarioId.Value;
                atendimentoExistente.Prontuario = prontuario;
            }

            try
            {
                await _context.SaveChangesAsync();

                // retornando o DTO 
                var responseDTO = new AtendimentoDTO
                {
                    Id = atendimentoExistente.Id,
                    DataHora = atendimentoExistente.DataHora,
                    Tipo = atendimentoExistente.Tipo,
                    Status = atendimentoExistente.Status,
                    Local = atendimentoExistente.Local,
                    ProntuarioId = atendimentoExistente.ProntuarioId,
                    PacienteId = atendimentoExistente.PacienteId,
                    ProfissionalId = atendimentoExistente.ProfissionalId,
                    NumeroProntuario = atendimentoExistente.Prontuario.NumeroProntuario,
                    NomePaciente = atendimentoExistente.Paciente.NomeCompleto,
                    NomeProfissional = atendimentoExistente.Profissional.NomeCompleto,
                    EspecialidadeProfissional = atendimentoExistente.Profissional.Especialidade?.Nome,
                    TemInternacao = atendimentoExistente.Internacao != null,
                    QtdPrescricoes = atendimentoExistente.Prescricoes.Count,
                    QtdExames = atendimentoExistente.Exames.Count
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Atendimentos
        [HttpPost]
        public async Task<ActionResult<AtendimentoDTO>> PostAtendimento(AtendimentoCreateDTO dto)
        {
            // veriicando o paciente      
            var paciente = await _context.Pacientes
                .Where(p => p.Id == dto.PacienteId && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (paciente == null)
            {
                return BadRequest("Paciente não encontrado ou está excluído");
            }

            // veriicando o profissional
            var profissional = await _context.Profissionais
                .Where(p => p.Id == dto.ProfissionalId && !p.Excluido)
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync();
                
            if (profissional == null)
            {
                return BadRequest("Profissional não encontrado ou está excluído");
            }

            // veriicando o prontuário
            var prontuario = await _context.Prontuarios
                .Where(p => p.Id == dto.ProntuarioId && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (prontuario == null)
            {
                return BadRequest("Prontuário não encontrado ou está excluído");
            }

            // veriicando se o prontuário é do paciente
            if (prontuario.PacienteId != dto.PacienteId)
            {
                return BadRequest("O prontuário informado não pertence ao paciente selecionado");
            }

            var atendimento = new Atendimento
            {
                DataHora = dto.DataHora,
                Tipo = dto.Tipo,
                Status = dto.Status,
                Local = dto.Local,
                ProntuarioId = dto.ProntuarioId,
                PacienteId = dto.PacienteId,
                ProfissionalId = dto.ProfissionalId,
                Excluido = false,
                DataExclusao = null
            };

            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            await _context.Entry(atendimento)
                .Collection(a => a.Prescricoes)
                .LoadAsync();
                
            await _context.Entry(atendimento)
                .Collection(a => a.Exames)
                .LoadAsync();
                
            await _context.Entry(atendimento)
                .Reference(a => a.Internacao)
                .LoadAsync();

            var atendimentoDTO = new AtendimentoDTO
            {
                Id = atendimento.Id,
                DataHora = atendimento.DataHora,
                Tipo = atendimento.Tipo,
                Status = atendimento.Status,
                Local = atendimento.Local,
                ProntuarioId = atendimento.ProntuarioId,
                PacienteId = atendimento.PacienteId,
                ProfissionalId = atendimento.ProfissionalId,
                NumeroProntuario = prontuario.NumeroProntuario,
                NomePaciente = paciente.NomeCompleto,
                NomeProfissional = profissional.NomeCompleto,
                EspecialidadeProfissional = profissional.Especialidade?.Nome,
                TemInternacao = atendimento.Internacao != null,
                QtdPrescricoes = atendimento.Prescricoes.Count,
                QtdExames = atendimento.Exames.Count
            };

            return CreatedAtAction("GetAtendimento", new { id = atendimento.Id }, atendimentoDTO);
        }

        // DELETE: api/Atendimentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == id && !a.Excluido)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync();

            if (atendimento == null)
            {
                return NotFound();
            }

            atendimento.Excluido = true;
            atendimento.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Atendimentos/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<AtendimentoDTO>>> GetAtendimentosExcluidos()
        {
            return await _context.Atendimentos
                .Where(a => a.Excluido) // Somente atendimentos excluídos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .Select(a => new AtendimentoDTO
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    Tipo = a.Tipo,
                    Status = a.Status,
                    Local = a.Local,
                    ProntuarioId = a.ProntuarioId,
                    PacienteId = a.PacienteId,
                    ProfissionalId = a.ProfissionalId,
                    NumeroProntuario = a.Prontuario.NumeroProntuario,
                    NomePaciente = a.Paciente.NomeCompleto,
                    NomeProfissional = a.Profissional.NomeCompleto,
                    EspecialidadeProfissional = a.Profissional.Especialidade.Nome,
                    TemInternacao = a.Internacao != null,
                    QtdPrescricoes = a.Prescricoes.Count,
                    QtdExames = a.Exames.Count
                })
                .ToListAsync();
        }

        // POST: api/Atendimentos/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<AtendimentoDTO>> RestaurarAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == id && a.Excluido)
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync();
                
            if (atendimento == null)
            {
                return NotFound("Atendimento não encontrado ou não está excluído");
            }

            // verificando o paciente e prontuário
            var pacienteExcluido = await _context.Pacientes
                .Where(p => p.Id == atendimento.PacienteId && p.Excluido)
                .AnyAsync();
                
            var prontuarioExcluido = await _context.Prontuarios
                .Where(p => p.Id == atendimento.ProntuarioId && p.Excluido)
                .AnyAsync();
                
            if (pacienteExcluido || prontuarioExcluido)
            {
                return BadRequest("Não é possível restaurar o atendimento pois o paciente ou prontuário associado está excluído.");
            }

            atendimento.Excluido = false;
            atendimento.DataExclusao = null;
            await _context.SaveChangesAsync();

            var atendimentoDTO = new AtendimentoDTO
            {
                Id = atendimento.Id,
                DataHora = atendimento.DataHora,
                Tipo = atendimento.Tipo,
                Status = atendimento.Status,
                Local = atendimento.Local,
                ProntuarioId = atendimento.ProntuarioId,
                PacienteId = atendimento.PacienteId,
                ProfissionalId = atendimento.ProfissionalId,
                NumeroProntuario = atendimento.Prontuario.NumeroProntuario,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                NomeProfissional = atendimento.Profissional.NomeCompleto,
                EspecialidadeProfissional = atendimento.Profissional.Especialidade?.Nome,
                TemInternacao = atendimento.Internacao != null,
                QtdPrescricoes = atendimento.Prescricoes.Count,
                QtdExames = atendimento.Exames.Count
            };

            return Ok(atendimentoDTO);
        }

        private bool AtendimentoExists(int id)
        {
            return _context.Atendimentos.Any(e => e.Id == id && !e.Excluido);
        }
    }
}