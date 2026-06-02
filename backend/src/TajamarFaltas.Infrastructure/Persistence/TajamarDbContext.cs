using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TajamarFaltas.Domain.Entities;
using TajamarFaltas.Domain.Enums;

namespace TajamarFaltas.Infrastructure.Persistence;

public sealed class TajamarDbContext : DbContext
{
    public TajamarDbContext(DbContextOptions<TajamarDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoleMirror> RolesMirror => Set<RoleMirror>();

    public DbSet<UsuarioMirror> UsuariosMirror => Set<UsuarioMirror>();

    public DbSet<CursoMirror> CursosMirror => Set<CursoMirror>();

    public DbSet<Falta> Faltas => Set<Falta>();

    public DbSet<UsuarioCurso> UsuariosCursos => Set<UsuarioCurso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoleMirror>(entity =>
        {
            entity.ToTable("RolesMirror");
            entity.HasKey(x => x.IdRole);
            entity.Property(x => x.IdRole).HasConversion<byte>().ValueGeneratedNever();
            entity.Property(x => x.Rolename).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => x.Rolename).IsUnique();
        });

        modelBuilder.Entity<UsuarioMirror>(entity =>
        {
            entity.ToTable("UsuariosMirror");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Apellidos).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
            entity.Property(x => x.Imagen).HasMaxLength(500);
            entity.Property(x => x.Password).HasMaxLength(100).IsRequired();
            entity.Property(x => x.IdRole).HasConversion<byte>();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasOne(x => x.Role)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.IdRole)
                .HasPrincipalKey(x => x.IdRole);
        });

        modelBuilder.Entity<CursoMirror>(entity =>
        {
            entity.ToTable("CursosMirror");
            entity.HasKey(x => x.IdCurso);
            entity.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(x => x.DuracionHoras).IsRequired();
        });

        modelBuilder.Entity<UsuarioCurso>(entity =>
        {
            entity.ToTable("UsuariosCursos");
            entity.HasKey(x => new { x.IdUsuario, x.IdCurso });
            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.UsuariosCursos)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Curso)
                .WithMany(x => x.UsuariosCursos)
                .HasForeignKey(x => x.IdCurso)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Falta>(entity =>
        {
            entity.ToTable("Faltas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FechaIncidencia).HasColumnType("datetime2(0)");
            entity.Property(x => x.TipoFalta)
                .HasConversion(new ValueConverter<TipoFalta, string>(
                    value => ConvertTipoFaltaToString(value),
                    value => ConvertStringToTipoFalta(value)))
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(x => x.EsJustificada).HasDefaultValue(false);
            entity.Property(x => x.Comentario).HasMaxLength(500);
            entity.HasIndex(x => new { x.IdUsuario, x.FechaIncidencia });
            entity.HasIndex(x => new { x.IdCurso, x.FechaIncidencia });
            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Faltas)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Curso)
                .WithMany(x => x.Faltas)
                .HasForeignKey(x => x.IdCurso)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static string ConvertTipoFaltaToString(TipoFalta tipoFalta) =>
        tipoFalta switch
        {
            TipoFalta.Falta => "Falta",
            TipoFalta.Retraso => "Retraso",
            TipoFalta.SalidaDeAntes => "Salida de antes",
            _ => "Falta"
        };

    private static TipoFalta ConvertStringToTipoFalta(string tipoFalta) =>
        tipoFalta switch
        {
            "Falta" => TipoFalta.Falta,
            "Retraso" => TipoFalta.Retraso,
            "Salida de antes" => TipoFalta.SalidaDeAntes,
            _ => TipoFalta.Falta
        };
}