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
    public class ExamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Exames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExameDTO>>> GetExames()
        {
            return await _context.Exames
                .Where(e => !e.Excluido) // Filtrar exames excluídos
                .Include(e => e.Atendimento)        
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .Select(e => new ExameDTO
                {
                    Id = e.Id,
                    AtendimentoId = e.AtendimentoId,
                    Tipo = e.Tipo,
                    DataSolicitacao = e.DataSolicitacao,
                    DataRealizacao = e.DataRealizacao,
                    Resultado = e.Resultado,
                    NomePaciente = e.Atendimento.Paciente.NomeCompleto,
                    NomeProfissional = e.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();
        }

        // GET: api/Exames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExameDTO>> GetExame(int id)
        {
            var exame = await _context.Exames
                .Where(e => e.Id == id && !e.Excluido) // Verificar se não está excluído
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();

            if (exame == null)
            {
                return NotFound();
            }

            var exameDTO = new ExameDTO
            {
                Id = exame.Id,
                AtendimentoId = exame.AtendimentoId,
                Tipo = exame.Tipo,
                DataSolicitacao = exame.DataSolicitacao,
                DataRealizacao = exame.DataRealizacao,
                Resultado = exame.Resultado,
                NomePaciente = exame.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = exame.Atendimento.Profissional.NomeCompleto
            };

            return exameDTO;
        }

        // GET: api/Exames/atendimento/5
        [HttpGet("atendimento/{atendimentoId}")]
        public async Task<ActionResult<IEnumerable<ExameDTO>>> GetExamesByAtendimento(int atendimentoId)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == atendimentoId && !a.Excluido) // Verificar se não está excluído
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .FirstOrDefaultAsync();

            if (atendimento == null)
            {
                return BadRequest("Atendimento não encontrado ou está excluído");
            }

            var exames = await _context.Exames
                .Where(e => e.AtendimentoId == atendimentoId && !e.Excluido) // Filtrar excluídos
                .OrderByDescending(e => e.DataSolicitacao)
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            var examesDTO = exames.Select(e => new ExameDTO
            {
                Id = e.Id,
                AtendimentoId = e.AtendimentoId,
                Tipo = e.Tipo,
                DataSolicitacao = e.DataSolicitacao,
                DataRealizacao = e.DataRealizacao,
                Resultado = e.Resultado,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                NomeProfissional = atendimento.Profissional.NomeCompleto
            }).ToList();

            return examesDTO;
        }

        // GET: api/Exames/paciente/5
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<ExameDTO>>> GetExamesByPaciente(int pacienteId)
        {
            var pacienteExiste = await _context.Pacientes
                .Where(p => p.Id == pacienteId && !p.Excluido)
                .AnyAsync();
                
            if (!pacienteExiste)
            {
                return BadRequest("Paciente não encontrado ou está excluído");
            }
            
            var exames = await _context.Exames
                .Where(e => !e.Excluido && e.Atendimento.PacienteId == pacienteId && !e.Atendimento.Excluido) // Filtrar excluídos
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .OrderByDescending(e => e.DataSolicitacao)
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            var examesDTO = exames.Select(e => new ExameDTO
            {
                Id = e.Id,
                AtendimentoId = e.AtendimentoId,
                Tipo = e.Tipo,
                DataSolicitacao = e.DataSolicitacao,
                DataRealizacao = e.DataRealizacao,
                Resultado = e.Resultado,
                NomePaciente = e.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = e.Atendimento.Profissional.NomeCompleto
            }).ToList();

            return examesDTO;   
        }

        // PUT: api/Exames/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ExameDTO>> PutExame(int id, ExameUpdateDTO exameDTO)
        {
            if (id != exameDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            var exameExistente = await _context.Exames
                .Where(e => e.Id == id && !e.Excluido)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)    
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();

            if (exameExistente == null)
            {
                return NotFound("Exame não encontrado ou está excluído.");
            }

            if (!string.IsNullOrEmpty(exameDTO.Tipo))
            {
                exameExistente.Tipo = exameDTO.Tipo;
            }
            
            if (exameDTO.DataSolicitacao.HasValue)
            {
                exameExistente.DataSolicitacao = exameDTO.DataSolicitacao.Value;
            }
            
            if (exameDTO.DataRealizacao != null) 
            {
                exameExistente.DataRealizacao = exameDTO.DataRealizacao;
            }
            
            if (exameDTO.Resultado != null) 
            {
                exameExistente.Resultado = exameDTO.Resultado;
            }

            if (exameDTO.AtendimentoId.HasValue && exameExistente.AtendimentoId != exameDTO.AtendimentoId.Value)
            {
                var atendimento = await _context.Atendimentos
                    .Where(a => a.Id == exameDTO.AtendimentoId.Value && !a.Excluido)
                    .Include(a => a.Paciente)
                    .Include(a => a.Profissional)
                    .FirstOrDefaultAsync();
                    
                if (atendimento == null)
                {
                    return BadRequest($"Atendimento com ID {exameDTO.AtendimentoId.Value} não encontrado ou está excluído.");
                }
                
                exameExistente.AtendimentoId = exameDTO.AtendimentoId.Value;
                exameExistente.Atendimento = atendimento;
            }

            try
            {
                await _context.SaveChangesAsync();
                
                var responseDTO = new ExameDTO
                {
                    Id = exameExistente.Id,
                    AtendimentoId = exameExistente.AtendimentoId,
                    Tipo = exameExistente.Tipo,
                    DataSolicitacao = exameExistente.DataSolicitacao,
                    DataRealizacao = exameExistente.DataRealizacao,
                    Resultado = exameExistente.Resultado,
                    NomePaciente = exameExistente.Atendimento.Paciente.NomeCompleto,
                    NomeProfissional = exameExistente.Atendimento.Profissional.NomeCompleto
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Exames
        [HttpPost]
        public async Task<ActionResult<ExameDTO>> PostExame(ExameCreateDTO dto)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == dto.AtendimentoId && !a.Excluido)
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .FirstOrDefaultAsync();
                
            if (atendimento == null)
            {
                return BadRequest("Atendimento não encontrado ou está excluído");
            }

            if (dto.DataSolicitacao == default)
            {
                dto.DataSolicitacao = DateTime.Now;
            }

            var exame = new Exame
            {
                AtendimentoId = dto.AtendimentoId,
                Tipo = dto.Tipo,
                DataSolicitacao = dto.DataSolicitacao,
                DataRealizacao = dto.DataRealizacao,
                Resultado = dto.Resultado,
                Excluido = false,
                DataExclusao = null
            };

            _context.Exames.Add(exame);
            await _context.SaveChangesAsync();

            var exameDTO = new ExameDTO
            {
                Id = exame.Id,
                AtendimentoId = exame.AtendimentoId,
                Tipo = exame.Tipo,
                DataSolicitacao = exame.DataSolicitacao,
                DataRealizacao = exame.DataRealizacao,
                Resultado = exame.Resultado,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                NomeProfissional = atendimento.Profissional.NomeCompleto
            };

            return CreatedAtAction("GetExame", new { id = exame.Id }, exameDTO);
        }

        // PUT: api/Exames/5/resultado
        [HttpPut("{id}/resultado")]
        public async Task<ActionResult<ExameDTO>> RegistrarResultadoExame(int id, [FromBody] string resultado)
        {
            var exame = await _context.Exames
                .Where(e => e.Id == id && !e.Excluido) // Verificar se não está excluído
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();
            
            if (exame == null)
            {
                return NotFound("Exame não encontrado ou está excluído");
            }

            exame.Resultado = resultado;
            exame.DataRealizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            var exameDTO = new ExameDTO
            {
                Id = exame.Id,
                AtendimentoId = exame.AtendimentoId,
                Tipo = exame.Tipo,
                DataSolicitacao = exame.DataSolicitacao,
                DataRealizacao = exame.DataRealizacao,
                Resultado = exame.Resultado,
                NomePaciente = exame.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = exame.Atendimento.Profissional.NomeCompleto
            };
            
            return Ok(exameDTO);
        }

        // DELETE: api/Exames/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExame(int id)
        {
            var exame = await _context.Exames
                .Where(e => e.Id == id && !e.Excluido)
                .FirstOrDefaultAsync();
                
            if (exame == null)
            {
                return NotFound();
            }

            exame.Excluido = true;
            exame.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Exames/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<ExameDTO>>> GetExamesExcluidos()
        {
            return await _context.Exames
                .Where(e => e.Excluido) // Somente exames excluídos
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .Select(e => new ExameDTO
                {
                    Id = e.Id,
                    AtendimentoId = e.AtendimentoId,
                    Tipo = e.Tipo,
                    DataSolicitacao = e.DataSolicitacao,
                    DataRealizacao = e.DataRealizacao,
                    Resultado = e.Resultado,
                    NomePaciente = e.Atendimento.Paciente.NomeCompleto,
                    NomeProfissional = e.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();
        }

        // POST: api/Exames/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<ExameDTO>> RestaurarExame(int id)
        {
            var exame = await _context.Exames
                .Where(e => e.Id == id && e.Excluido)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(e => e.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();
                
            if (exame == null)
            {
                return NotFound("Exame não encontrado ou não está excluído");
            }

            if (exame.Atendimento.Excluido)
            {
                return BadRequest("Não é possível restaurar o exame pois o atendimento associado está excluído.");
            }

            exame.Excluido = false;
            exame.DataExclusao = null;
            await _context.SaveChangesAsync();

            var exameDTO = new ExameDTO
            {
                Id = exame.Id,
                AtendimentoId = exame.AtendimentoId,
                Tipo = exame.Tipo,
                DataSolicitacao = exame.DataSolicitacao,
                DataRealizacao = exame.DataRealizacao,
                Resultado = exame.Resultado,
                NomePaciente = exame.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = exame.Atendimento.Profissional.NomeCompleto
            };

            return Ok(exameDTO);
        }

        private bool ExameExists(int id)
        {
            return _context.Exames.Any(e => e.Id == id && !e.Excluido);
        }
    }
}