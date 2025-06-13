using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hospisim.Data;
using hospisim.Models;
using hospisim.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;

namespace hospisim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDTO>>> GetPacientes()
        {
            return await _context.Pacientes
                .Where(p => !p.Excluido) // Filtrar pacientes excluídos
                .Select(p => new PacienteDTO
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    CPF = p.CPF,
                    DataNascimento = p.DataNascimento,
                    Sexo = p.Sexo,
                    TipoSanguineo = p.TipoSanguineo,
                    Telefone = p.Telefone,
                    Email = p.Email,
                    EnderecoCompleto = p.EnderecoCompleto,
                    NumeroCartaoSUS = p.NumeroCartaoSUS,
                    EstadoCivil = p.EstadoCivil,
                    PossuiPlanoSaude = p.PossuiPlanoSaude
                })
                .ToListAsync();
        }

        // GET: api/Pacientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDTO>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == id && !p.Excluido) // Verificar se não está excluído
                .FirstOrDefaultAsync();

            if (paciente == null)
            {
                return NotFound();
            }

            var pacienteDTO = new PacienteDTO
            {
                Id = paciente.Id,
                NomeCompleto = paciente.NomeCompleto,
                CPF = paciente.CPF,
                DataNascimento = paciente.DataNascimento,
                Sexo = paciente.Sexo,
                TipoSanguineo = paciente.TipoSanguineo,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                EnderecoCompleto = paciente.EnderecoCompleto,
                NumeroCartaoSUS = paciente.NumeroCartaoSUS,
                EstadoCivil = paciente.EstadoCivil,
                PossuiPlanoSaude = paciente.PossuiPlanoSaude
            };

            return pacienteDTO;
        }

        // PUT: api/Pacientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest();
            }

            var pacienteExistente = await _context.Pacientes
                .Where(p => p.Id == id && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (pacienteExistente == null)
            {
                return NotFound();
            }

            bool statusExcluido = pacienteExistente.Excluido;
            DateTime? dataExclusao = pacienteExistente.DataExclusao;

            _context.Entry(pacienteExistente).State = EntityState.Detached;
            _context.Entry(paciente).State = EntityState.Modified;

            paciente.Excluido = statusExcluido;
            paciente.DataExclusao = dataExclusao;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<ActionResult<PacienteDTO>> PostPaciente(PacienteCreateDTO dto)
        {
            var pacienteExistente = await _context.Pacientes
                .Where(p => p.CPF == dto.CPF && !p.Excluido)
                .FirstOrDefaultAsync();
                
            if (pacienteExistente != null)
            {
                return BadRequest($"Já existe um paciente cadastrado com o CPF {dto.CPF}");
            }

            if (dto.DataNascimento > DateTime.Now)
            {
                return BadRequest("A data de nascimento não pode ser no futuro");
            }

            if (dto.Sexo != "Masculino" && dto.Sexo != "Feminino" && dto.Sexo != "Outro")
            {
                return BadRequest("O sexo deve ser 'Masculino', 'Feminino' ou 'Outro'");
            }

            var paciente = new Paciente
            {
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                Sexo = dto.Sexo,
                TipoSanguineo = dto.TipoSanguineo,
                Telefone = dto.Telefone,
                Email = dto.Email,
                EnderecoCompleto = dto.EnderecoCompleto,
                NumeroCartaoSUS = dto.NumeroCartaoSUS,
                EstadoCivil = dto.EstadoCivil,
                PossuiPlanoSaude = dto.PossuiPlanoSaude,
                Excluido = false,
                DataExclusao = null
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            var pacienteDTO = new PacienteDTO
            {
                Id = paciente.Id,
                NomeCompleto = paciente.NomeCompleto,
                CPF = paciente.CPF,
                DataNascimento = paciente.DataNascimento,
                Sexo = paciente.Sexo,
                TipoSanguineo = paciente.TipoSanguineo,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                EnderecoCompleto = paciente.EnderecoCompleto,
                NumeroCartaoSUS = paciente.NumeroCartaoSUS,
                EstadoCivil = paciente.EstadoCivil,
                PossuiPlanoSaude = paciente.PossuiPlanoSaude
            };

            return CreatedAtAction("GetPaciente", new { id = paciente.Id }, pacienteDTO);
        }

        // DELETE: api/Pacientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            paciente.Excluido = true;       
            paciente.DataExclusao = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Pacientes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPaciente(int id, [FromBody] PacienteUpdateDTO pacienteAtualizado)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == id && !p.Excluido) // Verificar se não está excluído
                .FirstOrDefaultAsync();
            
            if (paciente == null)
            {
                return NotFound();
            }

            if (pacienteAtualizado.NomeCompleto != null)
                paciente.NomeCompleto = pacienteAtualizado.NomeCompleto;
                
            if (pacienteAtualizado.CPF != null)
                paciente.CPF = pacienteAtualizado.CPF;
                
            if (pacienteAtualizado.DataNascimento.HasValue)
                paciente.DataNascimento = pacienteAtualizado.DataNascimento.Value;
                
            if (pacienteAtualizado.Sexo != null)
                paciente.Sexo = pacienteAtualizado.Sexo;
                
            if (pacienteAtualizado.TipoSanguineo != null)
                paciente.TipoSanguineo = pacienteAtualizado.TipoSanguineo;
                
            if (pacienteAtualizado.Telefone != null)
                paciente.Telefone = pacienteAtualizado.Telefone;
                
            if (pacienteAtualizado.Email != null)
                paciente.Email = pacienteAtualizado.Email;
                
            if (pacienteAtualizado.EnderecoCompleto != null)
                paciente.EnderecoCompleto = pacienteAtualizado.EnderecoCompleto;
                
            if (pacienteAtualizado.NumeroCartaoSUS != null)
                paciente.NumeroCartaoSUS = pacienteAtualizado.NumeroCartaoSUS;
                
            if (pacienteAtualizado.EstadoCivil != null)
                paciente.EstadoCivil = pacienteAtualizado.EstadoCivil;
            
            if (pacienteAtualizado.PossuiPlanoSaude.HasValue)
                paciente.PossuiPlanoSaude = pacienteAtualizado.PossuiPlanoSaude.Value;

            try
            {
                await _context.SaveChangesAsync();
                
                var pacienteDTO = new PacienteDTO
                {
                    Id = paciente.Id,
                    NomeCompleto = paciente.NomeCompleto,
                    CPF = paciente.CPF,
                    DataNascimento = paciente.DataNascimento,
                    Sexo = paciente.Sexo,
                    TipoSanguineo = paciente.TipoSanguineo,
                    Telefone = paciente.Telefone,
                    Email = paciente.Email,
                    EnderecoCompleto = paciente.EnderecoCompleto,
                    NumeroCartaoSUS = paciente.NumeroCartaoSUS,
                    EstadoCivil = paciente.EstadoCivil,
                    PossuiPlanoSaude = paciente.PossuiPlanoSaude
                };
                
                return Ok(pacienteDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id && !e.Excluido);
        }

        [HttpGet("excluidos")]
        public async Task<ActionResult<IEnumerable<PacienteDTO>>> GetPacientesExcluidos()
        {
            return await _context.Pacientes
                .Where(p => p.Excluido)
                .Select(p => new PacienteDTO
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    CPF = p.CPF,
                    DataNascimento = p.DataNascimento,
                    Sexo = p.Sexo,
                    TipoSanguineo = p.TipoSanguineo,
                    Telefone = p.Telefone,
                    Email = p.Email,
                    EnderecoCompleto = p.EnderecoCompleto,
                    NumeroCartaoSUS = p.NumeroCartaoSUS,
                    EstadoCivil = p.EstadoCivil,
                    PossuiPlanoSaude = p.PossuiPlanoSaude
                })
                .ToListAsync();
        }

        [HttpPost("{id}/restaurar")]
        public async Task<ActionResult<PacienteDTO>> RestaurarPaciente(int id)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == id && p.Excluido)
                .FirstOrDefaultAsync();
                
            if (paciente == null)
            {
                return NotFound("Paciente não encontrado ou não está excluído");
            }

            paciente.Excluido = false;
            paciente.DataExclusao = null;
            await _context.SaveChangesAsync();

            var pacienteDTO = new PacienteDTO
            {
                Id = paciente.Id,
                NomeCompleto = paciente.NomeCompleto,
                CPF = paciente.CPF,
                DataNascimento = paciente.DataNascimento,
                Sexo = paciente.Sexo,
                TipoSanguineo = paciente.TipoSanguineo,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                EnderecoCompleto = paciente.EnderecoCompleto,
                NumeroCartaoSUS = paciente.NumeroCartaoSUS,
                EstadoCivil = paciente.EstadoCivil,
                PossuiPlanoSaude = paciente.PossuiPlanoSaude
            };

            return Ok(pacienteDTO);
        }
    }
}