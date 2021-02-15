using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class ibnorca_mokContext : DbContext
    {
        public ibnorca_mokContext()
        {
        }

        public ibnorca_mokContext(DbContextOptions<ibnorca_mokContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ciclocronograma> Ciclocronogramas { get; set; }
        public virtual DbSet<Ciclonormassistema> Ciclonormassistemas { get; set; }
        public virtual DbSet<Cicloparticipante> Cicloparticipantes { get; set; }
        public virtual DbSet<Ciclosprogauditorium> Ciclosprogauditoria { get; set; }
        public virtual DbSet<Direccionespaproducto> Direccionespaproductos { get; set; }
        public virtual DbSet<Direccionespasistema> Direccionespasistemas { get; set; }
        public virtual DbSet<Parea> Pareas { get; set; }
        public virtual DbSet<Pcargosparticipante> Pcargosparticipantes { get; set; }
        public virtual DbSet<Pdepartamento> Pdepartamentos { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Pestadosparticipante> Pestadosparticipantes { get; set; }
        public virtual DbSet<Pestadosprogauditorium> Pestadosprogauditoria { get; set; }
        public virtual DbSet<Pnorma> Pnormas { get; set; }
        public virtual DbSet<Ppaise> Ppaises { get; set; }
        public virtual DbSet<Programasdeauditorium> Programasdeauditoria { get; set; }
        public virtual DbSet<Ptipoauditorium> Ptipoauditoria { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=192.168.0.105;database=ibnorca_mok;user=ibnorca;password=admin.123;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciclocronograma>(entity =>
            {
                entity.HasKey(e => e.IdCiclosCronogramas)
                    .HasName("PRIMARY");

                entity.ToTable("ciclocronogramas");

                entity.HasIndex(e => e.IdCicloProgAuditoria, "FK_CicloCronograma");

                entity.Property(e => e.IdCiclosCronogramas).HasColumnName("idCiclosCronogramas");

                entity.Property(e => e.FechaDeFinDeEjecucionAuditoria).HasColumnType("datetime");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioDeEjecucionDeAuditoria).HasColumnType("datetime");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloProgAuditoriaNavigation)
                    .WithMany(p => p.Ciclocronogramas)
                    .HasForeignKey(d => d.IdCicloProgAuditoria)
                    .HasConstraintName("FK_CicloCronograma");
            });

            modelBuilder.Entity<Ciclonormassistema>(entity =>
            {
                entity.HasKey(e => e.IdCicloNormaSistema)
                    .HasName("PRIMARY");

                entity.ToTable("ciclonormassistema");

                entity.HasIndex(e => e.IdCicloProgAuditoria, "FK_CicloNormaSistema");

                entity.Property(e => e.IdCicloNormaSistema).HasColumnName("idCicloNormaSistema");

                entity.Property(e => e.Alcance)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.IdpNorma).HasColumnName("idpNorma");

                entity.Property(e => e.NumeroDeCertificacion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloProgAuditoriaNavigation)
                    .WithMany(p => p.Ciclonormassistemas)
                    .HasForeignKey(d => d.IdCicloProgAuditoria)
                    .HasConstraintName("FK_CicloNormaSistema");
            });

            modelBuilder.Entity<Cicloparticipante>(entity =>
            {
                entity.HasKey(e => e.IdCicloParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("cicloparticipantes");

                entity.HasIndex(e => e.IdCicloProgAuditoria, "FK_CicloParticipantes");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.IdParticipanteWs)
                    .HasColumnType("varchar(20)")
                    .HasColumnName("idParticipante_ws")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdpCargoParticipante).HasColumnName("idpCargoParticipante");

                entity.Property(e => e.IdpEstadoParticipante).HasColumnName("idpEstadoParticipante");

                entity.Property(e => e.ParticipanteContextWs)
                    .HasColumnType("json")
                    .HasColumnName("ParticipanteContext_ws");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloProgAuditoriaNavigation)
                    .WithMany(p => p.Cicloparticipantes)
                    .HasForeignKey(d => d.IdCicloProgAuditoria)
                    .HasConstraintName("FK_CicloParticipantes");
            });

            modelBuilder.Entity<Ciclosprogauditorium>(entity =>
            {
                entity.HasKey(e => e.IdCicloProgAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("ciclosprogauditorium");

                entity.HasIndex(e => e.IdProgramaAuditoria, "FK_CicloPrograma");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdProgramaAuditoria).HasColumnName("idProgramaAuditoria");

                entity.Property(e => e.IdpTipoAuditoria).HasColumnName("idpTipoAuditoria");

                entity.Property(e => e.NombreOrganizacionCertificado)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdProgramaAuditoriaNavigation)
                    .WithMany(p => p.Ciclosprogauditoria)
                    .HasForeignKey(d => d.IdProgramaAuditoria)
                    .HasConstraintName("FK_CicloPrograma");
            });

            modelBuilder.Entity<Direccionespaproducto>(entity =>
            {
                entity.HasKey(e => e.IdDireccionPaproducto)
                    .HasName("PRIMARY");

                entity.ToTable("direccionespaproducto");

                entity.HasIndex(e => e.IdCicloProgAuditoria, "FK_CicloDireccionesProducto");

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.Marca)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NumeroDeCertificacion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloProgAuditoriaNavigation)
                    .WithMany(p => p.Direccionespaproductos)
                    .HasForeignKey(d => d.IdCicloProgAuditoria)
                    .HasConstraintName("FK_CicloDireccionesProducto");
            });

            modelBuilder.Entity<Direccionespasistema>(entity =>
            {
                entity.HasKey(e => e.IdDireccionPasistema)
                    .HasName("PRIMARY");

                entity.ToTable("direccionespasistema");

                entity.HasIndex(e => e.IdCicloProgAuditoria, "FK_CicloDireccionesSistema");

                entity.Property(e => e.IdDireccionPasistema).HasColumnName("idDireccionPASistema");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdCicloProgAuditoria).HasColumnName("idCicloProgAuditoria");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.Oficina)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloProgAuditoriaNavigation)
                    .WithMany(p => p.Direccionespasistemas)
                    .HasForeignKey(d => d.IdCicloProgAuditoria)
                    .HasConstraintName("FK_CicloDireccionesSistema");
            });

            modelBuilder.Entity<Parea>(entity =>
            {
                entity.HasKey(e => e.IdpArea)
                    .HasName("PRIMARY");

                entity.ToTable("parea");

                entity.Property(e => e.IdpArea).HasColumnName("idpArea");

                entity.Property(e => e.Area)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pcargosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdpCargoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("pcargosparticipantes");

                entity.Property(e => e.IdpCargoParticipante).HasColumnName("idpCargoParticipante");

                entity.Property(e => e.CargoParticipante)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro");

                entity.Property(e => e.IdpTipoCertificacion).HasColumnName("idpTipoCertificacion");
            });

            modelBuilder.Entity<Pdepartamento>(entity =>
            {
                entity.HasKey(e => e.IdpDepartamento)
                    .HasName("PRIMARY");

                entity.ToTable("pdepartamentos");

                entity.Property(e => e.IdpDepartamento).HasColumnName("idpDepartamento");

                entity.Property(e => e.Departamento)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpPais).HasColumnName("idpPais");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Idperson)
                    .HasName("PRIMARY");

                entity.ToTable("person");

                entity.Property(e => e.Idperson).HasColumnName("idperson");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnType("varchar(250)")
                    .HasColumnName("lastname")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(250)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Pestadosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdpEstadoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("pestadosparticipante");

                entity.Property(e => e.IdpEstadoParticipante).HasColumnName("idpEstadoParticipante");

                entity.Property(e => e.EstadoParticipante)
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pestadosprogauditorium>(entity =>
            {
                entity.HasKey(e => e.IdpEstadosProgAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("pestadosprogauditoria");

                entity.Property(e => e.IdpEstadosProgAuditoria).HasColumnName("idpEstadosProgAuditoria");

                entity.Property(e => e.EstadosProgAuditoria)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pnorma>(entity =>
            {
                entity.HasKey(e => e.IdpNorma)
                    .HasName("PRIMARY");

                entity.ToTable("pnormas");

                entity.Property(e => e.IdpNorma).HasColumnName("idpNorma");

                entity.Property(e => e.CodigoDeNorma)
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpArea).HasColumnName("idpArea");

                entity.Property(e => e.Norma)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Ppaise>(entity =>
            {
                entity.HasKey(e => e.IdpPais)
                    .HasName("PRIMARY");

                entity.ToTable("ppaises");

                entity.Property(e => e.IdpPais).HasColumnName("idpPais");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Programasdeauditorium>(entity =>
            {
                entity.HasKey(e => e.IdProgramaAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("programasdeauditorium");

                entity.Property(e => e.IdProgramaAuditoria).HasColumnName("idProgramaAuditoria");

                entity.Property(e => e.CodigoServicioWs)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("CodigoServicio_ws")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DetalleServicio).HasColumnType("json");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdCodigoDeServicioCodigoIafWs)
                    .HasColumnType("json")
                    .HasColumnName("idCodigoDeServicioCodigoIAF_ws");

                entity.Property(e => e.IdExternalsWs)
                    .HasColumnType("json")
                    .HasColumnName("idExternals_ws");

                entity.Property(e => e.IdOrganizacionWs)
                    .HasColumnType("varchar(20)")
                    .HasColumnName("idOrganizacion_ws")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdpArea).HasColumnName("idpArea");

                entity.Property(e => e.IdpDepartamento).HasColumnName("idpDepartamento");

                entity.Property(e => e.IdpEstadosProgAuditoria).HasColumnName("idpEstadosProgAuditoria");

                entity.Property(e => e.IdpPais).HasColumnName("idpPais");

                entity.Property(e => e.IdpTipoServicio).HasColumnName("idpTipoServicio");

                entity.Property(e => e.Nit)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("NIT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OrganizacionContentWs)
                    .HasColumnType("json")
                    .HasColumnName("OrganizacionContent_ws");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Ptipoauditorium>(entity =>
            {
                entity.HasKey(e => e.IdpTipoAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("ptipoauditoria");

                entity.Property(e => e.IdpTipoAuditoria).HasColumnName("idpTipoAuditoria");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpTipoCertificacion).HasColumnName("idpTipoCertificacion");

                entity.Property(e => e.TipoAuditoria)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
