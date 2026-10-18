using Microsoft.EntityFrameworkCore;
using Web_Api.Controllers.Models;

namespace Web_Api.Contexts
{
    public class DbContextSQLSRV : DbContext
    {
        public DbContextSQLSRV(DbContextOptions<DbContextSQLSRV> options) : base(options)
        {
        }

        // Tablas
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones 1 a muchos hacia Asignacion.
            // Restrict: no deja borrar un curso/estudiante/docente que tenga asignaciones.
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Curso)
                .WithMany(c => c.Asignaciones)
                .HasForeignKey(a => a.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Estudiante)
                .WithMany(e => e.Asignaciones)
                .HasForeignKey(a => a.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Docente)
                .WithMany(d => d.Asignaciones)
                .HasForeignKey(a => a.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
