using Business.Main.DataMapping;
using Business.Main.Modules.ApeeturaAuditoria;
using Business.Main.Modules.ApeeturaAuditoria.Domain;
using Domain.Main.AperturaAuditoria;
using Domain.Main.Wraper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitBusinessMain
{
    public class TestProgramaAuditoria
    {
        [Test]
        public void RegisterTest01()
        {

            AperturaAuditoriaManager objProgramaAudi = new AperturaAuditoriaManager();
            ComplexProgramaAuditoria objComplex = new ComplexProgramaAuditoria();
            Praprogramasdeauditorium objPrograma = new Praprogramasdeauditorium
            {
                IdparamArea = 1, // /*SISTEMA - PRODUCTO*/
                Nit = "123456",
                Fecha = "01/01/2001",
                IdOrganizacionWs = "5",
                OrganizacionContentWs = "{\"NOmbre\":\"ruben\"}",
                CodigoServicioWs = "REG-PROG-XXXXXXX",
                //DetalleServicio = "{\"NOmbre\":\"ruben\"}",
                IdparamTipoServicio = 1,/*CERTIFICACION - RENOVACION*/
                //CodigoDeServicioCodigoIafWs = "{\"NOmbre\":\"ruben\"}",
                NumeroAnios = 1,                
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null
            };

            Praciclosprogauditorium ciclosprogauditorium = new Praciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                //Ano = 2000,
                IdparamTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };
            objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);

            ciclosprogauditorium = new Praciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                //Ano = 2001,
                IdparamTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };
            objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);
            ciclosprogauditorium = new Praciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                //Ano = 2002,
                IdparamTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };


            Praciclocronograma objCicloCrono = new Praciclocronograma
            {
                IdPrAcicloProgAuditoria = 1,
                DiasInsitu = 5,
                //MesProgramado = 1,
                //MesReprogramado = 3,
                FechaInicioDeEjecucionDeAuditoria = DateTime.Now,
                FechaDeFinDeEjecucionAuditoria = DateTime.Now,
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null

            }; //doble tipo de auditoria, se repite en la cabecera por sis ac los dos

            ciclosprogauditorium.Praciclocronogramas.Add(objCicloCrono);
            //Participante
            Pracicloparticipante objParticipante = new Pracicloparticipante
            {
                IdPrAcicloProgAuditoria = 1,
                //IdParticipanteWs = "1WS",
                //ParticipanteContextWs = "{\"NOmbre\":\"ruben\"}",
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                //IdparamEstadoParticipante = 1 ///baja  - vigente
            };
            ciclosprogauditorium.Pracicloparticipantes.Add(objParticipante);

            //Productos
            Pradireccionespaproducto objDirProd = new Pradireccionespaproducto
            {
                IdPrAcicloProgAuditoria = 1,
                Nombre = "cemento Portland",
                Direccion = "Planta industrial Viacha",
                Marca = "NB",
                Sello = "1",
                //IdparamPais = 1,
                //IdparamDepartamento = 1,
                Ciudad = "el alto",
                FechaEmisionPrimerCertificado = DateTime.Now,
                FechaVencimientoUltimoCertificado = DateTime.Now,
                FechaVencimientoCertificado = DateTime.Now,
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null
            };
            ciclosprogauditorium.Pradireccionespaproductos.Add(objDirProd);

            objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);
            //objComplex.reqPrograma = objPrograma;

            var resul = objProgramaAudi.RegisterProgramaAuditoria(objPrograma);
            Assert.AreEqual(resul.State, ResponseType.Success);
                        
        }
        [Test]
        public void GetParametricas()
        {
            ComplexParametricas objComplex = new ComplexParametricas();
            AperturaAuditoriaManager objProgramaAudi = new AperturaAuditoriaManager();
            var resul = objProgramaAudi.GetParametricas(objComplex);
            Assert.AreEqual(resul.State, ResponseType.Success);

        }
        [Test]
        public void ObtenerProgramaAuditoriaTest() {
            AperturaAuditoriaManager aperturaAuditoriaManager = new AperturaAuditoriaManager();
            //var resul = aperturaAuditoriaManager.ObtenerProgramaAuditoria(14455, "rchalco");
            var resul = aperturaAuditoriaManager.ObtenerProgramaAuditoria(7665, "rchalco");
        }

        [Test]
        public void ObtenerProgramaAuditoriaTest1()
        {
            AperturaAuditoriaManager aperturaAuditoriaManager = new AperturaAuditoriaManager();
            var resul = aperturaAuditoriaManager.ObtenerProgramaAuditoria(14455, "rchalco");
        }



    }
}
