using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Business.Main.DataMapping
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

        public virtual DbSet<Elaauditorium> Elaauditoria { get; set; }
        public virtual DbSet<Elacronogama> Elacronogamas { get; set; }
        public virtual DbSet<Paramarea> Paramareas { get; set; }
        public virtual DbSet<Paramcargosparticipante> Paramcargosparticipantes { get; set; }
        public virtual DbSet<Paramdepartamento> Paramdepartamentos { get; set; }
        public virtual DbSet<Paramestadosparticipante> Paramestadosparticipantes { get; set; }
        public virtual DbSet<Paramestadosprogauditorium> Paramestadosprogauditoria { get; set; }
        public virtual DbSet<Parametapaauditorium> Parametapaauditoria { get; set; }
        public virtual DbSet<Paramitemselect> Paramitemselects { get; set; }
        public virtual DbSet<Paramlistasitemselect> Paramlistasitemselects { get; set; }
        public virtual DbSet<Paramnorma> Paramnormas { get; set; }
        public virtual DbSet<Parampaise> Parampaises { get; set; }
        public virtual DbSet<Paramtipoauditorium> Paramtipoauditoria { get; set; }
        public virtual DbSet<Paramtiposervicio> Paramtiposervicios { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Plaauditorium> Plaauditoria { get; set; }
        public virtual DbSet<Placronoequipo> Placronoequipos { get; set; }
        public virtual DbSet<Pladiasequipo> Pladiasequipos { get; set; }
        public virtual DbSet<Praciclocronograma> Praciclocronogramas { get; set; }
        public virtual DbSet<Praciclonormassistema> Praciclonormassistemas { get; set; }
        public virtual DbSet<Pracicloparticipante> Pracicloparticipantes { get; set; }
        public virtual DbSet<Praciclosprogauditorium> Praciclosprogauditoria { get; set; }
        public virtual DbSet<Pradireccionespaproducto> Pradireccionespaproductos { get; set; }
        public virtual DbSet<Pradireccionespasistema> Pradireccionespasistemas { get; set; }
        public virtual DbSet<Praprogramasdeauditorium> Praprogramasdeauditoria { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=192.168.0.106;database=ibnorca_mok;user=ibnorca;password=admin.123;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elaauditorium>(entity =>
            {
                entity.HasKey(e => e.IdelaAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("elaauditoria");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "fk_auditoria_ciclo_idx");

                entity.Property(e => e.IdelaAuditoria)
                    .ValueGeneratedNever()
                    .HasColumnName("idelaAuditoria");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Elaauditoria)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .HasConstraintName("fk_elaauditoria_pracicloauditoria");
            });

            modelBuilder.Entity<Elacronogama>(entity =>
            {
                entity.HasKey(e => e.IdElAcronograma)
                    .HasName("PRIMARY");

                entity.ToTable("elacronogama");

                entity.HasIndex(e => e.IdDireccionPasistema, "fk_elacronograma_direccionessistema_idx");

                entity.HasIndex(e => e.IdDireccionPaproducto, "fk_elacronograma_direccionproducto_idx");

                entity.HasIndex(e => e.Idelaauditoria, "fk_elacronograma_idx");

                entity.HasIndex(e => e.IdCicloParticipante, "fk_elacronograma_participantes");

                entity.Property(e => e.IdElAcronograma).HasColumnName("idElACronograma");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Horario)
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.IdDireccionPasistema).HasColumnName("idDireccionPASistema");

                entity.Property(e => e.PersonaEntrevistadaCargo)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RequisitosEsquema)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdCicloParticipanteNavigation)
                    .WithMany(p => p.Elacronogamas)
                    .HasForeignKey(d => d.IdCicloParticipante)
                    .HasConstraintName("fk_elacronograma_participantes");

                entity.HasOne(d => d.IdDireccionPaproductoNavigation)
                    .WithMany(p => p.Elacronogamas)
                    .HasForeignKey(d => d.IdDireccionPaproducto)
                    .HasConstraintName("fk_elacronograma_direccionproducto");

                entity.HasOne(d => d.IdDireccionPasistemaNavigation)
                    .WithMany(p => p.Elacronogamas)
                    .HasForeignKey(d => d.IdDireccionPasistema)
                    .HasConstraintName("fk_elacronograma_direccionessistema");

                entity.HasOne(d => d.IdelaauditoriaNavigation)
                    .WithMany(p => p.Elacronogamas)
                    .HasForeignKey(d => d.Idelaauditoria)
                    .HasConstraintName("fk_elacronograma");
            });

            modelBuilder.Entity<Paramarea>(entity =>
            {
                entity.HasKey(e => e.IdparamArea)
                    .HasName("PRIMARY");

                entity.ToTable("paramarea");

                entity.Property(e => e.IdparamArea).HasColumnName("idparamArea");

                entity.Property(e => e.Area)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Paramcargosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdparamCargoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("paramcargosparticipantes");

                entity.Property(e => e.IdparamCargoParticipante).HasColumnName("idparamCargoParticipante");

                entity.Property(e => e.CargoParticipante)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro");

                entity.Property(e => e.IdpTipoCertificacion).HasColumnName("idpTipoCertificacion");
            });

            modelBuilder.Entity<Paramdepartamento>(entity =>
            {
                entity.HasKey(e => e.IdparamDepartamento)
                    .HasName("PRIMARY");

                entity.ToTable("paramdepartamentos");

                entity.Property(e => e.IdparamDepartamento).HasColumnName("idparamDepartamento");

                entity.Property(e => e.Departamento)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdparamPais).HasColumnName("idparamPais");
            });

            modelBuilder.Entity<Paramestadosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdparamEstadoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("paramestadosparticipante");

                entity.Property(e => e.IdparamEstadoParticipante).HasColumnName("idparamEstadoParticipante");

                entity.Property(e => e.EstadoParticipante)
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Paramestadosprogauditorium>(entity =>
            {
                entity.HasKey(e => e.IdparamEstadosProgAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("paramestadosprogauditoria");

                entity.Property(e => e.IdparamEstadosProgAuditoria).HasColumnName("idparamEstadosProgAuditoria");

                entity.Property(e => e.EstadosProgAuditoria)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Parametapaauditorium>(entity =>
            {
                entity.HasKey(e => e.IdParametapaauditoria)
                    .HasName("PRIMARY");

                entity.ToTable("parametapaauditoria");

                entity.Property(e => e.IdParametapaauditoria)
                    .ValueGeneratedNever()
                    .HasColumnName("idParametapaauditoria");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Paramitemselect>(entity =>
            {
                entity.HasKey(e => e.IdparamItemSelect)
                    .HasName("PRIMARY");

                entity.ToTable("paramitemselect");

                entity.Property(e => e.IdparamItemSelect).HasColumnName("idparamItemSelect");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdParamListaItemSelect).HasColumnName("idParamListaItemSelect");

                entity.Property(e => e.ItemSelect)
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Paramlistasitemselect>(entity =>
            {
                entity.HasKey(e => e.IdParamListaItemSelect)
                    .HasName("PRIMARY");

                entity.ToTable("paramlistasitemselect");

                entity.Property(e => e.IdParamListaItemSelect).HasColumnName("idParamListaItemSelect");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Lista)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("lista")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Paramnorma>(entity =>
            {
                entity.HasKey(e => e.IdparamNorma)
                    .HasName("PRIMARY");

                entity.ToTable("paramnormas");

                entity.Property(e => e.IdparamNorma).HasColumnName("idparamNorma");

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

            modelBuilder.Entity<Parampaise>(entity =>
            {
                entity.HasKey(e => e.IdparamPais)
                    .HasName("PRIMARY");

                entity.ToTable("parampaises");

                entity.Property(e => e.IdparamPais).HasColumnName("idparamPais");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Paramtipoauditorium>(entity =>
            {
                entity.HasKey(e => e.IdparamTipoAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("paramtipoauditoria");

                entity.Property(e => e.IdparamTipoAuditoria).HasColumnName("idparamTipoAuditoria");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpTipoCertificacion).HasColumnName("idpTipoCertificacion");

                entity.Property(e => e.TipoAuditoria)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Paramtiposervicio>(entity =>
            {
                entity.HasKey(e => e.IdparamTipoServicio)
                    .HasName("PRIMARY");

                entity.ToTable("paramtiposervicio");

                entity.Property(e => e.IdparamTipoServicio).HasColumnName("idparamTipoServicio");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.TipoServicio)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
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

            modelBuilder.Entity<Praciclocronograma>(entity =>
            {
                entity.HasKey(e => e.IdCiclosCronogramas)
                    .HasName("PRIMARY");

                entity.ToTable("praciclocronogramas");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloCronograma");

                entity.Property(e => e.IdCiclosCronogramas).HasColumnName("idCiclosCronogramas");

                entity.Property(e => e.DiasInsitu).HasPrecision(10, 2);

                entity.Property(e => e.DiasRemoto).HasPrecision(10, 2);

                entity.Property(e => e.FechaDeFinDeEjecucionAuditoria).HasColumnType("datetime");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioDeEjecucionDeAuditoria).HasColumnType("datetime");

                entity.Property(e => e.HorarioTrabajo)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.MesProgramado).HasColumnType("datetime");

                entity.Property(e => e.MesReprogramado).HasColumnType("datetime");

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

                entity.Property(e => e.IdparamNorma).HasColumnName("idparamNorma");

                entity.Property(e => e.Norma)
                    .HasColumnType("varchar(1000)")
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

                entity.Property(e => e.CargoDetalleWs).HasColumnType("json");

                entity.Property(e => e.DiasInsistu)
                    .HasPrecision(10, 2)
                    .HasColumnName("diasInsistu");

                entity.Property(e => e.DiasRemoto)
                    .HasPrecision(10, 2)
                    .HasColumnName("diasRemoto");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.IdParticipanteWs).HasColumnName("idParticipante_ws");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.ParticipanteDetalleWs)
                    .HasColumnType("json")
                    .HasColumnName("ParticipanteDetalle_ws");

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

                entity.HasIndex(e => e.IdParametapaauditoria, "FK_PraCicloEstado_idx");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.EstadoDescripcion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdParametapaauditoria).HasColumnName("idParametapaauditoria");

                entity.Property(e => e.IdPrAprogramaAuditoria).HasColumnName("idPrAProgramaAuditoria");

                entity.Property(e => e.IdparamEstadosProgAuditoria).HasColumnName("idparamEstadosProgAuditoria");

                entity.Property(e => e.IdparamTipoAuditoria).HasColumnName("idparamTipoAuditoria");

                entity.Property(e => e.NombreOrganizacionCertificado)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Referencia)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdParametapaauditoriaNavigation)
                    .WithMany(p => p.Praciclosprogauditoria)
                    .HasForeignKey(d => d.IdParametapaauditoria)
                    .HasConstraintName("FK_PraCicloEstado");

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

                entity.Property(e => e.IdDireccionPaproducto)
                    .ValueGeneratedNever()
                    .HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Estado)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.Marca)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Norma)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NumeroDeCertificacion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Sello)
                    .HasColumnType("varchar(50)")
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

                entity.Property(e => e.IdDireccionPasistema)
                    .ValueGeneratedNever()
                    .HasColumnName("idDireccionPASistema");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Departamento)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(100)")
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

                entity.Property(e => e.CodigoIafws)
                    .HasColumnType("varchar(60)")
                    .HasColumnName("CodigoIAFWS")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CodigoServicioWs)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("CodigoServicioWS")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DetalleServicioWs)
                    .HasColumnType("json")
                    .HasColumnName("DetalleServicioWS");

                entity.Property(e => e.Estado)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Fecha)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdOrganizacionWs)
                    .HasColumnType("varchar(20)")
                    .HasColumnName("IdOrganizacionWS")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdparamArea).HasColumnName("idparamArea");

                entity.Property(e => e.IdparamTipoServicio).HasColumnName("idparamTipoServicio");

                entity.Property(e => e.Nit)
                    .HasColumnType("varchar(10)")
                    .HasColumnName("NIT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Oficina)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OrganismoCertificador)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OrganizacionContentWs)
                    .HasColumnType("json")
                    .HasColumnName("OrganizacionContentWS");

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
