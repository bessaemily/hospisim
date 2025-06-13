using hospisim.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hospisim.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // verificando se j� existem dados
                if (context.Pacientes.Any())
                {
                    return; // DB j� foi populado
                }

                // adiciona 10 especialidades
                var especialidades = new Especialidade[10];
                var nomeEspecialidades = new[] {
                    "Cl�nica Geral", "Cardiologia", "Ortopedia", "Pediatria", "Neurologia",
                    "Dermatologia", "Oftalmologia", "Ginecologia", "Urologia", "Psiquiatria"
                };
                var descricaoEspecialidades = new[] {
                    "Atendimento geral de pacientes",
                    "Especialidade que trata de doen�as do cora��o",
                    "Especialidade que trata de problemas �sseos e musculares",
                    "Especialidade que trata de crian�as",
                    "Especialidade que trata de doen�as do sistema nervoso",
                    "Especialidade que trata de doen�as da pele",
                    "Especialidade que trata de doen�as dos olhos",
                    "Especialidade que trata da sa�de da mulher",
                    "Especialidade que trata do sistema urin�rio e reprodutor masculino",
                    "Especialidade que trata de transtornos mentais"
                };

                for (int i = 0; i < 10; i++)
                {
                    especialidades[i] = new Especialidade
                    {
                        Nome = nomeEspecialidades[i],
                        Descricao = descricaoEspecialidades[i]
                    };
                }
                context.Especialidades.AddRange(especialidades);
                context.SaveChanges();

                // adicionando 10 profissionais 
                var profissionais = new Profissional[10];
                var nomesProfissionais = new[] {
                    "Dr. Carlos Silva", "Dra. Ana Souza", "Dr. Ricardo Oliveira", "Dra. Juliana Santos", "Dr. Fernando Costa",
                    "Dra. Mariana Lima", "Dr. Roberto Alves", "Dra. Cristina Martins", "Dr. Paulo Ferreira", "Dra. Beatriz Gomes"
                };
                var cpfProfissionais = new[] {
                    "111.222.333-44", "222.333.444-55", "333.444.555-66", "444.555.666-77", "555.666.777-88",
                    "666.777.888-99", "777.888.999-00", "888.999.000-11", "999.000.111-22", "000.111.222-33"
                };

                for (int i = 0; i < 10; i++)
                {
                    profissionais[i] = new Profissional
                    {
                        NomeCompleto = nomesProfissionais[i],
                        CPF = cpfProfissionais[i],
                        Email = nomesProfissionais[i].ToLower().Replace(" ", ".").Replace(".", "") + "@hospital.com",
                        Telefone = $"(11) 9{i + 1}234-5678",
                        RegistroConselho = $"CRM-{10000 + i}",
                        TipoRegistro = "CRM",
                        DataAdmissao = DateTime.Now.AddYears(-(i % 5 + 1)),
                        CargaHorariaSemanal = 20 + (i % 3) * 10,
                        Turno = i % 3 == 0 ? "Manh�" : (i % 3 == 1 ? "Tarde" : "Noite"),
                        Ativo = true,
                        EspecialidadeId = especialidades[i].Id
                    };
                }
                context.Profissionais.AddRange(profissionais);
                context.SaveChanges();

                // adicionando 10 pacientes
                var pacientes = new Paciente[10];
                var nomesPacientes = new[] {
                    "Jo�o da Silva", "Maria Oliveira", "Pedro Santos", "Ana Souza", "Lucas Ferreira",
                    "Juliana Lima", "Roberto Almeida", "Camila Costa", "Marcelo Dias", "Fernanda Ramos"
                };
                var cpfPacientes = new[] {
                    "123.456.789-01", "234.567.890-12", "345.678.901-23", "456.789.012-34", "567.890.123-45",
                    "678.901.234-56", "789.012.345-67", "890.123.456-78", "901.234.567-89", "012.345.678-90"
                };
                var dataNascimentos = new[] {
                    new DateTime(1980, 5, 15), new DateTime(1975, 8, 20), new DateTime(1990, 3, 10), new DateTime(1985, 12, 5), new DateTime(1995, 7, 25),
                    new DateTime(1982, 9, 30), new DateTime(1970, 4, 15), new DateTime(1988, 11, 20), new DateTime(1979, 2, 8), new DateTime(1992, 6, 12)
                };
                var sexos = new[] {
                    "Masculino", "Feminino", "Masculino", "Feminino", "Masculino",
                    "Feminino", "Masculino", "Feminino", "Masculino", "Feminino"
                };
                var tipoSanguineos = new[] {
                    "A+", "O-", "B+", "AB+", "A-", "O+", "B-", "AB-", "O+", "A+"
                };
                var enderecos = new[] {
                    "Rua das Flores, 123 - S�o Paulo/SP", "Av. Paulista, 1000 - S�o Paulo/SP", "Rua Augusta, 500 - S�o Paulo/SP",
                    "Rua Consola��o, 200 - S�o Paulo/SP", "Av. Rebou�as, 300 - S�o Paulo/SP", "Rua Oscar Freire, 400 - S�o Paulo/SP",
                    "Av. Brigadeiro Faria Lima, 800 - S�o Paulo/SP", "Rua dos Pinheiros, 100 - S�o Paulo/SP", "Av. Santo Amaro, 600 - S�o Paulo/SP",
                    "Rua Haddock Lobo, 700 - S�o Paulo/SP"
                };
                var estadosCivis = new[] {
                    "Casado", "Casada", "Solteiro", "Divorciada", "Solteiro",
                    "Casada", "Vi�vo", "Solteira", "Casado", "Solteira"
                };

                for (int i = 0; i < 10; i++)
                {
                    pacientes[i] = new Paciente
                    {
                        NomeCompleto = nomesPacientes[i],
                        CPF = cpfPacientes[i],
                        DataNascimento = dataNascimentos[i],
                        Sexo = sexos[i],
                        TipoSanguineo = tipoSanguineos[i],
                        Telefone = $"(11) 9{i + 1}234-5678",
                        Email = nomesPacientes[i].ToLower().Replace(" ", ".") + "@email.com",
                        EnderecoCompleto = enderecos[i],
                        NumeroCartaoSUS = $"{i + 1}2345678901234",
                        EstadoCivil = estadosCivis[i],
                        PossuiPlanoSaude = i % 2 == 0
                    };
                }
                context.Pacientes.AddRange(pacientes);
                context.SaveChanges();

                // adicionadno 10 prontu�rios
                var prontuarios = new Prontuario[10];
                for (int i = 0; i < 10; i++)
                {
                    prontuarios[i] = new Prontuario
                    {
                        NumeroProntuario = $"PRONT-{DateTime.Now.Year}-{1000 + i}",
                        DataCriacao = DateTime.Now.AddDays(-i * 30),
                        Observacoes = $"Prontu�rio inicial do paciente {pacientes[i].NomeCompleto}",
                        PacienteId = pacientes[i].Id
                    };
                }
                context.Prontuarios.AddRange(prontuarios);
                context.SaveChanges();

                // adicionando 10 atendimentos
                var atendimentos = new Atendimento[10];
                var tiposAtendimento = new[] { "Emerg�ncia", "Consulta", "Interna��o" };
                var statusAtendimento = new[] { "Realizado", "Em andamento", "Cancelado" };
                var locaisAtendimento = new[] { "Consult�rio 1", "Emerg�ncia", "Ala A", "Ala B", "Consult�rio 2" };

                for (int i = 0; i < 10; i++)
                {
                    atendimentos[i] = new Atendimento
                    {
                        DataHora = DateTime.Now.AddDays(-i * 3),
                        Tipo = tiposAtendimento[i % 3],
                        Status = statusAtendimento[i % 3],
                        Local = locaisAtendimento[i % 5],
                        PacienteId = pacientes[i].Id,
                        ProntuarioId = prontuarios[i].Id,
                        ProfissionalId = profissionais[i % profissionais.Length].Id
                    };
                }
                context.Atendimentos.AddRange(atendimentos);
                context.SaveChanges();

                // adidionando 10 prescri��es
                var prescricoes = new Prescricao[10];
                var medicamentos = new[] {
                    "Dipirona 500mg", "Paracetamol 750mg", "Amoxicilina 500mg",
                    "Losartana 50mg", "Omeprazol 20mg", "Atenolol 25mg",
                    "Ibuprofeno 600mg", "Metformina 850mg", "Enalapril 10mg", "Azitromicina 500mg"
                };
                var dosagens = new[] { "1 comprimido", "2 comprimidos", "1 c�psula", "1 comprimido", "1 c�psula",
                                      "1 comprimido", "1 comprimido", "1 comprimido", "1 comprimido", "1 c�psula" };
                var frequencias = new[] { "8/8h", "6/6h", "12/12h", "24h", "12/12h",
                                         "24h", "8/8h", "12/12h", "12/12h", "24h" };
                var vias = new[] { "Oral", "Oral", "Oral", "Oral", "Oral",
                                  "Oral", "Oral", "Oral", "Oral", "Oral" };

                for (int i = 0; i < 10; i++)
                {
                    prescricoes[i] = new Prescricao
                    {
                        AtendimentoId = atendimentos[i].Id,
                        ProfissionalId = atendimentos[i].ProfissionalId,
                        Medicamento = medicamentos[i],
                        Dosagem = dosagens[i],
                        Frequencia = frequencias[i],
                        ViaAdministracao = vias[i],
                        DataInicio = atendimentos[i].DataHora,
                        DataFim = atendimentos[i].DataHora.AddDays(7),
                        Observacoes = "Tomar ap�s as refei��es",
                        StatusPrescricao = "Ativo",
                        ReacoesAdversas = null
                    };
                }
                context.Prescricoes.AddRange(prescricoes);
                context.SaveChanges();

                // adicionando 10 exames
                var exames = new Exame[10];
                var tiposExame = new[] {
                    "Hemograma", "Glicemia", "Colesterol Total", "Raio-X T�rax", "Eletrocardiograma",
                    "Ultrassom Abdominal", "Resson�ncia Magn�tica", "Tomografia", "Densitometria �ssea", "Endoscopia"
                };

                for (int i = 0; i < 10; i++)
                {
                    exames[i] = new Exame
                    {
                        AtendimentoId = atendimentos[i].Id,
                        Tipo = tiposExame[i],
                        DataSolicitacao = atendimentos[i].DataHora,
                        DataRealizacao = i % 3 == 0 ? null : atendimentos[i].DataHora.AddDays(1),
                        Resultado = i % 3 == 0 ? null : "Resultados dentro dos padr�es de normalidade."
                    };
                }
                context.Exames.AddRange(exames);
                context.SaveChanges();

                // adicionando interna��es
                var atendimentosInternacao = atendimentos.Where(a => a.Tipo == "Interna��o").ToArray();
                var setores = new[] { "Cl�nica M�dica", "Cardiologia", "Ortopedia", "Pediatria", "UTI" };
                var statusInternacao = new[] { "Em andamento", "Finalizada", "Em andamento", "Finalizada", "Em andamento" };

                var internacoes = new Internacao[atendimentosInternacao.Length];
                for (int i = 0; i < atendimentosInternacao.Length; i++)
                {
                    internacoes[i] = new Internacao
                    {
                        PacienteId = atendimentosInternacao[i].PacienteId,
                        AtendimentoId = atendimentosInternacao[i].Id,
                        DataEntrada = atendimentosInternacao[i].DataHora,
                        PrevisaoAlta = atendimentosInternacao[i].DataHora.AddDays(5),
                        MotivoInternacao = "Necessidade de observa��o e tratamento cont�nuo",
                        Leito = $"{(char)('A' + (i % 3))}-{(i % 10) + 1}",
                        Quarto = $"{(i % 5) + 1}0{(i % 9) + 1}",
                        Setor = setores[i % 5],
                        PlanoSaudeUtilizado = i % 2 == 0,
                        ObservacoesClinicas = "Paciente sob observa��o e medica��o controlada",
                        StatusInternacao = statusInternacao[i % 5]
                    };
                }
                context.Internacoes.AddRange(internacoes);
                context.SaveChanges();

                // adicionando altas hospitalares - apenas para interna��es finalizadas
                var internacoesFinalizadas = internacoes.Where(i => i.StatusInternacao == "Finalizada").ToArray();
                var altas = new AltaHospitalar[internacoesFinalizadas.Length];

                for (int i = 0; i < internacoesFinalizadas.Length; i++)
                {
                    altas[i] = new AltaHospitalar
                    {
                        InternacaoId = internacoesFinalizadas[i].Id,
                        Data = internacoesFinalizadas[i].PrevisaoAlta.Value,
                        CondicaoPaciente = "Est�vel, com melhora significativa",
                        InstrucoesPosAlta = "Repouso por 7 dias, retorno para consulta em 15 dias, continuar medica��o conforme receita."
                    };
                }
                context.AltasHospitalares.AddRange(altas);

                // adidionando interna��es extras para completar 10 registros

                var internacoesExtras = new Internacao[10 - internacoes.Length];
                for (int i = 0; i < internacoesExtras.Length; i++)
                {
                    var atendimento = atendimentos.FirstOrDefault(a => a.Tipo == "Consulta" &&
                                                                       !context.Internacoes.Any(intern => intern.AtendimentoId == a.Id));
                    if (atendimento != null)
                    {
                        // alterando o tipo do atendimento para interna��o
                        atendimento.Tipo = "Interna��o";
                        context.Entry(atendimento).State = EntityState.Modified;

                        internacoesExtras[i] = new Internacao
                        {
                            PacienteId = atendimento.PacienteId,
                            AtendimentoId = atendimento.Id,
                            DataEntrada = atendimento.DataHora,
                            PrevisaoAlta = atendimento.DataHora.AddDays(5),
                            MotivoInternacao = "Complica��es p�s-consulta",
                            Leito = $"X-{i + 1}",
                            Quarto = $"10{i + 1}",
                            Setor = setores[i % 5],
                            PlanoSaudeUtilizado = i % 2 == 0,
                            ObservacoesClinicas = "Interna��o n�o planejada ap�s consulta",
                            StatusInternacao = i % 2 == 0 ? "Em andamento" : "Finalizada"
                        };
                    }
                }
                context.Internacoes.AddRange(internacoesExtras.Where(i => i != null));
                context.SaveChanges();

                var todasInternacoesFinalizadas = context.Internacoes
                    .Where(i => i.StatusInternacao == "Finalizada" && !context.AltasHospitalares.Any(a => a.InternacaoId == i.Id))
                    .ToList();

                var altasExtras = new AltaHospitalar[todasInternacoesFinalizadas.Count];
                for (int i = 0; i < todasInternacoesFinalizadas.Count; i++)
                {
                    altasExtras[i] = new AltaHospitalar
                    {
                        InternacaoId = todasInternacoesFinalizadas[i].Id,
                        Data = todasInternacoesFinalizadas[i].PrevisaoAlta ?? DateTime.Now,
                        CondicaoPaciente = "Recuperado, sem complica��es",
                        InstrucoesPosAlta = "Seguir medica��o conforme receita. Retorno em 10 dias."
                    };
                }
                context.AltasHospitalares.AddRange(altasExtras);
                context.SaveChanges();

                // verificando se todas as entidades tem pelo menos 10 registros e adicionar mais se necess�rio
                CompletarEntidadeComRegistros<Especialidade>(context, 10);
                CompletarEntidadeComRegistros<Profissional>(context, 10);
                CompletarEntidadeComRegistros<Paciente>(context, 10);
                CompletarEntidadeComRegistros<Prontuario>(context, 10);
                CompletarEntidadeComRegistros<Atendimento>(context, 10);
                CompletarEntidadeComRegistros<Prescricao>(context, 10);
                CompletarEntidadeComRegistros<Exame>(context, 10);
                CompletarEntidadeComRegistros<Internacao>(context, 10);
                CompletarEntidadeComRegistros<AltaHospitalar>(context, 10);

                context.SaveChanges();
            }
        }

        // M�todo para garantir que cada entidade tenha pelo menos o n�mero m�nimo de registros
        private static void CompletarEntidadeComRegistros<T>(ApplicationDbContext context, int minRegistros) where T : class
        {
            var dbSet = context.Set<T>();
            var count = dbSet.Count();

            if (count >= minRegistros)
                return;

            Console.WriteLine($"A entidade {typeof(T).Name} tem apenas {count} registros. � necess�rio adicionar mais registros manualmente.");
        }
    }
}