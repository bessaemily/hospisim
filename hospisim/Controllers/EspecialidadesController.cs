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
    public class EspecialidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EspecialidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Especialidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EspecialidadeDTO>>> GetEspecialidades()
        {
            return await _context.Especialidades
                .Where(e => !e.Excluido) // Filtrar especialidades excluídas
                .Select(e => new EspecialidadeDTO
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Descricao = e.Descricao,
                    QuantidadeProfissionais = e.Profissionais.Count(p => !p.Excluido)
                })
                .ToListAsync();
        }

        // GET: api/Especialidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EspecialidadeDTO>> GetEspecialidade(int id)
        {
            var especialidade = await _context.Especialidades
                .Where(e => e.Id == id && !e.Excluido) // Verificar se não está excluída
                .Include(e => e.Profissionais.Where(p => !p.Excluido))
                .FirstOrDefaultAsync();

            if (especialidade == null)
            {
                return NotFound();
            }

            var especialidadeDTO = new EspecialidadeDTO
            {
                Id = especialidade.Id,
                Nome = especialidade.Nome,
                Descricao = especialidade.Descricao,
                QuantidadeProfissionais = especialidade.Profissionais.Count
            };

            return especialidadeDTO;
        }

        // GET: api/Especialidades/5/profissionais
        [HttpGet("{id}/profissionais")]
        public async Task<ActionResult<object>> GetEspecialidadeWithProfissionais(int id)
        {
            var especialidade = await _context.Especialidades
                .Include(e => e.Profissionais.Where(p => !p.Excluido)) // Incluir apenas profissionais não excluídos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidade == null)
            {
                return NotFound();
            }

            return new
            {
                Especialidade = new EspecialidadeDTO
                {
                    Id = especialidade.Id,
                    Nome = especialidade.Nome,
                    Descricao = especialidade.Descricao,
                    QuantidadeProfissionais = especialidade.Profissionais.Count
                },
                Profissionais = especialidade.Profissionais.Select(p => new
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    RegistroConselho = p.RegistroConselho,
                    Ativo = p.Ativo
                }).ToList()
            };
        }

        // GET: api/Especialidades/excluidos
        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<EspecialidadeDTO>>> GetEspecialidadesExcluidas()
        {
            return await _context.Especialidades
                .Where(e => e.Excluido) // Somente especialidades excluídas
                .Select(e => new EspecialidadeDTO
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Descricao = e.Descricao,
                    QuantidadeProfissionais = e.Profissionais.Count(p => !p.Excluido)
                })
                .ToListAsync();
        }

        // POST: api/Especialidades/5/restaurar
        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<EspecialidadeDTO>> RestaurarEspecialidade(int id)
        {
            var especialidade = await _context.Especialidades
                .Where(e => e.Id == id && e.Excluido)
                .FirstOrDefaultAsync();
                
            if (especialidade == null)
            {
                return NotFound("Especialidade não encontrada ou não está excluída");
            }

            if (await _context.Especialidades.AnyAsync(e => e.Nome == especialidade.Nome && !e.Excluido))
            {
                return BadRequest($"Não é possível restaurar esta especialidade pois já existe outra com o nome '{especialidade.Nome}' ativa.");
            }

            especialidade.Excluido = false;
            especialidade.DataExclusao = null;
            await _context.SaveChangesAsync();

            var especialidadeDTO = new EspecialidadeDTO
            {
                Id = especialidade.Id,
                Nome = especialidade.Nome,
                Descricao = especialidade.Descricao,
                QuantidadeProfissionais = especialidade.Profissionais.Count(p => !p.Excluido)
            };

            return Ok(especialidadeDTO);
        }

        // PUT: api/Especialidades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecialidade(int id, Especialidade especialidade)
        {
            if (id != especialidade.Id)
            {
                return BadRequest();
            }

            var existenteComMesmoNome = await _context.Especialidades
                .FirstOrDefaultAsync(e => e.Id != id && e.Nome == especialidade.Nome);
                
            if (existenteComMesmoNome != null)
            {
                return BadRequest($"Já existe uma especialidade com o nome '{especialidade.Nome}'.");
            }

            var especialidadeExistente = await _context.Especialidades
                .Include(e => e.Profissionais.Where(p => !p.Excluido)) // Incluir apenas profissionais não excluídos
                .FirstOrDefaultAsync(e => e.Id == id);
                
            if (especialidadeExistente == null)
            {
                return NotFound();
            }
            
            especialidadeExistente.Nome = especialidade.Nome;
            especialidadeExistente.Descricao = especialidade.Descricao;

            try
            {
                await _context.SaveChangesAsync();
                
                var especialidadeDTO = new EspecialidadeDTO
                {
                    Id = especialidadeExistente.Id,
                    Nome = especialidadeExistente.Nome,
                    Descricao = especialidadeExistente.Descricao,
                    QuantidadeProfissionais = especialidadeExistente.Profissionais.Count
                };
                
                return Ok(especialidadeDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecialidadeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Especialidades
        [HttpPost]
        public async Task<ActionResult<EspecialidadeDTO>> PostEspecialidade(EspecialidadeCreateDTO dto)
        {
            if (await _context.Especialidades.AnyAsync(e => e.Nome == dto.Nome && !e.Excluido))
            {
                return BadRequest($"Já existe uma especialidade com o nome '{dto.Nome}'.");
            }

            var especialidade = new Especialidade
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Excluido = false,
                DataExclusao = null
            };

            _context.Especialidades.Add(especialidade);
            await _context.SaveChangesAsync();

            var especialidadeDTO = new EspecialidadeDTO
            {
                Id = especialidade.Id,
                Nome = especialidade.Nome,
                Descricao = especialidade.Descricao,
                QuantidadeProfissionais = 0 // Nova especialidade sem profissionais
            };

            return CreatedAtAction("GetEspecialidade", new { id = especialidade.Id }, especialidadeDTO);
        }

        // DELETE: api/Especialidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidade(int id)
        {
            var especialidade = await _context.Especialidades
                .Include(e => e.Profissionais.Where(p => !p.Excluido)) // Incluir apenas profissionais não excluídos
                .FirstOrDefaultAsync(e => e.Id == id && !e.Excluido);
                
            if (especialidade == null)
            {
                return NotFound("Especialidade não encontrada ou já está excluída");
            }

            if (especialidade.Profissionais.Any())
            {
                var profissionaisAtivos = especialidade.Profissionais.Count(p => p.Ativo);
                var profissionaisInativos = especialidade.Profissionais.Count(p => !p.Ativo);
                
                string mensagem = $"Não é possível excluir a especialidade '{especialidade.Nome}' pois existem {especialidade.Profissionais.Count} profissionais associados a ela.";
                
                if (profissionaisAtivos > 0)
                {
                    mensagem += $" ({profissionaisAtivos} ativos e {profissionaisInativos} inativos)";
                }
                
                mensagem += " Você deve primeiro transferir esses profissionais para outra especialidade ou excluí-los.";
                
                return BadRequest(mensagem);
            }

            especialidade.Excluido = true;
            especialidade.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EspecialidadeExists(int id)
        {
            return _context.Especialidades.Any(e => e.Id == id);
        }
    }
}