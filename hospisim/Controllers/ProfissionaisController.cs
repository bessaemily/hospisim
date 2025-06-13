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
    public class ProfissionaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfissionaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Profissionais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfissionalDTO>>> GetProfissionais()
        {
            return await _context.Profissionais
                .Where(p => !p.Excluido) // Filtrar profissionais excluídos
                .Include(p => p.Especialidade) // Carregar especialidade para o nome
                .Select(p => new ProfissionalDTO
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    CPF = p.CPF,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    RegistroConselho = p.RegistroConselho,
                    TipoRegistro = p.TipoRegistro,
                    DataAdmissao = p.DataAdmissao,
                    CargaHorariaSemanal = p.CargaHorariaSemanal,
                    Turno = p.Turno,
                    Ativo = p.Ativo, // Manter a propriedade Ativo separada do soft delete
                    EspecialidadeId = p.EspecialidadeId,
                    NomeEspecialidade = p.Especialidade.Nome
                })
                .ToListAsync();
        }

        // GET: api/Profissionais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfissionalDTO>> GetProfissional(int id)
        {
            var profissional = await _context.Profissionais
                .Where(p => p.Id == id && !p.Excluido) // Verificar se não está excluído
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync();

            if (profissional == null)
            {
                return NotFound();
            }

            var profissionalDTO = new ProfissionalDTO
            {
                Id = profissional.Id,
                NomeCompleto = profissional.NomeCompleto,
                CPF = profissional.CPF,
                Email = profissional.Email,
                Telefone = profissional.Telefone,
                RegistroConselho = profissional.RegistroConselho,
                TipoRegistro = profissional.TipoRegistro,
                DataAdmissao = profissional.DataAdmissao,
                CargaHorariaSemanal = profissional.CargaHorariaSemanal,
                Turno = profissional.Turno,
                Ativo = profissional.Ativo,
                EspecialidadeId = profissional.EspecialidadeId,
                NomeEspecialidade = profissional.Especialidade.Nome
            };

            return profissionalDTO;
        }

        // GET: api/Profissionais/ativos
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<ProfissionalDTO>>> GetProfissionaisAtivos()
        {
            return await _context.Profissionais
                .Where(p => !p.Excluido && p.Ativo) // Não excluídos E ativos
                .Include(p => p.Especialidade)
                .Select(p => new ProfissionalDTO
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    CPF = p.CPF,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    RegistroConselho = p.RegistroConselho,
                    TipoRegistro = p.TipoRegistro,
                    DataAdmissao = p.DataAdmissao,
                    CargaHorariaSemanal = p.CargaHorariaSemanal,
                    Turno = p.Turno,
                    Ativo = p.Ativo,
                    EspecialidadeId = p.EspecialidadeId,
                    NomeEspecialidade = p.Especialidade.Nome
                })
                .ToListAsync();
        }

        // PUT: api/Profissionais/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfissional(int id, ProfissionalUpdateDTO profissionalDTO)
        {
            if (id != profissionalDTO.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no objeto.");
            }

            var profissionalExistente = await _context.Profissionais
                .Where(p => p.Id == id && !p.Excluido)
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync();

            if (profissionalExistente == null)
            {
                return NotFound("Profissional não encontrado ou está excluído.");
            }

            profissionalExistente.NomeCompleto = profissionalDTO.NomeCompleto;
            profissionalExistente.CPF = profissionalDTO.CPF;
            profissionalExistente.Email = profissionalDTO.Email;
            profissionalExistente.Telefone = profissionalDTO.Telefone;
            profissionalExistente.RegistroConselho = profissionalDTO.RegistroConselho;
            profissionalExistente.TipoRegistro = profissionalDTO.TipoRegistro;
            profissionalExistente.DataAdmissao = profissionalDTO.DataAdmissao;
            profissionalExistente.CargaHorariaSemanal = profissionalDTO.CargaHorariaSemanal;
            profissionalExistente.Turno = profissionalDTO.Turno;
            profissionalExistente.Ativo = profissionalDTO.Ativo;

            if (profissionalExistente.EspecialidadeId != profissionalDTO.EspecialidadeId)
            {
                var especialidade = await _context.Especialidades
                    .FirstOrDefaultAsync(e => e.Id == profissionalDTO.EspecialidadeId);

                if (especialidade == null)
                {
                    return BadRequest($"Especialidade com ID {profissionalDTO.EspecialidadeId} não encontrada.");
                }

                profissionalExistente.EspecialidadeId = profissionalDTO.EspecialidadeId;
                profissionalExistente.Especialidade = especialidade;
            }

            try
            {
                await _context.SaveChangesAsync();

                var responseDTO = new ProfissionalDTO
                {
                    Id = profissionalExistente.Id,
                    NomeCompleto = profissionalExistente.NomeCompleto,
                    CPF = profissionalExistente.CPF,
                    Email = profissionalExistente.Email,
                    Telefone = profissionalExistente.Telefone,
                    RegistroConselho = profissionalExistente.RegistroConselho,
                    TipoRegistro = profissionalExistente.TipoRegistro,
                    DataAdmissao = profissionalExistente.DataAdmissao,
                    CargaHorariaSemanal = profissionalExistente.CargaHorariaSemanal,
                    Turno = profissionalExistente.Turno,
                    Ativo = profissionalExistente.Ativo,
                    EspecialidadeId = profissionalExistente.EspecialidadeId,
                    NomeEspecialidade = profissionalExistente.Especialidade?.Nome
                };

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Profissionais
        [HttpPost]
        public async Task<ActionResult<ProfissionalDTO>> PostProfissional(ProfissionalCreateDTO dto)
        {
            var profissionalCpfExistente = await _context.Profissionais
                .Where(p => p.CPF == dto.CPF && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (profissionalCpfExistente != null)
            {
                return BadRequest($"Já existe um profissional cadastrado com o CPF {dto.CPF}");
            }

            var profissionalRegistroExistente = await _context.Profissionais
                .Where(p => p.RegistroConselho == dto.RegistroConselho && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (profissionalRegistroExistente != null)
            {
                return BadRequest($"Já existe um profissional cadastrado com o registro {dto.RegistroConselho}");
            }

            var especialidade = await _context.Especialidades
                .Where(e => e.Id == dto.EspecialidadeId && !e.Excluido)
                .FirstOrDefaultAsync();
                
            if (especialidade == null)
            {
                return BadRequest($"Especialidade com ID {dto.EspecialidadeId} não encontrada ou está excluída");
            }

            if (dto.DataAdmissao > DateTime.Now)
            {
                return BadRequest("A data de admissão não pode ser no futuro");
            }

            var profissional = new Profissional
            {
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                Email = dto.Email,
                Telefone = dto.Telefone,
                RegistroConselho = dto.RegistroConselho,
                TipoRegistro = dto.TipoRegistro,
                DataAdmissao = dto.DataAdmissao == default ? DateTime.Now : dto.DataAdmissao,
                CargaHorariaSemanal = dto.CargaHorariaSemanal,
                Turno = dto.Turno,
                Ativo = dto.Ativo,
                EspecialidadeId = dto.EspecialidadeId,
                Excluido = false,
                DataExclusao = null
            };

            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();

            await _context.Entry(profissional)
                .Reference(p => p.Especialidade)
                .LoadAsync();

            var profissionalDTO = new ProfissionalDTO
            {
                Id = profissional.Id,
                NomeCompleto = profissional.NomeCompleto,
                CPF = profissional.CPF,
                Email = profissional.Email,
                Telefone = profissional.Telefone,
                RegistroConselho = profissional.RegistroConselho,
                TipoRegistro = profissional.TipoRegistro,
                DataAdmissao = profissional.DataAdmissao,
                CargaHorariaSemanal = profissional.CargaHorariaSemanal,
                Turno = profissional.Turno,
                Ativo = profissional.Ativo,
                EspecialidadeId = profissional.EspecialidadeId,
                NomeEspecialidade = profissional.Especialidade?.Nome
            };

            return CreatedAtAction("GetProfissional", new { id = profissional.Id }, profissionalDTO);
        }

        // DELETE: api/Profissionais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfissional(int id)
        {
            var profissional = await _context.Profissionais
                .Where(p => p.Id == id && !p.Excluido)
                .FirstOrDefaultAsync();

            if (profissional == null)
            {
                return NotFound();
            }

            var temAtendimentos = await _context.Atendimentos
                .AnyAsync(a => a.ProfissionalId == id && !a.Excluido);

            var temPrescricoes = await _context.Prescricoes
                .AnyAsync(p => p.ProfissionalId == id && !p.Excluido);

            profissional.Excluido = true;
            profissional.DataExclusao = DateTime.Now;

            profissional.Ativo = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Profissionais/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<ProfissionalDTO>>> GetProfissionaisExcluidos()
        {
            return await _context.Profissionais
                .Where(p => p.Excluido) // Somente profissionais excluídos
                .Include(p => p.Especialidade)
                .Select(p => new ProfissionalDTO
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    CPF = p.CPF,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    RegistroConselho = p.RegistroConselho,
                    TipoRegistro = p.TipoRegistro,
                    DataAdmissao = p.DataAdmissao,
                    CargaHorariaSemanal = p.CargaHorariaSemanal,
                    Turno = p.Turno,
                    Ativo = p.Ativo,
                    EspecialidadeId = p.EspecialidadeId,
                    NomeEspecialidade = p.Especialidade.Nome
                })
                .ToListAsync();
        }

        // POST: api/Profissionais/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<ProfissionalDTO>> RestaurarProfissional(int id)
        {
            var profissional = await _context.Profissionais
                .Where(p => p.Id == id && p.Excluido)
                .Include(p => p.Especialidade)
                .FirstOrDefaultAsync();

            if (profissional == null)
            {
                return NotFound("Profissional não encontrado ou não está excluído");
            }

            profissional.Excluido = false;
            profissional.DataExclusao = null;

            await _context.SaveChangesAsync();

            var profissionalDTO = new ProfissionalDTO
            {
                Id = profissional.Id,
                NomeCompleto = profissional.NomeCompleto,
                CPF = profissional.CPF,
                Email = profissional.Email,
                Telefone = profissional.Telefone,
                RegistroConselho = profissional.RegistroConselho,
                TipoRegistro = profissional.TipoRegistro,
                DataAdmissao = profissional.DataAdmissao,
                CargaHorariaSemanal = profissional.CargaHorariaSemanal,
                Turno = profissional.Turno,
                Ativo = profissional.Ativo,
                EspecialidadeId = profissional.EspecialidadeId,
                NomeEspecialidade = profissional.Especialidade.Nome
            };

            return Ok(profissionalDTO);
        }

        private bool ProfissionalExists(int id)
        {
            return _context.Profissionais.Any(e => e.Id == id && !e.Excluido);
        }
    }
}