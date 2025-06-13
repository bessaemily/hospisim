using hospisim.Models;
using Microsoft.EntityFrameworkCore;

namespace hospisim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }
        public DbSet<Internacao> Internacoes { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<AltaHospitalar> AltasHospitalares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configurações para Atendimento
            modelBuilder.Entity<Atendimento>()
                .HasOne(a => a.Prontuario)
                .WithMany()
                .HasForeignKey(a => a.ProntuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Atendimento>()
                .HasOne(a => a.Paciente)
                .WithMany()
                .HasForeignKey(a => a.PacienteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Atendimento>()
                .HasOne(a => a.Profissional)
                .WithMany(p => p.Atendimentos)
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.NoAction);

            // configurações para Internação
            modelBuilder.Entity<Internacao>()
                .HasOne(i => i.Atendimento)
                .WithOne(a => a.Internacao)
                .HasForeignKey<Internacao>(i => i.AtendimentoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Internacao>()
                .HasOne(i => i.Paciente)
                .WithMany(p => p.Internacoes)
                .HasForeignKey(i => i.PacienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // configurações para Prescrição
            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Atendimento)
                .WithMany(a => a.Prescricoes)
                .HasForeignKey(p => p.AtendimentoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Profissional)
                .WithMany(p => p.Prescricoes)
                .HasForeignKey(p => p.ProfissionalId)
                .OnDelete(DeleteBehavior.NoAction);

            // configurações para Exame
            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Atendimento)
                .WithMany(a => a.Exames)
                .HasForeignKey(e => e.AtendimentoId)
                .OnDelete(DeleteBehavior.NoAction);

            // configurações para Prontuário
            modelBuilder.Entity<Prontuario>()
                .HasOne(p => p.Paciente)
                .WithMany(p => p.Prontuarios)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // configurações para AltaHospitalar
            modelBuilder.Entity<AltaHospitalar>()
                .HasOne(a => a.Internacao)
                .WithMany()
                .HasForeignKey(a => a.InternacaoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
