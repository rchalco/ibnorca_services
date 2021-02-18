using Business.Main.IbnorcaContext;
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
            Programasdeauditorium objPrograma = new Programasdeauditorium
            {
                IdpArea = 1, // /*SISTEMA - PRODUCTO*/
                Nit = "123456",
                Gestion = 2021,
                IdpPais = 1,
                IdpDepartamento = 1,
                IdOrganizacionWs = "5",
                OrganizacionContentWs = "{\"NOmbre\":\"ruben\"}",
                CodigoServicioWs = "REG-PROG-XXXXXXX",
                DetalleServicio = "{\"NOmbre\":\"ruben\"}",
                IdpTipoServicio = 1,/*CERTIFICACION - RENOVACION*/
                IdCodigoDeServicioCodigoIafWs = "{\"NOmbre\":\"ruben\"}",
                NumeroAnos = 1,
                IdpEstadosProgAuditoria = 2, /*'Sin fecha de auditoría' -  Con  - audi realizada*/
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null
            };

            Ciclosprogauditorium ciclosprogauditorium = new Ciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                Ano = 2000,
                IdpTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };
            objPrograma.Ciclosprogauditoria.Add(ciclosprogauditorium);

           ciclosprogauditorium = new Ciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                Ano = 2001,
                IdpTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };
            objPrograma.Ciclosprogauditoria.Add(ciclosprogauditorium);
            ciclosprogauditorium = new Ciclosprogauditorium
            {
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null,
                Ano = 2002,
                IdpTipoAuditoria = 1,
                NombreOrganizacionCertificado = "nombre que va en el certificado"

            };
            

            Ciclocronograma objCicloCrono = new Ciclocronograma
            {
                IdCicloProgAuditoria = 1,
                CantidadDeDiasTotal = 5,
                MesProgramado = 1,
                MesReprogramado = 3,
                FechaInicioDeEjecucionDeAuditoria = DateTime.Now,
                FechaDeFinDeEjecucionAuditoria = DateTime.Now,
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null

            }; //doble tipo de auditoria, se repite en la cabecera por sis ac los dos
            ciclosprogauditorium.Ciclocronogramas.Add(objCicloCrono);
            //Participante
            Cicloparticipante objParticipante = new Cicloparticipante
            {
                IdCicloProgAuditoria = 1,
                IdParticipanteWs = "1WS",
                ParticipanteContextWs = "{\"NOmbre\":\"ruben\"}",
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                IdpEstadoParticipante = 1 ///baja  - vigente
            };
            ciclosprogauditorium.Cicloparticipantes.Add(objParticipante);

            //Productos
            Direccionespaproducto objDirProd = new Direccionespaproducto
            {
                IdCicloProgAuditoria = 1,
                Nombre = "cemento Portland",
                Direccion = "Planta industrial Viacha",
                Marca = "NB",
                Sello = 1,
                IdPais = 1,
                IdDepartamento = 1,
                Ciudad = "el alto",
                FechaEmisionPrimerCertificado = DateTime.Now,
                FechaVencimientoUltimoCertificado = DateTime.Now,
                FechaVencimientoCertificado = DateTime.Now,
                UsuarioRegistro = "ivan.vilela",
                FechaDesde = DateTime.Now,
                FechaHasta = null
            };
            ciclosprogauditorium.Direccionespaproductos.Add(objDirProd);

            objPrograma.Ciclosprogauditoria.Add(ciclosprogauditorium);
            objComplex.reqPrograma = objPrograma;

            var resul = objProgramaAudi.RegisterProgramaAuditoria(objComplex);
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
            var resul = aperturaAuditoriaManager.ObtenerProgramaAuditoria(11);
        }

    }
}
