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
    public class PrescricoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescricoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prescricoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescricaoDTO>>> GetPrescricoes()
        {
            return await _context.Prescricoes
                .Where(p => !p.Excluido) // Filtrar prescrições excluídas
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Select(p => new PrescricaoDTO
                {
                    Id = p.Id,
                    AtendimentoId = p.AtendimentoId,
                    ProfissionalId = p.ProfissionalId,
                    Medicamento = p.Medicamento,
                    Dosagem = p.Dosagem,
                    Frequencia = p.Frequencia,
                    ViaAdministracao = p.ViaAdministracao,
                    DataInicio = p.DataInicio,
                    DataFim = p.DataFim,
                    Observacoes = p.Observacoes,
                    StatusPrescricao = p.StatusPrescricao,
                    ReacoesAdversas = p.ReacoesAdversas,
                    NomeProfissional = p.Profissional.NomeCompleto,
                    EspecialidadeProfissional = p.Profissional.Especialidade.Nome,
                    NomePaciente = p.Atendimento.Paciente.NomeCompleto,
                    DataAtendimento = p.Atendimento.DataHora
                })
                .ToListAsync();
        }

        // GET: api/Prescricoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescricaoDTO>> GetPrescricao(int id)
        {
            var prescricao = await _context.Prescricoes
                .Where(p => p.Id == id && !p.Excluido) // Verificar se não está excluída
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .FirstOrDefaultAsync();

            if (prescricao == null)
            {
                return NotFound();
            }

            var prescricaoDTO = new PrescricaoDTO
            {
                Id = prescricao.Id,
                AtendimentoId = prescricao.AtendimentoId,
                ProfissionalId = prescricao.ProfissionalId,
                Medicamento = prescricao.Medicamento,
                Dosagem = prescricao.Dosagem,
                Frequencia = prescricao.Frequencia,
                ViaAdministracao = prescricao.ViaAdministracao,
                DataInicio = prescricao.DataInicio,
                DataFim = prescricao.DataFim,
                Observacoes = prescricao.Observacoes,
                StatusPrescricao = prescricao.StatusPrescricao,
                ReacoesAdversas = prescricao.ReacoesAdversas,
                NomeProfissional = prescricao.Profissional.NomeCompleto,
                EspecialidadeProfissional = prescricao.Profissional.Especialidade.Nome,
                NomePaciente = prescricao.Atendimento.Paciente.NomeCompleto,
                DataAtendimento = prescricao.Atendimento.DataHora
            };

            return prescricaoDTO;
        }

        // GET: api/Prescricoes/atendimento/5
        [HttpGet("atendimento/{atendimentoId}")]
        public async Task<ActionResult<IEnumerable<PrescricaoDTO>>> GetPrescricoesByAtendimento(int atendimentoId)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == atendimentoId && !a.Excluido) // Verificar se não está excluído
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync();
                
            if (atendimento == null)
            {
                return BadRequest("Atendimento não encontrado ou está excluído");
            }

            var prescricoes = await _context.Prescricoes
                .Where(p => p.AtendimentoId == atendimentoId && !p.Excluido) // Filtrar excluídos
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .OrderByDescending(p => p.DataInicio)
                .ToListAsync();

            if (prescricoes == null || !prescricoes.Any())
            {
                return NotFound();
            }

            var prescricoesDTO = prescricoes.Select(p => new PrescricaoDTO
            {
                Id = p.Id,
                AtendimentoId = p.AtendimentoId,
                ProfissionalId = p.ProfissionalId,
                Medicamento = p.Medicamento,
                Dosagem = p.Dosagem,
                Frequencia = p.Frequencia,
                ViaAdministracao = p.ViaAdministracao,
                DataInicio = p.DataInicio,
                DataFim = p.DataFim,
                Observacoes = p.Observacoes,
                StatusPrescricao = p.StatusPrescricao,
                ReacoesAdversas = p.ReacoesAdversas,
                NomeProfissional = p.Profissional.NomeCompleto,
                EspecialidadeProfissional = p.Profissional.Especialidade.Nome,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                DataAtendimento = atendimento.DataHora
            }).ToList();

            return prescricoesDTO;
        }

        // GET: api/Prescricoes/profissional/5
        [HttpGet("profissional/{profissionalId}")]
        public async Task<ActionResult<IEnumerable<PrescricaoDTO>>> GetPrescricoesByProfissional(int profissionalId)
        {
            var profissional = await _context.Profissionais
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync(p => p.Id == profissionalId);
                
            if (profissional == null)
            {
                return BadRequest("Profissional não encontrado");
            }

            var prescricoes = await _context.Prescricoes
                .Where(p => p.ProfissionalId == profissionalId && !p.Excluido) // Filtrar excluídos
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .OrderByDescending(p => p.DataInicio)
                .ToListAsync();

            if (prescricoes == null || !prescricoes.Any())
            {
                return NotFound();
            }

            var prescricoesDTO = prescricoes.Select(p => new PrescricaoDTO
            {
                Id = p.Id,
                AtendimentoId = p.AtendimentoId,
                ProfissionalId = p.ProfissionalId,
                Medicamento = p.Medicamento,
                Dosagem = p.Dosagem,
                Frequencia = p.Frequencia,
                ViaAdministracao = p.ViaAdministracao,
                DataInicio = p.DataInicio,
                DataFim = p.DataFim,
                Observacoes = p.Observacoes,
                StatusPrescricao = p.StatusPrescricao,
                ReacoesAdversas = p.ReacoesAdversas,
                NomeProfissional = profissional.NomeCompleto,
                EspecialidadeProfissional = profissional.Especialidade.Nome,
                NomePaciente = p.Atendimento.Paciente.NomeCompleto,
                DataAtendimento = p.Atendimento.DataHora
            }).ToList();        

            return prescricoesDTO;
        }

        // PUT: api/Prescricoes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PrescricaoDTO>> PutPrescricao(int id, PrescricaoUpdateDTO prescricaoDTO)
        {
            if (id != prescricaoDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            var prescricaoExistente = await _context.Prescricoes
                .Where(p => p.Id == id && !p.Excluido)
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .FirstOrDefaultAsync();
                
            if (prescricaoExistente == null)
            {
                return NotFound("Prescrição não encontrada ou está excluída.");
            }

            if (prescricaoDTO.Medicamento != null)
                prescricaoExistente.Medicamento = prescricaoDTO.Medicamento;
                
            if (prescricaoDTO.Dosagem != null)
                prescricaoExistente.Dosagem = prescricaoDTO.Dosagem;
                
            if (prescricaoDTO.Frequencia != null)
                prescricaoExistente.Frequencia = prescricaoDTO.Frequencia;
                
            if (prescricaoDTO.ViaAdministracao != null)
                prescricaoExistente.ViaAdministracao = prescricaoDTO.ViaAdministracao;
                
            if (prescricaoDTO.DataInicio.HasValue)
                prescricaoExistente.DataInicio = prescricaoDTO.DataInicio.Value;
                
            if (prescricaoDTO.DataFim != null) // Usando null em vez de HasValue para permitir definir como null
                prescricaoExistente.DataFim = prescricaoDTO.DataFim;
                
            if (prescricaoDTO.Observacoes != null)
                prescricaoExistente.Observacoes = prescricaoDTO.Observacoes;
                
            if (prescricaoDTO.StatusPrescricao != null)
                prescricaoExistente.StatusPrescricao = prescricaoDTO.StatusPrescricao;
                
            if (prescricaoDTO.ReacoesAdversas != null)
                prescricaoExistente.ReacoesAdversas = prescricaoDTO.ReacoesAdversas;

            if (prescricaoDTO.ProfissionalId.HasValue && prescricaoExistente.ProfissionalId != prescricaoDTO.ProfissionalId.Value)
            {
                var profissional = await _context.Profissionais
                    .Include(p => p.Especialidade)
                    .FirstOrDefaultAsync(p => p.Id == prescricaoDTO.ProfissionalId.Value);
                    
                if (profissional == null)
                {
                    return BadRequest($"Profissional com ID {prescricaoDTO.ProfissionalId.Value} não encontrado.");
                }
                
                prescricaoExistente.ProfissionalId = prescricaoDTO.ProfissionalId.Value;
                prescricaoExistente.Profissional = profissional;
            }

            try
            {
                await _context.SaveChangesAsync();
                
                var responseDTO = new PrescricaoDTO
                {
                    Id = prescricaoExistente.Id,
                    AtendimentoId = prescricaoExistente.AtendimentoId,
                    ProfissionalId = prescricaoExistente.ProfissionalId,
                    Medicamento = prescricaoExistente.Medicamento,
                    Dosagem = prescricaoExistente.Dosagem,
                    Frequencia = prescricaoExistente.Frequencia,
                    ViaAdministracao = prescricaoExistente.ViaAdministracao,
                    DataInicio = prescricaoExistente.DataInicio,
                    DataFim = prescricaoExistente.DataFim,
                    Observacoes = prescricaoExistente.Observacoes,
                    StatusPrescricao = prescricaoExistente.StatusPrescricao,
                    ReacoesAdversas = prescricaoExistente.ReacoesAdversas,
                    NomeProfissional = prescricaoExistente.Profissional.NomeCompleto,
                    EspecialidadeProfissional = prescricaoExistente.Profissional.Especialidade.Nome,
                    NomePaciente = prescricaoExistente.Atendimento.Paciente.NomeCompleto,
                    DataAtendimento = prescricaoExistente.Atendimento.DataHora
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Prescricoes
        [HttpPost]
        public async Task<ActionResult<PrescricaoDTO>> PostPrescricao(PrescricaoCreateDTO dto)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == dto.AtendimentoId && !a.Excluido)
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync();
                
            if (atendimento == null)
            {
                return BadRequest("Atendimento não encontrado ou está excluído");
            }

            var profissional = await _context.Profissionais
                .Where(p => p.Id == dto.ProfissionalId && !p.Excluido)
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync(p => p.Ativo);
                
            if (profissional == null)
            {
                return BadRequest("Profissional não encontrado, está excluído ou não está ativo");
            }

            if (dto.DataInicio == default)
            {
                dto.DataInicio = DateTime.Now;
            }

            if (string.IsNullOrEmpty(dto.StatusPrescricao))
            {
                dto.StatusPrescricao = "Ativo";
            }

            var prescricao = new Prescricao
            {
                AtendimentoId = dto.AtendimentoId,
                ProfissionalId = dto.ProfissionalId,
                Medicamento = dto.Medicamento,
                Dosagem = dto.Dosagem,
                Frequencia = dto.Frequencia,
                ViaAdministracao = dto.ViaAdministracao,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Observacoes = dto.Observacoes,
                StatusPrescricao = dto.StatusPrescricao,
                ReacoesAdversas = dto.ReacoesAdversas,
                Excluido = false,
                DataExclusao = null
            };

            _context.Prescricoes.Add(prescricao);
            await _context.SaveChangesAsync();

            var prescricaoDTO = new PrescricaoDTO
            {
                Id = prescricao.Id,
                AtendimentoId = prescricao.AtendimentoId,
                ProfissionalId = prescricao.ProfissionalId,
                Medicamento = prescricao.Medicamento,
                Dosagem = prescricao.Dosagem,
                Frequencia = prescricao.Frequencia,
                ViaAdministracao = prescricao.ViaAdministracao,
                DataInicio = prescricao.DataInicio,
                DataFim = prescricao.DataFim,
                Observacoes = prescricao.Observacoes,
                StatusPrescricao = prescricao.StatusPrescricao,
                ReacoesAdversas = prescricao.ReacoesAdversas,
                NomeProfissional = profissional.NomeCompleto,
                EspecialidadeProfissional = profissional.Especialidade.Nome,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                DataAtendimento = atendimento.DataHora
            };

            return CreatedAtAction("GetPrescricao", new { id = prescricao.Id }, prescricaoDTO);
        }

        // DELETE: api/Prescricoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescricao(int id)
        {
            var prescricao = await _context.Prescricoes
                .Where(p => p.Id == id && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (prescricao == null)
            {
                return NotFound();
            }

            prescricao.Excluido = true;
            prescricao.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Prescricoes/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<PrescricaoDTO>>> GetPrescricoesExcluidas()
        {
            return await _context.Prescricoes
                .Where(p => p.Excluido) // Somente prescrições excluídas
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .Select(p => new PrescricaoDTO
                {
                    Id = p.Id,
                    AtendimentoId = p.AtendimentoId,
                    ProfissionalId = p.ProfissionalId,
                    Medicamento = p.Medicamento,
                    Dosagem = p.Dosagem,
                    Frequencia = p.Frequencia,
                    ViaAdministracao = p.ViaAdministracao,
                    DataInicio = p.DataInicio,
                    DataFim = p.DataFim,
                    Observacoes = p.Observacoes,
                    StatusPrescricao = p.StatusPrescricao,
                    ReacoesAdversas = p.ReacoesAdversas,
                    NomeProfissional = p.Profissional.NomeCompleto,
                    EspecialidadeProfissional = p.Profissional.Especialidade.Nome,
                    NomePaciente = p.Atendimento.Paciente.NomeCompleto,
                    DataAtendimento = p.Atendimento.DataHora
                })
                .ToListAsync();
        }

        // POST: api/Prescricoes/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<PrescricaoDTO>> RestaurarPrescricao(int id)
        {
            var prescricao = await _context.Prescricoes
                .Where(p => p.Id == id && p.Excluido)
                .Include(p => p.Atendimento)
                    .ThenInclude(a => a.Paciente)
                .Include(p => p.Profissional)
                    .ThenInclude(p => p.Especialidade)
                .FirstOrDefaultAsync();
                
            if (prescricao == null)
            {
                return NotFound("Prescrição não encontrada ou não está excluída");
            }

            var atendimentoExcluido = await _context.Atendimentos
                .Where(a => a.Id == prescricao.AtendimentoId && a.Excluido)
                .AnyAsync();
                
            var profissionalInativo = await _context.Profissionais
                .Where(p => p.Id == prescricao.ProfissionalId && !p.Ativo)
                .AnyAsync();
                
            if (atendimentoExcluido)
            {
                return BadRequest("Não é possível restaurar a prescrição pois o atendimento associado está excluído.");
            }
            
            if (profissionalInativo)
            {
                return BadRequest("Atenção: O profissional associado a esta prescrição não está mais ativo.");
            }

            prescricao.Excluido = false;
            prescricao.DataExclusao = null;
            await _context.SaveChangesAsync();

            var prescricaoDTO = new PrescricaoDTO
            {
                Id = prescricao.Id,
                AtendimentoId = prescricao.AtendimentoId,
                ProfissionalId = prescricao.ProfissionalId,
                Medicamento = prescricao.Medicamento,
                Dosagem = prescricao.Dosagem,
                Frequencia = prescricao.Frequencia,
                ViaAdministracao = prescricao.ViaAdministracao,
                DataInicio = prescricao.DataInicio,
                DataFim = prescricao.DataFim,
                Observacoes = prescricao.Observacoes,
                StatusPrescricao = prescricao.StatusPrescricao,
                ReacoesAdversas = prescricao.ReacoesAdversas,
                NomeProfissional = prescricao.Profissional.NomeCompleto,
                EspecialidadeProfissional = prescricao.Profissional.Especialidade.Nome,
                NomePaciente = prescricao.Atendimento.Paciente.NomeCompleto,
                DataAtendimento = prescricao.Atendimento.DataHora
            };

            return Ok(prescricaoDTO);
        }

        private bool PrescricaoExists(int id)
        {
            return _context.Prescricoes.Any(e => e.Id == id && !e.Excluido);
        }
    }
}