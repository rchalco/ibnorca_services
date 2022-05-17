using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Business.Main.DataMapping
{
    public partial class sigadContext : DbContext
    {
        public sigadContext()
        {
        }

        public sigadContext(DbContextOptions<sigadContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Elaadp> Elaadps { get; set; }
        public virtual DbSet<Elaauditorium> Elaauditoria { get; set; }
        public virtual DbSet<Elacontenidoauditorium> Elacontenidoauditoria { get; set; }
        public virtual DbSet<Elacronogama> Elacronogamas { get; set; }
        public virtual DbSet<Elahallazgo> Elahallazgos { get; set; }
        public virtual DbSet<Elalistaspredefinida> Elalistaspredefinidas { get; set; }
        public virtual DbSet<Importacionsolicitud> Importacionsolicituds { get; set; }
        public virtual DbSet<Paramarea> Paramareas { get; set; }
        public virtual DbSet<Paramcargosparticipante> Paramcargosparticipantes { get; set; }
        public virtual DbSet<Paramdepartamento> Paramdepartamentos { get; set; }
        public virtual DbSet<Paramdocumento> Paramdocumentos { get; set; }
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
        public virtual DbSet<Tmddocumentacionauditorium> Tmddocumentacionauditoria { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=admin.123;database=sigad", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Elaadp>(entity =>
            {
                entity.HasKey(e => e.Idelaadp)
                    .HasName("PRIMARY");

                entity.ToTable("elaadp");

                entity.HasComment("registro de areas de preocupacion	");

                entity.HasIndex(e => e.IdelaAuditoria, "fk_apd_auditoria_idx");

                entity.Property(e => e.Idelaadp).HasColumnName("idelaadp");

                entity.Property(e => e.Area)
                    .HasMaxLength(100)
                    .HasColumnName("area");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasMaxLength(45)
                    .HasColumnName("fecha");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.IdelaAuditoriaNavigation)
                    .WithMany(p => p.Elaadps)
                    .HasForeignKey(d => d.IdelaAuditoria)
                    .HasConstraintName("fk_apd_auditoria");
            });

            modelBuilder.Entity<Elaauditorium>(entity =>
            {
                entity.HasKey(e => e.IdelaAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("elaauditoria");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "fk_auditoria_ciclo_idx");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(100);

                entity.HasOne(d => d.IdPrAcicloProgAuditoriaNavigation)
                    .WithMany(p => p.Elaauditoria)
                    .HasForeignKey(d => d.IdPrAcicloProgAuditoria)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_elaauditoria_pracicloauditoria");
            });

            modelBuilder.Entity<Elacontenidoauditorium>(entity =>
            {
                entity.HasKey(e => e.IdelaContenidoauditoria)
                    .HasName("PRIMARY");

                entity.ToTable("elacontenidoauditoria");

                entity.HasIndex(e => e.IdelaAuditoria, "fk_contenido_auditoria_idx");

                entity.Property(e => e.IdelaContenidoauditoria).HasColumnName("idela_contenidoauditoria");

                entity.Property(e => e.Area)
                    .HasMaxLength(45)
                    .HasColumnName("area");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .HasColumnName("categoria");

                entity.Property(e => e.Contenido)
                    .HasMaxLength(2000)
                    .HasColumnName("contenido");

                entity.Property(e => e.Endocumento).HasColumnName("endocumento");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Label)
                    .HasMaxLength(500)
                    .HasColumnName("label");

                entity.Property(e => e.Nemotico)
                    .HasMaxLength(100)
                    .HasColumnName("nemotico");

                entity.Property(e => e.Seleccionado).HasColumnName("seleccionado");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(300)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdelaAuditoriaNavigation)
                    .WithMany(p => p.Elacontenidoauditoria)
                    .HasForeignKey(d => d.IdelaAuditoria)
                    .HasConstraintName("fk_contenido_auditoria");
            });

            modelBuilder.Entity<Elacronogama>(entity =>
            {
                entity.HasKey(e => e.IdElAcronograma)
                    .HasName("PRIMARY");

                entity.ToTable("elacronogama");

                entity.HasIndex(e => e.Idelaauditoria, "fk_cronograma_auditoria_idx");

                entity.Property(e => e.IdElAcronograma).HasColumnName("idElACronograma");

                entity.Property(e => e.Auditor).HasMaxLength(100);

                entity.Property(e => e.Cargo).HasMaxLength(100);

                entity.Property(e => e.Direccion).HasMaxLength(500);

                entity.Property(e => e.FechaFin).HasMaxLength(50);

                entity.Property(e => e.FechaInicio).HasMaxLength(50);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Horario).HasMaxLength(100);

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.IdDireccionPasistema).HasColumnName("idDireccionPASistema");

                entity.Property(e => e.PersonaEntrevistadaCargo).HasMaxLength(200);

                entity.Property(e => e.ProcesoArea).HasMaxLength(100);

                entity.Property(e => e.RequisitosEsquema).HasMaxLength(500);

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

                entity.HasOne(d => d.IdelaauditoriaNavigation)
                    .WithMany(p => p.Elacronogamas)
                    .HasForeignKey(d => d.Idelaauditoria)
                    .HasConstraintName("fk_cronograma_auditoria");
            });

            modelBuilder.Entity<Elahallazgo>(entity =>
            {
                entity.HasKey(e => e.Idelahallazgo)
                    .HasName("PRIMARY");

                entity.ToTable("elahallazgo");

                entity.HasIndex(e => e.IdelaAuditoria, "fk_hallazgo_auditoria_idx");

                entity.Property(e => e.Idelahallazgo).HasColumnName("idelahallazgo");

                entity.Property(e => e.Fecha)
                    .HasMaxLength(50)
                    .HasColumnName("fecha");

                entity.Property(e => e.Hallazgo)
                    .HasMaxLength(1000)
                    .HasColumnName("hallazgo");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Normas)
                    .HasMaxLength(500)
                    .HasColumnName("normas");

                entity.Property(e => e.Proceso)
                    .HasMaxLength(200)
                    .HasColumnName("proceso");

                entity.Property(e => e.Sitio)
                    .HasMaxLength(200)
                    .HasColumnName("sitio");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(45)
                    .HasColumnName("tipo");

                entity.Property(e => e.TipoNemotico)
                    .HasMaxLength(45)
                    .HasColumnName("tipo_nemotico");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.IdelaAuditoriaNavigation)
                    .WithMany(p => p.Elahallazgos)
                    .HasForeignKey(d => d.IdelaAuditoria)
                    .HasConstraintName("fk_hallazgo_auditoria");
            });

            modelBuilder.Entity<Elalistaspredefinida>(entity =>
            {
                entity.HasKey(e => e.Idelalistaspredefinidas)
                    .HasName("PRIMARY");

                entity.ToTable("elalistaspredefinidas");

                entity.Property(e => e.Idelalistaspredefinidas).HasColumnName("idelalistaspredefinidas");

                entity.Property(e => e.Area)
                    .HasMaxLength(45)
                    .HasColumnName("area");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .HasColumnName("categoria");

                entity.Property(e => e.Decripcion)
                    .HasMaxLength(2000)
                    .HasColumnName("decripcion");

                entity.Property(e => e.Endocumento).HasColumnName("endocumento");

                entity.Property(e => e.Label)
                    .HasMaxLength(500)
                    .HasColumnName("label");

                entity.Property(e => e.Nemotico)
                    .HasMaxLength(100)
                    .HasColumnName("nemotico");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(300)
                    .HasColumnName("titulo");
            });

            modelBuilder.Entity<Importacionsolicitud>(entity =>
            {
                entity.HasKey(e => e.IdimportacionSolicitud)
                    .HasName("PRIMARY");

                entity.ToTable("importacionsolicitud");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdimportacionSolicitud).HasColumnName("idimportacionSolicitud");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(500)
                    .HasColumnName("cliente");

                entity.Property(e => e.Detalle)
                    .HasColumnType("json")
                    .HasColumnName("detalle");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_registro");

                entity.Property(e => e.Nit)
                    .HasMaxLength(100)
                    .HasColumnName("nit");
            });

            modelBuilder.Entity<Paramarea>(entity =>
            {
                entity.HasKey(e => e.IdparamArea)
                    .HasName("PRIMARY");

                entity.ToTable("paramarea");

                entity.Property(e => e.IdparamArea).HasColumnName("idparamArea");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Paramcargosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdparamCargoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("paramcargosparticipantes");

                entity.Property(e => e.IdparamCargoParticipante).HasColumnName("idparamCargoParticipante");

                entity.Property(e => e.CargoParticipante).HasMaxLength(100);

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

                entity.Property(e => e.Departamento).HasMaxLength(150);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdparamPais).HasColumnName("idparamPais");
            });

            modelBuilder.Entity<Paramdocumento>(entity =>
            {
                entity.HasKey(e => e.Idparamdocumentos)
                    .HasName("PRIMARY");

                entity.ToTable("paramdocumentos");

                entity.Property(e => e.Idparamdocumentos).HasColumnName("idparamdocumentos");

                entity.Property(e => e.Area).HasMaxLength(20);

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.Method).HasMaxLength(500);

                entity.Property(e => e.NombrePlantilla).HasMaxLength(100);

                entity.Property(e => e.Path).HasMaxLength(300);

                entity.Property(e => e.Proceso).HasMaxLength(70);
            });

            modelBuilder.Entity<Paramestadosparticipante>(entity =>
            {
                entity.HasKey(e => e.IdparamEstadoParticipante)
                    .HasName("PRIMARY");

                entity.ToTable("paramestadosparticipante");

                entity.Property(e => e.IdparamEstadoParticipante).HasColumnName("idparamEstadoParticipante");

                entity.Property(e => e.EstadoParticipante).HasMaxLength(30);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<Paramestadosprogauditorium>(entity =>
            {
                entity.HasKey(e => e.IdparamEstadosProgAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("paramestadosprogauditoria");

                entity.Property(e => e.IdparamEstadosProgAuditoria).HasColumnName("idparamEstadosProgAuditoria");

                entity.Property(e => e.EstadosProgAuditoria).HasMaxLength(50);

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

                entity.Property(e => e.Descripcion).HasMaxLength(200);
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
                    .HasMaxLength(2000)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
            });

            modelBuilder.Entity<Paramlistasitemselect>(entity =>
            {
                entity.HasKey(e => e.IdParamListaItemSelect)
                    .HasName("PRIMARY");

                entity.ToTable("paramlistasitemselect");

                entity.Property(e => e.IdParamListaItemSelect).HasColumnName("idParamListaItemSelect");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Lista)
                    .HasMaxLength(100)
                    .HasColumnName("lista");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
            });

            modelBuilder.Entity<Paramnorma>(entity =>
            {
                entity.HasKey(e => e.IdparamNorma)
                    .HasName("PRIMARY");

                entity.ToTable("paramnormas");

                entity.Property(e => e.IdparamNorma).HasColumnName("idparamNorma");

                entity.Property(e => e.CodigoDeNorma).HasMaxLength(30);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpArea).HasColumnName("idpArea");

                entity.Property(e => e.Norma).HasMaxLength(500);

                entity.Property(e => e.PathNorma)
                    .HasMaxLength(300)
                    .HasColumnName("pathNorma");
            });

            modelBuilder.Entity<Parampaise>(entity =>
            {
                entity.HasKey(e => e.IdparamPais)
                    .HasName("PRIMARY");

                entity.ToTable("parampaises");

                entity.Property(e => e.IdparamPais).HasColumnName("idparamPais");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Pais).HasMaxLength(150);
            });

            modelBuilder.Entity<Paramtipoauditorium>(entity =>
            {
                entity.HasKey(e => e.IdparamTipoAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("paramtipoauditoria");

                entity.Property(e => e.IdparamTipoAuditoria).HasColumnName("idparamTipoAuditoria");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdpTipoCertificacion).HasColumnName("idpTipoCertificacion");

                entity.Property(e => e.TipoAuditoria).HasMaxLength(50);
            });

            modelBuilder.Entity<Paramtiposervicio>(entity =>
            {
                entity.HasKey(e => e.IdparamTipoServicio)
                    .HasName("PRIMARY");

                entity.ToTable("paramtiposervicio");

                entity.Property(e => e.IdparamTipoServicio).HasColumnName("idparamTipoServicio");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.TipoServicio).HasMaxLength(50);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Idperson)
                    .HasName("PRIMARY");

                entity.ToTable("person");

                entity.Property(e => e.Idperson).HasColumnName("idperson");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("lastname");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Plaauditorium>(entity =>
            {
                entity.HasKey(e => e.IdPlAauditoria)
                    .HasName("PRIMARY");

                entity.ToTable("plaauditorium");

                entity.Property(e => e.IdPlAauditoria).HasColumnName("idPlAAuditoria");

                entity.Property(e => e.FechaDeRegistro).HasColumnType("datetime");

                entity.Property(e => e.IidPrAcicloProgAuditoria).HasColumnName("iidPrACicloProgAuditoria");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
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

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
            });

            modelBuilder.Entity<Pladiasequipo>(entity =>
            {
                entity.HasKey(e => e.IdPlAdiasEquipo)
                    .HasName("PRIMARY");

                entity.ToTable("pladiasequipo");

                entity.Property(e => e.IdPlAdiasEquipo).HasColumnName("idPlADiasEquipo");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.IdCicloParticipante).HasColumnName("idCicloParticipante");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
            });

            modelBuilder.Entity<Praciclocronograma>(entity =>
            {
                entity.HasKey(e => e.IdCiclosCronogramas)
                    .HasName("PRIMARY");

                entity.ToTable("praciclocronogramas");

                entity.HasIndex(e => e.IdPrAcicloProgAuditoria, "FK_PrACicloCronograma");

                entity.Property(e => e.IdCiclosCronogramas).HasColumnName("idCiclosCronogramas");

                entity.Property(e => e.DiasInsitu).HasPrecision(10, 2);

                entity.Property(e => e.DiasPresupuesto).HasPrecision(10, 2);

                entity.Property(e => e.DiasRemoto).HasPrecision(10, 2);

                entity.Property(e => e.FechaDeFinDeEjecucionAuditoria).HasColumnType("datetime");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioDeEjecucionDeAuditoria).HasColumnType("datetime");

                entity.Property(e => e.HorarioTrabajo).HasMaxLength(100);

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.MesProgramado).HasColumnType("datetime");

                entity.Property(e => e.MesReprogramado).HasColumnType("datetime");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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

                entity.Property(e => e.Alcance).HasMaxLength(500);

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.IdparamNorma).HasColumnName("idparamNorma");

                entity.Property(e => e.Norma).HasMaxLength(1000);

                entity.Property(e => e.NumeroDeCertificacion).HasMaxLength(100);

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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

                entity.Property(e => e.EstadoDescripcion).HasMaxLength(100);

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdParametapaauditoria).HasColumnName("idParametapaauditoria");

                entity.Property(e => e.IdPrAprogramaAuditoria).HasColumnName("idPrAProgramaAuditoria");

                entity.Property(e => e.IdparamEstadosProgAuditoria).HasColumnName("idparamEstadosProgAuditoria");

                entity.Property(e => e.IdparamTipoAuditoria).HasColumnName("idparamTipoAuditoria");

                entity.Property(e => e.NombreOrganizacionCertificado).HasMaxLength(150);

                entity.Property(e => e.Referencia).HasMaxLength(500);

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.Ciudad).HasMaxLength(100);

                entity.Property(e => e.Direccion).HasMaxLength(1000);

                entity.Property(e => e.Estado).HasMaxLength(45);

                entity.Property(e => e.EstadoConcer)
                    .HasMaxLength(100)
                    .HasColumnName("EstadoCONCER");

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaEmisionPrimerCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoCertificado).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoUltimoCertificado).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.Marca).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(1000);

                entity.Property(e => e.Norma).HasMaxLength(100);

                entity.Property(e => e.NumeroDeCertificacion).HasMaxLength(100);

                entity.Property(e => e.Pais).HasMaxLength(45);

                entity.Property(e => e.ReivsionConcer)
                    .HasMaxLength(600)
                    .HasColumnName("ReivsionCONCER");

                entity.Property(e => e.Sello).HasMaxLength(50);

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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

                entity.Property(e => e.Ciudad).HasMaxLength(100);

                entity.Property(e => e.Departamento).HasMaxLength(100);

                entity.Property(e => e.Dias).HasPrecision(10, 2);

                entity.Property(e => e.Direccion).HasMaxLength(150);

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdPrAcicloProgAuditoria).HasColumnName("idPrACicloProgAuditoria");

                entity.Property(e => e.Nombre).HasMaxLength(150);

                entity.Property(e => e.Pais).HasMaxLength(100);

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);

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
                    .HasMaxLength(60)
                    .HasColumnName("CodigoIAFWS");

                entity.Property(e => e.CodigoServicioWs)
                    .HasMaxLength(50)
                    .HasColumnName("CodigoServicioWS");

                entity.Property(e => e.DetalleServicioWs)
                    .HasColumnType("json")
                    .HasColumnName("DetalleServicioWS");

                entity.Property(e => e.Estado).HasMaxLength(100);

                entity.Property(e => e.Fecha).HasMaxLength(20);

                entity.Property(e => e.FechaDesde).HasColumnType("datetime");

                entity.Property(e => e.FechaHasta).HasColumnType("datetime");

                entity.Property(e => e.IdOrganizacionWs)
                    .HasMaxLength(20)
                    .HasColumnName("IdOrganizacionWS");

                entity.Property(e => e.IdparamArea).HasColumnName("idparamArea");

                entity.Property(e => e.IdparamTipoServicio).HasColumnName("idparamTipoServicio");

                entity.Property(e => e.Nit)
                    .HasMaxLength(10)
                    .HasColumnName("NIT");

                entity.Property(e => e.Oficina).HasMaxLength(50);

                entity.Property(e => e.OrganismoCertificador).HasMaxLength(200);

                entity.Property(e => e.OrganizacionContentWs)
                    .HasColumnType("json")
                    .HasColumnName("OrganizacionContentWS");

                entity.Property(e => e.UsuarioRegistro).HasMaxLength(50);
            });

            modelBuilder.Entity<Tmddocumentacionauditorium>(entity =>
            {
                entity.HasKey(e => e.IdTmdDocumentacionAuditoria)
                    .HasName("PRIMARY");

                entity.ToTable("tmddocumentacionauditoria");

                entity.HasIndex(e => e.IdElaAuditoria, "fk_documento_auditoria_idx");

                entity.Property(e => e.IdTmdDocumentacionAuditoria)
                    .ValueGeneratedNever()
                    .HasColumnName("idTmdDocumentacionAuditoria");

                entity.Property(e => e.CiteDocumento)
                    .HasMaxLength(200)
                    .HasColumnName("citeDocumento");

                entity.Property(e => e.CorrelativoDocumento).HasColumnName("correlativoDocumento");

                entity.Property(e => e.FechaDeRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaDeRegistro");

                entity.Property(e => e.IdElaAuditoria).HasColumnName("idElaAuditoria");

                entity.Property(e => e.IdparamDocumentos).HasColumnName("idparamDocumentos");

                entity.Property(e => e.TmdDocumentoAuditoria)
                    .HasMaxLength(3200)
                    .HasColumnName("tmdDocumentoAuditoria")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.IdElaAuditoriaNavigation)
                    .WithMany(p => p.Tmddocumentacionauditoria)
                    .HasForeignKey(d => d.IdElaAuditoria)
                    .HasConstraintName("fk_documento_auditoria");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
