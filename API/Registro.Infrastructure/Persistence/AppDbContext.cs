using Microsoft.EntityFrameworkCore;
using Registro.Domain.Entities;

namespace Registro.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Persona> Personas => Set<Persona>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(b =>
        {
            b.ToTable("Personas", "dbo");
            b.HasKey(x => x.PersonaId);

            b.Property(x => x.Nombres).HasMaxLength(100).IsRequired();
            b.Property(x => x.Apellidos).HasMaxLength(100).IsRequired();
            b.Property(x => x.NumeroIdentificacion).HasMaxLength(30).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200).IsRequired();
            b.Property(x => x.TipoIdentificacion).HasMaxLength(10).IsRequired();
            b.Property(x => x.FechaCreacion).IsRequired()
                       .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSDATETIME()")
                        .ValueGeneratedOnAdd();
            
            b.Property(x => x.IdentificacionCompleta)
             .HasMaxLength(50)
             .ValueGeneratedOnAddOrUpdate();

            b.Property(x => x.NombreCompleto)
             .HasMaxLength(205)
             .ValueGeneratedOnAddOrUpdate();

            b.HasIndex(x => x.Email).IsUnique();
            b.HasIndex(x => new { x.TipoIdentificacion, x.NumeroIdentificacion }).IsUnique();
        });

        modelBuilder.Entity<Usuario>(b =>
        {
            b.ToTable("Usuarios", "dbo");
            b.HasKey(x => x.UsuarioId);

            b.Property(x => x.UsuarioNombre)
             .HasColumnName("Usuario")
             .HasMaxLength(100)
             .IsRequired();

            b.Property(x => x.PassHash).IsRequired();
            b.Property(x => x.PassSalt).IsRequired();
            b.Property(x => x.FechaCreacion).IsRequired();

            b.HasIndex(x => x.UsuarioNombre).IsUnique();
        });
    }
}