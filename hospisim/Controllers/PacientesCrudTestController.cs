using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using hospisim.Data;
using hospisim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hospisim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesCrudTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PacientesCrudTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TestarCrudPacientes()
        {
            try
            {
                // 1. TESTE CREATE (POST)
                var novoPaciente = new Paciente
                {
                    NomeCompleto = "Paciente de Teste",
                    CPF = "123.456.789-00",
                    DataNascimento = new DateTime(1980, 1, 1),
                    Sexo = "Masculino",
                    TipoSanguineo = "O+",
                    Telefone = "(11) 99999-9999",
                    Email = "teste@exemplo.com",
                    EnderecoCompleto = "Rua de Teste, 123",
                    NumeroCartaoSUS = "123456789012345",
                    EstadoCivil = "Solteiro",
                    PossuiPlanoSaude = true
                };

                _context.Pacientes.Add(novoPaciente);
                await _context.SaveChangesAsync();
                
                var pacienteId = novoPaciente.Id;
                
                // Verificar se o paciente foi criado
                if (pacienteId <= 0)
                    return BadRequest("CREATE: Falha ao criar paciente de teste");
                
                // 2. TESTE READ (GET)
                var pacienteConsultado = await _context.Pacientes.FindAsync(pacienteId);
                if (pacienteConsultado == null || pacienteConsultado.NomeCompleto != "Paciente de Teste")
                    return BadRequest("READ: Falha ao recuperar paciente criado");
                
                // 3. TESTE UPDATE (PUT)
                pacienteConsultado.NomeCompleto = "Paciente de Teste Atualizado";
                _context.Entry(pacienteConsultado).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                // Verificar atualização
                var pacienteAtualizado = await _context.Pacientes.FindAsync(pacienteId);
                if (pacienteAtualizado.NomeCompleto != "Paciente de Teste Atualizado")
                    return BadRequest("UPDATE: Falha ao atualizar paciente");
                
                // 4. TESTE READ ALL
                var todosPacientes = await _context.Pacientes.ToListAsync();
                if (!todosPacientes.Any(p => p.Id == pacienteId))
                    return BadRequest("READ ALL: Paciente não encontrado na lista completa");
                
                // 5. TESTE DELETE
                _context.Pacientes.Remove(pacienteAtualizado);
                await _context.SaveChangesAsync();
                
                // 6. Verificar exclusão
                var pacienteExcluido = await _context.Pacientes.FindAsync(pacienteId);
                if (pacienteExcluido != null)
                    return BadRequest("DELETE: Falha ao excluir paciente");
                
                return Ok(new { 
                    Mensagem = "Todas as operações CRUD foram testadas com sucesso!",
                    Detalhes = new {
                        Create = "Paciente criado com ID " + pacienteId,
                        Read = "Paciente recuperado corretamente",
                        Update = "Paciente atualizado com sucesso",
                        ReadAll = "Lista de pacientes recuperada com " + todosPacientes.Count + " paciente(s)",
                        Delete = "Paciente excluído com sucesso"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    Erro = "Erro ao testar operações CRUD",
                    Detalhes = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }
    }
}