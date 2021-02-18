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

        public virtual DbSet<Parea> Pareas { get; set; }
        public virtual DbSet<Pcargosparticipante> Pcargosparticipantes { get; set; }
        public virtual DbSet<Pdepartamento> Pdepartamentos { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Pestadosparticipante> Pestadosparticipantes { get; set; }
        public virtual DbSet<Pestadosprogauditorium> Pestadosprogauditoria { get; set; }
        public virtual DbSet<Plaauditorium> Plaauditoria { get; set; }
        public virtual DbSet<Placronoequipo> Placronoequipos { get; set; }
        public virtual DbSet<Placronogama> Placronogamas { get; set; }
        public virtual DbSet<Pladiasequipo> Pladiasequipos { get; set; }
        public virtual DbSet<Plaplanetapa> Plaplanetapas { get; set; }
        public virtual DbSet<Pnorma> Pnormas { get; set; }
        public virtual DbSet<Ppaise> Ppaises { get; set; }
        public virtual DbSet<Praciclocronograma> Praciclocronogramas { get; set; }
        public virtual DbSet<Praciclonormassistema> Praciclonormassistemas { get; set; }
        public virtual DbSet<Pracicloparticipante> Pracicloparticipantes { get; set; }
        public virtual DbSet<Praciclosprogauditorium> Praciclosprogauditoria { get; set; }
        public virtual DbSet<Pradireccionespaproducto> Pradireccionespaproductos { get; set; }
        public virtual DbSet<Pradireccionespasistema> Pradireccionespasistemas { get; set; }
        public virtual DbSet<Praprogramasdeauditorium> Praprogramasdeauditoria { get; set; }
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

            modelBuilder.Entity<Plaauditorium>(entity =>
            {
                entity.HasKey(e => e.IdPlAauditoria)
                    .HasName("PRIMARY");

                entity.ToTable("plaauditorium");

                entity.Property(e => e.IdPlAauditoria).HasColumnName("idPlAAuditoria");

                entity.Property(e => e.FechaDeRegistro).HasColumnType("datetime");

                entity.Property(e => e.IidPrAcicloProgAuditoria).HasColumnName("iidPrACicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Placronoequipo>(entity =>
            {
                entity.HasKey(e => e.IdPlAcronoEquipo)
                    .HasName("PRIMARY");

                entity.ToTable("placronoequipo");

                entity.Property(e => e.IdPlAcronoEquipo).HasColumnName("idPlACronoEquipo");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.IdPlAcronograma).HasColumnName("idPlACronograma");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Placronogama>(entity =>
            {
                entity.HasKey(e => e.IdPlAcronograma)
                    .HasName("PRIMARY");

                entity.ToTable("placronogama");

                entity.Property(e => e.IdPlAcronograma).HasColumnName("idPlACronograma");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdPlaPlanEtapa).HasColumnName("idPlaPlanEtapa");

                entity.Property(e => e.PersonaEntrevistadaCargo)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SitioCronograma)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Pladiasequipo>(entity =>
            {
                entity.HasKey(e => e.IdPlAdiasEquipo)
                    .HasName("PRIMARY");

                entity.ToTable("pladiasequipo");

                entity.Property(e => e.IdPlAdiasEquipo).HasColumnName("idPlADiasEquipo");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Plaplanetapa>(entity =>
            {
                entity.HasKey(e => e.IdPlaPlanEtapa)
                    .HasName("PRIMARY");

                entity.ToTable("plaplanetapa");

                entity.Property(e => e.IdPlaPlanEtapa).HasColumnName("idPlaPlanEtapa");

                entity.Property(e => e.FechaDeAprobacionDeCliente).HasColumnType("datetime");

                entity.Property(e => e.FechaDeElaboracionDePa)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaDeElaboracionDePA");

                entity.Property(e => e.FechaInicioPlan)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdPlAauditoria).HasColumnName("idPlAAuditoria");

                entity.Property(e => e.QuejaVc)
                    .HasColumnType("varchar(5000)")
                    .HasColumnName("Queja_vc")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
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
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PathNorma)
                    .HasColumnType("varchar(300)")
                    .HasColumnName("pathNorma")
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

            modelBuilder.Entity<Praciclocronograma>(entity =>
            {
                entity.HasKey(e => e.IdCiclosCronogramas)
                    .HasName("PRIMARY");

                entity.ToTable("praciclocronogramas");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloCronograma");

                entity.Property(e => e.IdCiclosCronogramas).HasColumnName("idCiclosCronogramas");

                entity.Property(e => e.FechaDeFinDeEjecucionAuditoria).HasColumnType("datetime");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioDeEjecucionDeAuditoria).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Praciclocronogramas)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("FK_PrACicloCronograma");
            });

            modelBuilder.Entity<Praciclonormassistema>(entity =>
            {
                entity.HasKey(e => e.IdCicloNormaSistema)
                    .HasName("PRIMARY");

                entity.ToTable("praciclonormassistema");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloNormaSistema");

                entity.Property(e => e.IdCicloNormaSistema).HasColumnName("idCicloNormaSistema");

                entity.Property(e => e.Alcance)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.IdpNorma).HasColumnName("idpNorma");

                entity.Property(e => e.NumeroDeCertificacion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Praciclonormassistemas)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("FK_PrACicloNormaSistema");
            });

            modelBuilder.Entity<Pracicloparticipante>(entity =>
            {
                entity.HasKey(e => e.IdCicloParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("pracicloparticipantes");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloParticipantes");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.IdParticipanteWs)
                    .HasColumnType("varchar(20)")
                    .HasColumnName("idParticipante_ws")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.IdpCargoParticipante).HasColumnName("idpCargoParticipante");

                entity.Property(e => e.IdpEstadoParticipante).HasColumnName("idpEstadoParticipante");

                entity.Property(e => e.ParticipanteContextWs)
                    .HasColumnType("json")
                    .HasColumnName("ParticipanteContext_ws");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Pracicloparticipantes)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("FK_PrACicloParticipantes");
            });

            modelBuilder.Entity<Praciclosprogauditorium>(entity =>
            {
                entity.HasKey(e => e.IdPrAcicloProgAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("praciclosprogauditorium");

                entity.HasIndex(e => e.IdPrAprogramaAuditoria, "FK_PrACicloPrograma");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdPrAprogramaAuditoria).HasColumnName("idPrAProgramaAuditoria");

                entity.Property(e => e.IdpTipoAuditoria).HasColumnName("idpTipoAuditoria");

                entity.Property(e => e.NombreOrganizacionCertificado)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAprogramaAuditoriaNavigation)
                    .WithMany(p => p.Praciclosprogauditoria)
                    .HasForeignKey(d => d.IdPrAprogramaAuditoria)
                    .HasConstraintName("FK_PrACicloPrograma");
            });

            modelBuilder.Entity<Pradireccionespaproducto>(entity =>
            {
                entity.HasKey(e => e.IdDireccionPaproducto)
                    .HasName("PRIMARY");

                entity.ToTable("pradireccionespaproducto");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloDireccionesProducto");

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

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

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

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Pradireccionespaproductos)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("FK_PrACicloDireccionesProducto");
            });

            modelBuilder.Entity<Pradireccionespasistema>(entity =>
            {
                entity.HasKey(e => e.IdDireccionPasistema)
                    .HasName("PRIMARY");

                entity.ToTable("pradireccionespasistema");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloDireccionesSistema");

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

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.Oficina)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Pradireccionespasistemas)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("FK_PrACicloDireccionesSistema");
            });

            modelBuilder.Entity<Praprogramasdeauditorium>(entity =>
            {
                entity.HasKey(e => e.IdPrAprogramaAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("praprogramasdeauditorium");

                entity.Property(e => e.IdPrAprogramaAuditoria).HasColumnName("idPrAProgramaAuditoria");

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
