using Lyntia.TestSet.Conditions;
using Lyntia.Utilities;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using Lyntia.TestSet.Actions;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Xml;

namespace Lyntia.TestSet
{

    [TestFixture]
    [AllureNUnit]
    [AllureSuite("OFERTA")]
    public class Oferta
    {

        readonly Utils utils = new Utils();

        private static IWebDriver driver;
        private static OfertaActions ofertaActions;
        private static OfertaConditions ofertaCondition;
        private static ProductoActions productoActions;
        private static ProductoConditions productoConditions;
        private static CommonActions commonActions;
        private static CommonConditions commonCondition;
        private static WebDriverWait wait;
        private static string fechaHoraActualFormateada = "";



        [SetUp]
        public void Instanciador()
        {
            // Instanciador del driver
            utils.Instanciador();

            driver = Utils.driver;
            ofertaActions = Utils.GetOfertaActions();
            ofertaCondition = Utils.GetOfertaConditions();
            productoActions = Utils.GetProductoActions();
            productoConditions = Utils.GetProductoConditions();
            commonActions = Utils.GetCommonActions();
            commonCondition = Utils.GetCommonConditions();
            fechaHoraActualFormateada = Utils.getFechaHoraActualFormateada();

            // Realizar login
            commonActions.Login();
        }

        [OneTimeSetUp]
        public void BorradoScreenshots()
        {
            utils.BorradoScreenshots();
        }

        [TearDown]
        public void Cierre()
        {
            driver.Quit();

        }

        public static string getFechaHoraActual()
        {
            return fechaHoraActualFormateada;
        }

        [Test(Description = "CRM-COF0001 Acceso a Ofertas")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0001_accesoOfertas()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas 
            ofertaActions.acceder_A_OfertasLyntia();
            //TODO Realizar comprobación
        }

        [Test(Description = "CRM-COF0002 Consultar Oferta")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0002_consultaOferta()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2A - Comprobar si hay alguna Oferta para abrir
            ofertaActions.AbrirOferta();

            TestContext.WriteLine("LA PRUEBA CRM-COF0002 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0003 Creación de Oferta")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0003_creacionOferta()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            //Creacion de nueva Oferta con estado borrador
            ofertaActions.crearOfertaBorrador(Utils.getDatosTestCase("oferta"));

            TestContext.WriteLine("LA PRUEBA CRM-COF0003 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0004 Creación de Oferta sin informar campos obligatorios")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0004_creacionOfertaSinCamposObligatorios()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Guardar sin campos obligatorios
            ofertaActions.GuardarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            // Paso 4 - Repetir el paso anterior pero pulsando Guardar y Cerrar
            ofertaActions.GuardarYCerrarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            // Paso 7 - Guardar informando solo el Cliente
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta1"));
            ofertaActions.GuardarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            // Paso 8 - Guardar y cerrar informando solo el Cliente
            ofertaActions.GuardarYCerrarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            driver.Navigate().Refresh();
            driver.SwitchTo().Alert().Accept();

            // Paso 5 - Guardar informando solo el Nombre
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta2"));
            ofertaActions.GuardarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            // Paso 6 - Guardar y cerrar informando solo el Nombre
            ofertaActions.GuardarYCerrarOferta();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            TestContext.WriteLine("LA PRUEBA CRM-COF0004 SE EJECUTÓ CORRECTAMENTE");
            Thread.Sleep(30000);
        }

        [Test(Description = "CRM-COF0005 Creación de Oferta de tipo Nuevo servicio")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0005_creacionOfertaNuevoServicio()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            driver.Navigate().Refresh();

            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 4 - Crear Nueva Oferta, pulsando Guardar y cerrar
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 5 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0005 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0006 Creación de Oferta de tipo Cambio de capacidad")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0006_creacionOfertaCambioCapacidad()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            driver.Navigate().Refresh();

            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 4 - Crear Nueva Oferta, pulsando Guardar y cerrar
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 5 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0006 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0007 Creacion Oferta Cambio Precio Renovacion")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0007_creacionOfertaCambioPrecioRenovacion()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            driver.Navigate().Refresh();

            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 4 - Crear Nueva Oferta, pulsando Guardar y cerrar
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 5 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0007 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0008 Creación de Oferta de tipo Cambio de Solución Técnica (Tecnología)")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0008_creacionOfertaCambioSolucionTecnica()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            driver.Navigate().Refresh();

            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 4 - Crear Nueva Oferta, pulsando Guardar y cerrar
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 5 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0008 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0009 Creación de Oferta de tipo Cambio de dirección (Migración)")]
        [AllureSubSuite("PRO CREAR OFERTA")]
        public void CRM_COF0009_creacionOfertaCambioDireccion()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Paso 3 - Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            driver.Navigate().Refresh();

            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 4 - Crear Nueva Oferta, pulsando Guardar y cerrar
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 5 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_OfertaGuardadaCorrectamente(Utils.getDatosTestCase("oferta"));

            ofertaActions.acceder_A_PestañaFechasOferta();
            ofertaCondition.validar_FechasInformadasCorrectamente();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0009 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0005 Eliminar Oferta en borrador con producto añadido")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0005_eliminarOfertaProductoAnadido()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Paso 3 y 4 - Pulsar Eliminar en la barra de herramientas y cancelar la eliminacion
            ofertaActions.eliminar_ofertaDetalle("Cancelar");

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Buscar Oferta creada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));

            // Paso 5 - Eliminar definitivamente
            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0005 ELIMINAR SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0006 Eliminar Oferta en borrador con producto añadido desde el grid")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0006_eliminarOfertaProductoAnadidoDesdeGrid()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 Seleccionar la Oferta del grid
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Seleccionar la Oferta del grid
            ofertaActions.SeleccionarOfertaGrid();

            // Paso 3 - Pulsar Eliminar en la barra de herramientas y cancelar la eliminacion
            ofertaActions.eliminarOfertaActual("Cancelar");

            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 4 - Repetir paso anterior eliminando la oferta
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0006 ELIMINAR SE EJECUTÓ CORRECTAMENTE");
        }

        //[Test(Description = "CRM-COF0007 Cerrar Oferta en borrador con producto añadido")]
        //[AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0007_cerrarOfertaProductoAnadido()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 Seleccionar la Oferta del grid
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Seleccionar la Oferta del grid
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));

            // Paso 3 y 4 - Pulsar Cerrar en la barra de herramientas y cancelar el cierre
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta1"));

            // Paso 5 - Repetir el paso anterior pero cerrando sin completar campos obligatorios
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta2"));
            ofertaCondition.validar_OfertaNoCerrada();

            driver.Navigate().Refresh();

            // Paso 6 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta3"));

            driver.Navigate().Refresh();

            // Acceso a Ofertas y buscar la Oferta Cerrada
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 7 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaCerradaCorrectamenteEnGrid(Utils.getDatosTestCase("cierreOferta3")["razon"]);

            // Eliminar Oferta
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0007 CERRAR SE EJECUTÓ CORRECTAMENTE");
        }

        //[Test(Description = "CRM-COF0008 Cerrar Oferta en borrador con producto añadido, No viable")]
        //[AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0008_cerrarOfertaProductoAnadidoNoViable_casoMOD()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 Seleccionar la Oferta del grid
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Seleccionar la Oferta del grid
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));

            // Paso 6 - Repetir el paso anterior pero cerrando de manera correcta
            // QA nos indica que validemos hasta el paso 6, opcion NO viable ya no se encuentra. Se comenta las opciones afectadas para que salten el paso
            // ofertaActions.CerrarOfertaActual("Aceptar", "No viable", "Sin Información/otro motivo", "01/02/2021");

            driver.Navigate().Refresh();

            // Acceso a Ofertas y buscar la Oferta Cerrada
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 7 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            // QA nos indica que validemos hasta el paso 6, opcion NO viable ya no se encuentra. Se comenta las opciones afectadas para que salten el paso
            // ofertaCondition.OfertaCerradaCorrectamenteEnGrid("No viable");

            // Eliminar Oferta
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0008 CERRAR SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0009 Cerrar Oferta en borrador con producto añadido, Perdida")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0009_cerrarOfertaProductoAnadidoPerdida()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 Seleccionar la Oferta del grid
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Seleccionar la Oferta del grid
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            ofertaActions.presentarOferta();

            // Paso 6 - Cerrar Oferta
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta1"));

            driver.Navigate().Refresh();

            // Acceso a Ofertas y buscar la Oferta Cerrada
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 7 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPerdidaCorrectamenteEnGrid(Utils.getDatosTestCase("cierreOferta1")["razon"]);

            // Eliminar Oferta
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0009 CERRAR SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF00010 Cerrar Oferta Presentada con producto añadido, Revisada")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF00010_cerrarOfertaPresentadaProductoAnadidoRevisada()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado Borrador con producto añadido.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));

            // Volver al grid
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 Seleccionar la Oferta del grid
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Seleccionar la Oferta del grid
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Presentar la Oferta
            ofertaActions.presentarOferta();
            ofertaCondition.validar_OfertaPresentada();

            // Acceso a Ofertas y buscar la Oferta Cerrada
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 7 - Se busca la Oferta, que debe aparecer duplicada
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OFertaRevisadaCorrectamente();

            driver.Navigate().Refresh();
            Thread.Sleep(3000);

            ofertaActions.acceder_A_OfertasLyntia();
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());

            // Eliminar las dos ofertas
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COF0010 CERRAR ADJUDICADA SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM_EOF0003 Editar campos de una Oferta")]
        [AllureSubSuite("PRO EDI OFERTA")]
        public void CRM_EOF0003_Editar_campos_de_una_oferta()
        {
            // Paso 1
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            //Paso 2            
            ofertaActions.crearOfertaBorrador(Utils.getDatosTestCase("oferta"));

            //Paso 3
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            CommonActions.agregarFiltroAvanzado("Estado", "Borrador");
            ofertaCondition.validar_OfertaGuardadaCorrectamenteEnGrid();

            // Paso 4 - Abrir la oferta anterior y comprobar datos cumplimentados
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_Acceso_A_SeleccionOferta(); //accede a la oferta

            // Paso 5
            ofertaActions.IntroduccirDatos(Utils.getDatosTestCase("edicionOferta")); //introduccir campos de la oferta
            ofertaActions.acceder_A_OfertasLyntia();
            commonCondition.validar_AccesoOferta();
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("edicionOferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_IntroduccionDatos(Utils.getDatosTestCase("edicionOferta")["nombreOferta"] + getFechaHoraActual()); //los datos se introduccen correctamente

            // Paso 6 - Reestablecer datos
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            //ofertaCondition.validar_EdicionCamposOferta(Utils.getDatosTestCase("edicionOferta"));


            TestContext.WriteLine("LA PRUEBA CRM-EOF0003 SE EJECUTÓ CORRECTAMENTE");
        }

        //CRM-EOF0004
        [Test(Description = "CRM_EOF0004 Editar campo 'Tipo de oferta' de una Oferta")]
        [AllureSubSuite("PRO EDI OFERTA")]
        public void CRM_EOF0004_Editar_campo_tipo_de_Oferta()
        {
            // Paso 1
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            //Paso 2
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            //Paso 3
            ofertaActions.Tipo_de_oferta_Cambiodecapacidad(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_AvisoCambioCapacidad();

            //Paso 4
            ofertaActions.Tipo_de_oferta_Cambiodeprecio();
            ofertaCondition.validar_AvisoCambioPrecio();

            //Paso 5
            ofertaActions.Tipo_de_oferta_Cambiodesolucion();
            ofertaCondition.validar_AvisoCambioSolucion();

            //Paso 6
            ofertaActions.Tipo_de_oferta_Cambiodedireccion();
            ofertaCondition.validar_AvisoCambiodedireccion();

            ofertaActions.GuardarYCerrarOferta();
            ofertaActions.ReestablecerDatosCRM_EOF0004();

            TestContext.WriteLine("LA PRUEBA CRM-EOF0004 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0011 Eliminar una Oferta Adjudicada")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0011_eliminarOferta_Adjudicada()
        {

            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();   
                
            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            //ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.salirOfertaCOF0011();
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            //Paso 2 - Acceder a una Oferta que esté en_en_estado_Adjudicada, con el check del listado
            ofertaActions.AccederOfertaestado_Adjudicada(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_Acceso_A_OfertaAdjudicadas();

            //Paso 3 - Pulsar Eliminar en la barra de herramientas.
            ofertaActions.eliminarOfertaActual("Cancelar");
            //ofertaCondition.validar_EliminacionDesdeBarraMenu();

            //Paso 4 - Cancelar la eliminación de la Oferta haciendo click en Cancelar 
            //ofertaActions.Cancelar();
            //ofertaCondition.Resultado_Cancelar();

            //Paso 5 - Repetir pasos 2 y 3 Acceso
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            ofertaActions.AccederOfertaestado_Adjudicada(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_Acceso_A_OfertaAdjudicadas();

            // Paso 6 - Repetir pasos 2 y 3 Eliminar 
            ofertaActions.eliminarOfertaActual("Eliminar");
            //ofertaCondition.validar_EliminacionDesdeBarraMenu();

            // Paso 7 - Eliminar la oferta desde popup
            //ofertaActions.Eliminar_Popup();
            ofertaCondition.validar_EliminacionPopUp();

            //Paso 6 - Regresar al grid de ofertas, seleccionar la Oferta Adjudicada con la que se trabaja.
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            ofertaActions.Seleccionofertarazonadjudicada(Utils.getDatosTestCase("oferta"));

            //ofertaActions.editarOferta();
            ofertaCondition.validar_OFertaCerrada();

            TestContext.WriteLine("LA PRUEBA CRM-COF0011 ELIMINAR SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0012 Cerrar Oferta Adjudicada")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0012_Oferta_Cerrar_Adjudicada()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            //Paso 2 - Desde el listado de ofertas, seleccionamos una en estado ganada/adjudicada y se pulsa editar
            ofertaActions.Seleccionofertarazonadjudicada();

            ofertaCondition.validar_CerrarOfertaNoVisible(); //Actualizar metodo, No comprueba el elemento correcto

            TestContext.WriteLine("LA PRUEBA CRM-COF0012 CERRAR SE EJECUTÓ CORRECTAMENTE");
        }


        [Test(Description = "CRM-POF0001 Presentar Oferta Circuito de Capacidad")]
        [AllureSubSuite("PRO PRESENTAR OFERTA")]
        public void CRM_POF0001_PresentarOferta()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta Circuito de Capacidad
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();   
            ofertaActions.rellenar_Datos_Productos_Oferta();
            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Regresar al grid, verificar oferta
            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPresentadaCorrectamente();

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-POF0001 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POF0002 Presentar Oferta Fibra Oscura")]
        [AllureSubSuite("PRO PRESENTAR OFERTA")]
        public void CRM_POF0002_PresentarOferta()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la Oferta Fibra Oscura
            // Maximo NRC -> 922337203685477,00 €
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Regresar al grid, verificar oferta
            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPresentadaCorrectamente();

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-POF0002 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POF0003 Presentar Oferta UbiRed Pro")]
        [AllureSubSuite("PRO PRESENTAR OFERTA")]
        public void CRM_POF0003_PresentarOferta_UbiRed_Pro()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la UbiRed Pro
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Regresar al grid, verificar oferta
            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPresentadaCorrectamente();

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-POF0003 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POF0004 Presentar Oferta UbiRed Business")]
        [AllureSubSuite("PRO PRESENTAR OFERTA")]
        public void CRM_POF0004_PresentarOferta_UbiRed_Business()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la UbiRed Pro
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Regresar al grid, verificar oferta
            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPresentadaCorrectamente();

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-POF0004 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POF0005 Presentar Oferta Rack")]
        [AllureSubSuite("PRO PRESENTAR OFERTA")]
        public void CRM_POF0005_PresentarOferta_Rack_Colocation()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la UbiRed Pro
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Regresar al grid, verificar oferta
            ofertaActions.acceder_A_OfertasLyntia();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaPresentadaCorrectamente();

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-POF0005 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POAF0001 - PRO ADJUDICAR OFERTA")] //Caso pendiente de desarrollo
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0001_Oferta_Adjudicar_CC()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la UbiRed Pro
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Adjudicar la oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 4 - Crear pedido
            ofertaActions.VentanaCrearPedido();
            //ofertaCondition.ResVentanaCrearPedido();
        }

        [Test(Description = "CRM-POAF0002 - PRO ADJUDICAR OFERTA")]//Caso pendiente de desarrollo
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0002_Ofeta_Adjudicar_FOC()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Crear Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();

            // Rellenar campos y click en Guardar
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Añadir Producto a la UbiRed Pro
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            // Paso 3 - Presentar la oferta
            ofertaActions.presentarOferta();

            // Paso 4 - Adjudicar la oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 4 - Crear pedido
            ofertaActions.VentanaCrearPedido();
            //ofertaCondition.ResVentanaCrearPedido();
        }

        [Test(Description = "CRM-EOF0001 - Edición_borrador_Sin_datos_obligatorios")]
        [AllureSubSuite("PRO EDI OFERTA")]
        public void CRM_EOF0001_Edición_borrador_Sin_datos_obligatorios()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Paso 4 - Rellenar campos de la oferta
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));

            // Paso 5 - Guardar oferta
            ofertaActions.GuardarOferta();

            // Paso 6 - Eliminar campos obligatorios
            ofertaActions.Eliminar_campos_obligatorios(1);

            // Paso 7 - Actualizar
            ofertaActions.ActualizarBarramenu();
            ofertaCondition.validar_CondicionesOfertaNoCreada();

            ofertaActions.eliminar_ofertaDetalle("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-EOF0001 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-EOF0002 - Edición_borrador_Con_datos_obligatorio")]
        [AllureSubSuite("PRO EDI OFERTA")]
        public void CRM_EOF0002_Edición_borrador_Con_datos_obligatorios()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Paso 4 - Rellenar campos de la oferta
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 5 - Guardar oferta
            ofertaActions.Eliminar_campos_obligatorios(1);
            ofertaActions.Modificar_campos_obligatorios(Utils.getDatosTestCase("edicionOferta"));
            ofertaActions.GuardarOferta();

            // Paso 6 - Actualizar oferta
            ofertaActions.ActualizarBarramenu();

            // Paso 7 - Guardar y cerrar
            ofertaActions.GuardarYCerrarOferta();

            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("edicionOferta")["nombreOferta"] + getFechaHoraActual());

            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-EOF0002 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0001 - Oferta_Eliminar_En_borrador_Sin_Producto")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0001_Oferta_Eliminar_En_borrador_Sin_Producto()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            //ofertaActions.GuardarOferta();
            //productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.GuardarYCerrarOferta();

            // Paso 4 - Buscar en vista la oferta
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_EdicionOFerta();

            // Paso 5 - Cancelar la eliminacion de la oferta
            ofertaActions.eliminar_ofertaDetalle("Cancelar");

            // Paso 6 - Eliminar oferta
            ofertaActions.eliminar_ofertaDetalle("Eliminar");
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.Datos_disponibles();

            TestContext.WriteLine("LA PRUEBA CRM-COF0001 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COF0002 - Vista Ofertas_En_borrador_Enelaboración_Sin_Producto")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0002_Vista_Ofertas_En_borrador_En_elaboración_Sin_Producto()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas            
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarYCerrarOferta();

            // Paso 4 - Buscar en vista la oferta
            ofertaActions.AbrirOfertaEnVista(Utils.getDatosTestCase("oferta"));
            ofertaCondition.validar_EdicionOFerta();

            // Paso 5 - Cancelar la eliminacion de la oferta
            ofertaActions.eliminar_ofertaDetalle("Cancelar");

            // Paso 6 - Salir al grid y eliminar oferta
            ofertaActions.GuardarYCerrarOferta();
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.eliminarOfertaActual("Eliminar");
            ofertaCondition.Datos_disponibles();
            TestContext.WriteLine("LA PRUEBA CRM-COF0002 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM_OFCAN0001 - Oferta_Cancelar_En_borrador_Sin_Producto")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_OFCAN0001_Oferta_Cancelar_En_borrador_Sin_Producto()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();
            ofertaActions.ActualizarBarramenu();

            // Paso 4 - Cerrar oferta("Cancelar")
            ofertaActions.CancelarOfertaActual(Utils.getDatosTestCase("cierreOferta1"));

            // Paso 5 - Cerrar oferta sin cumplimentar campos obligatorios
            ofertaActions.CancelarOfertaActual(Utils.getDatosTestCase("cierreOferta2"));
            ofertaCondition.validar_OfertaNoCerrada();

            driver.Navigate().Refresh();

            // Paso 6 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.CancelarOfertaActual(Utils.getDatosTestCase("cierreOferta3"));

            // Paso 7 - Accedemos al grid, buscamos la oferta y se comprueba estados
            ofertaActions.acceder_A_OfertasLyntia();
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.validar_OfertaCerradaCorrectamenteEnGrid(Utils.getDatosTestCase("cierreOferta3")["razon"]);

            // Reestablecer dato
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.eliminarOfertaActual("Eliminar");
            TestContext.WriteLine("LA PRUEBA CRM_OFCAN0001 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-COFNR0001 Cerrar Oferta En Elaboracion No Realizable")]
        [AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COFNR0001_cerrarOfertaEnElaboracion_NoRealizable()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();
            commonCondition.validar_Acceso_A_GestionCliente();

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 2 - Acceder a una Oferta que esté en estado En Elaboracion.
            ofertaActions.acceder_A_NuevaOferta();

            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            ofertaCondition.validar_CerrarOfertaNoVisible();

            driver.Navigate().Refresh();

            // Acceso a Ofertas y buscar la Oferta Cerrada
            ofertaActions.acceder_A_OfertasLyntia();

            // Paso 7 - Repetir el paso anterior pero cerrando de manera correcta
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());

            // Eliminar Oferta
            ofertaActions.SeleccionarOfertaGrid();

            ofertaActions.eliminarOfertaActual("Eliminar");

            TestContext.WriteLine("LA PRUEBA CRM-COFNR0001 CERRAR SE EJECUTÓ CORRECTAMENTE");
        }

        //[Test(Description = "CRM-COF0004 - Oferta_Cerrar_En_borrador_Sin_Producto_No_viable")]
        //[AllureSubSuite("PRO ELIMINAR-CERRAR OFERTA")]
        public void CRM_COF0004_Oferta_Cerrar_En_borrador_Sin_Producto_No_viable_caso_MOD()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Cerrar oferta("Cancelar")
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta1"));

            // Paso 5 - Cerrar oferta sin cumplimentar campos obligatorios
            // TODO: No se puede dejar sin informar campo obligatorios con la opcion de no viable, por lo que se pone cancelada para mostrar campos obligatorios
            ofertaActions.CerrarOfertaActual(Utils.getDatosTestCase("cierreOferta2"));
            ofertaCondition.validar_OfertaNoCerrada();

            driver.Navigate().Refresh();

            // Paso 6 - Accedemos al grid, buscamos la oferta y se comprueba estados
            ofertaActions.acceder_A_OfertasLyntia();
            ofertaActions.BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());

            // Reestablecer dato
            ofertaActions.SeleccionarOfertaGrid();
            ofertaActions.eliminarOfertaActual("Eliminar");
            TestContext.WriteLine("LA PRUEBA CRM-COF0004 SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POAF0001 - Oferta/Adjudicar/varios CC")]
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0001_Oferta_Adjudicar_varios_CC_circuito_de_capacidad()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto2"));
            ofertaActions.acceder_A_linea2();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();
            

            // Paso 9 - Buscar una oferta desde el servicio contratado
            ofertaActions.acceder_A_OfertasLyntia();
            productoActions.BuscarOferta_desde_servicio_contratado(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.ResBuscarOferta_desde_servicio_contratado();

            TestContext.WriteLine("LA PRUEBA CRM-POAF0001 - Oferta/Adjudicar/varios CC SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POAF0002 - Oferta/Adjudicar/varios CC")]
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0002_Oferta_Adjudicar_varios_CC_Fibra_oscura()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba            
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto2"));
            ofertaActions.acceder_A_linea2();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido   
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 8 - Buscar una oferta desde el servicio contratado
            ofertaActions.acceder_A_OfertasLyntia();
            productoActions.BuscarOferta_desde_servicio_contratado(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.ResBuscarOferta_desde_servicio_contratado();

            TestContext.WriteLine("LA PRUEBA CRM-POAF0002 - Oferta/Adjudicar/varios CC SE EJECUTÓ CORRECTAMENTE");
        }


        [Test(Description = "CRM-POAF0003 - Oferta/Adjudicar/varios CC")]
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0003_Oferta_Adjudicar_varios_CC_UbiredPRO()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto2"));
            ofertaActions.acceder_A_linea2();
            ofertaActions.rellenar_Datos_Productos_Oferta();


            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 9 - Buscar una oferta desde el servicio contratado
            ofertaActions.acceder_A_OfertasLyntia();
            productoActions.BuscarOferta_desde_servicio_contratado(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.ResBuscarOferta_desde_servicio_contratado();

            TestContext.WriteLine("LA PRUEBA CRM-POAF0003 - Oferta/Adjudicar/varios CC SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-POAF0004 - Oferta/Adjudicar/varios CC")]
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0004_Oferta_Adjudicar_varios_CC_UbiRedBusiness()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto2"));
            ofertaActions.acceder_A_linea2();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());

            // Paso 8 - Buscar una oferta desde el servicio contratado
            ofertaActions.acceder_A_OfertasLyntia();
            productoActions.BuscarOferta_desde_servicio_contratado(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            ofertaCondition.ResBuscarOferta_desde_servicio_contratado();

            TestContext.WriteLine("LA PRUEBA CRM-POAF0004 - Oferta/Adjudicar/varios CC SE EJECUTÓ CORRECTAMENTE");
        }


        [Test(Description = "CRM-POAF0005 - Oferta/Adjudicar/varios CC")]
        [AllureSubSuite("PRO ADJUDICAR OFERTA")]
        public void CRM_POAF0005_Oferta_Adjudicar_varios_CC_Rack()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC

            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();
            
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto2"));
            ofertaActions.acceder_A_linea2();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 8 - Buscar una oferta desde el servicio contratado
            ofertaActions.acceder_A_OfertasLyntia();
            productoActions.BuscarOferta_desde_servicio_contratado(Utils.getDatosTestCase("oferta")["nombreOferta"] + getFechaHoraActual());
            CommonActions.editarPrimeraFilaGrid();
            ofertaCondition.ResBuscarOferta_desde_servicio_contratado();

            TestContext.WriteLine("LA PRUEBA CRM-POAF0003 - Oferta/Adjudicar/varios CC SE EJECUTÓ CORRECTAMENTE");
        }

        [Test(Description = "CRM-SECAN0001 - Oferta/Servicio/CANCELACIÓN CC")]
        [AllureSubSuite("SERVICIO CANCELAR")]
        public void CRM_SECAN0001_Oferta_Servicio_CANCELACIÓN_CC()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            //ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 8 - Seleccion del primer registro de los 2 productos y cumplimentar datos obligatorios
            ofertaActions.rellenarCamposObligatoriosProducto("CC", Utils.getDatosTestCase("servicioContratado"));

            // Paso 10 - Enviar a Jira
            ofertaActions.Enviar_A_Jira();
            //ofertaCondition.ResBuscarOferta_enviarJira();

            // Paso 11 - Cancelar motivo Jira
            ofertaActions.Enviar_A_Jira_cancelar();
        }

        [Test(Description = "CRM-SECAN0002 - Oferta/Servicio/CANCELACIÓN FO")]
        [AllureSubSuite("SERVICIO CANCELAR")]
        public void CRM_SECAN0002_Oferta_Servicio_CANCELACIÓN_FO()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            //ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            // Paso 8 - Seleccion del primer registro de los 2 productos y cumplimentar datos obligatorios
            ofertaActions.rellenarCamposObligatoriosProducto("Fibra", Utils.getDatosTestCase("servicioContratado"));

            // Paso 10 - Enviar a Jira
            ofertaActions.Enviar_A_Jira();
            //ofertaCondition.ResBuscarOferta_enviarJira();

            // Paso 11 - Cancelar motivo Jira
            ofertaActions.Enviar_A_Jira_cancelar();
        }

        [Test(Description = "CRM-SECAN0003 - Oferta/Servicio/CANCELACIÓN Ubired Pro")]
        [AllureSubSuite("SERVICIO CANCELAR")]
        public void CRM_SECAN0003_Oferta_Servicio_CANCELACIÓN_Ubired_Pro()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            //ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputUTPRx")) && Utils.SearchWebElement("Oferta.inputUTPRx").GetAttribute("aria-required") == "true")
            {
                ofertaActions.rellenarUTPRX("20");
                ofertaActions.rellenarCodigoTarea("19");
            }

            // Paso 8 - Seleccion del primer registro de los 2 productos y cumplimentar datos obligatorios
            ofertaActions.rellenarCamposObligatoriosProducto("UbiRed Pro", Utils.getDatosTestCase("servicioContratado"));

            // Paso 9 - Enviar a Jira
            ofertaActions.Enviar_A_Jira();
            // ofertaCondition.ResBuscarOferta_enviarJira();

            // Paso 10 - Cancelar motivo Jira
            ofertaActions.Enviar_A_Jira_cancelar();
        }

        [Test(Description = "CRM-SECAN0004 - Oferta/Servicio/CANCELACIÓN RACK")]
        [AllureSubSuite("SERVICIO CANCELAR")]
        public void CRM_SECAN0004_Oferta_Servicio_CANCELACIÓN_RACK()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Paso 3 - Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            // Preparacion de datos de la prueba
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Paso 4 - Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Paso 5 - Presentamos oferta
            ofertaActions.presentarOferta();

            // Paso 6 - Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Paso 7 - Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            //ofertaCondition.ResultadResVentanaCrearPedidofechaposterior();

            if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputUTPRx")) && Utils.SearchWebElement("Oferta.inputUTPRx").GetAttribute("aria-required") == "true")
            {
                ofertaActions.rellenarUTPRX(Utils.getDatosTestCase("oferta")["utprx"]);
            }
            if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputCodigoTarea")) && Utils.SearchWebElement("Oferta.inputCodigoTarea").GetAttribute("aria-required") == "true")
            {
                ofertaActions.rellenarCodigoTarea(Utils.getDatosTestCase("oferta")["codigoTarea"]);
            }

            // Paso 8 - Seleccion del primer registro de los 2 productos y cumplimentar datos obligatorios
            ofertaActions.rellenarCamposObligatoriosProducto("RACK", Utils.getDatosTestCase("servicioContratado"));

            // Paso 10 - Enviar a Jira
            ofertaActions.Enviar_A_Jira();

            // Paso 11 - Cancelar motivo Jira
            ofertaActions.Enviar_A_Jira_cancelar();
            //ofertaCondition.Resultado_Enviar_A_Jira_cancelar();

        }

        [Test(Description = "CRM-BILLING001 - Generar/Facturas")]
        [AllureSubSuite("GENERAR FACTURAS")]
        public void CRM_BILLING001_Generar_Facturas()
        {
            // Login y Acceso a Gestión de Cliente
            commonActions.acceder_A_GestionCliente();//Acceso al modulo de Gestion de Cliente(Apliaciones)
            commonCondition.validar_Acceso_A_GestionCliente();//Acceso correcto

            // Paso 1 - Hacer click en Ofertas
            ofertaActions.acceder_A_OfertasLyntia();//Oferta menu
            commonCondition.validar_AccesoOferta();//comprobamos el acceso

            // Nueva Oferta
            ofertaActions.acceder_A_NuevaOferta();
            ofertaCondition.validar_CondicionesCreacionOferta();

            //Preparacion de datos de la prueba
            string nombreServicio = Utils.getDatosTestCase("oferta")["nombreOferta"] + Oferta.getFechaHoraActual();
            ofertaActions.rellenarCamposOferta(Utils.getDatosTestCase("oferta"));
            ofertaActions.GuardarOferta();

            // Creamos 2 tipos de productos CC
            productoActions.CreacionProducto(Utils.getDatosTestCase("producto1"));
            ofertaActions.acceder_A_linea1();
            ofertaActions.rellenar_Datos_Productos_Oferta();

            // Presentamos oferta
            ofertaActions.presentarOferta();

            // Adjudicamos oferta
            ofertaActions.Adjudicar_Oferta();
            ofertaCondition.validar_AdjudicacionOferta();

            // Creamos el pedido
            ofertaActions.VentanaCrearPedidofechaposterior(Utils.getFechaActual());
            Thread.Sleep(8000);

            // Seleccion del primer registro de los productos y cumplimentar datos obligatorios
            // ofertaActions.acceder_A_linea1(); - Aparentemente Obsoleto
            ofertaActions.rellenarCamposObligatoriosProducto("Fibra Billing", Utils.getDatosTestCase("servicioContratado"));

            //Acceder a configuraciones de facturacion en pestaña relacionados
            Thread.Sleep(1000);
            Utils.clickWithJavascript("Producto.PestañaRelacionados");
            CommonActions.getElement("Oferta.configuracionesFacturacionRelacionadosBilling").Click();

            //Validar configuracion de facturación
            string nombreGeneradorFacturacion = ofertaActions.esperar_A_CargarConfiguracionFacturacion();
            ofertaActions.validarConfiguracionFacturacion();

            //Acceder a Modulo Billing
            ofertaActions.acceder_A_ModuloBilling();

            //Crear nuevo Generador de Facturacion
            ofertaActions.crearNuevoGeneradorFacturacion();
            ofertaActions.rellenarCamposNuevoGeneradorFacturacion(Utils.getDatosTestCase("oferta")["cliente"], nombreServicio);

            //Generar Facturas            
            ofertaActions.generarFactura();

            //Comprobar estado generador de facturacion completado     - No detecta el elemento celdaRazonEstado, revisar         
            //Assert.AreEqual("Completado", CommonActions.getElement("celdaRazonEstado").Text);

            //Enviar a nav
            ofertaActions.acceder_A_Facturas();
            ofertaActions.enviar_A_Nav(nombreGeneradorFacturacion);

            //comprobar que el estado de la factura ha cambiado a Enviada a NAV
            ofertaCondition.validarFacturaEnviada_A_Nav(nombreGeneradorFacturacion);
        }

    }

}