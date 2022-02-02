-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: sigad
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `elaadp`
--

DROP TABLE IF EXISTS `elaadp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elaadp` (
  `idelaadp` int NOT NULL AUTO_INCREMENT,
  `idelaAuditoria` int DEFAULT NULL,
  `area` varchar(100) DEFAULT NULL,
  `descripcion` varchar(1000) DEFAULT NULL,
  `fecha` varchar(45) DEFAULT NULL,
  `usuario` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`idelaadp`),
  KEY `fk_apd_auditoria_idx` (`idelaAuditoria`),
  CONSTRAINT `fk_apd_auditoria` FOREIGN KEY (`idelaAuditoria`) REFERENCES `elaauditoria` (`idelaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='registro de areas de preocupacion	';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elaadp`
--

LOCK TABLES `elaadp` WRITE;
/*!40000 ALTER TABLE `elaadp` DISABLE KEYS */;
INSERT INTO `elaadp` VALUES (13,10,NULL,'ADP 1 No s etienen implementado evalaución de proveedores','17/05/2021','ruben.chalco'),(14,10,NULL,'ADP 2 No se tienne implementado requisitos legales','17/05/2021','ruben.chalco');
/*!40000 ALTER TABLE `elaadp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `elaauditoria`
--

DROP TABLE IF EXISTS `elaauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elaauditoria` (
  `idelaAuditoria` int NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  `UsuarioRegistro` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`idelaAuditoria`),
  KEY `fk_auditoria_ciclo_idx` (`idPrACicloProgAuditoria`),
  CONSTRAINT `fk_elaauditoria_pracicloauditoria` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elaauditoria`
--

LOCK TABLES `elaauditoria` WRITE;
/*!40000 ALTER TABLE `elaauditoria` DISABLE KEYS */;
INSERT INTO `elaauditoria` VALUES (8,223,'2021-05-13 13:36:18','ruben.chalco'),(9,224,'2021-05-13 13:48:20','ruben.chalco'),(10,232,'2021-05-13 15:28:04','ruben.chalco');
/*!40000 ALTER TABLE `elaauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `elacontenidoauditoria`
--

DROP TABLE IF EXISTS `elacontenidoauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elacontenidoauditoria` (
  `idela_contenidoauditoria` int NOT NULL AUTO_INCREMENT,
  `idelaAuditoria` int DEFAULT NULL,
  `label` varchar(500) DEFAULT NULL,
  `nemotico` varchar(100) DEFAULT NULL,
  `titulo` varchar(300) DEFAULT NULL,
  `contenido` varchar(2000) DEFAULT NULL,
  `categoria` varchar(50) DEFAULT NULL,
  `area` varchar(45) DEFAULT NULL,
  `seleccionado` int DEFAULT NULL,
  `endocumento` int DEFAULT NULL,
  PRIMARY KEY (`idela_contenidoauditoria`),
  KEY `fk_contenido_auditoria_idx` (`idelaAuditoria`),
  CONSTRAINT `fk_contenido_auditoria` FOREIGN KEY (`idelaAuditoria`) REFERENCES `elaauditoria` (`idelaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=188 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elacontenidoauditoria`
--

LOCK TABLES `elacontenidoauditoria` WRITE;
/*!40000 ALTER TABLE `elacontenidoauditoria` DISABLE KEYS */;
INSERT INTO `elacontenidoauditoria` VALUES (111,8,'','INFPROD_CONCLUSION','CONCLUSIONES','Las no conformidades mayores (si aplica) y menores (si aplica) detectadas en la anterior auditoría han sido superadas.','INFORME_PRODUCTO','TCP',0,1),(112,8,'','INFPROD_CONCLUSION','CONCLUSIONES','La empresa cuenta con un sistema de gestión conforme a los requisitos del Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. Si bien se han presentado No conformidades durante la auditoría, la empresa ha elaborado un plan de acciones correctivas que ha sido revisado y aprobado por el Auditor Líder','INFORME_PRODUCTO','TCP',0,1),(113,8,'','INFPROD_CONCLUSION','CONCLUSIONES','El(los) producto(s) CUMPLE con los requisitos establecidos en la(s) norma(s) XXXX','INFORME_PRODUCTO','TCP',0,1),(114,8,'','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','En la evaluación al sistema de gestión conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA se han identificado desvíos descritos a continuación','INFORME_PRODUCTO','TCP',0,1),(115,8,'','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','El sistema de gestión es conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. (Redacción aplicable cuando no se presentan No conformidades)','INFORME_PRODUCTO','TCP',0,1),(116,8,'No, se recibió queja(s):','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','','INFORME_PRELIMINAR','TCP',0,1),(117,8,'Si, se recibió queja(s)','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','','INFORME_PRELIMINAR','TCP',0,1),(118,8,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización cumple con el reglamento y la especificación de uso del logo de certificación. ','INFORME_PRELIMINAR','TCP',0,1),(119,8,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización usa el logo de una manera que parece clara y sincera.','INFORME_PRELIMINAR','TCP',0,1),(120,8,'','INFPROD_CONCLUSION','CONCLUSIONES','De ser necesario adicionar conclusiones para mejorar la explicación del informe','INFORME_PRODUCTO','TCP',0,1),(121,8,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización ejerce su derecho a usar el logo de certificación proporcionado por el Organismo de certificación','INFORME_PRELIMINAR','TCP',0,1),(122,8,'N/A','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',0,1),(123,8,'No','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',0,1),(124,8,'Si','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',0,1),(125,8,'Con modificaciones','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','','INFORME_PRELIMINAR','TCP',0,1),(126,8,'Con modificaciones','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','','INFORME_PRELIMINAR','TCP',0,1),(127,8,'Sin modificación','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','','INFORME_PRELIMINAR','TCP',0,1),(128,8,'Criterios de auditoría','PLAN_CRITERIO','Criterios de auditoría','','PLAN_AUDITORIA','TCP',0,1),(129,8,'Sin modificación','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','','INFORME_PRELIMINAR','TCP',0,1),(130,8,'Externo','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','','ACTA_MUESTREO','TCP',0,1),(131,8,'','ACTAMUESTREO_PLAN','PLAN DE MUESTREO','','ACTA_MUESTREO','TCP',0,1),(132,8,'Interno','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','','ACTA_MUESTREO','TCP',0,1),(133,9,'','INFPROD_CONCLUSION','CONCLUSIONES','Las no conformidades mayores (si aplica) y menores (si aplica) detectadas en la anterior auditoría han sido superadas.','INFORME_PRODUCTO','TCP',1,1),(134,9,'','INFPROD_CONCLUSION','CONCLUSIONES','La empresa cuenta con un sistema de gestión conforme a los requisitos del Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. Si bien se han presentado No conformidades durante la auditoría, la empresa ha elaborado un plan de acciones correctivas que ha sido revisado y aprobado por el Auditor Líder','INFORME_PRODUCTO','TCP',1,1),(135,9,'','INFPROD_CONCLUSION','CONCLUSIONES','El(los) producto(s) CUMPLE con los requisitos establecidos en la(s) norma(s) XXXX','INFORME_PRODUCTO','TCP',1,1),(136,9,'','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','En la evaluación al sistema de gestión conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA se han identificado desvíos descritos a continuación','INFORME_PRODUCTO','TCP',1,1),(137,9,'','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','El sistema de gestión es conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. (Redacción aplicable cuando no se presentan No conformidades)','INFORME_PRODUCTO','TCP',0,1),(138,9,'No, se recibió queja(s):','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','','INFORME_PRELIMINAR','TCP',1,1),(139,9,'Si, se recibió queja(s)','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','','INFORME_PRELIMINAR','TCP',0,1),(140,9,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización cumple con el reglamento y la especificación de uso del logo de certificación. ','INFORME_PRELIMINAR','TCP',1,1),(141,9,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización usa el logo de una manera que parece clara y sincera.','INFORME_PRELIMINAR','TCP',1,1),(142,9,'','INFPROD_CONCLUSION','CONCLUSIONES','De ser necesario adicionar conclusiones para mejorar la explicación del informe','INFORME_PRODUCTO','TCP',0,1),(143,9,'','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','La organización ejerce su derecho a usar el logo de certificación proporcionado por el Organismo de certificación','INFORME_PRELIMINAR','TCP',1,1),(144,9,'N/A','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',0,1),(145,9,'No','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',0,1),(146,9,'Si','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','','INFORME_PRELIMINAR','TCP',1,1),(147,9,'Con modificaciones','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','','INFORME_PRELIMINAR','TCP',1,1),(148,9,'Con modificaciones','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','','INFORME_PRELIMINAR','TCP',0,1),(149,9,'Sin modificación','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','','INFORME_PRELIMINAR','TCP',1,1),(150,9,'Criterios de auditoría','PLAN_CRITERIO','Criterios de auditoría','Norma NB 011:2012 Cemento, definiciones y otros criterios\nReglamento particular RP_TCP_01\nDocumentos legales aplicables\n\n','PLAN_AUDITORIA','TCP',1,1),(151,9,'Sin modificación','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','','INFORME_PRELIMINAR','TCP',0,1),(152,9,'Externo','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','','ACTA_MUESTREO','TCP',0,1),(153,9,'','ACTAMUESTREO_PLAN','PLAN DE MUESTREO','Gaviones y rollos 10x12\n154; S-3 NAC: 4 AC:2 RE: 1\n\nGaviones y rollos 8x10\n154; S-3 NAC: 4 AC:2 RE: 1\n\nGaviones y rollos 6X8ccc\n154; S-3 NAC: 4 AC:2 RE: 1\n\nggg','ACTA_MUESTREO','TCP',1,1),(154,9,'Interno','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','','ACTA_MUESTREO','TCP',1,1),(155,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Actualización de Norma','DATOCERT','TCS',0,1),(156,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Cambio de nombre de la organización','DATOCERT','TCS',0,1),(157,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Cambio de alcance','DATOCERT','TCS',0,1),(158,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Renovación','DATOCERT','TCS',0,1),(159,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Certificación','DATOCERT','TCS',0,1),(160,10,'','IE1_CONCLUSIONES','CONCLUSIONES','Es conveniente llevar a cabo una nueva auditoría de etapa I.','IE1','TCS',0,1),(161,10,'','IE1_CONCLUSIONES','CONCLUSIONES','Es necesario recalcular el tiempo de auditoría de la etapa II.','IE1','TCS',0,1),(162,10,'','IE1_CONCLUSIONES','CONCLUSIONES','No en la situación actual. Los problemas graves observados no pueden corregirse razonablemente antes de la fecha prevista de la auditoría de la etapa II. (se requiere la validación del cliente en este siguiente párrafo)','IE1','TCS',0,1),(163,10,'','IE1_CONCLUSIONES','CONCLUSIONES','Sí, sujeto a la resolución de las áreas de preocupación mencionadas antes de la fecha de inicio de la auditoría programada para el AAAA-MM-DD (se requiere la validación del cliente en este párrafo).','IE1','TCS',0,1),(164,10,'','IE1_CONCLUSIONES','CONCLUSIONES','Sí, en la fecha prevista AAAA-MM-DD','IE1','TCS',0,1),(165,10,'','IE1_RESULTADOS','RESULTADOS','El auditor líder considera que el equipo auditor tiene las habilidades necesarias para llevar a cabo la etapa II.','IE1','TCS',0,1),(166,10,'','IE1_RESULTADOS','RESULTADOS','Se han planificado y llevado a cabo auditorías internas y al menos una revisión de gestión que cubre el sistema de gestión, los procesos y varios sitios afectados por la certificación. El nivel de implementación del sistema de gestión demuestra que el cliente está listo para la auditoría de la etapa II.','IE1','TCS',0,1),(167,10,'','IE1_RESULTADOS','RESULTADOS','La asignación de los auditores, la asignación de tiempo, las opciones de muestreo y los aspectos logísticos han sido discutidos y acordados con el cliente. ','IE1','TCS',0,1),(168,10,'','IE1_RESULTADOS','RESULTADOS','Los aspectos reglamentarios y legales que el cliente debe cumplir, incluido su nivel de conformidad, se han recopilado, presentado y puesto a disposición del auditor líder. Esta información confirma la viabilidad de la auditoría de la etapa II.','IE1','TCS',0,1),(169,10,'','IE1_RESULTADOS','RESULTADOS','Se ha proporcionado la información requerida sobre el alcance del sistema de gestión, los procesos y equipos utilizados y los niveles de control establecidos (especialmente para clientes de multisitios). El alcance es consistente con los imperativos y los requisitos de las partes interesadas.','IE1','TCS',0,1),(170,10,'','IE1_RESULTADOS','RESULTADOS','Las actividades, los indicadores clave de rendimiento, los procesos mapeados (incluidos los procesos subcontratados), las líneas de productos, el contexto / los problemas, los principales riesgos y oportunidades identificados, los servicios y/o productos comercializados se han presentado al Auditor Líder. Estos elementos demuestran una comprensión suficiente de los requisitos de las normas aplicables.','IE1','TCS',0,1),(171,10,'','IE1_RESULTADOS','RESULTADOS','Los intercambios de información con el cliente demuestran un nivel adecuado de preparación para la auditoría de la etapa II.','IE1','TCS',0,1),(172,10,'','PLAN_OBJETIVOS','OBJETIVOS','Determinar la conformidad del Sistema de Gestión del cliente, con los criterios de auditoría, evaluando su implementación y eficacia. ','PLAN','TCS',0,1),(173,10,'','PLAN_OBJETIVOS2','OBJETIVOS','Determinar la conformidad del/los Sistema(s) de Gestión del cliente, con los criterios de auditoría, evaluando su implementación y eficacia. (aplicable a una auditoría de Etapa II/Certificación, borrar las demás)','PLAN','TCS',0,1),(174,10,'','PLAN_OBJETIVOS3','OBJETIVOS','Determinar y verificar que la organización continúa siendo conforme con los requisitos del/los sistema(s) de gestión y criterios de auditoría. (aplicable a una auditoría de Seguimientos, borrar las demás)','PLAN','TCS',0,1),(175,10,'','PLAN_OBJETIVOS4','OBJETIVOS','Evaluar el cumplimento continuo y la eficacia del/los sistema(s) de gestión. (aplicable a una auditoría de Renovación, borrar las demás)','PLAN','TCS',0,1),(176,10,'','IE1_CRITERIOS','CRITERIOS','Enfoque integrado para definir, documentar e implementar la política y los objetivos de la gestión.','IE1','TCS',0,1),(177,10,'','IE1_CRITERIOS','CRITERIOS','Enfoque integrado para definir las responsabilidades (incluyendo los relacionados con representantes de la dirección, si corresponde).','IE1','TCS',0,1),(178,10,'','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','Otro','DATOCERT','TCS',0,1),(179,10,'','IE1_CRITERIOS','CRITERIOS','Sistema integrado de información documentada (manual, documentos y registros).','IE1','TCS',0,1),(180,10,'','IE1_CRITERIOS','CRITERIOS','Revisión por la dirección integrada.','IE1','TCS',0,1),(181,10,'','IE1_CRITERIOS','CRITERIOS','Gestión integrada de auditorías internas (planificación, métodos, herramientas, etc.).','IE1','TCS',0,1),(182,10,'','IE1_CRITERIOS','CRITERIOS','Enfoque integrado en la gestión de procesos (documentación y dirección).','IE1','TCS',0,1),(183,10,'','IE1_COMENTARIOS','COMENTARIOS','El nivel de integración encontrado en la etapa I es idéntico o superior al identificado durante el proceso comercial, lo que confirma la duración planificada de la auditoría.','IE1','TCS',0,1),(184,10,'','IE1_COMENTARIOS','COMENTARIOS','El nivel de integración encontrado en la etapa I es inferior al nivel de integración identificado durante el proceso comercial. Por lo tanto, el Organismo de Certificación debe verificar que la duración de la auditoría de etapa II sea suficiente.','IE1','TCS',0,1),(185,10,'','IE1_RESULTADOS','RESULTADOS','El Auditor Líder ha recibido la información documentada necesaria para la auditoría. La documentación del sistema de gestión ha sido examinada. El sistema de información documentada cumple con los requisitos de la(s) norma(s).','IE1','TCS',0,1),(186,10,'','IE1_CRITERIOS','CRITERIOS','Gestión integrada de mejoras.','IE1','TCS',0,1),(187,10,'','PLAN_OBJETIVOS5','OBJETIVOS','Evaluar la capacidad del sistema de gestión para asegurar que la organización cliente','PLAN','TCS',0,1);
/*!40000 ALTER TABLE `elacontenidoauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `elacronogama`
--

DROP TABLE IF EXISTS `elacronogama`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elacronogama` (
  `idElACronograma` bigint NOT NULL AUTO_INCREMENT,
  `Idelaauditoria` int DEFAULT NULL,
  `idDireccionPAProducto` bigint DEFAULT NULL,
  `idDireccionPASistema` bigint DEFAULT NULL,
  `FechaInicio` varchar(50) DEFAULT NULL,
  `FechaFin` varchar(50) DEFAULT NULL,
  `Horario` varchar(100) DEFAULT NULL,
  `RequisitosEsquema` varchar(500) DEFAULT NULL,
  `PersonaEntrevistadaCargo` varchar(200) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  `ProcesoArea` varchar(100) DEFAULT NULL,
  `Auditor` varchar(100) DEFAULT NULL,
  `Cargo` varchar(100) DEFAULT NULL,
  `Direccion` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`idElACronograma`),
  KEY `fk_cronograma_auditoria_idx` (`Idelaauditoria`),
  CONSTRAINT `fk_cronograma_auditoria` FOREIGN KEY (`Idelaauditoria`) REFERENCES `elaauditoria` (`idelaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elacronogama`
--

LOCK TABLES `elacronogama` WRITE;
/*!40000 ALTER TABLE `elacronogama` DISABLE KEYS */;
INSERT INTO `elacronogama` VALUES (24,9,103,NULL,'2021-05-13',NULL,'12:30','--','----',NULL,NULL,'Pausa almuerzo','',NULL,'\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú '),(28,10,NULL,386,'2021-05-24',NULL,'11:00 - 11:30','6','Encargado de mantenimiento',NULL,NULL,'Mantenimiento','Benjamin Joel Torrico Herbas ;',NULL,NULL),(29,9,NULL,NULL,'2021-04-23',NULL,'8AM','sin req','SSSSSSS',NULL,NULL,'SSSSSSSS','',NULL,'\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú '),(33,9,NULL,NULL,'2021-05-25',NULL,'14:30','6.8','bsbsbsb',NULL,NULL,'comercial','Cintya Marcela Zarate Cazas;',NULL,'Av. Néstor Gambetta 6429, Callao, Lima - Perú.'),(34,9,NULL,NULL,'2021-05-25',NULL,'14:30','6.8','bsbsbsb',NULL,NULL,'comercial','Cintya Marcela Zarate Cazas;',NULL,'Av. Néstor Gambetta 6429, Callao, Lima - Perú.'),(36,10,NULL,NULL,'2021-05-24',NULL,'8:30','8','Encargada de producción',NULL,NULL,'Producción','Benjamin Joel Torrico Herbas ;',NULL,'OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA');
/*!40000 ALTER TABLE `elacronogama` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `elahallazgo`
--

DROP TABLE IF EXISTS `elahallazgo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elahallazgo` (
  `idelahallazgo` int NOT NULL AUTO_INCREMENT,
  `idelaAuditoria` int DEFAULT NULL,
  `tipo` varchar(45) DEFAULT NULL,
  `tipo_nemotico` varchar(45) DEFAULT NULL,
  `normas` varchar(500) DEFAULT NULL,
  `proceso` varchar(200) DEFAULT NULL,
  `hallazgo` varchar(1000) DEFAULT NULL,
  `sitio` varchar(200) DEFAULT NULL,
  `fecha` varchar(50) DEFAULT NULL,
  `usuario` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`idelahallazgo`),
  KEY `fk_hallazgo_auditoria_idx` (`idelaAuditoria`),
  CONSTRAINT `fk_hallazgo_auditoria` FOREIGN KEY (`idelaAuditoria`) REFERENCES `elaauditoria` (`idelaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elahallazgo`
--

LOCK TABLES `elahallazgo` WRITE;
/*!40000 ALTER TABLE `elahallazgo` DISABLE KEYS */;
INSERT INTO `elahallazgo` VALUES (7,9,'No-Conformidad Mayor','NCM',NULL,'2.2.2 Control de la información documentada','EO.1. El el procedimiento xxxx no se hace refrencia al formualrio xxx, de acuerdo a lo establecido en el procedimiento.','La organización no mantiene el control de algunos documentos ','13/05/2021','ruben.chalco'),(8,9,'No Conformidad Menor','NCm',NULL,'4.3 No conformidad y acción correctiva','EO. La no conformidad relacionada a control de la infomración','La organziación no reviso la eficacia de las acciones','13/05/2021','ruben.chalco'),(9,9,'Fortaleza','F',NULL,NULL,'Se destaca el compromiso del personal',NULL,'13/05/2021','ruben.chalco'),(10,9,'Oportunidad de mejora','OM',NULL,NULL,'se identifica como una mejora contar con un documento donde se establezca la metodologia de verificación de los equipos de laboratorio',NULL,'13/05/2021','ruben.chalco'),(11,10,'No Conformidad Menor','NCm',NULL,'8.3','No se cumple este requisito\n\nEO1: VVVVVV\nEO2. XXXX','aaaaaaaa','17/05/2021','ruben.chalco'),(12,9,'No-Conformidad Mayor','NCM',NULL,'NCM 1 2.2.2 Control de documentos','En algun caso no se cumple con con el control de la documentaci\'on\\\n\nEO1: No se tiene el documento de plan de auditoria \nEO2: Se usa un programa diferente al aprobado',NULL,'14/05/2021','ruben.chalco'),(13,10,'Oportunidad de mejora','OM',NULL,'','sSe considera un a portunidad de mejora sssss',NULL,'17/05/2021','ruben.chalco'),(14,10,'No Conformidad Menor','NCm',NULL,'4.5','En algun caso no se cuenta\nEO1: NO se tiene xxx\nEO2: YYYY',NULL,'17/05/2021','ruben.chalco'),(15,10,'Fortaleza','F',NULL,NULL,'Se resalta el compromiso de la dirección',NULL,'17/05/2021','ruben.chalco');
/*!40000 ALTER TABLE `elahallazgo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `elalistaspredefinidas`
--

DROP TABLE IF EXISTS `elalistaspredefinidas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `elalistaspredefinidas` (
  `idelalistaspredefinidas` int NOT NULL AUTO_INCREMENT,
  `decripcion` varchar(2000) DEFAULT NULL,
  `nemotico` varchar(100) DEFAULT NULL,
  `titulo` varchar(300) DEFAULT NULL,
  `categoria` varchar(50) DEFAULT NULL,
  `label` varchar(500) DEFAULT NULL,
  `area` varchar(45) DEFAULT NULL,
  `orden` int DEFAULT NULL,
  `endocumento` int DEFAULT NULL,
  PRIMARY KEY (`idelalistaspredefinidas`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `elalistaspredefinidas`
--

LOCK TABLES `elalistaspredefinidas` WRITE;
/*!40000 ALTER TABLE `elalistaspredefinidas` DISABLE KEYS */;
INSERT INTO `elalistaspredefinidas` VALUES (1,'- Evaluar la capacidad del sistema de gestión para asegurar que la organización cliente cumple con los requisitos legales, reglamentarios y contractuales aplicables. \n\n- Revisar la información documentada, el grado de comprensión de la organización respecto al/los requisitos del Sistema(s) de Gestión, recopilar información necesaria correspondiente al/los alcance(s) información del desempeño clave y de aspectos, procesos, objetivos y funcionamiento significativos del sistema de gestión, determinando el estado de preparación para la Etapa II. (aplicable a una auditoría de Etapa I, borrar las demás)\n\n- Determinar la conformidad del/los Sistema(s) de Gestión del cliente, con los criterios de auditoría, evaluando su implementación y eficacia. (aplicable a una auditoría de Etapa II/Certificación, borrar las demás)\n\n- Determinar y verificar que la organización continúa siendo conforme con los requisitos del/los sistema(s) de gestión y criterios de auditoría. (aplicable a una auditoría de Seguimientos, borrar las demás)\n\n- Evaluar el cumplimento continuo y la eficacia del/los sistema(s) de gestión. (aplicable a una auditoría de Renovación, borrar las demás)','PLAN_OBJETIVOS','OBJETIVOS','PLAN','','TCS',1,1),(2,'Revisar la información documentada, el grado de comprensión de la organización respecto al/los requisitos del Sistema(s) de Gestión, recopilar información necesaria correspondiente al/los alcance(s) información del desempeño clave y de aspectos, procesos, objetivos y funcionamiento significativos del sistema de gestión, determinando el estado de preparación para la Etapa II. (aplicable a una auditoría de Etapa I, borrar las demás)','PLAN_OBJETIVOS2','OBJETIVOS','PLAN','','TCS',2,1),(3,'Determinar la conformidad del/los Sistema(s) de Gestión del cliente, con los criterios de auditoría, evaluando su implementación y eficacia. (aplicable a una auditoría de Etapa II/Certificación, borrar las demás)','PLAN_OBJETIVOS3','OBJETIVOS','PLAN','','TCS',3,1),(4,'Determinar y verificar que la organización continúa siendo conforme con los requisitos del/los sistema(s) de gestión y criterios de auditoría. (aplicable a una auditoría de Seguimientos, borrar las demás)','PLAN_OBJETIVOS4','OBJETIVOS','PLAN','','TCS',4,1),(5,'Evaluar el cumplimento continuo y la eficacia del/los sistema(s) de gestión. (aplicable a una auditoría de Renovación, borrar las demás)','PLAN_OBJETIVOS5','OBJETIVOS','PLAN','','TCS',5,1),(6,'Enfoque integrado para definir, documentar e implementar la política y los objetivos de la gestión.','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',1,1),(7,'Enfoque integrado para definir las responsabilidades (incluyendo los relacionados con representantes de la dirección, si corresponde).','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',2,1),(8,'Sistema integrado de información documentada (manual, documentos y registros).','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',3,1),(9,'Gestión integrada de mejoras.','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',4,1),(10,'Revisión por la dirección integrada.','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',5,1),(11,'Gestión integrada de auditorías internas (planificación, métodos, herramientas, etc.).','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',6,1),(12,'Enfoque integrado en la gestión de procesos (documentación y dirección).','IE1_CRITERIOS','CRITERIOS','IE1','','TCS',7,1),(13,'El nivel de integración encontrado en la etapa I es idéntico o superior al identificado durante el proceso comercial, lo que confirma la duración planificada de la auditoría.','IE1_COMENTARIOS','COMENTARIOS','IE1','','TCS',1,1),(14,'El nivel de integración encontrado en la etapa I es inferior al nivel de integración identificado durante el proceso comercial. Por lo tanto, el Organismo de Certificación debe verificar que la duración de la auditoría de etapa II sea suficiente.','IE1_COMENTARIOS','COMENTARIOS','IE1','','TCS',2,1),(15,'El Auditor Líder ha recibido la información documentada necesaria para la auditoría. La documentación del sistema de gestión ha sido examinada. El sistema de información documentada cumple con los requisitos de la(s) norma(s).','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',1,1),(16,'Los intercambios de información con el cliente demuestran un nivel adecuado de preparación para la auditoría de la etapa II.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',2,1),(17,'Las actividades, los indicadores clave de rendimiento, los procesos mapeados (incluidos los procesos subcontratados), las líneas de productos, el contexto / los problemas, los principales riesgos y oportunidades identificados, los servicios y/o productos comercializados se han presentado al Auditor Líder. Estos elementos demuestran una comprensión suficiente de los requisitos de las normas aplicables.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',3,1),(18,'Se ha proporcionado la información requerida sobre el alcance del sistema de gestión, los procesos y equipos utilizados y los niveles de control establecidos (especialmente para clientes de multisitios). El alcance es consistente con los imperativos y los requisitos de las partes interesadas.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',4,1),(19,'Los aspectos reglamentarios y legales que el cliente debe cumplir, incluido su nivel de conformidad, se han recopilado, presentado y puesto a disposición del auditor líder. Esta información confirma la viabilidad de la auditoría de la etapa II.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',5,1),(20,'La asignación de los auditores, la asignación de tiempo, las opciones de muestreo y los aspectos logísticos han sido discutidos y acordados con el cliente. ','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',6,1),(21,'Se han planificado y llevado a cabo auditorías internas y al menos una revisión de gestión que cubre el sistema de gestión, los procesos y varios sitios afectados por la certificación. El nivel de implementación del sistema de gestión demuestra que el cliente está listo para la auditoría de la etapa II.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',7,1),(22,'El auditor líder considera que el equipo auditor tiene las habilidades necesarias para llevar a cabo la etapa II.','IE1_RESULTADOS','RESULTADOS','IE1','','TCS',8,1),(23,'Sí, en la fecha prevista AAAA-MM-DD','IE1_CONCLUSIONES','CONCLUSIONES','IE1','','TCS',1,1),(24,'Sí, sujeto a la resolución de las áreas de preocupación mencionadas antes de la fecha de inicio de la auditoría programada para el AAAA-MM-DD (se requiere la validación del cliente en este párrafo).','IE1_CONCLUSIONES','CONCLUSIONES','IE1','','TCS',2,1),(25,'No en la situación actual. Los problemas graves observados no pueden corregirse razonablemente antes de la fecha prevista de la auditoría de la etapa II. (se requiere la validación del cliente en este siguiente párrafo)','IE1_CONCLUSIONES','CONCLUSIONES','IE1','','TCS',3,1),(26,'Es necesario recalcular el tiempo de auditoría de la etapa II.','IE1_CONCLUSIONES','CONCLUSIONES','IE1','','TCS',4,1),(27,'Es conveniente llevar a cabo una nueva auditoría de etapa I.','IE1_CONCLUSIONES','CONCLUSIONES','IE1','','TCS',5,1),(28,'Certificación','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',1,1),(29,'Renovación','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',2,1),(30,'Cambio de alcance','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',3,1),(31,'Cambio de nombre de la organización','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',4,1),(32,'Actualización de Norma','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',5,1),(33,'Otro','DATOCERT_MOTIVO','MOTIVO CERTIFICACION','DATOCERT','','TCS',6,1),(34,'','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','ACTA_MUESTREO','Interno','TCP',1,1),(35,'','ACT_LAB_ENSAYO','Laboratorio donde se ensayarán las muestras','ACTA_MUESTREO','Externo','TCP',2,1),(36,'','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','INFORME_PRELIMINAR','Sin modificación','TCP',1,1),(37,'','PLAN_CRITERIO','Criterios de auditoría','PLAN_AUDITORIA','Criterios de auditoría','TCP',1,1),(38,'','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','INFORME_PRELIMINAR','Sin modificación','TCP',1,1),(39,'','INFPRE_DESVIACION','Se presentó alguna desviación al plan de auditoría (Sea Remota e In situ)','INFORME_PRELIMINAR','Con modificaciones','TCP',1,1),(40,'','INFPRE_CONFIMACION','Confirmación de los productos, la razón social y la dirección de los sitios del alcance de la certificación.','INFORME_PRELIMINAR','Con modificaciones','TCP',2,1),(41,'','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','INFORME_PRELIMINAR','Si','TCP',1,1),(42,'','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','INFORME_PRELIMINAR','No','TCP',2,1),(43,'','INFPRE_REMOTO','La auditoría remota se realizó de manera eficiente (ver plan de auditoría) y logró cumplir con los objetivos de la auditoría','INFORME_PRELIMINAR','N/A','TCP',3,1),(44,'','ACTAMUESTREO_PLAN','PLAN DE MUESTREO','ACTA_MUESTREO','','TCP',1,1),(45,'La organización ejerce su derecho a usar el logo de certificación proporcionado por el Organismo de certificación','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','INFORME_PRELIMINAR','','TCP',1,1),(46,'La organización usa el logo de una manera que parece clara y sincera.','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','INFORME_PRELIMINAR','','TCP',2,1),(47,'La organización cumple con el reglamento y la especificación de uso del logo de certificación. ','INFPRE_USOLOGO','Comentarios sobre el uso del logo de IBNORCA','INFORME_PRELIMINAR','','TCP',3,1),(48,'','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','INFORME_PRELIMINAR','Si, se recibió queja(s)','TCP',1,1),(49,'','INFPRE_QUEJAS','Manejo de quejas de las partes interesadas recibidas por el Organismo de Certificación','INFORME_PRELIMINAR','No, se recibió queja(s):','TCP',2,1),(50,'El sistema de gestión es conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. (Redacción aplicable cuando no se presentan No conformidades)','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','INFORME_PRODUCTO','','TCP',1,1),(51,'En la evaluación al sistema de gestión conforme con los requisitos establecidos en el Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA se han identificado desvíos descritos a continuación','INFPROD_CERTPROD','SISTEMA DE GESTIÓN PARA LA CERTIFICACIÓN DE PRODUCTO','INFORME_PRODUCTO','','TCP',2,1),(52,'El(los) producto(s) CUMPLE con los requisitos establecidos en la(s) norma(s) XXXX','INFPROD_CONCLUSION','CONCLUSIONES','INFORME_PRODUCTO','','TCP',1,1),(53,'La empresa cuenta con un sistema de gestión conforme a los requisitos del Anexo 1 de la Especificación Esquema 5 para la certificación de productos con Sello IBNORCA. Si bien se han presentado No conformidades durante la auditoría, la empresa ha elaborado un plan de acciones correctivas que ha sido revisado y aprobado por el Auditor Líder','INFPROD_CONCLUSION','CONCLUSIONES','INFORME_PRODUCTO','','TCP',2,1),(54,'Las no conformidades mayores (si aplica) y menores (si aplica) detectadas en la anterior auditoría han sido superadas.','INFPROD_CONCLUSION','CONCLUSIONES','INFORME_PRODUCTO','','TCP',3,1),(55,'De ser necesario adicionar conclusiones para mejorar la explicación del informe','INFPROD_CONCLUSION','CONCLUSIONES','INFORME_PRODUCTO','','TCP',4,1);
/*!40000 ALTER TABLE `elalistaspredefinidas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `importacionsolicitud`
--

DROP TABLE IF EXISTS `importacionsolicitud`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `importacionsolicitud` (
  `idimportacionSolicitud` int NOT NULL AUTO_INCREMENT,
  `nit` varchar(100) DEFAULT NULL,
  `cliente` varchar(500) DEFAULT NULL,
  `detalle` json DEFAULT NULL,
  `fecha_registro` datetime DEFAULT NULL,
  PRIMARY KEY (`idimportacionSolicitud`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `importacionsolicitud`
--

LOCK TABLES `importacionsolicitud` WRITE;
/*!40000 ALTER TABLE `importacionsolicitud` DISABLE KEYS */;
INSERT INTO `importacionsolicitud` VALUES (1,'13215','ssssssssssssssssss','{\"fax\": \"asdf\", \"nit\": \"13215\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ssssssssssssssssss\", \"laboratorios\": [{\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"ddddddddddddddddddd\", \"listReglamentacion\": [{\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}, {\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}, {\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}, {\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}, {\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}, {\"marca\": \"\", \"norma\": \"\", \"nombre\": \"\", \"cantidad\": \"\", \"ubicacion\": \"\"}]}',NULL),(2,'13215','ssssssssssssssssss','{\"fax\": \"asdf\", \"nit\": \"13215\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ssssssssssssssssss\", \"laboratorios\": [{\"column1\": \"ddddddddddddddd\", \"column2\": \"\"}, {\"column1\": \"fffffffffffff\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"ddddddddddddddddddd\", \"listReglamentacion\": [{\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"d\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"dddd\", \"nombre\": \"dddd\", \"cantidad\": \"\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}]}','2021-05-05 00:12:51'),(3,'13215','ssssssssssssssssss','{\"fax\": \"asdf\", \"nit\": \"13215\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ssssssssssssssssss\", \"laboratorios\": [{\"column1\": \"ddddddddddddddd\", \"column2\": \"\"}, {\"column1\": \"fffffffffffff\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"ddddddddddddddddddd\", \"listReglamentacion\": [{\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"d\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"dddd\", \"nombre\": \"dddd\", \"cantidad\": \"\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}]}','2021-05-06 09:14:40'),(4,'13215','ssssssssssssssssss','{\"fax\": \"asdf\", \"nit\": \"13215\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ssssssssssssssssss\", \"laboratorios\": [{\"column1\": \"ddddddddddddddd\", \"column2\": \"\"}, {\"column1\": \"fffffffffffff\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"ddddddddddddddddddd\", \"listReglamentacion\": [{\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"d\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"dddd\", \"nombre\": \"dddd\", \"cantidad\": \"\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}]}','2021-06-07 16:22:53'),(5,'12345678','ferreteria herrera','{\"fax\": \"asdf\", \"nit\": \"12345678\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ferreteria herrera\", \"laboratorios\": [{\"column1\": \"ddddddddddddddd\", \"column2\": \"\"}, {\"column1\": \"fffffffffffff\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"av sanchez lima esq aspiazu\", \"listReglamentacion\": [{\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"d\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"dddd\", \"nombre\": \"dddd\", \"cantidad\": \"\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}]}','2021-06-07 16:37:51'),(6,'12345678','ferreteria herrera','{\"fax\": \"asdf\", \"nit\": \"12345678\", \"mail\": \"adsf\", \"cargo\": \"rrrr\", \"fecha\": \"rrrr\", \"ciudad\": \"la pz \", \"nombre\": \"rrrr\", \"holding\": \"es mi holding\", \"mae_mail\": \"mimail@gmail.com\", \"telefono\": \"778887878\", \"mae_cargo\": \"jefaso\", \"mae_nombre\": \"ruben chalco\", \"pagina_web\": \"asdf\", \"razonSocial\": \"ferreteria herrera\", \"laboratorios\": [{\"column1\": \"ddddddddddddddd\", \"column2\": \"\"}, {\"column1\": \"fffffffffffff\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}, {\"column1\": \"\", \"column2\": \"\"}], \"mae_telefono\": \"75354988\", \"contacto_mail\": \"nnn4\", \"contacto_cargo\": \"nnn2\", \"contacto_nombre\": \"nnn1\", \"departamentoPais\": \"adfa\", \"fecha_aproximada\": \"ddddddddddddddddddd\", \"listMateriaPrima\": [{\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}, {\"column1\": \"fffffff\", \"column2\": \"fffffffff\"}], \"contacto_telefono\": \"nnn3n\", \"direccionAlmacenes\": \"asdfadf\", \"direccionPrincipal\": \"av sanchez lima esq aspiazu\", \"listReglamentacion\": [{\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}, {\"column1\": \"ddddd\", \"column2\": \"ddddd\"}], \"promedioVentaAnual\": \"250000\", \"actividadesTerceros\": \"aaaaaaaaaaaaaaaaa\", \"certificacion_previa\": \"ddddd\", \"direccionFabricacion\": \"asdfasdfasdf\", \"listSolicitudProducto\": [{\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"d\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"ddd\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"dddd\", \"nombre\": \"dddd\", \"cantidad\": \"\", \"ubicacion\": \"ddd\"}, {\"marca\": \"dddd\", \"norma\": \"\", \"nombre\": \"dddd\", \"cantidad\": \"ddd\", \"ubicacion\": \"ddd\"}]}','2021-06-07 16:54:46');
/*!40000 ALTER TABLE `importacionsolicitud` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramarea`
--

DROP TABLE IF EXISTS `paramarea`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramarea` (
  `idparamArea` smallint NOT NULL AUTO_INCREMENT,
  `Area` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamArea`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramarea`
--

LOCK TABLES `paramarea` WRITE;
/*!40000 ALTER TABLE `paramarea` DISABLE KEYS */;
INSERT INTO `paramarea` VALUES (38,'TCS','2021-02-25 13:07:28'),(39,'TCP','2021-02-25 13:07:28');
/*!40000 ALTER TABLE `paramarea` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramcargosparticipantes`
--

DROP TABLE IF EXISTS `paramcargosparticipantes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramcargosparticipantes` (
  `idparamCargoParticipante` smallint NOT NULL AUTO_INCREMENT,
  `CargoParticipante` varchar(100) DEFAULT NULL,
  `idpTipoCertificacion` smallint DEFAULT NULL,
  `fechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamCargoParticipante`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramcargosparticipantes`
--

LOCK TABLES `paramcargosparticipantes` WRITE;
/*!40000 ALTER TABLE `paramcargosparticipantes` DISABLE KEYS */;
INSERT INTO `paramcargosparticipantes` VALUES (1,'AUDITOR LIDER',1,'2021-02-25 14:05:59'),(2,'AUDITOR 1',1,'2021-02-25 14:05:59'),(3,'AUDITOR 2',1,'2021-02-25 14:05:59'),(4,'AUDITOR 3',1,'2021-02-25 14:05:59'),(5,'AUDITOR ENSAYOS CON TESTIGO',1,'2021-02-25 14:05:59'),(6,'EXPERTO TECNICO',1,'2021-02-25 14:05:59'),(7,'AUDITOR EN FORMACION',1,'2021-02-25 14:05:59'),(8,'EXPERTO TÉCNICO 1',1,'2021-02-25 14:05:59'),(9,'EXPERTO TÉCNICO 2',1,'2021-02-25 14:05:59'),(10,'EXPERTO TÉCNICO 3',1,'2021-02-25 14:05:59');
/*!40000 ALTER TABLE `paramcargosparticipantes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramdepartamentos`
--

DROP TABLE IF EXISTS `paramdepartamentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramdepartamentos` (
  `idparamDepartamento` int NOT NULL AUTO_INCREMENT,
  `idparamPais` int DEFAULT NULL,
  `Departamento` varchar(150) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamDepartamento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramdepartamentos`
--

LOCK TABLES `paramdepartamentos` WRITE;
/*!40000 ALTER TABLE `paramdepartamentos` DISABLE KEYS */;
/*!40000 ALTER TABLE `paramdepartamentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramdocumentos`
--

DROP TABLE IF EXISTS `paramdocumentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramdocumentos` (
  `idparamdocumentos` int NOT NULL AUTO_INCREMENT,
  `NombrePlantilla` varchar(100) DEFAULT NULL,
  `Descripcion` varchar(100) DEFAULT NULL,
  `Path` varchar(300) DEFAULT NULL,
  `Area` varchar(20) DEFAULT NULL,
  `Habilitado` int DEFAULT NULL,
  `Proceso` varchar(70) DEFAULT NULL,
  `Method` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`idparamdocumentos`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramdocumentos`
--

LOCK TABLES `paramdocumentos` WRITE;
/*!40000 ALTER TABLE `paramdocumentos` DISABLE KEYS */;
INSERT INTO `paramdocumentos` VALUES (1,'REG-PRO-TCS-05-01.07','Plan de auditoria','REG-PRO-TCS-05-01.07 Plan de auditoria V.1.0.doc','TCS',1,'ELABORACION','TCSGenerarPlanAuditoria'),(2,'REG-PRO-TCS-05-02.06','Lista de Verificación Reunión de Apertura','REG-PRO-TCS-05-02.06 Lista de Verificación Reunión de Apertura V.1.0.doc','TCS',1,'ELABORACION','TCSGenerarREPListaVerificacionReunionApertura'),(3,'REG-PRO-TCS-05-03.04','Lista de verificación reunión de cierre','REG-PRO-TCS-05-03.04 Lista de verificación reunión de cierre (Ver1.0).doc','TCS',1,'ELABORACION','TCSGenerarREPListaVerificacionReunionCierre'),(4,'REG-PRO-TCS-05-05.00','Lista de verificación auditor','REG-PRO-TCS-05-05.00 Lista de verificación auditor V.1.0.doc','TCS',1,'ELABORACION','TCSGenerarREPListaVerificacionAuditor'),(5,'REG-PRO-TCS-05-05B_00','Lista de verificación auditoria BPM','REG-PRO-TCS-05-05B_00 Lista de verificación auditoria BPM - (Ver 1.0).doc','TCS',1,'ELABORACION','TCSGenerarREPLIstaVerificacionBPM'),(6,'REG-PRO-TCS-05-06.01','Informe Etapa I','REG-PRO-TCS-05-06.01 Informe Etapa I V.1.0.doc','TCS',1,'ELABORACION','TCSRepInformeAuditoriaEtapaI'),(7,'REG-PRO-TCS-05-07.11','Informe de auditoria','REG-PRO-TCS-05-07.11 Informe de auditoria V.1.0.doc','TCS',1,'ELABORACION','TCSGenerarREPInformeAuditoria'),(8,'REG-PRO-TCS-05-08.03','Datos de la organizacion','REG-PRO-TCS-05-08.03 Datos de la organizacion (Ver 1.0).doc','TCS',1,'ELABORACION','TCSGenerarREPDatosDeLaOrganizacion'),(9,'REG-PRO-TCS-06-01_00','Evaluacion y Recomendacion','REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0).doc','TCS',1,'TOMA_DECISION','TCSGenerarREPEvaluacionYRecomendacionesParaProceso'),(10,'REG-PRO-TCS-06-02_04','Decision Favorable','REG-PRO-TCS-06-02_04_DecisionFavorableDeLaCertificacion (Ver 1.0).doc','TCS',1,'TOMA_DECISION','TCSGenerarDescisionFavorableCertificacion'),(11,'REG-PRO-TCS-06-03_00','Decision No Favorable','REG-PRO-TCS-06-03_00_DecisionNoFavorable (Ver 1.0).doc','TCS',1,'TOMA_DECISION','TCSGenerarREPDecisionNOFavorable'),(12,'REG-PRO-TCS-07-01.00','Nota de suspensión de certificacion','REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc','TCS',1,'TOMA_DECISION','TCSGenerarNotaSuspension'),(13,'REG-PRO-TCS-07-02.00','Nota de retiro de certificacion','REG-PRO-TCS-07-02.00 Nota de retiro de certificacion.doc','TCS',1,'TOMA_DECISION','TCSGenerarREPNotaDeRetiroDeCertificacion'),(14,'REG-PRO-TCP-03-01','Designación auditoria','REG-PRO-TCP-03-01 Designación auditoria V.1.0.doc','TCP',1,'DESIGNACION','TCPRepDesignacionAuditoria'),(15,'REG-PRO-TCP-05-01.01','Plan de Auditoria','REG-PRO-TCP-05-01.01 Plan de Auditoria V.1.0.doc','TCP',1,'ELABORACION','TCPREPPlanAuditoria'),(16,'REG-PRO-TCP-05-02.01 ','Lista de verificación reunión de apertura','REG-PRO-TCP-05-02.01 Lista de verificación reunión de apertura V.1.0.doc','TCP',1,'ELABORACION','TCPREPListaVerificacionReunionApertura'),(17,'REG-PRO-TCP-05-03.01 ','Lista de Verificación reunión de cierre','REG-PRO-TCP-05-03.01 Lista de Verificación reunión de cierre V.1.0.doc','TCP',1,'ELABORACION','TCPREPListaVerificacionReunionCierre'),(18,'REG-PRO-TCP-05-04.01','Lista de Asistencia','REG-PRO-TCP-05-04.01 Lista de Asistencia V.1.0.doc','TCP',1,'ELABORACION','TCPREPListaAsistencia'),(19,'REG-PRO-TCP-05-05.01','Acta de Muestreo','REG-PRO-TCP-05-05.01 Acta de Muestreo V.1.0.doc','TCP',1,'ELABORACION','TCPREPActaMuestreo'),(20,'REG-PRO-TCP-05-06.01','Lista de Verificación Auditor','REG-PRO-TCP-05-06.01 Lista de Verificación Auditor V.1.0.doc','TCP',1,'ELABORACION','TCPREPListaVerificacionAuditor'),(21,'REG-PRO-TCP-05-07.01','Informe Preliminar ','REG-PRO-TCP-05-07.01 Informe Preliminar V.1.0.doc','TCP',1,'ELABORACION','TCPREPInformePreliminar'),(22,'REG-PRO-TCP-05-08.01','Informe','REG-PRO-TCP-05-08.01 Informe.doc','TCP',1,'ELABORACION','TCPGenerarTCPREPInforme'),(23,'REG-PRO-TCP-05-09.00','Plan de accion','REG-PRO-TCP-05-09.00 Plan de accion V.1.0.doc','TCP',1,'ELABORACION','TCPGenerarTCPREPPlanAccion'),(24,'REG-PRO-TCP-06-01.01','Lista de Miembros del CONCER','REG-PRO-TCP-06-01.01 Lista de Miembros del CONCER.doc','TCP',0,'TOMA_DECISION',NULL),(25,'REG-PRO-TCP-06-02.01','REG-PRO-Lista de Asistencia','REG-PRO-TCP-06-02.01 Lista de Asistencia V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPListaAsistencia'),(26,'REG-PRO-TCP-06-03.01','Acta de Reunión','REG-PRO-TCP-06-03.01 Acta de Reunión V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPActaReunion'),(27,'REG-PRO-TCP-06-04.02','Resolución administrativa V.1.0.doc','REG-PRO-TCP-06-04.02 Resolución administrativa V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPResolucionAdministrativa'),(28,'REG-PRO-TCP-06-05.00','Decision de la certificación','REG-PRO-TCP-06-05.00 Decision de la certificación V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPDecisionCertificacion'),(29,'REG-PRO-TCP-06-06.00','Decisión conforme a reglamento ','REG-PRO-TCP-06-06.00 Decisión conforme a reglamento V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPDecisionConformeReglamento'),(30,'REG-PRO-TCP-07-01.00','Nota de suspensión de certificación de producto','REG-PRO-TCP-07-01.00 Nota de suspensión de certificación de producto V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPNotaSuspencionCertificado'),(31,'REG-PRO-TCP-07-02.00','Nota de retiro de certificacion de producto','REG-PRO-TCP-07-02.00 Nota de retiro de certificacion de producto V.1.0.doc','TCP',1,'TOMA_DECISION','TCPGenerarTCPREPNotaRetiroCertificacion'),(32,'REG-PRO-TCS-05-04.05','Lista de asistencia V.1.0.doc','REG-PRO-TCS-05-04.05 Lista de asistencia V.1.0.doc','TCS',1,'ELABORACION','TCSGenerarREPListaAsistencia'),(33,'REG-PRO-TCS-03-01','DesignacionAuditoria.doc','REG-PRO-TCS-03-01 DesignacionAuditoria(Ver1.0).doc','TCS',1,'APERTURA','TCSDesignacionAuditoria'),(34,'REG-PRO-TCP-03-01','Designación auditoria V.1.0.doc','REG-PRO-TCP-03-01 Designación auditoria V.1.0.doc','TCP',1,'APERTURA','TCPRepDesignacionAuditoria'),(35,'REG-PRO-TCS-05-09.04',' Plan de acciones correctivas Ver1.0','REG-PRO-TCS-05-09.04 Plan de acciones correctivas Ver1.0.doc','TCS',1,'ELABORACION','TCSGenerarTCSREPPlanAccion'),(36,'REG-PRO-TCP-02-03.02','TCP Oferta Contrato Ver1.0.doc','REG-PRO-TCP-02-03.02 Oferta Contrato Ver1.0.doc','TCP',1,'OFERTA','TCPRepOfertaContrato'),(37,'REG-PRO-TCS-02-03A.01','TCS Oferta Contrato','REG-PRO-TCS-02-03A.01 Oferta Contrato.doc','TCS',1,'OFERTA','TCSRepOfertaContrato');
/*!40000 ALTER TABLE `paramdocumentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramestadosparticipante`
--

DROP TABLE IF EXISTS `paramestadosparticipante`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramestadosparticipante` (
  `idparamEstadoParticipante` smallint NOT NULL AUTO_INCREMENT,
  `EstadoParticipante` varchar(30) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamEstadoParticipante`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramestadosparticipante`
--

LOCK TABLES `paramestadosparticipante` WRITE;
/*!40000 ALTER TABLE `paramestadosparticipante` DISABLE KEYS */;
INSERT INTO `paramestadosparticipante` VALUES (1,'VIGENTE','2021-02-25 13:24:54'),(2,'BAJA','2021-02-25 13:24:54');
/*!40000 ALTER TABLE `paramestadosparticipante` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramestadosprogauditoria`
--

DROP TABLE IF EXISTS `paramestadosprogauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramestadosprogauditoria` (
  `idparamEstadosProgAuditoria` smallint NOT NULL AUTO_INCREMENT,
  `EstadosProgAuditoria` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamEstadosProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramestadosprogauditoria`
--

LOCK TABLES `paramestadosprogauditoria` WRITE;
/*!40000 ALTER TABLE `paramestadosprogauditoria` DISABLE KEYS */;
INSERT INTO `paramestadosprogauditoria` VALUES (1,'Sin fecha de auditoría','2021-02-25 13:22:50'),(2,'Con fecha de auditoría','2021-02-25 13:22:50'),(3,'Auditoría realizada','2021-02-25 13:22:50'),(4,'Decisión tomada','2021-02-25 13:22:51'),(5,'Recomendación tomada','2021-02-25 13:22:51');
/*!40000 ALTER TABLE `paramestadosprogauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parametapaauditoria`
--

DROP TABLE IF EXISTS `parametapaauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parametapaauditoria` (
  `idParametapaauditoria` int NOT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`idParametapaauditoria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parametapaauditoria`
--

LOCK TABLES `parametapaauditoria` WRITE;
/*!40000 ALTER TABLE `parametapaauditoria` DISABLE KEYS */;
INSERT INTO `parametapaauditoria` VALUES (1,'SIN FECHA DE AUDITORIA'),(2,'CON FECHA DE AUDITORIA'),(3,'AUDITORIA INICIADA'),(4,'AUDITORIA TERMINADA'),(5,'DECISION TOMADA'),(6,'RECOMENDACION TOMADA');
/*!40000 ALTER TABLE `parametapaauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramitemselect`
--

DROP TABLE IF EXISTS `paramitemselect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramitemselect` (
  `idparamItemSelect` bigint NOT NULL AUTO_INCREMENT,
  `idParamListaItemSelect` int DEFAULT NULL,
  `ItemSelect` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamItemSelect`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramitemselect`
--

LOCK TABLES `paramitemselect` WRITE;
/*!40000 ALTER TABLE `paramitemselect` DISABLE KEYS */;
INSERT INTO `paramitemselect` VALUES (1,1,'1. Presentación del equipo auditor, incluyendo una breve descripción de funciones.','1','2021-03-07 10:51:41'),(2,1,'2. Confirmación del idioma que se utilizará durante la auditoría.','1','2021-03-07 10:52:30'),(3,1,'3. Confirmación de los temas relativos a la confidencialidad.','1','2021-03-07 10:52:30'),(4,1,'4. Confirmación del alcance y objetivos de la auditoría, norma auditada.','1','2021-03-07 10:52:30'),(5,1,'5. Confirmación del plan de auditoría (incluye tipo y alcance de auditoría los objetivos y los criterios), cualquier cambio, y otros acuerdos pertinentes con el cliente.','1','2021-03-07 10:52:30'),(6,1,'6. Confirmación de los canales comunicación formal.','1','2021-03-07 10:52:30'),(7,1,'7. Confirmación del estado de los hallazgos de la revisión o auditoría anterior cuando corresponda.','1','2021-03-07 10:52:30'),(8,1,'8. Confirmación de la disponibilidad, de las funciones y de la identidad de los guías y observadores.','1','2021-03-07 10:52:30'),(9,1,'9. El método para presentar la información, incluyendo cualquier categorización de los hallazgos de la auditoría.','1','2021-03-07 10:52:31'),(10,1,'10. Información sobre las condiciones bajo las cuales la auditoría puede darse por terminada prematuramente.','1','2021-03-07 10:52:31'),(11,1,'11.Confirmación que durante la auditoría se mantendrá informada a la organización sobre el proceso de la auditoría y cualquier problema.','1','2021-03-07 10:52:31'),(12,1,'12. Confirmación de que el líder y los miembros del equipo auditor, que representan a IBNORCA, son responsables de la auditoría y que deben controlar la ejecución del plan de auditoría, incluyendo las actividades y las líneas de investigación de la auditoría.','1','2021-03-07 10:52:31'),(13,1,'13. Los métodos y procedimientos a utilizar para llevar a cabo la auditoría sobre la base de un muestreo.','1','2021-03-07 10:52:31'),(14,1,'14. Confirmación de que están disponibles los recursos y la logística necesaria para el equipo auditor.','1','2021-03-07 10:52:31'),(15,1,'15. Confirmación de los procedimientos de protección, emergencia y seguridad en el trabajo pertinentes para el equipo auditor.','1','2021-03-07 10:52:31'),(16,1,'16. Confirmación de las TICs a aplicar y su disponibilidad.','1','2021-03-07 10:52:31'),(17,1,'17. Aclaraciones','1','2021-03-07 10:52:31'),(18,2,'1. Agradecimiento','1','2021-03-07 10:56:42'),(19,2,'2. Confirmación del alcance y objetivos de la auditoría, alcance de la certificación y norma auditada.','1','2021-03-07 10:56:42'),(20,2,'3. Informar que las evidencias de auditoría reunidas se basan en un muestreo por lo tanto se introduce un elemento de incertidumbre.','1','2021-03-07 10:56:42'),(21,2,'4. Lectura y hallazgos de auditoría indicando su categorización.','1','2021-03-07 10:56:42'),(22,2,'5. Lectura de conclusión de auditoría.','1','2021-03-07 10:56:42'),(23,2,'6. Informar sobre el plazo máximo de 15 días calendario a partir de la culminación de la auditoría para él envió de las correcciones y acciones correctivas al auditor líder de las No conformidades menores y/o mayores identificadas para posterior aprobación por el auditor líder. En el caso de no conformidades mayores la organización debe de enviar las evidencias en un plazo de 90 días calendario a partir de la auditoría, para posterior aprobación por el auditor líder.','1','2021-03-07 10:56:43'),(24,2,'7. Fecha estimada de la próxima auditoría.','1','2021-03-07 10:56:43'),(25,2,'8. Informar acerca de los procesos que tienen IBNORCA, para el tratamiento de quejas y de apelaciones.','1','2021-03-07 10:56:43'),(26,2,'9. Aclaraciones.','1','2021-03-07 10:56:43'),(27,2,'10. Firma del informe de auditoría.','1','2021-03-07 10:56:43'),(28,2,'11. Revisión del alcance en el informe y el formulario de Datos de la organización.','1','2021-03-07 10:56:43');
/*!40000 ALTER TABLE `paramitemselect` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramlistasitemselect`
--

DROP TABLE IF EXISTS `paramlistasitemselect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramlistasitemselect` (
  `idParamListaItemSelect` int NOT NULL AUTO_INCREMENT,
  `lista` varchar(100) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idParamListaItemSelect`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramlistasitemselect`
--

LOCK TABLES `paramlistasitemselect` WRITE;
/*!40000 ALTER TABLE `paramlistasitemselect` DISABLE KEYS */;
INSERT INTO `paramlistasitemselect` VALUES (1,'TCS - LISTA DE VERIFICACION DE APERTURA','1','2021-03-07 10:39:42'),(2,'TCS-LISTA DE VERIFICACION DE CIERRE','1','2021-03-07 10:39:42');
/*!40000 ALTER TABLE `paramlistasitemselect` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramnormas`
--

DROP TABLE IF EXISTS `paramnormas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramnormas` (
  `idparamNorma` smallint NOT NULL AUTO_INCREMENT,
  `CodigoDeNorma` varchar(30) DEFAULT NULL,
  `Norma` varchar(500) DEFAULT NULL,
  `idpArea` smallint DEFAULT NULL,
  `pathNorma` varchar(300) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamNorma`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramnormas`
--

LOCK TABLES `paramnormas` WRITE;
/*!40000 ALTER TABLE `paramnormas` DISABLE KEYS */;
INSERT INTO `paramnormas` VALUES (1,'ISO/27001','TEC - 1ER SELLO',1,NULL,'2021-02-25 13:07:46'),(2,'ISO/27001','TEC - 2DO SELLO;',2,NULL,'2021-02-25 13:07:46');
/*!40000 ALTER TABLE `paramnormas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parampaises`
--

DROP TABLE IF EXISTS `parampaises`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parampaises` (
  `idparamPais` int NOT NULL AUTO_INCREMENT,
  `Pais` varchar(150) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamPais`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parampaises`
--

LOCK TABLES `parampaises` WRITE;
/*!40000 ALTER TABLE `parampaises` DISABLE KEYS */;
/*!40000 ALTER TABLE `parampaises` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramtipoauditoria`
--

DROP TABLE IF EXISTS `paramtipoauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramtipoauditoria` (
  `idparamTipoAuditoria` smallint NOT NULL AUTO_INCREMENT,
  `TipoAuditoria` varchar(50) DEFAULT NULL,
  `idpTipoCertificacion` int DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamTipoAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramtipoauditoria`
--

LOCK TABLES `paramtipoauditoria` WRITE;
/*!40000 ALTER TABLE `paramtipoauditoria` DISABLE KEYS */;
INSERT INTO `paramtipoauditoria` VALUES (1,'Certificación',2,'2021-02-25 15:06:16'),(2,'Seguimiento I',2,'2021-02-25 15:06:16'),(3,'Seguimiento II',2,'2021-02-25 15:06:16'),(4,'ETAPA I',1,'2021-02-25 15:06:16'),(5,'ETAPA II',1,'2021-02-25 15:06:16'),(6,'RENOVACIÓN',1,'2021-02-25 15:06:16'),(7,'Seguimiento I',1,'2021-02-25 15:06:16'),(8,'Seguimiento II',1,'2021-02-25 15:06:16');
/*!40000 ALTER TABLE `paramtipoauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paramtiposervicio`
--

DROP TABLE IF EXISTS `paramtiposervicio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paramtiposervicio` (
  `idparamTipoServicio` smallint NOT NULL AUTO_INCREMENT,
  `TipoServicio` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idparamTipoServicio`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paramtiposervicio`
--

LOCK TABLES `paramtiposervicio` WRITE;
/*!40000 ALTER TABLE `paramtiposervicio` DISABLE KEYS */;
INSERT INTO `paramtiposervicio` VALUES (1,'CERTIFICACION','2021-02-25 13:10:30'),(2,'RENOVACION','2021-02-25 13:10:30');
/*!40000 ALTER TABLE `paramtiposervicio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person` (
  `idperson` int NOT NULL AUTO_INCREMENT,
  `name` varchar(250) NOT NULL,
  `lastname` varchar(250) NOT NULL,
  PRIMARY KEY (`idperson`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person`
--

LOCK TABLES `person` WRITE;
/*!40000 ALTER TABLE `person` DISABLE KEYS */;
INSERT INTO `person` VALUES (1,'ruben','chalco'),(3,'fff','aaaaa'),(4,'prueba1','app'),(5,'prueba1','app'),(6,'prueba1','app'),(7,'prueba1','app'),(8,'prueba1','app'),(9,'prueba1','app'),(10,'prueba1','app'),(11,'Dario','Chalco'),(12,'Loro','Verde');
/*!40000 ALTER TABLE `person` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plaauditorium`
--

DROP TABLE IF EXISTS `plaauditorium`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plaauditorium` (
  `idPlAAuditoria` bigint NOT NULL AUTO_INCREMENT,
  `iidPrACicloProgAuditoria` bigint DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDeRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idPlAAuditoria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plaauditorium`
--

LOCK TABLES `plaauditorium` WRITE;
/*!40000 ALTER TABLE `plaauditorium` DISABLE KEYS */;
/*!40000 ALTER TABLE `plaauditorium` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `placronoequipo`
--

DROP TABLE IF EXISTS `placronoequipo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `placronoequipo` (
  `idPlACronoEquipo` bigint NOT NULL AUTO_INCREMENT,
  `idPlACronograma` bigint DEFAULT NULL,
  `idCicloParticipante` bigint DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idPlACronoEquipo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `placronoequipo`
--

LOCK TABLES `placronoequipo` WRITE;
/*!40000 ALTER TABLE `placronoequipo` DISABLE KEYS */;
/*!40000 ALTER TABLE `placronoequipo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pladiasequipo`
--

DROP TABLE IF EXISTS `pladiasequipo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pladiasequipo` (
  `idPlADiasEquipo` bigint NOT NULL AUTO_INCREMENT,
  `idCicloParticipante` bigint DEFAULT NULL,
  `DiasInSitu` int DEFAULT NULL,
  `DiasRemoto` int DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT NULL,
  PRIMARY KEY (`idPlADiasEquipo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pladiasequipo`
--

LOCK TABLES `pladiasequipo` WRITE;
/*!40000 ALTER TABLE `pladiasequipo` DISABLE KEYS */;
/*!40000 ALTER TABLE `pladiasequipo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `praciclocronogramas`
--

DROP TABLE IF EXISTS `praciclocronogramas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `praciclocronogramas` (
  `idCiclosCronogramas` bigint NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `DiasInsitu` decimal(10,2) DEFAULT NULL,
  `DiasRemoto` decimal(10,2) DEFAULT NULL,
  `HorarioTrabajo` varchar(100) DEFAULT NULL,
  `MesProgramado` datetime DEFAULT NULL,
  `MesReprogramado` datetime DEFAULT NULL,
  `FechaInicioDeEjecucionDeAuditoria` datetime DEFAULT NULL,
  `FechaDeFinDeEjecucionAuditoria` datetime DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  `DiasPresupuesto` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`idCiclosCronogramas`),
  KEY `FK_PrACicloCronograma` (`idPrACicloProgAuditoria`),
  CONSTRAINT `FK_PrACicloCronograma` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=220 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `praciclocronogramas`
--

LOCK TABLES `praciclocronogramas` WRITE;
/*!40000 ALTER TABLE `praciclocronogramas` DISABLE KEYS */;
INSERT INTO `praciclocronogramas` VALUES (204,223,0.00,0.00,NULL,NULL,NULL,NULL,NULL,'ruben.chalco','2021-05-13 13:32:11',NULL,6.00),(205,224,0.00,6.00,'08:30-17:30','2021-09-12 00:00:00','2021-06-01 04:00:00','2021-06-13 04:00:00','2021-06-15 04:00:00','ruben.chalco','2021-05-13 13:32:11',NULL,1.00),(206,226,0.00,0.00,NULL,'2023-09-12 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 13:32:11',NULL,1.00),(207,225,0.00,0.00,NULL,'2022-09-12 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 13:32:11',NULL,1.00),(208,227,0.00,0.00,NULL,NULL,NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:17:22',NULL,3.00),(209,228,0.00,0.00,NULL,'2019-05-08 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:17:22',NULL,1.00),(210,229,0.00,0.00,NULL,'2020-05-08 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:17:22',NULL,1.00),(211,230,0.00,0.00,NULL,'2021-05-08 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:17:22',NULL,1.00),(212,231,0.00,0.00,NULL,NULL,NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:20:24',NULL,4.50),(213,233,0.00,0.00,NULL,'2022-04-18 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:20:24',NULL,3.00),(214,234,0.00,0.00,NULL,'2023-04-18 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-13 15:20:24',NULL,3.00),(215,232,3.00,0.00,'07:30-16:30','2021-04-18 00:00:00','2021-05-01 04:00:00','2021-05-24 04:00:00','2021-05-26 04:00:00','ruben.chalco','2021-05-13 15:20:24',NULL,3.00),(216,235,0.00,0.00,NULL,NULL,NULL,NULL,NULL,'ruben.chalco','2021-05-17 14:07:17',NULL,2.50),(217,236,0.00,0.00,NULL,'2019-04-17 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-17 14:07:17',NULL,1.50),(218,237,0.00,0.00,NULL,'2020-04-17 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-17 14:07:17',NULL,1.50),(219,238,0.00,0.00,NULL,'2021-04-17 00:00:00',NULL,NULL,NULL,'ruben.chalco','2021-05-17 14:07:17',NULL,1.50);
/*!40000 ALTER TABLE `praciclocronogramas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `praciclonormassistema`
--

DROP TABLE IF EXISTS `praciclonormassistema`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `praciclonormassistema` (
  `idCicloNormaSistema` bigint NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `idparamNorma` smallint DEFAULT NULL,
  `Norma` varchar(1000) DEFAULT NULL,
  `Alcance` varchar(500) DEFAULT NULL,
  `FechaEmisionPrimerCertificado` datetime DEFAULT NULL,
  `FechaVencimientoUltimoCertificado` datetime DEFAULT NULL,
  `NumeroDeCertificacion` varchar(100) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  PRIMARY KEY (`idCicloNormaSistema`),
  KEY `FK_PrACicloNormaSistema` (`idPrACicloProgAuditoria`),
  CONSTRAINT `FK_PrACicloNormaSistema` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=117 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `praciclonormassistema`
--

LOCK TABLES `praciclonormassistema` WRITE;
/*!40000 ALTER TABLE `praciclonormassistema` DISABLE KEYS */;
INSERT INTO `praciclonormassistema` VALUES (109,231,NULL,'NB/ISO/TS 29001:2008','\"Comercialización y despacho de materiales de acero para la construcción civil; Servicio de corte y doblado de barras de acero para la construcción; Proceso de producción de mallas electrosoldadas y perfiles.\"','2020-09-30 00:00:00','2023-06-18 00:00:00','10546','ruben.chalco','2021-05-13 15:20:24',NULL),(110,233,NULL,'S/A','\"Comercialización y despacho de materiales de acero para la construcción civil; Servicio de corte y doblado de barras de acero para la construcción; Proceso de producción de mallas electrosoldadas y perfiles.\"','2020-09-30 00:00:00','2023-06-18 00:00:00','10546','ruben.chalco','2021-05-13 15:20:24',NULL),(111,234,NULL,'S/A','\"Comercialización y despacho de materiales de acero para la construcción civil; Servicio de corte y doblado de barras de acero para la construcción; Proceso de producción de mallas electrosoldadas y perfiles.\"','2020-09-30 00:00:00','2023-06-18 00:00:00','10546','ruben.chalco','2021-05-13 15:20:24',NULL),(112,232,NULL,'NB 39001:2014','\"Comercialización y despacho de materiales de acero para la construcción civil; Servicio de corte y doblado de barras de acero para la construcción; Proceso de producción de mallas electrosoldadas y perfiles.\"','2020-09-30 00:00:00','2023-06-18 00:00:00','10546','ruben.chalco','2021-05-13 15:20:24',NULL),(113,235,NULL,'','\"Despacho Aduanero de importación y exportación.”','2018-06-18 00:00:00','2021-06-17 00:00:00','758','ruben.chalco','2021-05-17 14:07:17',NULL),(114,236,NULL,'','\"Despacho Aduanero de importación y exportación.”','2018-06-18 00:00:00','2021-06-17 00:00:00','758','ruben.chalco','2021-05-17 14:07:17',NULL),(115,237,NULL,'','\"Despacho Aduanero de importación y exportación.”','2018-06-18 00:00:00','2021-06-17 00:00:00','758','ruben.chalco','2021-05-17 14:07:17',NULL),(116,238,NULL,'','\"Despacho Aduanero de importación y exportación.”','2018-06-18 00:00:00','2021-06-17 00:00:00','758','ruben.chalco','2021-05-17 14:07:17',NULL);
/*!40000 ALTER TABLE `praciclonormassistema` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pracicloparticipantes`
--

DROP TABLE IF EXISTS `pracicloparticipantes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pracicloparticipantes` (
  `idCicloParticipante` bigint NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `idParticipante_ws` int DEFAULT NULL,
  `ParticipanteDetalle_ws` json DEFAULT NULL,
  `IdCargoWs` int DEFAULT NULL,
  `CargoDetalleWs` json DEFAULT NULL,
  `diasInsistu` decimal(10,2) DEFAULT NULL,
  `diasRemoto` decimal(10,2) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  PRIMARY KEY (`idCicloParticipante`),
  KEY `FK_PrACicloParticipantes` (`idPrACicloProgAuditoria`),
  CONSTRAINT `FK_PrACicloParticipantes` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=348 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pracicloparticipantes`
--

LOCK TABLES `pracicloparticipantes` WRITE;
/*!40000 ALTER TABLE `pracicloparticipantes` DISABLE KEYS */;
INSERT INTO `pracicloparticipantes` VALUES (328,224,13,'{\"norma\": \"NB/ISO 9001:2015\", \"ciudad\": \"La Paz\", \"correo\": \"rcastroparada@gmail.com\", \"codsIAF\": \"16, 3\", \"idCiudad\": \"62\", \"telefono\": \"-, 73034330\", \"idCliente\": \"13\", \"cargoPuesto\": \"Auditor\", \"calificacion\": \"Calificado\", \"idCargoPuesto\": \"2408\", \"calificacionId\": \"3639\", \"idCalificacion\": \"108\", \"identificacion\": \"3433771\", \"nombreCompleto\": \"René Castro Parada\"}',2408,'{\"cargoPuesto\": \"Auditor\", \"idCargoPuesto\": \"2408\"}',NULL,3.00,'ruben.chalco','2021-05-13 13:32:11'),(329,224,177,'{\"norma\": null, \"ciudad\": \"La Paz\", \"correo\": \"cintycazas@gmail.com\", \"codsIAF\": \"28, 16, 17, 3, 14\", \"idCiudad\": \"62\", \"telefono\": \", 73477524\", \"idCliente\": \"177\", \"cargoPuesto\": \"Auditor Lider\", \"calificacion\": \"Calificado\", \"idCargoPuesto\": \"2409\", \"calificacionId\": \"3639\", \"idCalificacion\": \"128\", \"identificacion\": \"6699734\", \"nombreCompleto\": \"Cintya Marcela Zarate Cazas\"}',2409,'{\"cargoPuesto\": \"Auditor Lider\", \"idCargoPuesto\": \"2409\"}',NULL,3.00,'ruben.chalco','2021-05-13 13:32:11'),(337,232,34,'{\"norma\": \"NB/ISO 9001:2015\", \"ciudad\": \"Cochabamba\", \"correo\": \"btorrico@gmail.com\", \"codsIAF\": \"28, 37, 16, 17, 14\", \"idCiudad\": \"64\", \"telefono\": \"4476267, 77492263\", \"idCliente\": \"34\", \"cargoPuesto\": \"Auditor Lider\", \"calificacion\": \"Calificado\", \"idCargoPuesto\": \"2409\", \"calificacionId\": \"3639\", \"idCalificacion\": \"4\", \"identificacion\": \"3570044\", \"nombreCompleto\": \"Benjamin Joel Torrico Herbas \"}',2409,'{\"cargoPuesto\": \"Auditor Lider\", \"idCargoPuesto\": \"2409\"}',3.00,NULL,'ruben.chalco','2021-05-13 15:20:24'),(344,235,NULL,NULL,2409,'{\"dias\": \"2.50\", \"monto\": \"1252.80\", \"codigo\": \"7500\", \"cod_anio\": \"1\", \"descripcion\": \"AUDITOR LIDER\", \"monto_total\": \"3132.00\", \"cod_tipoauditor\": \"2409\", \"monto_solicitado\": \"175\", \"cod_simulacionservicio\": \"248\"}',NULL,NULL,'ruben.chalco','2021-05-17 14:07:17'),(345,236,NULL,NULL,2409,'{\"dias\": \"1.50\", \"monto\": \"1252.80\", \"codigo\": \"7508\", \"cod_anio\": \"2\", \"descripcion\": \"AUDITOR LIDER\", \"monto_total\": \"1879.20\", \"cod_tipoauditor\": \"2409\", \"monto_solicitado\": \"105\", \"cod_simulacionservicio\": \"248\"}',NULL,NULL,'ruben.chalco','2021-05-17 14:07:17'),(346,237,NULL,NULL,2409,'{\"dias\": \"1.50\", \"monto\": \"1252.80\", \"codigo\": \"7516\", \"cod_anio\": \"3\", \"descripcion\": \"AUDITOR LIDER\", \"monto_total\": \"1879.20\", \"cod_tipoauditor\": \"2409\", \"monto_solicitado\": \"105\", \"cod_simulacionservicio\": \"248\"}',NULL,NULL,'ruben.chalco','2021-05-17 14:07:17'),(347,238,NULL,NULL,2409,'{\"dias\": \"1.50\", \"monto\": \"1252.80\", \"codigo\": \"7516\", \"cod_anio\": \"3\", \"descripcion\": \"AUDITOR LIDER\", \"monto_total\": \"1879.20\", \"cod_tipoauditor\": \"2409\", \"monto_solicitado\": \"105\", \"cod_simulacionservicio\": \"248\"}',NULL,NULL,'ruben.chalco','2021-05-17 14:07:17');
/*!40000 ALTER TABLE `pracicloparticipantes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `praciclosprogauditorium`
--

DROP TABLE IF EXISTS `praciclosprogauditorium`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `praciclosprogauditorium` (
  `idPrACicloProgAuditoria` bigint NOT NULL AUTO_INCREMENT,
  `idPrAProgramaAuditoria` bigint DEFAULT NULL,
  `NombreOrganizacionCertificado` varchar(150) DEFAULT NULL,
  `idparamEstadosProgAuditoria` int DEFAULT NULL,
  `EstadoDescripcion` varchar(100) DEFAULT NULL,
  `Anio` int DEFAULT NULL,
  `idparamTipoAuditoria` int DEFAULT NULL,
  `Referencia` varchar(500) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  `idParametapaauditoria` int DEFAULT NULL,
  `IdEtapaDocumento` int DEFAULT NULL,
  PRIMARY KEY (`idPrACicloProgAuditoria`),
  KEY `FK_PrACicloPrograma` (`idPrAProgramaAuditoria`),
  KEY `FK_PraCicloEstado_idx` (`idParametapaauditoria`),
  CONSTRAINT `FK_PraCicloEstado` FOREIGN KEY (`idParametapaauditoria`) REFERENCES `parametapaauditoria` (`idParametapaauditoria`),
  CONSTRAINT `FK_PrACicloPrograma` FOREIGN KEY (`idPrAProgramaAuditoria`) REFERENCES `praprogramasdeauditorium` (`idPrAProgramaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=239 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `praciclosprogauditorium`
--

LOCK TABLES `praciclosprogauditorium` WRITE;
/*!40000 ALTER TABLE `praciclosprogauditorium` DISABLE KEYS */;
INSERT INTO `praciclosprogauditorium` VALUES (223,68,'PRODUCTOS DE ACERO CASSADO S.A. PRODAC  ',NULL,'SIN FECHA DE AUDITORIA',1,1,'Renovación','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,24),(224,68,'PRODUCTOS DE ACERO CASSADO S.A. PRODAC  ',NULL,'Con fecha de auditoría',2,1,'SEGUIMIENTO I','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,24),(225,68,'PRODUCTOS DE ACERO CASSADO S.A. PRODAC  ',NULL,'SIN FECHA DE AUDITORIA',3,1,'SEGUIMIENTO II','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,24),(226,68,'PRODUCTOS DE ACERO CASSADO S.A. PRODAC  ',NULL,'SIN FECHA DE AUDITORIA',4,1,'RENOVACIÓN','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,24),(227,69,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',1,1,'Auditoria de Certificación/Renovacion','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,24),(228,69,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',2,1,'Derecho Uso de Sello año 2','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,24),(229,69,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',3,1,'Derecho Uso de Sello año 3','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,24),(230,69,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',4,1,'Renovacion','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,24),(231,70,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',1,1,'ETAPA 2/RENOVACIÓN (Año 1)','ruben.chalco','2021-05-13 15:20:24',NULL,NULL,24),(232,70,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'Con fecha de auditoría',2,1,'SEGUIMIENTO 1 (Año 2)','ruben.chalco','2021-05-13 15:20:24',NULL,NULL,24),(233,70,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',3,1,'SEGUIMIENTO 2 (Año 3)','ruben.chalco','2021-05-13 15:20:24',NULL,NULL,24),(234,70,'IMPORT EXPORT LAS LOMAS LTDA.  ',NULL,'SIN FECHA DE AUDITORIA',4,1,'Renovacion','ruben.chalco','2021-05-13 15:20:24',NULL,NULL,24),(235,71,'AGENCIA DESPACHANTE DE ADUANA MERSUR  S.R.L  ',NULL,'SIN FECHA DE AUDITORIA',1,1,'ETAPA 2/RENOVACIÓN (Año 1)','ruben.chalco','2021-05-17 14:07:17',NULL,NULL,24),(236,71,'AGENCIA DESPACHANTE DE ADUANA MERSUR  S.R.L  ',NULL,'SIN FECHA DE AUDITORIA',2,1,'SEGUIMIENTO 1 (Año 2)','ruben.chalco','2021-05-17 14:07:17',NULL,NULL,24),(237,71,'AGENCIA DESPACHANTE DE ADUANA MERSUR  S.R.L  ',NULL,'SIN FECHA DE AUDITORIA',3,1,'SEGUIMIENTO 2 (Año 3)','ruben.chalco','2021-05-17 14:07:17',NULL,NULL,24),(238,71,'AGENCIA DESPACHANTE DE ADUANA MERSUR  S.R.L  ',NULL,'SIN FECHA DE AUDITORIA',4,1,'Renovacion','ruben.chalco','2021-05-17 14:07:17',NULL,NULL,24);
/*!40000 ALTER TABLE `praciclosprogauditorium` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pradireccionespaproducto`
--

DROP TABLE IF EXISTS `pradireccionespaproducto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pradireccionespaproducto` (
  `idDireccionPAProducto` bigint NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `Nombre` varchar(1000) DEFAULT NULL,
  `Direccion` varchar(1000) DEFAULT NULL,
  `Marca` varchar(50) DEFAULT NULL,
  `Norma` varchar(100) DEFAULT NULL,
  `Sello` varchar(50) DEFAULT NULL,
  `Ciudad` varchar(100) DEFAULT NULL,
  `Estado` varchar(45) DEFAULT NULL,
  `Pais` varchar(45) DEFAULT NULL,
  `FechaEmisionPrimerCertificado` datetime DEFAULT NULL,
  `FechaVencimientoUltimoCertificado` datetime DEFAULT NULL,
  `FechaVencimientoCertificado` datetime DEFAULT NULL,
  `NumeroDeCertificacion` varchar(100) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  `EstadoCONCER` varchar(100) DEFAULT NULL,
  `ReivsionCONCER` varchar(600) DEFAULT NULL,
  PRIMARY KEY (`idDireccionPAProducto`),
  KEY `FK_PrACicloDireccionesProducto` (`idPrACicloProgAuditoria`),
  CONSTRAINT `FK_PrACicloDireccionesProducto` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=113 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pradireccionespaproducto`
--

LOCK TABLES `pradireccionespaproducto` WRITE;
/*!40000 ALTER TABLE `pradireccionespaproducto` DISABLE KEYS */;
INSERT INTO `pradireccionespaproducto` VALUES (97,226,'\r\nRedes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \r\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \r\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \r\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC \r\n','NB 710:2015','1',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11289','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(98,225,'•	MALLA HEXAGONAL DE ACERO PARA APLICACIONES EN INGENIERÍA CIVIL (6X8, 8X10 Y 10X12):  A) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS CON ALAMBRE DE ACERO CON REVESTIMIENTO METÁLICO DE ALEACIÓN ZN90%/AL10%; B) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS C','AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.','PRODAC',',EN 10223-3:2013','3',NULL,'Lima y Callao','Peru',NULL,NULL,NULL,'','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(99,225,'\r\nGaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\r\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\r\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC\r\n',',ASTM A974-97:2016','2',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11579','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(100,224,'Malla hexagonal de acero para aplicaciones en ingeniería civil (6x8, 8x10 y 10x12):  a) gaviones, colchones, gaviones saco y rollos con alambre de acero con revestimiento metálico de aleación Zn90%/Al10%; b) gaviones, colchones, gaviones saco y rollos con alambre de acero con revestimiento metálico de aleación Zn90%/Al10%, adicionalmente recubiertos con una capa de PVC.','Av. Néstor Gambetta 6429, Callao, Lima - Perú.','PRODAC',',EN 10223-3:2013','3',NULL,'Lima y Callao','Peru',NULL,NULL,NULL,'','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(101,226,'\r\nGaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\r\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\r\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC\r\n',',ASTM A974-97:2016','2',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11579','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(102,224,'\r\nGaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\r\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\r\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC\r\n',',ASTM A974-97:2016','2',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11579','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(103,224,'\r\nRedes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \r\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \r\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \r\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC \r\n','NB 710:2015','1',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11289','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(104,223,'•	MALLA HEXAGONAL DE ACERO PARA APLICACIONES EN INGENIERÍA CIVIL (6X8, 8X10 Y 10X12):  A) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS CON ALAMBRE DE ACERO CON REVESTIMIENTO METÁLICO DE ALEACIÓN ZN90%/AL10%; B) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS C','AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.','PRODAC',',EN 10223-3:2013','3',NULL,'Lima y Callao','Peru',NULL,NULL,NULL,'','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(105,223,'\r\nGaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\r\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\r\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC\r\n',',ASTM A974-97:2016','2',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11579','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(106,223,'\r\nRedes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \r\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \r\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \r\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC \r\n','NB 710:2015','1',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11289','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(107,225,'\r\nRedes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \r\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \r\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \r\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\r\n','\r\n\r\nAv. Néstor Gambetta 6429, Callao, Lima - Perú ','\r\n\r\nGAVIONES/COLCHONES PRODAC \r\n','NB 710:2015','1',NULL,'Lima y Callao','Peru','2020-12-16 00:00:00','2023-12-12 00:00:00','2023-12-12 00:00:00','11289','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(108,226,'•	MALLA HEXAGONAL DE ACERO PARA APLICACIONES EN INGENIERÍA CIVIL (6X8, 8X10 Y 10X12):  A) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS CON ALAMBRE DE ACERO CON REVESTIMIENTO METÁLICO DE ALEACIÓN ZN90%/AL10%; B) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS C','AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.','PRODAC',',EN 10223-3:2013','3',NULL,'Lima y Callao','Peru',NULL,NULL,NULL,'','ruben.chalco','2021-05-13 13:32:11',NULL,NULL,NULL),(109,227,'\r\nMallas Electrosoldadas de acero para hormigón armado, tipo cuadradas con barras de diámetros: 4,2 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 6 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 8 mm y espaciamiento de 150 mm y 200 mm\r\n','\r\n\r\nCarretera a Cotoca km 15, Santa Cruz - Bolivia','\r\n \r\nLAS LOMAS\r\n','NB 734:2017','1','Santa Cruz','Santa Cruz','Bolivia','2020-11-18 00:00:00','2021-08-08 00:00:00','2021-08-08 00:00:00','11165','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,NULL),(110,228,'\r\nMallas Electrosoldadas de acero para hormigón armado, tipo cuadradas con barras de diámetros: 4,2 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 6 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 8 mm y espaciamiento de 150 mm y 200 mm\r\n','\r\n\r\nCarretera a Cotoca km 15, Santa Cruz - Bolivia','\r\n \r\nLAS LOMAS\r\n','NB 734:2017','1','Santa Cruz','Santa Cruz','Bolivia','2020-11-18 00:00:00','2021-08-08 00:00:00','2021-08-08 00:00:00','11165','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,NULL),(111,229,'\r\nMallas Electrosoldadas de acero para hormigón armado, tipo cuadradas con barras de diámetros: 4,2 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 6 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 8 mm y espaciamiento de 150 mm y 200 mm\r\n','\r\n\r\nCarretera a Cotoca km 15, Santa Cruz - Bolivia','\r\n \r\nLAS LOMAS\r\n','NB 734:2017','1','Santa Cruz','Santa Cruz','Bolivia','2020-11-18 00:00:00','2021-08-08 00:00:00','2021-08-08 00:00:00','11165','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,NULL),(112,230,'\r\nMallas Electrosoldadas de acero para hormigón armado, tipo cuadradas con barras de diámetros: 4,2 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 6 mm y espaciamiento de 100 mm, 150 mm, 200 mm y 250 mm; 8 mm y espaciamiento de 150 mm y 200 mm\r\n','\r\n\r\nCarretera a Cotoca km 15, Santa Cruz - Bolivia','\r\n \r\nLAS LOMAS\r\n','NB 734:2017','1','Santa Cruz','Santa Cruz','Bolivia','2020-11-18 00:00:00','2021-08-08 00:00:00','2021-08-08 00:00:00','11165','ruben.chalco','2021-05-13 15:17:22',NULL,NULL,NULL);
/*!40000 ALTER TABLE `pradireccionespaproducto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pradireccionespasistema`
--

DROP TABLE IF EXISTS `pradireccionespasistema`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pradireccionespasistema` (
  `idDireccionPASistema` bigint NOT NULL AUTO_INCREMENT,
  `idPrACicloProgAuditoria` bigint DEFAULT NULL,
  `Nombre` varchar(150) DEFAULT NULL,
  `Direccion` varchar(150) DEFAULT NULL,
  `Pais` varchar(100) DEFAULT NULL,
  `Departamento` varchar(100) DEFAULT NULL,
  `Ciudad` varchar(100) DEFAULT NULL,
  `Dias` decimal(10,2) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  PRIMARY KEY (`idDireccionPASistema`),
  KEY `FK_PrACicloDireccionesSistema` (`idPrACicloProgAuditoria`),
  CONSTRAINT `FK_PrACicloDireccionesSistema` FOREIGN KEY (`idPrACicloProgAuditoria`) REFERENCES `praciclosprogauditorium` (`idPrACicloProgAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=400 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pradireccionespasistema`
--

LOCK TABLES `pradireccionespasistema` WRITE;
/*!40000 ALTER TABLE `pradireccionespasistema` DISABLE KEYS */;
INSERT INTO `pradireccionespasistema` VALUES (368,234,'OFICINA CENTRAL','OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(369,234,'SUCURSAL VIRGEN DE COTOCA','PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(370,234,'SUCURSAL SANTOS DUMONT','PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(371,234,'SUCURSAL DOBLE VÍA','SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(372,233,'PLANTA INDUSTRIAL LA PAZ','SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA','Bolivia','La Paz','El Alto',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(373,233,'PLANTA INDUSTRIAL SANTA CRUZ','SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(374,233,'SUCURSAL MONTERO','SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Montero',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(375,233,'SUCURSAL DOBLE VÍA','SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(376,233,'SUCURSAL SANTOS DUMONT','PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(377,234,'SUCURSAL MONTERO','SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Montero',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(378,233,'SUCURSAL VIRGEN DE COTOCA','PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(379,232,'PLANTA INDUSTRIAL LA PAZ','SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA','Bolivia','La Paz','El Alto',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(380,234,'PLANTA INDUSTRIAL SANTA CRUZ','SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(381,232,'PLANTA INDUSTRIAL SANTA CRUZ','SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(382,232,'SUCURSAL MONTERO','SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Montero',0.50,'ruben.chalco','2021-05-13 15:20:24',NULL),(383,232,'SUCURSAL DOBLE VÍA','SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.50,'ruben.chalco','2021-05-13 15:20:24',NULL),(384,232,'SUCURSAL SANTOS DUMONT','PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.50,'ruben.chalco','2021-05-13 15:20:24',NULL),(385,232,'SUCURSAL VIRGEN DE COTOCA','PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.50,'ruben.chalco','2021-05-13 15:20:24',NULL),(386,232,'OFICINA CENTRAL','OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',1.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(387,231,'PLANTA INDUSTRIAL LA PAZ','SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA','Bolivia','La Paz','El Alto',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(388,231,'PLANTA INDUSTRIAL SANTA CRUZ','SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(389,231,'SUCURSAL MONTERO','SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Montero',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(390,231,'SUCURSAL DOBLE VÍA','SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(391,231,'SUCURSAL SANTOS DUMONT','PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(392,231,'SUCURSAL VIRGEN DE COTOCA','PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(393,231,'OFICINA CENTRAL','OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(394,233,'OFICINA CENTRAL','OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(395,234,'PLANTA INDUSTRIAL LA PAZ','SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA','Bolivia','La Paz','El Alto',0.00,'ruben.chalco','2021-05-13 15:20:24',NULL),(396,235,'AGENCIA DESPACHANTE DE ADUANA MERSUR S.R.L.','SISTEMA DE GESTIÓN DE LA CALIDAD','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-17 14:07:17',NULL),(397,236,'AGENCIA DESPACHANTE DE ADUANA MERSUR S.R.L.','SISTEMA DE GESTIÓN DE LA CALIDAD','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-17 14:07:17',NULL),(398,237,'AGENCIA DESPACHANTE DE ADUANA MERSUR S.R.L.','SISTEMA DE GESTIÓN DE LA CALIDAD','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-17 14:07:17',NULL),(399,238,'AGENCIA DESPACHANTE DE ADUANA MERSUR S.R.L.','SISTEMA DE GESTIÓN DE LA CALIDAD','Bolivia','Santa Cruz','Santa Cruz',0.00,'ruben.chalco','2021-05-17 14:07:17',NULL);
/*!40000 ALTER TABLE `pradireccionespasistema` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `praprogramasdeauditorium`
--

DROP TABLE IF EXISTS `praprogramasdeauditorium`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `praprogramasdeauditorium` (
  `idPrAProgramaAuditoria` bigint NOT NULL AUTO_INCREMENT,
  `idparamArea` int DEFAULT NULL,
  `Estado` varchar(100) DEFAULT NULL,
  `NIT` varchar(10) DEFAULT NULL,
  `Fecha` varchar(20) DEFAULT NULL,
  `Oficina` varchar(50) DEFAULT NULL,
  `IdOrganizacionWS` varchar(20) DEFAULT NULL,
  `OrganizacionContentWS` json DEFAULT NULL,
  `CodigoServicioWS` varchar(50) DEFAULT NULL,
  `DetalleServicioWS` json DEFAULT NULL,
  `idparamTipoServicio` int DEFAULT NULL,
  `CodigoIAFWS` varchar(60) DEFAULT NULL,
  `NumeroAnios` int DEFAULT NULL,
  `OrganismoCertificador` varchar(200) DEFAULT NULL,
  `UsuarioRegistro` varchar(50) DEFAULT NULL,
  `FechaDesde` datetime DEFAULT NULL,
  `FechaHasta` datetime DEFAULT NULL,
  PRIMARY KEY (`idPrAProgramaAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `praprogramasdeauditorium`
--

LOCK TABLES `praprogramasdeauditorium` WRITE;
/*!40000 ALTER TABLE `praprogramasdeauditorium` DISABLE KEYS */;
INSERT INTO `praprogramasdeauditorium` VALUES (68,39,'INICIAL','0','2020-08-11','Of. La Paz','2140','{\"NIT\": \"0\", \"Web\": null, \"Pais\": null, \"Ciudad\": null, \"Correo\": null, \"IdPais\": null, \"Factura\": \"0\", \"IdCiudad\": null, \"Telefono\": null, \"Vigencia\": \"1\", \"Direccion\": \"Av. Néstor Gambetta 6429.  Callao Perú. Telf.\", \"EsCliente\": \"1\", \"IdCliente\": \"2140\", \"CiudadOtro\": null, \"TipoNalInt\": null, \"NombreRazon\": \"PRODUCTOS DE ACERO CASSADO S.A. PRODAC\", \"TipoCliente\": \"E\", \"Departamento\": null, \"FacturaRazon\": null, \"IdDepartamento\": null, \"Identificacion\": \"0\"}','RLP-TCP-CSP-00126','{\"area\": \"TCP\", \"Codigo\": \"RLP-TCP-CSP-00126\", \"IdArea\": \"39\", \"oficina\": \"Of. La Paz\", \"IdCliente\": \"2140\", \"IdOficina\": \"5\", \"IdServicio\": \"13678\", \"idPropuesta\": \"10\", \"responsable\": \"Cintya Marcela Zarate Cazas\", \"ListaProducto\": [{\"pais\": \"Peru\", \"marca\": \"PRODAC\", \"norma\": \"NB 710:2015\", \"ciudad\": null, \"codigo\": \"1039\", \"estado\": \"Lima y Callao\", \"nombre\": \"REDES DE ACERO DE MALLA HEXAGONAL DE DOBLE TORSIÓN (MALLAS 6X8; 8X10 Y 10X12):  A)\\tGAVIONES TIPO CAJA, GAVIONES TIPO COLCHONETA, GAVIONES TIPO SACO Y MALLAS EN ROLLOS; CLASE 1 ALAMBRE CON REVESTIMIENTO METÁLICO DE ZINC PURO.  B)\\tGAVIONES TIPO CAJA, GAVION\", \"cod_pais\": \"172\", \"direccion\": \"AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.\", \"nro_sello\": \"1\", \"cod_ciudad\": \"0\", \"cod_estado\": \"2825\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"1\", \"cod_simulacionservicio\": \"10\"}, {\"pais\": \"Peru\", \"marca\": \"PRODAC\", \"norma\": \",ASTM A974-97:2016\", \"ciudad\": null, \"codigo\": \"1040\", \"estado\": \"Lima y Callao\", \"nombre\": \"•\\tGAVIONES DE MALLA ELECTROSOLDADA CON ABERTURA 3X3 PULGADAS TIPOS 1 Y 3:  A)\\tFABRICADO CON ALAMBRE DE ACERO AL CARBONO REVESTIDO DE ZINC (GALVANIZADO), DIÁMETRO NOMINAL 3,05 MM.  B)\\tFABRICADO CON ALAMBRE DE ACERO AL CARBONO REVESTIDO DE ALEACIÓN DE ZINC \", \"cod_pais\": \"172\", \"direccion\": \"AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.\", \"nro_sello\": \"2\", \"cod_ciudad\": \"0\", \"cod_estado\": \"2825\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"1\", \"cod_simulacionservicio\": \"10\"}, {\"pais\": \"Peru\", \"marca\": \"PRODAC\", \"norma\": \",EN 10223-3:2013\", \"ciudad\": null, \"codigo\": \"1041\", \"estado\": \"Lima y Callao\", \"nombre\": \"•\\tMALLA HEXAGONAL DE ACERO PARA APLICACIONES EN INGENIERÍA CIVIL (6X8, 8X10 Y 10X12):  A) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS CON ALAMBRE DE ACERO CON REVESTIMIENTO METÁLICO DE ALEACIÓN ZN90%/AL10%; B) GAVIONES, COLCHONES, GAVIONES SACO Y ROLLOS C\", \"cod_pais\": \"172\", \"direccion\": \"AV. NÉSTOR GAMBETTA 6429, CALLAO, LIMA - PERÚ.\", \"nro_sello\": \"3\", \"cod_ciudad\": \"0\", \"cod_estado\": \"2825\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"1\", \"cod_simulacionservicio\": \"10\"}], \"fecharegistro\": \"2020-08-11\", \"ListaDireccion\": [], \"cod_responsable\": \"177\", \"cod_iaf_primario\": \"772\", \"alcance_propuesta\": \"\", \"cod_iaf_secundario\": \"0\", \"iaf_primario_codigo\": \"17\", \"descripcion_servicio\": \"RENOVACIÓN DE LA CERTIFICACIÓN PRODUCTO CON SELLO IBNORCA\", \"ListaProductoCertificado\": [{\"Norma\": \"NB 710:2015\", \"marca\": \"GAVIONES/COLCHONES PRODAC\", \"nombre\": \"Redes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \\r\\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \\r\\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \\r\\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\", \"direccion\": \"Av. Néstor Gambetta 6429, Callao, Lima - Perú\", \"FechaValido\": \"2023-12-12\", \"FechaEmision\": \"2020-12-16\", \"tipoCertificado\": \"CSP\", \"ProductoServicio\": \"Al Producto:\\r\\nRedes de acero de malla hexagonal de doble torsión (Mallas 6x8, 8x10 y 10x12): \\r\\na) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 1 Alambre con revestimiento metálico de Zinc puro; \\r\\nb) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 2 Alambre con revestimiento metálico de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM); \\r\\nc) Gaviones tipo caja, gaviones tipo colchoneta, gaviones tipo saco y mallas en rollos; Clase 3 Alambre con revestimiento metálico de Zinc puro o de aleación de Zinc - 5 % Aluminio - Mischmetal (Zn-5Al-MM), adicionalmente recubiertos con una capa de PVC.\\r\\n|\\r\\nMarca Comercial:\\r\\nGAVIONES/COLCHONES PRODAC \\r\\n|\\r\\nLugar de Fabricación:\\r\\nAv. Néstor Gambetta 6429, Callao, Lima - Perú \", \"IdCertificadoServicios\": \"11289\", \"sub_partida_arancelaria\": \"\"}, {\"Norma\": \"ASTM A974-97:2016\", \"marca\": \"GAVIONES/COLCHONES PRODAC\", \"nombre\": \"Gaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\\r\\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\\r\\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\", \"direccion\": \"Av. Néstor Gambetta 6429, Callao, Lima - Perú\", \"FechaValido\": \"2023-12-12\", \"FechaEmision\": \"2020-12-16\", \"tipoCertificado\": \"CSP\", \"ProductoServicio\": \"Al Producto:\\r\\nGaviones de malla electrosoldada con abertura 3x3 pulgadas tipos 1y3:\\r\\na) Fabricado con alambre de acero al carbono revestido de Zinc (galvanizado), diámetro nominal 3,05 mm.\\r\\nb) Fabricado con alambre de acero al carbono revestido de aleación de Zinc - 5% Aluminio - Mischmetal (Zn-5Al-MM), diámetro nominal 3,05 mm.\\r\\n|\\r\\nMarca Comercial:\\r\\nGAVIONES/COLCHONES PRODAC\\r\\n|\\r\\nLugar de Fabricación:\\r\\nAv. Néstor Gambetta 6429, Callao, Lima - Perú \", \"IdCertificadoServicios\": \"11579\", \"sub_partida_arancelaria\": \"\"}], \"iaf_primario_descripcion\": \"Primera transformacion de metales y productos metálicos\", \"ListaDireccionCertificado\": []}',1,'17 - Primera transformacion de metales y productos metálicos',0,'IBNORCA','ruben.chalco','2021-05-13 13:32:11',NULL),(69,39,'INICIAL','1028443027','2021-04-06','Of. Santa Cruz','2118','{\"NIT\": \"1028443027\", \"Web\": null, \"Pais\": \"Bolivia\", \"Ciudad\": \"La Paz\", \"Correo\": \"laslomasscz@laslomas.com.bo\", \"IdPais\": \"26\", \"Factura\": \"0\", \"IdCiudad\": \"62\", \"Telefono\": null, \"Vigencia\": \"1\", \"Direccion\": \"Casa Matriz: Av. Banzer entre 3er anillo externo y 4to anillo (Santa Cruz)\", \"EsCliente\": \"1\", \"IdCliente\": \"2118\", \"CiudadOtro\": null, \"TipoNalInt\": null, \"NombreRazon\": \"Import Export Las Lomas Ltda.\", \"TipoCliente\": \"E\", \"Departamento\": \"Santa Cruz\", \"FacturaRazon\": null, \"IdDepartamento\": \"484\", \"Identificacion\": \"1028443027\"}','RSC-TCP-CSP-00080','{\"area\": \"TCP\", \"Codigo\": \"RSC-TCP-CSP-00080\", \"IdArea\": \"39\", \"oficina\": \"Of. Santa Cruz\", \"IdCliente\": \"2118\", \"IdOficina\": \"10\", \"IdServicio\": \"15107\", \"idPropuesta\": \"209\", \"responsable\": \"Cintya Marcela Zarate Cazas\", \"ListaProducto\": [{\"pais\": \"Bolivia\", \"marca\": \"LAS LOMAS\", \"norma\": \"NB 734:2017\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"786\", \"estado\": \"Santa Cruz\", \"nombre\": \"MALLAS ELECTROSOLDADAS DE ACERO PARA HORMIGÓN ARMADO: TIPO CUADRADAS CON BARRAS DE DIÁMETROS 4,2 MM Y ESPACIAMIENTO DE 100 MM, 150 MM, 200 MM Y 250 MM; 6MM Y ESPACIAMIENTO DE 100 MM, 150 MM, 200 MM Y 250 MM; 8 MM Y ESPACIAMIENTO DE 150 MM Y 200 MM\", \"cod_pais\": \"26\", \"direccion\": \"CARRETERA A COTOCA KM 15, SANTA CRUZ - BOLIVIA.\", \"nro_sello\": \"1\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"1\", \"cod_simulacionservicio\": \"209\"}], \"fecharegistro\": \"2021-04-06\", \"ListaDireccion\": [], \"cod_responsable\": \"177\", \"cod_iaf_primario\": \"772\", \"alcance_propuesta\": \"\", \"cod_iaf_secundario\": \"0\", \"iaf_primario_codigo\": \"17\", \"descripcion_servicio\": \"RENOVACIÓN DE LA CERTIFICACIÓN DE SELLO PRODUCTO\", \"ListaProductoCertificado\": [], \"iaf_primario_descripcion\": \"Primera transformacion de metales y productos metálicos\", \"ListaDireccionCertificado\": []}',1,'17 - Primera transformacion de metales y productos metálicos',0,NULL,'ruben.chalco','2021-05-13 15:17:22',NULL),(70,38,'INICIAL','1028443027','2020-09-02','Of. Santa Cruz','2118','{\"NIT\": \"1028443027\", \"Web\": null, \"Pais\": \"Bolivia\", \"Ciudad\": \"La Paz\", \"Correo\": \"laslomasscz@laslomas.com.bo\", \"IdPais\": \"26\", \"Factura\": \"0\", \"IdCiudad\": \"62\", \"Telefono\": null, \"Vigencia\": \"1\", \"Direccion\": \"Casa Matriz: Av. Banzer entre 3er anillo externo y 4to anillo (Santa Cruz)\", \"EsCliente\": \"1\", \"IdCliente\": \"2118\", \"CiudadOtro\": null, \"TipoNalInt\": null, \"NombreRazon\": \"Import Export Las Lomas Ltda.\", \"TipoCliente\": \"E\", \"Departamento\": \"Santa Cruz\", \"FacturaRazon\": null, \"IdDepartamento\": \"484\", \"Identificacion\": \"1028443027\"}','RSC-TCS-SGC-00238','{\"area\": \"TCS\", \"Codigo\": \"RSC-TCS-SGC-00238\", \"IdArea\": \"38\", \"oficina\": \"Of. Santa Cruz\", \"IdCliente\": \"2118\", \"IdOficina\": \"10\", \"IdServicio\": \"13835\", \"idPropuesta\": \"37\", \"responsable\": \"Cintya Marcela Zarate Cazas\", \"ListaProducto\": [], \"fecharegistro\": \"2020-09-02\", \"ListaDireccion\": [{\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"122\", \"estado\": \"Santa Cruz\", \"nombre\": \"OFICINA CENTRAL\", \"cod_pais\": \"26\", \"direccion\": \"AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"123\", \"estado\": \"Santa Cruz\", \"nombre\": \"SUCURSAL VIRGEN DE COTOCA\", \"cod_pais\": \"26\", \"direccion\": \"AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"124\", \"estado\": \"Santa Cruz\", \"nombre\": \"SUCURSAL SANTOS DUMONT\", \"cod_pais\": \"26\", \"direccion\": \"AV. SANTOS DUMONT, 6TO ANILLO\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"125\", \"estado\": \"Santa Cruz\", \"nombre\": \"SUCURSAL DOBLE VÍA\", \"cod_pais\": \"26\", \"direccion\": \"AV. DOBLE VÍA, 6TO ANILLO\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Montero\", \"codigo\": \"126\", \"estado\": \"Santa Cruz\", \"nombre\": \"SUCURSAL MONTERO\", \"cod_pais\": \"26\", \"direccion\": \"AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ\", \"nro_sello\": \"0\", \"cod_ciudad\": \"48390\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"127\", \"estado\": \"Santa Cruz\", \"nombre\": \"PLANTA INDUSTRIAL SANTA CRUZ\", \"cod_pais\": \"26\", \"direccion\": \"CARRETERA A COTOCA KM 15\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}, {\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"El Alto\", \"codigo\": \"128\", \"estado\": \"La Paz\", \"nombre\": \"PLANTA INDUSTRIAL LA PAZ\", \"cod_pais\": \"26\", \"direccion\": \"AV. PANORÁMICA N°80, ZONA ROSAS PAMPA\", \"nro_sello\": \"0\", \"cod_ciudad\": \"72\", \"cod_estado\": \"480\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"37\"}], \"cod_responsable\": \"177\", \"cod_iaf_primario\": \"0\", \"alcance_propuesta\": \"\\\"Comercialización y despacho de materiales de acero para la construcción civil; Servicio de corte, doblado y despacho de barras de acero para la construcción; Proceso de producción de mallas electrosoldadas; Proceso de producción de perfiles\\\".\", \"cod_iaf_secundario\": \"0\", \"iaf_primario_codigo\": null, \"descripcion_servicio\": \"SISTEMAS DE GESTIÓN DE LA CALIDAD NB/ISO 9001 - RENOVACIÓN (2020 - 2022)\", \"ListaProductoCertificado\": [], \"iaf_primario_descripcion\": null, \"ListaDireccionCertificado\": [{\"Norma\": \"NB/ISO 9001:2015 & ISO 9001:2015\", \"FechaValido\": \"2023-06-18\", \"direcciones\": [\"OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA\", \"PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA\", \"PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA\", \"SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA\", \"SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA\", \"SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA\", \"SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA\"], \"FechaEmision\": \"2020-09-30\", \"tipoCertificado\": \"SGC\", \"ProductoServicio\": \"OFICINA CENTRAL: AV. BANZER ENTRE 3ER ANILLO EXTERNO Y 4TO ANILLO, SANTA CRUZ - BOLIVIA|PLANTA INDUSTRIAL:  CARRETERA A COTOCA km 15, SANTA CRUZ - BOLIVIA|PLANTA INDUSTRIAL: AV. PANORÁMICA N°80, ZONA ROSAS PAMPA, EL ALTO, LA PAZ - BOLIVIA|SUCURSAL: AV. VIRGEN DE COTOCA ENTRE 5TO ANILLO Y 6TO ANILLO, SANTA CRUZ - BOLIVIA|SUCURSAL: AV. SANTOS DUMONT, 6TO ANILLO, SANTA CRUZ - BOLIVIA|SUCURSAL: AV. DOBLE VÍA, 6TO ANILLO, SANTA CRUZ - BOLIVIA|SUCURSAL: AV. VÍCTOR PAZ ESTENSORO ESQ. PAICHANÉ, MONTERO, SANTA CRUZ - BOLIVIA\", \"IdCertificadoServicios\": \"10546\"}]}',1,' - ',0,NULL,'ruben.chalco','2021-05-13 15:20:24',NULL),(71,38,'INICIAL','157954024','2021-04-26','Of. Santa Cruz','2011','{\"NIT\": \"157954024\", \"Web\": null, \"Pais\": null, \"Ciudad\": \"Santa Cruz\", \"Correo\": null, \"IdPais\": null, \"Factura\": \"0\", \"IdCiudad\": \"65\", \"Telefono\": null, \"Vigencia\": \"1\", \"Direccion\": \"Av. Alemania entre 2do y 3er anillo , Calle Taropes esquina Cupechicha.\", \"EsCliente\": \"1\", \"IdCliente\": \"2011\", \"CiudadOtro\": null, \"TipoNalInt\": null, \"NombreRazon\": \"AGENCIA DESPACHANTE DE ADUANA MERSUR  S.R.L\", \"TipoCliente\": \"E\", \"Departamento\": null, \"FacturaRazon\": null, \"IdDepartamento\": null, \"Identificacion\": \"157954024\"}','RSC-TCS-SGC-00259','{\"area\": \"TCS\", \"Codigo\": \"RSC-TCS-SGC-00259\", \"IdArea\": \"38\", \"oficina\": \"Of. Santa Cruz\", \"IdCliente\": \"2011\", \"IdOficina\": \"10\", \"IdServicio\": \"15220\", \"idPropuesta\": \"248\", \"responsable\": \"Gabriela Muñoz Acosta\", \"ListaProducto\": [], \"fecharegistro\": \"2021-04-26\", \"ListaDireccion\": [{\"pais\": \"Bolivia\", \"marca\": \"\", \"norma\": \"\", \"ciudad\": \"Santa Cruz\", \"codigo\": \"929\", \"estado\": \"Santa Cruz\", \"nombre\": \"AGENCIA DESPACHANTE DE ADUANA MERSUR S.R.L.\", \"cod_pais\": \"26\", \"direccion\": \"AV. ALEMANIA, ENTRE 2DO Y 3ER ANILLO, CALLE LOS TAROPES, ESQ. CUPECHICHO, S/#, SANTA CRUZ -  BOLIVIA.\", \"nro_sello\": \"0\", \"cod_ciudad\": \"65\", \"cod_estado\": \"484\", \"habilitado\": \"1\", \"cod_tipoatributo\": \"2\", \"cod_simulacionservicio\": \"248\"}], \"cod_responsable\": \"54\", \"cod_iaf_primario\": \"790\", \"alcance_propuesta\": \"\\\"Servicio de despacho aduanero de Importación y Exportación.\\\"\", \"cod_iaf_secundario\": \"0\", \"iaf_primario_codigo\": \"35\", \"descripcion_servicio\": \"SISTEMAS DE GESTIÓN DE LA CALIDAD NB/ISO 9001:2015\", \"ListaProductoCertificado\": [], \"iaf_primario_descripcion\": \"Otros Servicios\", \"ListaDireccionCertificado\": []}',1,'35 - Otros Servicios',0,NULL,'ruben.chalco','2021-05-17 14:07:17',NULL);
/*!40000 ALTER TABLE `praprogramasdeauditorium` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tmddocumentacionauditoria`
--

DROP TABLE IF EXISTS `tmddocumentacionauditoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tmddocumentacionauditoria` (
  `idTmdDocumentacionAuditoria` int NOT NULL,
  `idElaAuditoria` int DEFAULT NULL,
  `idparamDocumentos` int DEFAULT NULL,
  `Gestion` int DEFAULT NULL,
  `correlativoDocumento` int DEFAULT NULL,
  `citeDocumento` varchar(200) DEFAULT NULL,
  `tmdDocumentoAuditoria` varchar(3200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `fechaDeRegistro` datetime DEFAULT NULL,
  `usuario` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`idTmdDocumentacionAuditoria`),
  KEY `fk_documento_auditoria_idx` (`idElaAuditoria`),
  CONSTRAINT `fk_documento_auditoria` FOREIGN KEY (`idElaAuditoria`) REFERENCES `elaauditoria` (`idelaAuditoria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tmddocumentacionauditoria`
--

LOCK TABLES `tmddocumentacionauditoria` WRITE;
/*!40000 ALTER TABLE `tmddocumentacionauditoria` DISABLE KEYS */;
/*!40000 ALTER TABLE `tmddocumentacionauditoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'sigad'
--
/*!50003 DROP PROCEDURE IF EXISTS `GetIdServicioByIdCiclo` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetIdServicioByIdCiclo`(in vIdCico int)
BEGIN
	select JSON_EXTRACT(DetalleServicioWS, "$.IdServicio") IdServicio
	from sigad.praciclosprogauditorium ciclo 
	inner join sigad.praprogramasdeauditorium programa on programa.idPrAProgramaAuditoria = ciclo.idPrAProgramaAuditoria
	where ciclo.idPrACicloProgAuditoria = vIdCico;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetPraCicParticipantes` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `GetPraCicParticipantes`(

	IN praCiclo int8

)
BEGIN

	SELECT 	JSON_EXTRACT(cicPra.ParticipanteDetalle_ws, '$.cargoPuesto') 'cargoPuesto',JSON_EXTRACT(cicPra.ParticipanteDetalle_ws, '$.nombreCompleto') 'Nombre' , 

			JSON_EXTRACT(cicPra.ParticipanteDetalle_ws, '$.correo') AS Correo,JSON_EXTRACT(cicPra.CargoDetalleWs, '$.idCargoPuesto') 'idCargoPuesto'

	  FROM PrAcicloParticipantes cicPra

	 WHERE cicPra.idPrACicloProgAuditoria=praCiclo

	 ORDER BY idCargoPuesto DESC;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetPraDesignacion` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `GetPraDesignacion`(

	IN praCiclo int8

)
BEGIN

	SELECT  'FechaDeAuditoria', cicTip.TipoAuditoria 'TipoAuditoria', 'Modalildad',cicCro.FechaInicioDeEjecucionDeAuditoria 'FechaInicioEjecucion',

			cicCro.FechaDeFinDeEjecucionAuditoria 'FechaFinAuditoria',cicCro.CantidadDeDiasTotal 'CantidadDeDiasAuditor',

			'IBNORCA' AS 'OrganismoCertificador',pra.CodigoServicioWS 'CodigoServicioIbnorca',pra.IdOrganizacionWS 'IdCliente'

	  FROM praprogramasdeauditorium pra

	 INNER JOIN praciclosprogauditorium praCic ON praCic.idPrAProgramaAuditoria =pra.idPrAProgramaAuditoria

	 INNER JOIN paramTipoAuditoria cicTip ON cicTip.idparamTipoAuditoria =praCic.idparamTipoAuditoria

	 INNER JOIN PrACicloCronogramas cicCro ON cicCro.idPrACicloProgAuditoria = praCic.idPrACicloProgAuditoria

	 WHERE praCic.idPrACicloProgAuditoria=praCiclo;

	END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetResumenCicloPorAuditor` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `GetResumenCicloPorAuditor`(pIdAuditor bigint)
BEGIN

select ciclo.idPrACicloProgAuditoria IdCicloAuditoria, 

	   programa.CodigoServicioWS CodigoServicio,

       ciclo.NombreOrganizacionCertificado NombreCliente,

       ciclo.Referencia ReferenciaCiclo,

       ciclo.UsuarioRegistro Responsable,

       DATE_FORMAT(cronograma.FechaInicioDeEjecucionDeAuditoria, '%d/%m/%Y')   FechaAuditoria,

       ciclo.EstadoDescripcion

from pracicloparticipantes participante

inner join praciclosprogauditorium ciclo on ciclo.idPrACicloProgAuditoria = participante.idPrACicloProgAuditoria

inner join praprogramasdeauditorium programa on programa.idPrAProgramaAuditoria = ciclo.idPrAProgramaAuditoria

inner join praciclocronogramas cronograma on cronograma.idPrACicloProgAuditoria = ciclo.idPrACicloProgAuditoria
where  ciclo.Anio <> 4;

/*where 

idParticipante_ws = pIdAuditor and 

(idparamEstadosProgAuditoria = 2 or idparamEstadosProgAuditoria = 3);*/

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `insert_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `insert_person`(in pname varchar(250), in plastname varchar(250))
BEGIN

insert into person values(0, pname, plastname);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SearchPerson` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `SearchPerson`(in pname varchar(250))
BEGIN

select *, concat(p.name, " ", p.lastname) FullName 

from person p

where concat(p.name, " ", p.lastname) like concat("%",pname,"%");

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spGetCargosParticipante` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spGetCargosParticipante`(in idTipoCert int2)
BEGIN

	select *

	from paramcargosparticipantes p

    where p.idptipoCertificacion= idTipoCert;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spGetCilcloByIdServicioAnio` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spGetCilcloByIdServicioAnio`(in IdServicio  varchar(50), in pAnio int)
BEGIN

select ciclo.*
from praprogramasdeauditorium prog
inner join praciclosprogauditorium  ciclo on prog.idPraProgramaAuditoria = ciclo.idPraProgramaAuditoria
where JSON_EXTRACT(prog.DetalleServicioWS, "$.IdServicio") = IdServicio
and anio = pAnio;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spGetDepartamentos` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spGetDepartamentos`(in idpPais int2)
BEGIN



	select d.*,d.idpDepartamento,d.Departamento 

	  from pDepartamentos d

	 where d.idpPais=idpPais;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spGetProgramaAuditoriaByIdServicio` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spGetProgramaAuditoriaByIdServicio`(in IdServicio  varchar(50))
BEGIN

SELECT *

	FROM  praprogramasdeauditorium programa

	where JSON_EXTRACT(DetalleServicioWS, "$.IdServicio") = IdServicio;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spGetTipoAuditoria` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spGetTipoAuditoria`(in idTipoCert int2)
BEGIN

	select  ta.idpTipoAuditoria,ta.tipoAuditoria 

  from pTipoAuditoria ta

 where ta.idpTipoCertificacion=idTipoCert;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spTmdConsecutivoDocAudi` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`usr_sigad`@`%` PROCEDURE `spTmdConsecutivoDocAudi`(in _idElaAuditoria INT8,_gestion int4,_idparamDocumentos int4)
BEGIN

				SELECT count(1) +1 AS 'ConsecutivoDocAudi'

				  FROM TmdDocumentacionAuditoria

				 WHERE idElaAuditoria=_idElaAuditoria

                   AND Gestion=_gestion

				   AND idparamDocumentos=_idparamDocumentos;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spWSGetDetalleProgramaTCP` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `spWSGetDetalleProgramaTCP`(
        IN `pIdProducto` INTEGER
)
BEGIN
	select prog.idPrAProgramaAuditoria, 
		prog.CodigoServicioWS,  
		ciclo.idPrACicloProgAuditoria idCiclo,
		replace(JSON_EXTRACT(prog.OrganizacionContentWS, "$.NombreRazon"), '"', '') Cliente,
		replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.IdServicio"), '"', '') IdServicio,
		ciclo.idPrACicloProgAuditoria,
		ciclo.Referencia,    
		ciclo.IdEtapaDocumento,
		replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.area"), '"', '') area,
		prod.idDireccionPAProducto idProducto,
		prod.Nombre nombre,
		prod.Marca marca,
		prod.Norma norma,
        prod.Direccion direccion,
		prod.NumeroDeCertificacion numeroDeCertificacion
	from praprogramasdeauditorium prog
	inner join praciclosprogauditorium  ciclo on prog.idPraProgramaAuditoria = ciclo.idPraProgramaAuditoria
	inner join pradireccionespaproducto prod on prod.idPrACicloProgAuditoria = ciclo.idPrACicloProgAuditoria
	where JSON_EXTRACT(prog.DetalleServicioWS, "$.area") = 'TCP' 
		and (prod.idDireccionPAProducto = pIdProducto or pIdProducto = 0);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spWSGetDetalleProgramaTCS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `spWSGetDetalleProgramaTCS`(
        IN `pIdSistema` INTEGER
)
BEGIN
	select prog.idPrAProgramaAuditoria, 
		prog.CodigoServicioWS,  
		ciclo.idPrACicloProgAuditoria idCiclo,
		replace(JSON_EXTRACT(prog.OrganizacionContentWS, "$.NombreRazon"), '"', '') Cliente,
		replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.IdServicio"), '"', '') IdServicio,
		ciclo.idPrACicloProgAuditoria,
		ciclo.Referencia,    
		ciclo.IdEtapaDocumento,
		replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.area"), '"', '') area,
		sistema.idCicloNormaSistema idSistema,
		sistema.Norma norma,
		sistema.Alcance alcance,
		sistema.NumeroDeCertificacion numeroDeCertificacion
	from praprogramasdeauditorium prog
	inner join praciclosprogauditorium  ciclo on prog.idPraProgramaAuditoria = ciclo.idPraProgramaAuditoria
	inner join praciclonormassistema sistema on sistema.idPrACicloProgAuditoria = ciclo.idPrACicloProgAuditoria
	where JSON_EXTRACT(prog.DetalleServicioWS, "$.area") = 'TCS'
		and (sistema.idCicloNormaSistema = pIdSistema or pIdSistema = 0);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spWSGetResumePrograma` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `spWSGetResumePrograma`(
        IN `parea` VARCHAR(50),
        IN `pIdCiclo` INTEGER
    )
BEGIN
select prog.idPrAProgramaAuditoria, 
	prog.CodigoServicioWS,  
    ciclo.idPrACicloProgAuditoria idCiclo,
    replace(JSON_EXTRACT(prog.OrganizacionContentWS, "$.NombreRazon"), '"', '') Cliente,
    replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.IdServicio"), '"', '') IdServicio,
    ciclo.idPrACicloProgAuditoria,
    ciclo.Referencia,    
    ciclo.IdEtapaDocumento,
    replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.area"), '"', '') area
from praprogramasdeauditorium prog
inner join praciclosprogauditorium  ciclo on prog.idPraProgramaAuditoria = ciclo.idPraProgramaAuditoria
where JSON_EXTRACT(prog.DetalleServicioWS, "$.area") = parea
and  (pIdCiclo = 0 or ciclo.idPrACicloProgAuditoria = pIdCiclo);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `spWSGetResumeProgramaProducto` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `spWSGetResumeProgramaProducto`()
BEGIN
select prog.idPrAProgramaAuditoria, 
	prog.CodigoServicioWS,  
    replace(JSON_EXTRACT(prog.OrganizacionContentWS, "$.NombreRazon"), '"', '') Cliente,
    replace(JSON_EXTRACT(prog.DetalleServicioWS, "$.IdServicio"), '"', '') IdServicio,
    ciclo.idPrACicloProgAuditoria,
    ciclo.Referencia,
    prod.Norma,
    prod.Nombre,
    prod.Marca
from praprogramasdeauditorium prog
inner join praciclosprogauditorium  ciclo on prog.idPraProgramaAuditoria = ciclo.idPraProgramaAuditoria
inner join pradireccionespaproducto prod on prod.idPrACicloProgAuditoria = ciclo.idPrACicloProgAuditoria;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-02-01 22:24:49
