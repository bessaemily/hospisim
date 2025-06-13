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
    public class InternacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InternacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Internacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternacaoDTO>>> GetInternacoes()
        {       
            return await _context.Internacoes
                .Where(i => !i.Excluido) // Filtrar internações excluídas
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)        
                    .ThenInclude(a => a.Profissional)
                .Select(i => new InternacaoDTO
                {
                    Id = i.Id,
                    PacienteId = i.PacienteId,
                    AtendimentoId = i.AtendimentoId,
                    DataEntrada = i.DataEntrada,
                    PrevisaoAlta = i.PrevisaoAlta,
                    MotivoInternacao = i.MotivoInternacao,
                    Leito = i.Leito,
                    Quarto = i.Quarto,
                    Setor = i.Setor,
                    PlanoSaudeUtilizado = i.PlanoSaudeUtilizado,
                    ObservacoesClinicas = i.ObservacoesClinicas,
                    StatusInternacao = i.StatusInternacao,
                    NomePaciente = i.Paciente.NomeCompleto,
                    TipoSanguineoPaciente = i.Paciente.TipoSanguineo,
                    DataHoraAtendimento = i.Atendimento.DataHora,
                    NomeProfissional = i.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();
        }

        // GET: api/Internacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InternacaoDTO>> GetInternacao(int id)
        {
            var internacao = await _context.Internacoes
                .Where(i => i.Id == id && !i.Excluido) // Verificar se não está excluído
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();

            if (internacao == null)
            {
                return NotFound();
            }

            var internacaoDTO = new InternacaoDTO
            {
                Id = internacao.Id,
                PacienteId = internacao.PacienteId,
                AtendimentoId = internacao.AtendimentoId,
                DataEntrada = internacao.DataEntrada,
                PrevisaoAlta = internacao.PrevisaoAlta,
                MotivoInternacao = internacao.MotivoInternacao,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado,
                ObservacoesClinicas = internacao.ObservacoesClinicas,
                StatusInternacao = internacao.StatusInternacao,
                NomePaciente = internacao.Paciente.NomeCompleto,
                TipoSanguineoPaciente = internacao.Paciente.TipoSanguineo,
                DataHoraAtendimento = internacao.Atendimento.DataHora,
                NomeProfissional = internacao.Atendimento.Profissional.NomeCompleto
            };

            return internacaoDTO;
        }

        // GET: api/Internacoes/paciente/5
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<InternacaoDTO>>> GetInternacoesByPaciente(int pacienteId)
        {
            var internacoes = await _context.Internacoes
                .Where(i => i.PacienteId == pacienteId && !i.Excluido) // Filtrar excluídos
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .OrderByDescending(i => i.DataEntrada)
                .Select(i => new InternacaoDTO
                {
                    Id = i.Id,
                    PacienteId = i.PacienteId,
                    AtendimentoId = i.AtendimentoId,
                    DataEntrada = i.DataEntrada,
                    PrevisaoAlta = i.PrevisaoAlta,
                    MotivoInternacao = i.MotivoInternacao,
                    Leito = i.Leito,
                    Quarto = i.Quarto,
                    Setor = i.Setor,
                    PlanoSaudeUtilizado = i.PlanoSaudeUtilizado,
                    ObservacoesClinicas = i.ObservacoesClinicas,
                    StatusInternacao = i.StatusInternacao,
                    NomePaciente = i.Paciente.NomeCompleto,
                    TipoSanguineoPaciente = i.Paciente.TipoSanguineo,
                    DataHoraAtendimento = i.Atendimento.DataHora,
                    NomeProfissional = i.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();

            if (internacoes == null || !internacoes.Any())
            {
                return NotFound();
            }

            return internacoes;
        }

        // GET: api/Internacoes/status/emandamento
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<InternacaoDTO>>> GetInternacoesByStatus(string status)
        {
            var statusNormalizado = status.ToLower().Replace(" ", "");
            
            var internacoes = await _context.Internacoes
                .Where(i => i.StatusInternacao != null && 
                           i.StatusInternacao.ToLower().Replace(" ", "") == statusNormalizado &&
                           !i.Excluido) // Filtrar excluídos
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .OrderByDescending(i => i.DataEntrada)
                .Select(i => new InternacaoDTO
                {
                    Id = i.Id,
                    PacienteId = i.PacienteId,
                    AtendimentoId = i.AtendimentoId,
                    DataEntrada = i.DataEntrada,
                    PrevisaoAlta = i.PrevisaoAlta,
                    MotivoInternacao = i.MotivoInternacao,
                    Leito = i.Leito,
                    Quarto = i.Quarto,
                    Setor = i.Setor,
                    PlanoSaudeUtilizado = i.PlanoSaudeUtilizado,
                    ObservacoesClinicas = i.ObservacoesClinicas,
                    StatusInternacao = i.StatusInternacao,
                    NomePaciente = i.Paciente.NomeCompleto,
                    TipoSanguineoPaciente = i.Paciente.TipoSanguineo,
                    DataHoraAtendimento = i.Atendimento.DataHora,
                    NomeProfissional = i.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();

            return internacoes;
        }

        // PUT: api/Internacoes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<InternacaoDTO>> PutInternacao(int id, InternacaoUpdateDTO internacaoDTO)
        {
            if (id != internacaoDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            var internacaoExistente = await _context.Internacoes
                .Where(i => i.Id == id && !i.Excluido)
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();
                
            if (internacaoExistente == null)
            {
                return NotFound("Internação não encontrada ou está excluída.");
            }

            if (internacaoExistente.StatusInternacao != "Finalizada" && 
                internacaoDTO.StatusInternacao == "Finalizada")
            {
                var existeAlta = await _context.AltasHospitalares.AnyAsync(a => a.InternacaoId == id);
                if (!existeAlta)
                {
                    return BadRequest("Para finalizar uma internação, é necessário registrar uma alta hospitalar primeiro.");
                }
            }

            if (internacaoDTO.DataEntrada.HasValue)
                internacaoExistente.DataEntrada = internacaoDTO.DataEntrada.Value;
                
            if (internacaoDTO.PrevisaoAlta != null)
                internacaoExistente.PrevisaoAlta = internacaoDTO.PrevisaoAlta;
                
            if (internacaoDTO.MotivoInternacao != null)
                internacaoExistente.MotivoInternacao = internacaoDTO.MotivoInternacao;
                
            if (internacaoDTO.Leito != null)
                internacaoExistente.Leito = internacaoDTO.Leito;
                
            if (internacaoDTO.Quarto != null)
                internacaoExistente.Quarto = internacaoDTO.Quarto;
                
            if (internacaoDTO.Setor != null)
                internacaoExistente.Setor = internacaoDTO.Setor;
                
            if (internacaoDTO.PlanoSaudeUtilizado.HasValue)
                internacaoExistente.PlanoSaudeUtilizado = internacaoDTO.PlanoSaudeUtilizado.Value;
                
            if (internacaoDTO.ObservacoesClinicas != null)
                internacaoExistente.ObservacoesClinicas = internacaoDTO.ObservacoesClinicas;
                
            if (internacaoDTO.StatusInternacao != null)
                internacaoExistente.StatusInternacao = internacaoDTO.StatusInternacao;

            try
            {
                await _context.SaveChangesAsync();
                
                var responseDTO = new InternacaoDTO
                {
                    Id = internacaoExistente.Id,
                    PacienteId = internacaoExistente.PacienteId,
                    AtendimentoId = internacaoExistente.AtendimentoId,
                    DataEntrada = internacaoExistente.DataEntrada,
                    PrevisaoAlta = internacaoExistente.PrevisaoAlta,
                    MotivoInternacao = internacaoExistente.MotivoInternacao,
                    Leito = internacaoExistente.Leito,
                    Quarto = internacaoExistente.Quarto,
                    Setor = internacaoExistente.Setor,
                    PlanoSaudeUtilizado = internacaoExistente.PlanoSaudeUtilizado,
                    ObservacoesClinicas = internacaoExistente.ObservacoesClinicas,
                    StatusInternacao = internacaoExistente.StatusInternacao,
                    NomePaciente = internacaoExistente.Paciente.NomeCompleto,
                    TipoSanguineoPaciente = internacaoExistente.Paciente.TipoSanguineo,
                    DataHoraAtendimento = internacaoExistente.Atendimento.DataHora,
                    NomeProfissional = internacaoExistente.Atendimento.Profissional.NomeCompleto
                };
                
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Internacoes
        [HttpPost]
        public async Task<ActionResult<InternacaoDTO>> PostInternacao(InternacaoCreateDTO dto)
        {     
            var paciente = await _context.Pacientes
                .Where(p => p.Id == dto.PacienteId && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (paciente == null)
            {
                return BadRequest("Paciente não encontrado ou está excluído");
            }

            var atendimento = await _context.Atendimentos
                .Where(a => a.Id == dto.AtendimentoId && !a.Excluido)
                .Include(a => a.Profissional)
                .FirstOrDefaultAsync();
                
            if (atendimento == null)
            {
                return BadRequest("Atendimento não encontrado ou está excluído");
            }

            if (await _context.Internacoes
                    .Where(i => i.AtendimentoId == dto.AtendimentoId && !i.Excluido)
                    .AnyAsync())
            {
                return BadRequest("Este atendimento já está associado a uma internação");
            }

            if (atendimento.Tipo != "Internação")
            {
                return BadRequest("Apenas atendimentos do tipo 'Internação' podem ser associados a internações");
            }

            if (atendimento.PacienteId != dto.PacienteId)
            {
                return BadRequest("O paciente do atendimento não corresponde ao paciente informado para a internação");
            }

            var internacao = new Internacao
            {
                PacienteId = dto.PacienteId,
                AtendimentoId = dto.AtendimentoId,
                DataEntrada = dto.DataEntrada == default ? DateTime.Now : dto.DataEntrada,
                PrevisaoAlta = dto.PrevisaoAlta,
                MotivoInternacao = dto.MotivoInternacao,
                Leito = dto.Leito,
                Quarto = dto.Quarto,
                Setor = dto.Setor,
                PlanoSaudeUtilizado = dto.PlanoSaudeUtilizado,
                ObservacoesClinicas = dto.ObservacoesClinicas,
                StatusInternacao = string.IsNullOrEmpty(dto.StatusInternacao) ? "Em andamento" : dto.StatusInternacao,
                Excluido = false,
                DataExclusao = null
            };

            _context.Internacoes.Add(internacao);
            await _context.SaveChangesAsync();

            var internacaoDTO = new InternacaoDTO
            {
                Id = internacao.Id,
                PacienteId = internacao.PacienteId,
                AtendimentoId = internacao.AtendimentoId,
                DataEntrada = internacao.DataEntrada,
                PrevisaoAlta = internacao.PrevisaoAlta,
                MotivoInternacao = internacao.MotivoInternacao,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado,
                ObservacoesClinicas = internacao.ObservacoesClinicas,
                StatusInternacao = internacao.StatusInternacao,
                NomePaciente = paciente.NomeCompleto,
                TipoSanguineoPaciente = paciente.TipoSanguineo,
                DataHoraAtendimento = atendimento.DataHora,
                NomeProfissional = atendimento.Profissional.NomeCompleto
            };

            return CreatedAtAction("GetInternacao", new { id = internacao.Id }, internacaoDTO);
        }

        // DELETE: api/Internacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInternacao(int id)
        {
            var internacao = await _context.Internacoes
                .Where(i => i.Id == id && !i.Excluido)
                .FirstOrDefaultAsync();
                
            if (internacao == null)
            {
                return NotFound();
            }

            internacao.Excluido = true;
            internacao.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Internacoes/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<InternacaoDTO>>> GetInternacoesExcluidas()
        {
            return await _context.Internacoes
                .Where(i => i.Excluido) // Somente internações excluídas
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .Select(i => new InternacaoDTO
                {
                    Id = i.Id,
                    PacienteId = i.PacienteId,
                    AtendimentoId = i.AtendimentoId,
                    DataEntrada = i.DataEntrada,
                    PrevisaoAlta = i.PrevisaoAlta,
                    MotivoInternacao = i.MotivoInternacao,
                    Leito = i.Leito,
                    Quarto = i.Quarto,
                    Setor = i.Setor,
                    PlanoSaudeUtilizado = i.PlanoSaudeUtilizado,
                    ObservacoesClinicas = i.ObservacoesClinicas,
                    StatusInternacao = i.StatusInternacao,
                    NomePaciente = i.Paciente.NomeCompleto,
                    TipoSanguineoPaciente = i.Paciente.TipoSanguineo,
                    DataHoraAtendimento = i.Atendimento.DataHora,
                    NomeProfissional = i.Atendimento.Profissional.NomeCompleto
                })
                .ToListAsync();
        }

        // POST: api/Internacoes/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<InternacaoDTO>> RestaurarInternacao(int id)
        {
            var internacao = await _context.Internacoes
                .Where(i => i.Id == id && i.Excluido)
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync();
                
            if (internacao == null)
            {
                return NotFound("Internação não encontrada ou não está excluída");
            }

            var pacienteExcluido = await _context.Pacientes
                .Where(p => p.Id == internacao.PacienteId && p.Excluido)
                .AnyAsync();
                
            var atendimentoExcluido = await _context.Atendimentos
                .Where(a => a.Id == internacao.AtendimentoId && a.Excluido)
                .AnyAsync();
                
            if (pacienteExcluido || atendimentoExcluido)
            {
                return BadRequest("Não é possível restaurar a internação pois o paciente ou atendimento associado está excluído.");
            }

            internacao.Excluido = false;
            internacao.DataExclusao = null;
            await _context.SaveChangesAsync();

            var internacaoDTO = new InternacaoDTO
            {
                Id = internacao.Id,
                PacienteId = internacao.PacienteId,
                AtendimentoId = internacao.AtendimentoId,
                DataEntrada = internacao.DataEntrada,
                PrevisaoAlta = internacao.PrevisaoAlta,
                MotivoInternacao = internacao.MotivoInternacao,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado,
                ObservacoesClinicas = internacao.ObservacoesClinicas,
                StatusInternacao = internacao.StatusInternacao,
                NomePaciente = internacao.Paciente.NomeCompleto,
                TipoSanguineoPaciente = internacao.Paciente.TipoSanguineo,
                DataHoraAtendimento = internacao.Atendimento.DataHora,
                NomeProfissional = internacao.Atendimento.Profissional.NomeCompleto
            };

            return Ok(internacaoDTO);
        }

        private bool InternacaoExists(int id)
        {
            return _context.Internacoes.Any(e => e.Id == id && !e.Excluido);
        }
    }
}