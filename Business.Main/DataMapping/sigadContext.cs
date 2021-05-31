using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PlumbingProps.Config;

#nullable disable

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
                optionsBuilder.UseMySql(ConfigManager.GetConfiguration().GetSection("conexionString").Value, Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elaadp>(entity =>
            {
                entity.HasKey(e => e.Idelaadp)
                    .HasName("PRIMARY");

                entity.ToTable("elaadp");

                entity.HasComment("registro de areas de preocupacion	");

                entity.HasIndex(e => e.IdelaAuditoria, "fk_apd_auditoria_idx");

                entity.Property(e => e.Idelaadp).HasColumnName("idelaadp");

                entity.Property(e => e.Area)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("area")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(1000)")
                    .HasColumnName("descripcion")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Fecha)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("fecha")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Usuario)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("usuario")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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

                entity.Property(e => e.UsuarioRegistro)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
                    .HasColumnType("varchar(45)")
                    .HasColumnName("area")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Categoria)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("categoria")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Contenido)
                    .HasColumnType("varchar(2000)")
                    .HasColumnName("contenido")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Endocumento).HasColumnName("endocumento");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Label)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("label")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Nemotico)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("nemotico")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Seleccionado).HasColumnName("seleccionado");

                entity.Property(e => e.Titulo)
                    .HasColumnType("varchar(300)")
                    .HasColumnName("titulo")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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

                entity.Property(e => e.Auditor)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Cargo)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Horario)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.IdDireccionPasistema).HasColumnName("idDireccionPASistema");

                entity.Property(e => e.PersonaEntrevistadaCargo)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ProcesoArea)
                    .HasColumnType("varchar(100)")
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
                    .HasColumnType("varchar(50)")
                    .HasColumnName("fecha")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Hallazgo)
                    .HasColumnType("varchar(1000)")
                    .HasColumnName("hallazgo")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdelaAuditoria).HasColumnName("idelaAuditoria");

                entity.Property(e => e.Normas)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("normas")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Proceso)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("proceso")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Sitio)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("sitio")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("tipo")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TipoNemotico)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("tipo_nemotico")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Usuario)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("usuario")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
                    .HasColumnType("varchar(45)")
                    .HasColumnName("area")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Categoria)
                    .HasColumnType("varchar(50)")
                    .HasColumnName("categoria")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Decripcion)
                    .HasColumnType("varchar(2000)")
                    .HasColumnName("decripcion")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Endocumento).HasColumnName("endocumento");

                entity.Property(e => e.Label)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("label")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Nemotico)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("nemotico")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.Titulo)
                    .HasColumnType("varchar(300)")
                    .HasColumnName("titulo")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Importacionsolicitud>(entity =>
            {
                entity.HasKey(e => e.IdimportacionSolicitud)
                    .HasName("PRIMARY");

                entity.ToTable("importacionsolicitud");

                entity.Property(e => e.IdimportacionSolicitud).HasColumnName("idimportacionSolicitud");

                entity.Property(e => e.Cliente)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("cliente")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Detalle)
                    .HasColumnType("json")
                    .HasColumnName("detalle");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_registro");

                entity.Property(e => e.Nit)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("nit")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
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

            modelBuilder.Entity<Paramdocumento>(entity =>
            {
                entity.HasKey(e => e.Idparamdocumentos)
                    .HasName("PRIMARY");

                entity.ToTable("paramdocumentos");

                entity.Property(e => e.Idparamdocumentos).HasColumnName("idparamdocumentos");

                entity.Property(e => e.Area)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Method)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NombrePlantilla)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Path)
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Proceso)
                    .HasColumnType("varchar(70)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
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

                entity.Property(e => e.DiasPresupuesto).HasPrecision(10, 2);

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

                entity.Property(e => e.IdDireccionPaproducto).HasColumnName("idDireccionPAProducto");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnType("varchar(1000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Estado)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EstadoConcer)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("EstadoCONCER")
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
                    .HasColumnType("varchar(1000)")
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

                entity.Property(e => e.ReivsionConcer)
                    .HasColumnType("varchar(600)")
                    .HasColumnName("ReivsionCONCER")
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

                entity.Property(e => e.IdDireccionPasistema).HasColumnName("idDireccionPASistema");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Departamento)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Dias).HasPrecision(10, 2);

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
                    .HasColumnType("varchar(200)")
                    .HasColumnName("citeDocumento")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CorrelativoDocumento).HasColumnName("correlativoDocumento");

                entity.Property(e => e.FechaDeRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaDeRegistro");

                entity.Property(e => e.IdElaAuditoria).HasColumnName("idElaAuditoria");

                entity.Property(e => e.IdparamDocumentos).HasColumnName("idparamDocumentos");

                entity.Property(e => e.TmdDocumentoAuditoria)
                    .HasColumnType("varchar(3200)")
                    .HasColumnName("tmdDocumentoAuditoria")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Usuario)
                    .HasColumnType("varchar(100)")
                    .HasColumnName("usuario")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
