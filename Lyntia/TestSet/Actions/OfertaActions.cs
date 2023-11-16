using Lyntia.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Lyntia.TestSet.Actions
{
    /// <summary>
    /// OfertaActions Class
    /// </summary>
    public class OfertaActions
    {
        readonly Utils utils = new Utils();

        private static IWebDriver driver;
        private static OpenQA.Selenium.Interactions.Actions accionesSelenium;
        private static WebDriverWait wait;

        /// <summary>
        /// OfertaActions
        /// </summary>
        public OfertaActions()
        {
            driver = Utils.driver;
            accionesSelenium = new OpenQA.Selenium.Interactions.Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
        }

        /// <summary>
        /// Método usado seleccionar vista de Ofertas (Mis Ofertas Lyntia, Todas las ofertas ...)
        /// </summary>
        /// <param name="seccion"></param>
        public void acceder_A_OfertasLyntia()
        {

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(Utils.GetIdentifier("Oferta.ofertaSection"))));
            if (Utils.EncontrarElemento(By.Id(Utils.GetIdentifier("Oferta.ofertaSection"))))
            {
                try
                {             
                    
                    CommonActions.getElement("Oferta.ofertaSection").Click();
                    
                    //Comprobamos si se muestra alerta de guardar cambios y continuar
                    if (Utils.EncontrarElemento(Utils.getByElement("Oferta.buttonGuardarYContinuar")))
                        CommonActions.getElement("Oferta.buttonGuardarYContinuar").Click();

                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(),'Ofertas')]")));
                    if (Utils.EncontrarElemento(By.XPath("//button/span/span/span[text()='OFERTAS KAM']")))
                    {
                        
                        driver.FindElement(By.XPath("//button/span/span/span[text()='OFERTAS KAM']")).Click();
                        driver.FindElement(By.XPath("//label[text()='OFERTAS LYNTIA']")).Click();
                    }


                    //Thread.Sleep(3000);

                    TestContext.WriteLine("Se accede correctamente a la sección Mis Ofertas lyntia");

                }
                catch (Exception e)
                {
                    CommonActions.CapturadorExcepcion(e, "AccesoSeccionOfertas.png", "No se pudo acceder a la sección  Mis Ofertas lyntia.");
                    throw e;
                }
            }
        }

        /// <summary>
        /// Método para hacer click en "+ Nuevo", barra de herramientas
        /// </summary>
        public void acceder_A_NuevaOferta()
        {
            try
            {
                //hay que cambiar los Thread.Sleep por WebDriverWait
                Thread.Sleep(7000);
                CommonActions.getElement("Oferta.newOferta").Click();
                //Thread.Sleep(3000);

                TestContext.WriteLine("Nueva Oferta creada correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AccesoNuevaOferta.png", "No se pudo crear una nueva Oferta.");
                throw e;
            }
        }

        /// <summary>
        /// Método para abrir la primera Oferta del grid
        /// </summary>
        public void AbrirOferta()
        {
            try
            {
                CommonActions.getElement("Oferta.firstFromGrid").Click();
                // Thread.Sleep(2000);

                TestContext.WriteLine("Se accede a la primera Oferta del grid.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "PrimeraOfertaGrid.png", "No se pudo acceder a la primera Oferta del grid.");
                throw e;
            }
        }

        /// <summary>
        /// Método para acceder a la pestaña de Fechas de Oferta
        /// </summary>
        public void acceder_A_PestañaFechasOferta()
        {
            try
            {
                CommonActions.getElement("Oferta.datesSection").Click();

                TestContext.WriteLine("Se accede correctamente a la pestaña Fechas de la Oferta.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AccesoFechas.png", "No se pudo acceder a la pestaña Fechas de la OFerta.");
                throw e;
            }
        }

        /// <summary>
        /// Método para hacer Click en Guardar Oferta en la barra de herramientas
        /// </summary>
        public void GuardarOferta()
        {
            try
            {
                CommonActions.getElement("Oferta.saveOferta").Click();
                Thread.Sleep(7000);
                CommonActions.esperarGuardado();
                TestContext.WriteLine("La Oferta se guarda correctamente.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "GuardarOferta.png", "No se pudo guardar la Oferta.");
                throw e;
            }
        }

        /// <summary>
        /// Método para hacer click en Guardar y cerrar en la barra de herramientas
        /// </summary>
        public void GuardarYCerrarOferta()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.saveAndCloseOferta");
                Thread.Sleep(3000);
                
                if (Utils.EncontrarElemento(By.CssSelector("h1[aria-label='Guardado en curso']")))
                {
                    CommonActions.waitAndClick("Oferta.buttonConfirmarCierre");
                    Thread.Sleep(7000);
                    CommonActions.waitAndClick("Oferta.saveAndCloseOferta");                             
                }

                TestContext.WriteLine("La Oferta se guarda y cierra correctamente.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "GuardarCerrarOferta.png", "No se pudo guardar y cerrar la Oferta.");
                throw e;
            }
        }

        /// <summary>
        /// Métdodo empleado para completar los campos necesarios para crear una nueva Oferta.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="cliente"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="kam"></param>
        /// 
        public void crearOfertaBorrador(Dictionary<string, string> datosTestCase)
        {
            try
            {
                acceder_A_OfertasLyntia();                
                acceder_A_NuevaOferta();

                //Validar campos para la creacion de nueva oferta
                Utils.GetOfertaConditions().validar_CondicionesCreacionOferta();

                //se rellenan los campos obligatorios
                CommonActions.rellenarCampoInput("Oferta.inputNameOferta", datosTestCase["nombreOferta"] + Oferta.getFechaHoraActual());
                CommonActions.rellenarCampoConDesplegable("Oferta.inputCustomerId", datosTestCase["cliente"]);
                acceder_A_PestañaFechasOferta();

                //Comprobar fechas sin informar
                Utils.GetOfertaConditions().validar_FechasSinInformar();                
                
                GuardarYCerrarOferta();
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AddDatosOferta.png", "No se ha podido crear una oferta con estado borrador");
                throw e;
            }
        }
        public void rellenarCamposOferta(Dictionary<string, string> datosOferta)
        {
            try
            {
                
                //Thread.Sleep(1000);
                if (!datosOferta["nombreOferta"].Equals("") && CommonActions.getElement("Oferta.inputNameOferta").GetAttribute("value").Contains("---"))
                    CommonActions.rellenarCampoInput("Oferta.inputNameOferta", datosOferta["nombreOferta"] + Oferta.getFechaHoraActual());
                //Thread.Sleep(1000);

                // Se altera el orden para que detecte un elemento sin hacer scroll
                if (!datosOferta["tipoOferta"].Equals(""))
                    CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", datosOferta["tipoOferta"]);

                if (!datosOferta["cliente"].Equals(""))
                {
                    // Rellenar Cliente de Oferta
                    CommonActions.rellenarCampoConDesplegable("Oferta.inputCustomerId", datosOferta["cliente"]);

                    if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputUTPRx")) && CommonActions.getElement("Oferta.inputUTPRx").GetAttribute("aria-required") == "true")
                        CommonActions.rellenarCampoInput("Oferta.inputUTPRx", datosOferta["utprx"]);

                    if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputCodigoTarea")) && CommonActions.getElement("Oferta.inputCodigoTarea").GetAttribute("aria-required") == "true")
                        CommonActions.rellenarCampoInput("Oferta.inputCodigoTarea", datosOferta["codigoTarea"]);
                }         

                //Cerrar calendario
                if (Utils.EncontrarElemento(By.CssSelector("[id^='DatePicker-Callout19']")))
                    CommonActions.getElement(By.CssSelector("[id^='DatePicker-Callout19']")).SendKeys(Keys.Escape);
                
                //Rellenar Fecha presentacion                
                CommonActions.rellenarCampoInput("Oferta.fechaPresentacion", datosOferta["fechaPresentacion"].Equals("") ? Utils.getFechaActual() : datosOferta["fechaPresentacion"]);

                CommonActions.getElement("Oferta.LabelGeneralPestaña").Click();

                TestContext.WriteLine("Se han introducido correctamente los campos de la Oferta: " + datosOferta["nombreOferta"] + ", " + datosOferta["cliente"] + ", " + datosOferta["tipoOferta"] + ", " + datosOferta["kam"] + ".");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AddDatosOferta.png", "No se introducen los datos de la Oferta: " + datosOferta["nombreOferta"] + ", " + datosOferta["cliente"] + ", " + datosOferta["tipoOferta"] + ", " + datosOferta["kam"] + ".");
                throw e;
            }
        }        

        /// <summary>
        /// Método para eliminar Oferta seleccionada o abierta. 
        /// El campo opción permite cancelar el borrado o realizarlo por completo.
        /// </summary>
        /// <param name="opcion"></param>
        public void eliminarOfertaActual(String opcion)
        
            {
            // Click en Eliminar
            try
            {
                CommonActions.waitAndClick("Oferta.deleteButtonOferta");
                // Confirmar Borrado
                if (opcion.Equals("Eliminar"))
                {
                    CommonActions.waitAndClick("Oferta.confirmDeleteOferta");
                    Thread.Sleep(4000);
                    TestContext.WriteLine("Se elimina la Oferta correctamente.");
                    Thread.Sleep(2000);
                }
                else
                {
                    CommonActions.waitAndClick("Oferta.cancelDeleteOferta");
                    Thread.Sleep(4000);
                    TestContext.WriteLine("Se cancela el proceso de eliminación.");
                }
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EliminarOferta.png", "No se puede completar el proceso de eliminación (" + opcion + ").");
                throw e;
            }            
        }

        /// <summary>
        /// Método para buscar Ofertas en el grid dado un parámetro de búsqueda.
        /// </summary>
        /// <param name="parametroBusqueda"></param>
        public void BuscarOfertaEnVista(string parametroBusqueda)
        {            
            try
            {
                Thread.Sleep(5000);
                CommonActions.rellenarCampoInput("Oferta.inputQuickFindOferta", parametroBusqueda);
                Thread.Sleep(2000);
                TestContext.WriteLine("Se busca la Oferta por parámetro: " + parametroBusqueda + ".");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "BuscaOferta.png", "El proceso de búsqueda de Oferta por parámetro " + parametroBusqueda + " ha fallado.");
                throw e;
            }
        }

        /// <summary>
        /// Método empleado para abrir una Oferta sabiendo su nombre.
        /// </summary>
        /// <param name="nombreOferta"></param>
        public void AbrirOfertaEnVista(Dictionary<string, string> datosRepositorio)
        {
            try
            {
                CommonActions.waitAndClick("Oferta.Grid.idOfertaGrid");
                Thread.Sleep(1000);
                CommonActions.waitAndClick("Oferta.buttonEditOferta");                
                Thread.Sleep(2000);
                TestContext.WriteLine("Se abre la Oferta " + datosRepositorio["nombreOferta"] + Oferta.getFechaHoraActual() + " se abre correctamente.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AbrirOferta.png", "No se abre la Oferta " + datosRepositorio["nombreOferta"] + Oferta.getFechaHoraActual() + ".");
                throw e;
            }
        }

        /// <summary>
        /// Método empleado para una vez buscada una Oferta en el grid, abrirla(seleccion con un ckeck)
        /// </summary>
        public void SeleccionarOfertaGrid()
        {
            try
            {
                CommonActions.getElement("Oferta.selectOfertaGrid").Click();
                Thread.Sleep(8000);
                TestContext.WriteLine("Se selecciona la Oferta del grid.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "SeleccionOferta.png", "Se selecciona la Oferta del grid.");
                throw e;
            }
        }

        /// <summary>
        /// Método para inserción de datos de una Oferta
        /// </summary>
        public void IntroduccirDatos(Dictionary<string,string> datosTestCase)
        {
            try
            {
                CommonActions.rellenarCampoInput("Oferta.inputNameOferta", datosTestCase["nombreOferta"] + Oferta.getFechaHoraActual());        

                // Seleccionar referencia
                CommonActions.rellenarCampoInput("Oferta.inputReferenceOferta", datosTestCase["referenciaOferta"]);
                Thread.Sleep(1000);

                // Resto de campos
                if (Utils.EncontrarElemento(Utils.getByElement("Oferta.labelPermutaDefaultReset")))
                {
                    //Toggle Switch                    
                    CommonActions.getElement("Oferta.labelPermutaDefault").SendKeys(Keys.Enter);
                    CommonActions.rellenarCampoInput("Oferta.fechaPresentacion", Utils.getFechaActual());                   
                    Thread.Sleep(3000);
                }
                
                CommonActions.rellenarCampoInput("Oferta.inputCampoDescripcion", datosTestCase["descripcion"]);//Escribimos Prueba campo descripcion en detalle de oferta
                CommonActions.rellenarCampoInput("Oferta.inputGVAL", datosTestCase["GVAL"]);//Escribimos prue123456 en codigo gval
                Thread.Sleep(3000);

                CommonActions.waitAndClick("Oferta.buttonGuardar"); //Guardar
                Thread.Sleep(3000);
                CommonActions.getElement("Oferta.inputNameOferta").SendKeys(Keys.PageDown);
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.ButtonGuardarYcerrar"); //Guarda y cierra
                Thread.Sleep(4000);
                TestContext.WriteLine("Se modifican los datos de la prueba CRM-EOF0003.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ModificacionOfertaEOF0003.png", "No se modifican los datos de la prueba CRM-EOF0003.");
                throw e;
            }
        }


        /// <summary>
        /// Método para abrir una Oferta y cambiar el Tipo de oferta a Cambio de capacidad.
        /// </summary>
        public void Tipo_de_oferta_Cambiodecapacidad(Dictionary<string, string> datosTestCase)
        {
            try
            {
                BuscarOfertaEnVista(datosTestCase["nombreOferta"]);
                AbrirOfertaEnVista(datosTestCase);
                Thread.Sleep(3000);

                CommonActions.getElement("Oferta.inputNameOferta").SendKeys(Keys.PageDown);

                CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", "Cambio Upgrade/Downgrade");
                TestContext.WriteLine("Se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio Upgrade/Downgrade.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "TipoOfertaCambioCapacidadEOF0004.png", "No se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de capacidad (Upgrade/Downgrade).");
                throw e;
            }
        }

        /// <summary>
        /// /// Método para abrir una Oferta y cambiar el Tipo de oferta a Cambio de precio.
        /// </summary>
        public void Tipo_de_oferta_Cambiodeprecio()
        {
            try
            {
                Thread.Sleep(3000);
                CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", "Cambio de precio/Renovación");
                TestContext.WriteLine("Se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de precio/Renovación.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "TipoOfertaCambioPrecioEOF0004.png", "No se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de precio/Renovación");
                throw e;
            }
        }

        /// <summary>
        /// /// Método para abrir una Oferta y cambiar el Tipo de oferta a Cambio de solución.
        /// </summary>
        public void Tipo_de_oferta_Cambiodesolucion()
        {
            try
            {
                Thread.Sleep(3000);
                CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", "Cambio de configuración");
                TestContext.WriteLine("Se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de configuración");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "TipoOfertaCambioSolucionEOF0004.png", "No se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de solución técnica (Tecnología).");
                throw e;
            }
        }

        /// <summary>
        /// /// Método para abrir una Oferta y cambiar el Tipo de oferta a Cambio de dirección.
        /// </summary>
        public void Tipo_de_oferta_Cambiodedireccion()
        {
            try
            {
                Thread.Sleep(3000);
                CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", "Cambio de dirección");
                TestContext.WriteLine("Se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de dirección");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "TipoOfertaCambioPrecioEOF0004.png", "No se modifica el tipo de Oferta de la prueba CRM-EOF0004: Cambio de dirección (Migración).");
                throw e;
            }
        }

        /// <summary>
        /// Método para Eliminar datos de la Oferta.
        /// </summary>
        public void Eliminar_campos_obligatorios(int vez)
        {
            try
            {
                // Nombre de la oferta
                Thread.Sleep(3000);
                CommonActions.waitAndClick("Oferta.inputNameOferta"); 
                CommonActions.limpiarCampo("Oferta.inputNameOferta");
                CommonActions.waitAndClick("Oferta.inputNameOferta");
                Thread.Sleep(3000);

                // Cliente
                if(vez == 1)
                {
                    accionesSelenium.SendKeys(Keys.Tab).Perform();
                    accionesSelenium.SendKeys(Keys.Tab).Perform();
                }
                else
                {
                    accionesSelenium.SendKeys(Keys.Tab).Perform();
                }

                //Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.buttonCancelCliente");
                Thread.Sleep(2000);

                // KAM
                CommonActions.waitAndClick("Oferta.kamOferta");
                Thread.Sleep(1000);
                CommonActions.getElement(By.XPath("//span[contains(text(), 'Usuario CRM')]")).Click();
                Thread.Sleep(3000);

                // Divisa
                /*if(!Utils.EncontrarElemento(By.XPath("//div[contains(@aria-label ,'Porfolio lyntia')]")))
                {
                    CommonActions.getElement("Oferta.inputListadeprecios").Click();
                    Thread.Sleep(1000);
                    CommonActions.getElement(By.XPath("//span[contains(text(), '2019')]")).Click();
                    //CommonActions.getElement(By.XPath("//span[contains(@data-id, 'transactioncurrencyid_microsoftIcon_cancelButton')]")).Click();
                }*/

                Thread.Sleep(2000);
                if(Utils.EncontrarElemento(Utils.getByElement("Oferta.buttonAceptarVentanaEmergente")))
                    CommonActions.waitAndClick("Oferta.buttonAceptarVentanaEmergente");

                TestContext.WriteLine("Se eliminan los campos obligatorios de la prueba.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EliminarCampos.png", "No se eliminan los campos obligatorios de la prueba.");
                throw e;
            }

        }

        /// <summary>
        /// Método para modificar datos de la Oferta.
        /// </summary>
        public void Modificar_campos_obligatorios(Dictionary<string,string> datosTestCase)
        {
            try
            {
                // Modificar el nombre de la oferta
                CommonActions.rellenarCampoInput("Oferta.inputNameOferta", datosTestCase["nombreOferta"] + Oferta.getFechaHoraActual());

                // Modificar el cliente
                CommonActions.rellenarCampoConDesplegable("Oferta.inputCustomerId", datosTestCase["cliente"]);

                // Modificar tipo de oferta
                CommonActions.getElement("Oferta.selectOfertaType").Click();
                accionesSelenium.SendKeys(Keys.PageDown);
                accionesSelenium.Build().Perform();
                Thread.Sleep(2000);

                // Modificar divisa
                if (!Utils.EncontrarElemento(By.XPath("//div[@aria-label = 'Euro']")))
                {
                    CommonActions.rellenarCampoConDesplegable("Oferta.inputDivisa", datosTestCase["divisa"]);
                    CommonActions.waitAndClick("Oferta.buttonAceptarVentanaEmergente");
                    Thread.Sleep(2000);
                }

                TestContext.WriteLine("Se modifican los campos obligatorios de la prueba.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EliminarCampos.png", "No se modifican los campos obligatorios de la prueba.");
                throw e;
            }

        }

        /// <summary>
        /// Método Cerrar Oferta
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="razonOferta"></param>
        /// <param name="motivoCierre"></param>
        public void CancelarOfertaActual(Dictionary<string, string> datosCierreOferta) 
        {
            try
            {
                // Click en Cerrar Oferta
                Thread.Sleep(4000);
                CommonActions.waitAndClick("Oferta.buttonCancelar");
                Thread.Sleep(4000);

                // Cambiar al frame de Cierre de Ofertas
                driver.SwitchTo().Frame(CommonActions.getElement("Oferta.frameCerrarOferta"));

                if (!datosCierreOferta["motivo"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Oferta.selectMotivoCierre", datosCierreOferta["motivo"]);
                    Thread.Sleep(3000);
                }

                if (!datosCierreOferta["consecuencia"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Oferta.selectConsecuenciaCierre", datosCierreOferta["consecuencia"]);
                }

                CommonActions.rellenarCampoInput("Oferta.inputFechaCierre", datosCierreOferta["fechaCierre"].Equals("") ? Utils.getFechaActual() : datosCierreOferta["fechaCierre"]);
                
                if (datosCierreOferta["opcion"].Equals("Aceptar"))
                {
                    // Cierre realizado
                    CommonActions.waitAndClick("Oferta.buttonConfirmarCierre");
                    esperarCierreOferta();
                    TestContext.WriteLine("Se completa correctamente el proceso de cierre de Oferta.");
                    driver.SwitchTo().DefaultContent();
                }
                else
                {
                    // Cierre anulado
                    CommonActions.waitAndClick("Oferta.buttonCancelarCierre");
                    Thread.Sleep(8000);
                    TestContext.WriteLine("Se cancela correctamente el proceso de cierre de Oferta.");
                    driver.SwitchTo().DefaultContent();
                }
                
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "CierreOferta.png", "No se completa el proceso de cierre de Oferta (" + datosCierreOferta["opcion"] + ").");
                throw e;
            }

            Thread.Sleep(3000);
        }

        /// <summary>
        /// Método Cerrar Oferta
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="razonOferta"></param>
        /// <param name="motivoCierre"></param>
        public void CerrarOfertaActual(Dictionary<string, string> datosCierreOferta) //Cambiar nombre a CancelarOferta
        {
            try
            {
                // Click en Cerrar Oferta
                Thread.Sleep(4000);
                //CommonActions.waitAndClick("Oferta.buttonCerrar");
                Thread.Sleep(4000);

                // Cambiar al frame de Cierre de Ofertas
                driver.SwitchTo().Frame(CommonActions.getElement("Oferta.frameCerrarOferta"));

                if (!datosCierreOferta["motivo"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Oferta.selectMotivoCierre", datosCierreOferta["motivo"]);
                    Thread.Sleep(3000);
                }

                if (!datosCierreOferta["consecuencia"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Oferta.selectConsecuenciaCierre", datosCierreOferta["consecuencia"]);
                }

                CommonActions.rellenarCampoInput("Oferta.inputFechaCierre", datosCierreOferta["fechaCierre"].Equals("") ? Utils.getFechaActual() : datosCierreOferta["fechaCierre"]);

                if (datosCierreOferta["opcion"].Equals("Aceptar"))
                {
                    // Cierre realizado
                    CommonActions.waitAndClick("Oferta.buttonConfirmarCierre");
                    esperarCierreOferta();
                    TestContext.WriteLine("Se completa correctamente el proceso de cierre de Oferta.");
                    driver.SwitchTo().DefaultContent();
                }
                else
                {
                    // Cierre anulado
                    CommonActions.waitAndClick("Oferta.buttonCancelarCierre");
                    Thread.Sleep(8000);
                    TestContext.WriteLine("Se cancela correctamente el proceso de cierre de Oferta.");
                    driver.SwitchTo().DefaultContent();
                }

            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "CierreOferta.png", "No se completa el proceso de cierre de Oferta (" + datosCierreOferta["opcion"] + ").");
                throw e;
            }

            Thread.Sleep(3000);
        }

        public void AccederOfertaestado_Adjudicada(Dictionary<string, string> datosTestCase)
        {
            try
            {
                Thread.Sleep(2000);
                CommonActions.agregarFiltroAvanzado("Razón para el estado", "Adjudicada");
                Thread.Sleep(500);
                BuscarOfertaEnVista(datosTestCase["nombreOferta"]);
                CommonActions.getElement("Oferta.Grid.selectLine2").Click();//seleccionamos una oferta ganada y pulsamos sobre el check
                Thread.Sleep(500);
                TestContext.WriteLine("Se accede a una Oferta Adjudicada.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AccesoOfertaAdjudicada.png", "No se accede a una Oferta Adjudicada.");
                throw e;
            }
        }

        /// <summary>
        /// Método para filtrar Oferta por ID de Revisión
        /// </summary>
        /// <param name="idRevision"></param>
        public void FiltrarPorIDRevision(string idRevision)
        {
            try
            {
                CommonActions.waitAndClick("Oferta.gridFilterIdRevision");
                CommonActions.waitAndClick("Oferta.gridFilterBuscarPor");
                CommonActions.rellenarCampoInput("Oferta.gridFilterBuscarPorInput", idRevision);
                Thread.Sleep(2000);
                TestContext.WriteLine("Se filtra correctamente la Oferta por ID de revisión: " + idRevision + ".");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "FiltradoIdRevisionOferta.png", "No se filtra la Oferta por ID de revisión: " + idRevision + ".");
                throw e;
            }
        }

        //public void FiltrarPorEstado(string estado)
        //{
        //    try
        //    {
        //        CommonActions.waitElement("Oferta.gridFilterEstado");
        //        CommonActions.getElement("Oferta.gridFilterEstado").Click();
        //        CommonActions.getElement("Oferta.gridFilterBuscarPor").Click();
        //        CommonActions.getElement("Oferta.gridFilterBuscarPorInput").Click();
        //        CommonActions.getElement(By.XPath("//div[contains(@class,'Check')]//span[contains(text(),'"+ estado +"')]")).Click();
        //        CommonActions.getElement(By.XPath("//h3[text()='Filtrar por']")).Click();
        //        CommonActions.getElement("Oferta.gridFilterBuscarPorAceptarButton").Click();
        //        Thread.Sleep(2000);
        //        TestContext.WriteLine("Se filtra correctamente la Oferta por ID de revisión: " + estado + ".");
        //    }
        //    catch (Exception e)
        //    {
        //        CommonActions.CapturadorExcepcion(e, "FiltradoEstadoOferta.png", "No se filtra la Oferta por estado: " + estado + ".");
        //        throw e;
        //    }
        //}        

        /// <summary>
        /// Método para seleccionar todas las Ofertas en vista del grid
        /// </summary>
        public void SeleccionarTodasOfertaGrid()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.gridSelectAll");
                Thread.Sleep(2000);
                TestContext.WriteLine("Se seleccionan todas las ofertas en vista del grid.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "SeleccionTodasOfertas.png", "No se seleccionan todas las ofertas en vista del grid.");
                throw e;
            }
        }

        /// <summary>
        /// Método para pulsar Eliminar en la barra de Ofertas
        /// </summary>
        public void eliminar_ofertaDetalle(String opcion)
        {
            try
            {
                CommonActions.waitAndClick("Oferta.buttonEliminar"); //pulsamos sobre eliminar de la barra superior del menu
                if (opcion.Equals("Eliminar"))
                {
                    CommonActions.waitAndClick("Oferta.confirmDeleteOferta");
                    Thread.Sleep(4000);
                    TestContext.WriteLine("Se elimina la Oferta correctamente.");
                    Thread.Sleep(2000);
                }
                else
                {
                    CommonActions.waitAndClick("Oferta.cancelDeleteOferta");
                    Thread.Sleep(4000);
                    TestContext.WriteLine("Se cancela el proceso de eliminación.");
                }
                TestContext.WriteLine("Se pulsa correctamente Eliminar.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EliminarBarraMenuOfertas.png", "No se pulsa Eliminar.");
                throw e;
            }
        }

        /// <summary>
        /// Método para cancelar popup de eliminación
        /// </summary>
        public void Cancelar()
        {
            try
            {
                CommonActions.waitAndClick("Producto.buttonCancelar"); //cancelar del pop up
                TestContext.WriteLine("Se cancela el popup de error al intentar eliminar.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "PopupEliminarOfertas.png", "No se cancelar el popup de eliminar Oferta.");
                throw e;
            }
        }

        /// <summary>
        /// Método para acceder a Oferta Adjudicada
        /// </summary>
        public void Seleccionofertarazonadjudicada()
        {
            try
            {
                BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"]);
                //Thread.Sleep(3000);
                CommonActions.waitAndClick("Oferta.labelAdjudicada"); //seleccionamos una oferta ganada y pulsamos sobre el ckeck
                //Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.buttonEditar"); //pulsamos sobre editar
                //Thread.Sleep(3000);
                TestContext.WriteLine("Se accede a Oferta adjudicada.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EditarOfertaAdjudicada.png", "No se accede a Oferta adjudicada.");
                throw e;
            }
        }

        public void Seleccionofertarazonadjudicada(Dictionary<string, string> datosTestCase)
        {
            try
            {
                BuscarOfertaEnVista(Utils.getDatosTestCase("oferta")["nombreOferta"]);
                Thread.Sleep(3000);
                CommonActions.waitAndClick("Oferta.labelAdjudicada"); //seleccionamos una oferta ganada y pulsamos sobre el ckeck
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.buttonEditar"); //pulsamos sobre editar
                Thread.Sleep(3000);
                TestContext.WriteLine("Se accede a Oferta adjudicada.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "EditarOfertaAdjudicada.png", "No se accede a Oferta adjudicada.");
                throw e;
            }
        }

        /// <summary>
        /// Método para confirmar eliminación desde popup
        /// </summary>
        public void Eliminar_Popup(){
            try
            {
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.confirmDeleteOferta");
                TestContext.WriteLine("Se confirma el borrado desde el popup.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ConfirmarPopupEliminarOferta.png", "No se confirma el borrado desde el popup.");
                throw e;
            }
        }

        public void presentarOferta()
        {
            Thread.Sleep(2000);
            if (!Utils.EncontrarElemento(Utils.getByElement("Oferta.buttonPresentOferta")))
                CommonActions.waitAndClick("Oferta.saveOferta");         
            CommonActions.waitAndClick("Oferta.buttonPresentOferta");
            Thread.Sleep(3000);            
            if (Utils.EncontrarElemento(Utils.getByElement("Oferta.buttonPresentOferta")))
                Thread.Sleep(2000);
            CommonActions.waitAndClick("BarraHerramientas.buttonActualizar");
            Thread.Sleep(2000);         

            if (Utils.EncontrarElemento(Utils.getByElement("Producto.buttonGuardaryContinuar")))
                CommonActions.waitAndClick("Producto.buttonGuardaryContinuar");
            Thread.Sleep(2000);
        }

        public void PresentarOferta(string fechaPresentacion)
        {
            try
            {
                CommonActions.waitAndClick("Oferta.LabelFechasPestaña");
                CommonActions.rellenarCampoInput("Oferta.fechaPresentacion", fechaPresentacion);
                CommonActions.waitAndClick("Oferta.buttonPresentOferta");
                TestContext.WriteLine("Se accede a la ventana de Presentar Oferta.");
                Thread.Sleep(3000);
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "PresentarOferta.png", "No se accede a la ventana de Presentar Oferta.");
                throw e;
            }
        }

        /// <summary>
        /// Reestablecimiento de datos de la prueba CRM-EOF0004
        /// </summary>
        public void ReestablecerDatosCRM_EOF0004()
        {
            try
            {
                //reestablece datos CRM-EOF0004

                CommonActions.seleccionarElementoTabla(0,1);
                CommonActions.waitAndClick("Oferta.buttonEditOferta");
                CommonActions.getElement("Oferta.inputNameOferta").SendKeys(Keys.PageDown);

                CommonActions.seleccionarEnCampoDesplegable("Oferta.selectOfertaType", "Nuevo servicio");

                GuardarYCerrarOferta();
                TestContext.WriteLine("Se reestablecen los datos de la prueba EOF0004.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ReestablecimientoEOF0004.png", "No se reestablecen los datos de la prueba EOF0004.");
                throw e;
            }
        }   
        
        /// <summary>
        /// Método para presentar Oferta
        /// </summary>
        public void Adjudicar_Oferta()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.buttonAdjudicarOferta");

                // Ventana emergente límite 60000€ - En algunas aparece, en otras no
                 //CommonActions.waitAndClick("Oferta.confirmDeleteOferta");                
                TestContext.WriteLine("Se accede a la ventana de Adjudicar Oferta.");
            }
            catch (ElementClickInterceptedException e)
            {
                if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonCancelar")))
                    CommonActions.getElement("Producto.botonCancelar");                
            }
        }
        /// <summary>
        /// Método para controlar fecha de la venta crear pedido de una oferta adjudicada
        /// </summary>
        public void VentanaCrearPedido()
        {
            try
            {
                driver.SwitchTo().Frame("FullPageWebResource");
                CommonActions.waitAndClick("Oferta.buttonAceptarVentanaEmergente");
                driver.SwitchTo().DefaultContent();
                TestContext.WriteLine("se continua con el pedido correctamente.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "VentanaCrearPedido.png", "No se continua con el pedido correctamente.");
                throw e;
            }
        }

        /// <summary>
        /// Método para indicar en la fecha el dia siguiente al actual
        /// </summary>
        public void VentanaCrearPedidofechaposterior(string fecha)
        {
            try
            {
                if (Utils.EncontrarElemento(Utils.getByElement("Oferta.capex")))
                    CommonActions.waitAndClick("Oferta.capex");
               

                driver.SwitchTo().Frame("FullPageWebResource");
                CommonActions.getElement("Oferta.fechaLogro").SendKeys(fecha.Replace("/", "-"));
                CommonActions.getElement("Oferta.crearPedidoDescripcion").SendKeys("descripcion");
                //Thread.Sleep(4000);
                CommonActions.waitAndClick("Oferta.buttonConfirmarCierre");
                //Thread.Sleep(4000);
                driver.SwitchTo().DefaultContent();

                //Thread.Sleep(3000);
                if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonConfirmar")))
                    CommonActions.waitAndClick("Producto.botonConfirmar");   
                

                TestContext.WriteLine("se continua con el pedido correctamente.");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "VentanaCrearPedido.png", "No se continua con el pedido correctamente.");
                throw e;
            }
        }

        public void salirOfertaCOF0011()
        {
            CommonActions.getElement("Oferta.ofertaSection").Click();
            CommonActions.getElement("Oferta.buttonDescartar").Click();
        }

        /// <summary>
        /// Método para acceder a proyectos del Grid
        /// </summary>
        public void AccesoServiciosGrid()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.labelServicioscontratadosGrid");
                Thread.Sleep(2000);

                TestContext.WriteLine("Se accede correctamente a la opcion de Servicios contratados");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AccesoProyectosGrid.png", "No se accede correctamente a la opcion de Servicios contratados");
                throw e;
            }
        }

        /// <summary>
        /// Método para actualizar desde la barra de menu superior con acciones guardar y descartar
        /// </summary>
        public void Actualizar(String accion)
        {
            try
            {
                Thread.Sleep(3000);
                CommonActions.waitAndClick("Oferta.buttonActualizar");
                if (accion.Equals("Guardar"))
                {
                    Thread.Sleep(2000);
                    CommonActions.waitAndClick("Producto.buttonGuardaryContinuar");
                    Thread.Sleep(4000);
                }
                else
                {
                    //cambios no guardados
                    Thread.Sleep(2000);
                    CommonActions.waitAndClick("Oferta.ButtonDescartarcambios");
                }
                    TestContext.WriteLine("La oferta se actualiza correctamente");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Actualizar.png", "La oferta no se ha actualizado correctamente");
                throw e;
            }
            
        }

        /// <summary>
        /// Método para actualizar desde la barra de menu superior
        /// </summary>
        public void ActualizarBarramenu()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.buttonActualizar");
                Thread.Sleep(500);

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.buttonGuardaryContinuar")))
                    CommonActions.waitAndClick("Producto.buttonGuardaryContinuar");
                
                Thread.Sleep(3000);
                TestContext.WriteLine("La oferta se actualiza correctamente");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ActualizarBarraMenu.png", "La oferta no se ha actualizado correctamente");
                throw e;
            }        

        }

        /// <summary>
        /// Método para acceder a servicios contratados
        /// </summary>
        public void Acceso_Servicios_contratados()
        {
            try
            {
                CommonActions.waitAndClick("Oferta.GridServiciosContratados");
                Thread.Sleep(3000);

                TestContext.WriteLine("Se accede correctamente a Servicios contratados");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Acceso servicios contratados.png", "No se accede a servicios contratados");
                throw e;
            }
        }

        /// <summary>
        /// Método para acceder a la primera linea de los productos
        /// </summary>
        public void acceder_A_linea1()
        {
            //Pulsar sobre el primer registro y editar 
            CommonActions.waitAndClick("Producto.Grid.SelectLine1");
            CommonActions.waitAndClick("Oferta.editarProducto");
        }
        public void acceder_A_linea2()
        {
            //Pulsar sobre el primer registro y editar 
            CommonActions.waitAndClick("Producto.Grid.SelectLine2");
            CommonActions.waitAndClick("Oferta.editarProducto");
        }

        /// <summary>
        /// Método para acceder a servicios contratados
        /// </summary>
        public void rellenarCamposObligatoriosProducto(string tipoProducto, Dictionary<string, string> datosTestCase)
        {
            
            // Pestaña General
            if (!datosTestCase["nombreAnexo"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaGeneralNombreAnexo")))
                CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);
            Thread.Sleep(2000);
            // Seleccion IRU/LEASE
            if (tipoProducto.Equals("Fibra Billing") && datosTestCase["opcionModalidad"].Equals("IRU"))
            {
                // Seleccion de IRU
                CommonActions.rellenarCampoConDesplegable("Producto.PestañanaGeneralIRU", "Normal");
                Thread.Sleep(3000);
            }

            if (!tipoProducto.Equals("RACK")) 
            { 
                // Pestaña Características
                CommonActions.waitAndClick("Producto.PestañaCaracteristicas");
                Thread.Sleep(2000);
                // Seleccion de Red
                if ((tipoProducto.Equals("CC") || tipoProducto.Equals("Fibra") || tipoProducto.Equals("Fibra Billing")) && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasRED")))
                   // CommonActions.waitAndClick("Producto.PestañaCaracteristicasRED"); //swich para ON/OFF red
                // Rellenar Num. Unidades
                if (!datosTestCase["numUnidades"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasNUMUNIDADES")))
                    if (Boolean.Parse(CommonActions.getElement("Producto.PestañaCaracteristicasRED").GetDomProperty("isContentEditable")))
                        CommonActions.rellenarCampoInput("Producto.PestañaCaracteristicasNUMUNIDADES", datosTestCase["numUnidades"]);
                // Seleccion de Operador ultima milla
                if (!datosTestCase["operador"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasOPERADOR")))
                    CommonActions.rellenarCampoConDesplegable("Producto.PestañaCaracteristicasOPERADOR", datosTestCase["operador"]);
                // Seleccion de Ambito
                if (!datosTestCase["ambito"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasAMBITO")))
                    CommonActions.seleccionarEnCampoDesplegable("Producto.PestañaCaracteristicasAMBITO", datosTestCase["ambito"]);
            }

            //Pestaña Direcciones y Coordenadas (datos obligatorios)
            /// </summary>
            /// Sitio origen, destino, Direccion origen, destino
            CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
            Thread.Sleep(2000);
            if (!datosTestCase["sitioOrigen"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaDireccionesYcoordenadasSITIORIGEN")))
                CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
            if (!datosTestCase["sitioDestino"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaDireccionesYcoordenadasSITIODESTINO")))
                CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIODESTINO", datosTestCase["sitioDestino"]);
            if (!datosTestCase["direccionOrigen"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN")))
                CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);
            if (!datosTestCase["direccionDestino"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO")))
                CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO", datosTestCase["direccionDestino"]);
            Thread.Sleep(2000);

            //Pestaña Jira (datos obligatorios)
            /// </summary>
            /// Capex fibra, equipo, contacto, fecha compromiso y detalle
            CommonActions.waitAndClick("Producto.PestañaJira");
            Thread.Sleep(2000);
            // Capex fibra y equipo
            if (!datosTestCase["capexFibra"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraKAPEXFIBRA")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);
            //input[@aria-label="Fibra óptica"]
            Thread.Sleep(2000);

            if(Utils.EncontrarElemento(Utils.getByElement("Oferta.capex")))
                // Mensaje CAPEX superior a 60.000€ en secan3 y 4
                CommonActions.waitAndClick("Oferta.capex"); 

            if (!datosTestCase["capexEquipos"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraKAPEXEQUIPOS")))
                
                CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXEQUIPOS", datosTestCase["capexEquipos"]);
            Thread.Sleep(2000);
            // Contacto
            if (!datosTestCase["contacto"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraCONTACTO")))
                CommonActions.rellenarCampoConDesplegable("Producto.PestañaJiraCONTACTO", datosTestCase["contacto"]);
            // Contacto local
            if (!datosTestCase["contactoLocal"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraCONTACTOLocal")))
                
                CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);
            Thread.Sleep(2000);
            // Detalle de servicio y fecha de compromiso
            if (!datosTestCase["detallesServicio"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraDETALLESERVICIO")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
            Thread.Sleep(2000);
            if (!datosTestCase["fechaCompromiso"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraFECHACOMPROMISO")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);
            Thread.Sleep(2000);

            //Pestaña Billing (datos obligatorios)
            CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
            Thread.Sleep(2000);
            //Contrato Marco
            if (!datosTestCase["contratoMarco"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingCONTRATOMARCO")))
                CommonActions.rellenarCampoConDesplegable("Producto.PestañaContratosYbillingCONTRATOMARCO", datosTestCase["contratoMarco"]);
            // UTPRX
            //if (!datosTestCase["utprx"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingUTPRX")))
                //CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingUTPRX", datosTestCase["utprx"]);
            // Tarea
            //if (!datosTestCase["codigoTarea"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingTAREA")))
                //CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingTAREA", datosTestCase["codigoTarea"]);

            // Añadir Modalidad Facturacion
            CommonActions.seleccionarEnCampoDesplegable("ServiciosContratados.ModalidadFacturacion", datosTestCase["modalidadFacturacion"]);

                // Nombre Anexo
                if (!datosTestCase["nombreAnexo"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingNOMBREANEXO")))
                CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingNOMBREANEXO", datosTestCase["nombreAnexo"]);

            
                if (tipoProducto.Equals("Contratos y Billing"))
            {
                // Modalidad Facturacion
                /*if (!datosTestCase["modalidadFacturacion"].Equals("") && Utils.EncontrarElemento(Utils.getByElement("ServiciosContratados.ModalidadFacturacion")))
                    CommonActions.seleccionarEnCampoDesplegable("ServiciosContratados.ModalidadFacturacion", datosTestCase["modalidadFacturacion"]);*/
                
                //Pestaña Fechas
                CommonActions.waitAndClick("Producto.PestañaFechas");
                Thread.Sleep(2000);
                CommonActions.rellenarCampoInput("Producto.PestañaFechas.inputFechaInicioFacturacion", datosTestCase["fechaInicioFacturacion"].Equals("") ? Utils.getFechaActual() : datosTestCase["fechaInicioFacturacion"]);
                
                //Alert
                Thread.Sleep(500);
                if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonConfirmar")))
                    CommonActions.waitAndClick("Producto.botonConfirmar");
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.saveOferta");
                CommonActions.esperarGuardado();
                
            }
            else 
            {
                //Pestaña Fechas
                CommonActions.waitAndClick("Producto.PestañaFechas");
                Thread.Sleep(2000);
                CommonActions.rellenarCampoInput("Producto.PestañaFechas.inputFechaInicioFacturacion", datosTestCase["fechaInicioFacturacion"].Equals("") ? Utils.getFechaActual() : datosTestCase["fechaInicioFacturacion"]);

                //Alert
                Thread.Sleep(500);
                if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonConfirmar")))
                    CommonActions.waitAndClick("Producto.botonConfirmar");
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.saveOferta");
                CommonActions.esperarGuardado();
            }
            Thread.Sleep(10000);
        }

        ///// <summary>
        ///// Método para acceder a servicios contratados
        ///// </summary>
        //public void CamposObligatoriosProductoCC(Dictionary<string, string> datosTestCase)
        //{
        //    // General
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaGeneralNombreAnexo").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);
        //    Thread.Sleep(2000);
        //    CommonActions.waitAndClick("Producto.PestañaCaracteristicas");
        //    Thread.Sleep(2000);

        //    // Seleccion de Red
        //    CommonActions.waitAndClick("Producto.PestañaCaracteristicasRED"); //swich para ON/OFF red

        //    // Seleccion de Operador ultima milla
        //    if (!datosTestCase["operador"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasOPERADOR").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaCaracteristicasOPERADOR", datosTestCase["operador"]);

        //    // Seleccion de Ambito
        //    if (!datosTestCase["ambito"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasAMBITO").GetAttribute("aria-required") == "true")
        //        CommonActions.seleccionarEnCampoDesplegable("Producto.PestañaCaracteristicasAMBITO", datosTestCase["ambito"]);

        //    //Pestaña Direcciones y Coordenadas (datos obligatorios)
        //    /// </summary>
        //    /// Sitio origen, destino, Direccion origen, destino
        //    CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
        //    Thread.Sleep(2000);
        //    if (!datosTestCase["sitioOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
        //    if (!datosTestCase["sitioDestino"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIODESTINO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIODESTINO", datosTestCase["sitioDestino"]);
        //    if (!datosTestCase["direccionOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);
        //    if (!datosTestCase["direccionDestino"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO", datosTestCase["direccionDestino"]);
        //    Thread.Sleep(2000);

        //    //Pestaña Jira (datos obligatorios)
        //    /// </summary>
        //    /// Capex fibra, equipo, contacto, fecha compromiso y detalle
        //    CommonActions.waitAndClick("Producto.PestañaJira");
        //    Thread.Sleep(2000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexFibra"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXFIBRA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);
        //    Thread.Sleep(2000);
        //    if (!datosTestCase["capexEquipos"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXEQUIPOS").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXEQUIPOS", datosTestCase["capexEquipos"]);    
        //    Thread.Sleep(2000);

        //    //Contacto local
        //    CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);

        //    // Detalle de servicio y fecha de compromiso
        //    if (!datosTestCase["detallesServicio"].Equals("") && CommonActions.getElement("Producto.PestañaJiraDETALLESERVICIO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
        //    Thread.Sleep(2000);
        //    if (!datosTestCase["fechaCompromiso"].Equals("") && CommonActions.getElement("Producto.PestañaJiraFECHACOMPROMISO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);
        //    Thread.Sleep(2000);

        //    //Pestaña Billing (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
        //    Thread.Sleep(2000);
        //    //&& CommonActions.getElement("Oferta.inputUTPRx").GetAttribute("aria-required") == "true"
        //    if(!datosTestCase["utprx"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingUTPRX").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingUTPRX", datosTestCase["utprx"]);
        //    if(!datosTestCase["codigoTarea"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingTAREA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingTAREA", datosTestCase["codigoTarea"]);
        //    Thread.Sleep(2000);

        //    if (!Utils.EncontrarElemento(By.XPath("//div[@title = '"+ datosTestCase["contratoMarco"] + "']")))
        //    {
        //        if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingCONTRATOMARCO")))
        //            CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingCONTRATOMARCO", datosTestCase["contratoMarco"]);
        //    }

        //    CommonActions.waitAndClick("Oferta.saveAndCloseOferta");
        //    Thread.Sleep(17000);
        //}

        ///// <summary>
        ///// Método para acceder a servicios contratados
        ///// </summary>
        //public void CamposObligatoriosProductoFIBRA(Dictionary<string, string> datosTestCase)
        //{
        //    // General
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaGeneralNombreAnexo").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);
        //    Thread.Sleep(2000);

        //    // Seleccion IRU/LEASE
        //    if (datosTestCase["opcionModalidad"].Equals("IRU"))
        //    {
        //        // Seleccion de IRU
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañanaGeneralIRU", "Normal");
        //        Thread.Sleep(3000);
        //    }

        //    ////Pestaña Caracteristicas (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaCaracteristicas");
        //    Thread.Sleep(2000);

        //    // Seleccion de Red
        //    CommonActions.getElement("Producto.PestañaCaracteristicasRED").Click();//swich para ON/OFF red
        //    Thread.Sleep(2000);

        //    // Seleccion de Ambito
        //    if (!datosTestCase["ambito"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasAMBITO").GetAttribute("aria-required") == "true")
        //        CommonActions.seleccionarEnCampoDesplegable("Producto.PestañaCaracteristicasAMBITO", datosTestCase["ambito"]);

        //    //Operador ultima milla
        //    if (!datosTestCase["operador"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasOPERADOR").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaCaracteristicasOPERADOR", datosTestCase["operador"]);

        //    //Pestaña Direcciones y Coordenadas (datos obligatorios)
        //    /// </summary>
        //    /// Sitio origen, destino, Direccion origen, destino
        //    CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
        //    Thread.Sleep(2000);

        //    if (!datosTestCase["sitioOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
        //    if (!datosTestCase["sitioDestino"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIODESTINO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIODESTINO", datosTestCase["sitioDestino"]);
        //    if (!datosTestCase["direccionOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);
        //    if (!datosTestCase["direccionDestino"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO", datosTestCase["direccionDestino"]);
        //    Thread.Sleep(2000);

        //    //Pestaña Jira (datos obligatorios)
        //    /// </summary>
        //    /// Capex fibra, equipo, contacto, fecha compromiso y detalle
        //    CommonActions.waitAndClick("Producto.PestañaJira");
        //    Thread.Sleep(3000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexFibra"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXFIBRA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);
        //    Thread.Sleep(3000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexEquipos"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXEQUIPOS").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXEQUIPOS", datosTestCase["capexEquipos"]);

        //    //Contacto Local
        //    if (!datosTestCase["contactoLocal"].Equals("") && CommonActions.getElement("Producto.PestañaJiraCONTACTOLocal").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);

        //    // Detalle de servicio y fecha de compromiso
        //    if (!datosTestCase["detallesServicio"].Equals("") && CommonActions.getElement("Producto.PestañaJiraDETALLESERVICIO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
        //    if (!datosTestCase["fechaCompromiso"].Equals("") && CommonActions.getElement("Producto.PestañaJiraFECHACOMPROMISO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);

        //    //Pestaña Billing (datos obligatorios)
        //    /// </summary>
        //    /// Contrato marco, Actualizacion precio, periodicidad, UTPRX, codigo tarea, sociedad de facturacion, limite ipc
        //    CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
        //    Thread.Sleep(2000);

        //    // UTPRX
        //    if (!datosTestCase["utprx"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingUTPRX").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingUTPRX", datosTestCase["utprx"]);

        //    // Tarea
        //    if (!datosTestCase["codigoTarea"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingTAREA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingTAREA", datosTestCase["codigoTarea"]);

        //    // Nombre anexo
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingNOMBREANEXO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingNOMBREANEXO", datosTestCase["nombreAnexo"]);

        //    // Limite IPC
        //    // CommonActions.getElement("Producto.PestañaContratosYbillingLIMITEIPC").SendKeys("10");
        //    Thread.Sleep(3000);

        //    //GuardaryCerrarOferta
        //    GuardarYCerrarOferta();            
        //    Thread.Sleep(1000);
        //}

        //public void CamposObligatoriosProductoFIBRABilling(Dictionary<string, string> datosTestCase)
        //{
        //    // General
        //    Thread.Sleep(2000);
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaGeneralNombreAnexo").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);
        //    Thread.Sleep(2000);

        //    // Seleccion IRU/LEASE
        //    if (datosTestCase["opcionModalidad"].Equals("IRU"))
        //    {
        //        // Seleccion de IRU
        //        Thread.Sleep(3000);
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañanaGeneralIRU", datosTestCase["iru"]);
        //        Thread.Sleep(3000);
        //    }

        //    ////Pestaña Caracteristicas (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaCaracteristicas");
        //    Thread.Sleep(2000);
        //    //Rellenar Num. Unidades
        //    if (!datosTestCase["numUnidades"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasNUMUNIDADES").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaCaracteristicasNUMUNIDADES", datosTestCase["numUnidades"]);
        //    // Seleccion de Red
        //    CommonActions.waitAndClick("Producto.PestañaCaracteristicasRED"); //swich para ON/OFF red
        //    Thread.Sleep(2000);
        //    // Seleccion de Ambito
        //    if (!datosTestCase["ambito"].Equals("") && CommonActions.getElement("Producto.PestañaCaracteristicasAMBITO").GetAttribute("aria-required") == "true")
        //        CommonActions.seleccionarEnCampoDesplegable("Producto.PestañaCaracteristicasAMBITO", datosTestCase["ambito"]);

        //    //Pestaña Direcciones y Coordenadas (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
        //    Thread.Sleep(2000);
        //    CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
        //    CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIODESTINO", datosTestCase["sitioDestino"]);
        //    CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);
        //    CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONDESTINO", datosTestCase["direccionDestino"]);
        //    Thread.Sleep(2000);


        //    //Pestaña Jira (datos obligatorios) 
        //    CommonActions.waitAndClick("Producto.PestañaJira");
        //    Thread.Sleep(3000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexFibra"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXFIBRA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);

        //    if (!datosTestCase["contactoLocal"].Equals("") && CommonActions.getElement("Producto.PestañaJiraCONTACTOLocal").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);

        //    // Detalle de servicio y fecha de compromiso
        //    if (!datosTestCase["detallesServicio"].Equals("") && CommonActions.getElement("Producto.PestañaJiraDETALLESERVICIO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
        //    if (!datosTestCase["fechaCompromiso"].Equals("") && CommonActions.getElement("Producto.PestañaJiraFECHACOMPROMISO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);

        //    //Pestaña Billing (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
        //    Thread.Sleep(2000);

        //    //Contrato Marco
        //    if (!datosTestCase["contratoMarco"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingCONTRATOMARCO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaContratosYbillingCONTRATOMARCO", datosTestCase["contratoMarco"]);

        //    // UTPRX
        //    if (!datosTestCase["utprx"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingUTPRX").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingUTPRX", datosTestCase["utprx"]);

        //    // Tarea
        //    if (!datosTestCase["codigoTarea"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingTAREA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingTAREA", datosTestCase["codigoTarea"]);

        //    // Nombre Anexo
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingNOMBREANEXO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingNOMBREANEXO", datosTestCase["nombreAnexo"]);

        //    if (!datosTestCase["modalidadFacturacion"].Equals("") && CommonActions.getElement("ServiciosContratados.ModalidadFacturacion").GetAttribute("aria-required") == "true")
        //        CommonActions.seleccionarEnCampoDesplegable("ServiciosContratados.ModalidadFacturacion", datosTestCase["modalidadFacturacion"]);
        //    CommonActions.waitAndClick("Oferta.saveOferta");
        //    CommonActions.esperarGuardado();

        //    //Pestaña Fechas
        //    CommonActions.waitAndClick("Producto.PestañaFechas");
        //    Thread.Sleep(2000);
        //    CommonActions.rellenarCampoInput("Producto.PestañaFechas.inputFechaInicioFacturacion", datosTestCase["fechaInicioFacturacion"].Equals("") ? Utils.getFechaActual() : datosTestCase["fechaInicioFacturacion"]);
        //    //Alert
        //    if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonConfirmar")))
        //        CommonActions.waitAndClick("Producto.botonConfirmar");

        //    CommonActions.waitAndClick("Oferta.saveOferta");
        //    CommonActions.esperarGuardado();
        //}

        //public void CamposObligatoriosProductoUbiredPRO(Dictionary<string,string> datosTestCase)
        //{
        //    // General
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaGeneralNombreAnexo").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);

        //    //Pestaña Direcciones y Coordenadas (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
        //    Thread.Sleep(2000);

        //    if (!datosTestCase["sitioOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
        //    if (!datosTestCase["direccionOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);

        //    //Pestaña Jira (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaJira");
        //    Thread.Sleep(2000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexFibra"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXFIBRA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);
        //    if (!datosTestCase["capexEquipos"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXEQUIPOS").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXEQUIPOS", datosTestCase["capexEquipos"]);

        //    // Detalle de servicio y fecha de compromiso
        //    if (!datosTestCase["detallesServicio"].Equals("") && CommonActions.getElement("Producto.PestañaJiraDETALLESERVICIO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
        //    //Thread.Sleep(2000);
        //    if (!datosTestCase["fechaCompromiso"].Equals("") && CommonActions.getElement("Producto.PestañaJiraFECHACOMPROMISO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);

        //    // Contacto            
        //    if (!Utils.EncontrarElemento(By.XPath("//div[@title='"+ datosTestCase["contacto"] +"']")))
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaJiraCONTACTO", datosTestCase["contacto"]);

        //    //Rellenar contacto local
        //    if (!datosTestCase["contactoLocal"].Equals("") && CommonActions.getElement("Producto.PestañaJiraCONTACTOLocal").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);

        //    //Pestaña Billing (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
        //    Thread.Sleep(2000);

        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingNOMBREANEXO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingNOMBREANEXO", datosTestCase["nombreAnexo"]);
        //    if (!datosTestCase["contratoMarco"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingCONTRATOMARCO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaContratosYbillingCONTRATOMARCO", datosTestCase["contratoMarco"]);
        //    if (!datosTestCase["utprx"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingUTPRX").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingUTPRX", datosTestCase["utprx"]);
        //    if (!datosTestCase["codigoTarea"].Equals("") && CommonActions.getElement("Producto.PestañaContratosYbillingTAREA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaContratosYbillingTAREA", datosTestCase["codigoTarea"]);

        //    CommonActions.waitAndClick("Oferta.saveAndCloseOferta");
        //    CommonActions.esperarGuardado();

        //    if (Utils.EncontrarElemento(Utils.getByElement("Producto.botonConfirmar")))
        //        CommonActions.waitAndClick("Producto.botonConfirmar");
        //    CommonActions.esperarGuardado();
        //}

        ///// <summary>
        ///// Método para acceder a servicios contratados
        ///// </summary>
        //public void CamposObligatoriosProductoRACK(Dictionary<string, string> datosTestCase)
        //{
        //    // General           
        //    if (!datosTestCase["nombreAnexo"].Equals("") && CommonActions.getElement("Producto.PestañaGeneralNombreAnexo").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaGeneralNombreAnexo", datosTestCase["nombreAnexo"]);

        //    //Pestaña Direcciones y Coordenadas (datos obligatorios)
        //    /// </summary>
        //    /// Sitio origen, destino, Direccion origen, destino
        //    CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
        //    if (!datosTestCase["sitioOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasSITIORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasSITIORIGEN", datosTestCase["sitioOrigen"]);
        //    if (!datosTestCase["direccionOrigen"].Equals("") && CommonActions.getElement("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaDireccionesYcoordenadasDIRECCIONORIGEN", datosTestCase["direccionOrigen"]);

        //    //Pestaña Jira (datos obligatorios)
        //    /// </summary>
        //    /// Capex fibra, equipo, contacto, fecha compromiso y detalle
        //    CommonActions.waitAndClick("Producto.PestañaJira");
        //    Thread.Sleep(1000);

        //    // Capex fibra y equipo
        //    if (!datosTestCase["capexFibra"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXFIBRA").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXFIBRA", datosTestCase["capexFibra"]);
        //    if (!datosTestCase["capexEquipos"].Equals("") && CommonActions.getElement("Producto.PestañaJiraKAPEXEQUIPOS").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraKAPEXEQUIPOS", datosTestCase["capexEquipos"]);

        //    // Detalle de servicio y fecha de compromiso
        //    if (!datosTestCase["detallesServicio"].Equals("") && CommonActions.getElement("Producto.PestañaJiraDETALLESERVICIO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraDETALLESERVICIO", datosTestCase["detallesServicio"]);
        //    if (!datosTestCase["fechaCompromiso"].Equals("") && CommonActions.getElement("Producto.PestañaJiraFECHACOMPROMISO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraFECHACOMPROMISO", datosTestCase["fechaCompromiso"]);

        //    // Contacto 
        //    if (!datosTestCase["contacto"].Equals("") && CommonActions.getElement("Producto.PestañaJiraCONTACTO").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaJiraCONTACTO", datosTestCase["contacto"]);
        //    if (!datosTestCase["contactoLocal"].Equals("") && CommonActions.getElement("Producto.PestañaJiraCONTACTOLocal").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Producto.PestañaJiraCONTACTOLocal", datosTestCase["contactoLocal"]);

        //    //Pestaña Billing (datos obligatorios)
        //    CommonActions.waitAndClick("Producto.PestañaContratosYbilling");
        //    Thread.Sleep(1000);            

        //    if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaContratosYbillingCONTRATOMARCO")))
        //        CommonActions.rellenarCampoConDesplegable("Producto.PestañaContratosYbillingCONTRATOMARCO", datosTestCase["contratoMarco"]);

        //    if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputUTPRx")) && CommonActions.getElement("Oferta.inputUTPRx").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Oferta.inputUTPRx", datosTestCase["utprx"]);

        //    if (Utils.EncontrarElemento(Utils.getByElement("Oferta.inputCodigoTarea")) && CommonActions.getElement("Oferta.inputCodigoTarea").GetAttribute("aria-required") == "true")
        //        CommonActions.rellenarCampoInput("Oferta.inputCodigoTarea", datosTestCase["codigoTarea"]);

        //    CommonActions.waitAndClick("Oferta.saveAndCloseOferta");

        //    CommonActions.esperarGuardado();
        //}

         /// <summary>
         /// Método para Enviar a Jira los productos
         /// </summary>
         public void Enviar_A_Jira()
        {
            CommonActions.waitAndClick("Producto.EnviarAJira");
            Thread.Sleep(5000);
            IWebElement botonAceptar = driver.FindElement(By.XPath("//button[@aria-label='Aceptar']"));
            botonAceptar.Click();
            CommonActions.checkAlert();
        }

        /// <summary>
        /// Método para cancelar producto enviados a Jira
        /// </summary>
        public void Enviar_A_Jira_cancelar()
        {
            
            // Punto critico
            Thread.Sleep(10000);
            CommonActions.waitAndClick("BarraHerramientas.buttonActualizar");
            
            //Pulsar sobre el primer registro y editar
            if (!Utils.EncontrarElemento(Utils.getByElement("Producto.Grid.SelectLine1")))
            {
                Thread.Sleep(2000);
                CommonActions.waitAndClick("BarraHerramientas.buttonActualizar");
                Thread.Sleep(2000);
            }

            CommonActions.waitAndClick("Oferta.LabelFechaspestaña");
            CommonActions.waitAndClick("Producto.PestañaFechas_fechacancelacion");
            CommonActions.waitAndClick("Oferta.ButtonNextday");
            CommonActions.rellenarCampoConDesplegable("Producto.PestañaFechas_fechacancelacion_Motivo", "El Cliente lo entregará con su propia red");
            Thread.Sleep(10000);

            //Punto critico
            CommonActions.waitAndClick("Oferta.saveAndCloseOferta");
            

        }
        
        public void crearNuevoGeneradorFacturacion()
        {
            CommonActions.waitAndClick("Producto.GeneradorFacturacion");
            CommonActions.waitAndClick("BarraHerramientas.buttonNuevo");
            Thread.Sleep(2000);
        }

        public void rellenarCamposNuevoGeneradorFacturacion(string cliente, string nombreServicio)
        {
            //Rellenar Cliente
            // Seleccionar cliente del desplegable
            CommonActions.waitAndClick("Producto.GeneradorFacturacion.inputCliente");
            CommonActions.rellenarCampoConDesplegable("Producto.GeneradorFacturacion.inputCliente", cliente);

            //Rellenar sociedad
            CommonActions.rellenarCampoConDesplegable("Producto.GeneradorFacturacion.inputSociedad", "España");

            //Rellenar servicio contratado
            CommonActions.rellenarCampoConDesplegable("Producto.GeneradorFacturacion.inputServicioContratado", nombreServicio);

            //Guardar oferta
            CommonActions.waitAndClick("Oferta.saveOferta");
            CommonActions.esperarGuardado();
            //Nuevo botón, darle aceptar
            //CommonActions.waitAndClick("Oferta.aceptarBoton");
        }

        public void rellenarUTPRX(string valor)
        {
            CommonActions.rellenarCampoInput("Oferta.inputUTPRx", valor);
        }

        public void rellenarCodigoTarea(string valor)
        {
            CommonActions.rellenarCampoInput("Oferta.inputCodigoTarea", valor);
        }

        public void generarFactura()
        {
            
            CommonActions.esperarGuardado();
            Thread.Sleep(4000);
            //CommonActions.waitAndClick("Oferta.aceptarBoton");
            CommonActions.waitAndClick("BarraHerramientas.GenerarFacturas");
            CommonActions.checkAlert();
            CommonActions.waitAndClick("BarraHerramientas.GenerarFacturas");
            CommonActions.checkAlert();
            CommonActions.checkAlert();
            CommonActions.waitAndClick("Producto.botonOk");
            CommonActions.waitAndClick("BarraHerramientas.buttonGuardaryCerrar");
        }

        public void esperarCierreOferta()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='presentation']//span[text()='Cerrando oferta...']")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
        }

        public void esperarEnvioAJira()
        {
            if(Utils.EncontrarElemento(By.XPath("//div[@role='presentation']//span[text()='Enviando a Jira...']")))
            { 
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='presentation']//span[text()='Enviando a Jira...']")));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            }
        }

        public void esperarEnvioANav()
        {
            bool enviado = false;
           
            for(int i = 0; i<5 && !enviado; i++) 
            {
                enviado = true;
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='presentation']//span[text()='Enviando a NAV']")));
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
                Thread.Sleep(20000);
                if (Utils.EncontrarElemento(Utils.getByElement("Facturas.dialogoError.texto")))
                {
                    enviado = false;
                    // El mensaje aparece
                    if(CommonActions.getElement("Facturas.dialogoError.texto").Text.Equals("Error, la factura se ha intentado enviar con numeración PA, espere unos instantes y vuelva a intentarlo."))
                        CommonActions.waitAndClick("Facturas.dialogoError.aceptar");
                    // Aparentemente aquí explota
                    Thread.Sleep(5000);
                    CommonActions.waitAndClick("BarraHerramientas.buttonEnviarNav");
                }
            }
        }
        public void enviar_A_Nav(string nombreGeneradorFacturacion)
        {
            //CommonActions.waitAndClick("Facturas");
            getUltimaFacturaBilling().Click();

            CommonActions.waitAndClick("BarraHerramientas.buttonMarcarRevisada");
            Assert.IsTrue(CommonActions.getElement("BarraHerramientas.buttonEnviarNav").Displayed);
            
            Thread.Sleep(10000);
            CommonActions.waitAndClick("BarraHerramientas.buttonEnviarNav");

            esperarEnvioANav();

            acceder_A_Facturas();

            // Exceso de tiempo
            Thread.Sleep(25000);

            int cont = 0;
            while (!Utils.EncontrarElemento(By.XPath("//div[@title='" + nombreGeneradorFacturacion + "']/..//div[@title='Enviada a NAV']")) && cont < 5)
            {
                getUltimaFacturaBilling().Click();

                if (Utils.EncontrarElemento(Utils.getByElement("BarraHerramientas.buttonEnviarNav")))
                    CommonActions.waitAndClick("BarraHerramientas.buttonEnviarNav");

                acceder_A_Facturas();

                Thread.Sleep(5000);
                cont++;
            }


        }

        public IWebElement getUltimaFacturaBilling()
        {
            string row = driver.FindElements(By.XPath("//div[contains(@title,'" + Oferta.getFechaHoraActual() + "')]/.."))[0].GetAttribute("aria-rowindex");
            return driver.FindElement(By.XPath("//div[@aria-rowindex=" + row + "]//div[@aria-colindex=2]"));
        }

        public void acceder_A_ModuloBilling()
        {
            CommonActions.waitAndClick("Modulo.gestionCliente");
            driver.SwitchTo().Frame("AppLandingPage");
            CommonActions.waitAndClick("Modulo.billing");
            driver.SwitchTo().DefaultContent();
        }

        public void validarConfiguracionFacturacion()
        {
            CommonActions.waitAndClick("Producto.GeneradorFacturacion.nombreGeneradorFacturacion");
            Assert.AreEqual("Borrador", CommonActions.getElement("Producto.ConfiguracionFacturacion.Cabecera.inputRazonEstado").Text);
            CommonActions.waitAndClick("Producto.ConfiguracionFacturacion.PestañaDetallesDeLineas");

            //Validar Oferta
            CommonActions.waitAndClick("Oferta.validar");
            //CommonActions.waitInvisibilityOfElement(Utils.getByElement("Oferta.validar"));
            Thread.Sleep(1000);
            //CommonActions.waitAndClick("Oferta.aceptarBoton");
        }

        public string esperar_A_CargarConfiguracionFacturacion()
        {
            int cont = 0;
            while (!Utils.EncontrarElemento(Utils.getByElement("Producto.GeneradorFacturacion.nombreGeneradorFacturacion")) && cont < 5)
            {
                Thread.Sleep(10000);
                CommonActions.waitAndClick("Oferta.ConfigFacturacionButtonActualizar");
                cont++;
            }
            //Devolvemos el nombre del generador de facturación
            return driver.FindElement(By.XPath("//div[@aria-rowindex='2']/div[@aria-colindex='2']")).GetAttribute("title");
        }

        public void acceder_A_Facturas()
        {
            try
            {
                CommonActions.waitAndClick("Facturas");
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//span[text()='Facturas activo']")));
            }
            catch (TimeoutException)
            {
                CommonActions.waitAndClick("Facturas");            
            }
            
        }

        public void rellenar_Datos_Productos_Oferta()
        {
            
            // Pestaña caracteristicas: Ambito urbano, Off net, operador BRODYINT
            CommonActions.waitAndClick("Producto.PestañaCaracteristicas");

            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasRED")))
                //switch para ON / OFF red
                CommonActions.waitAndClick("Producto.PestañaCaracteristicasRED");
            // Seleccion Operador
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasOPERADOR")))
                CommonActions.rellenarCampoConDesplegable("Producto.PestañaCaracteristicasOPERADOR", "BRODYNT");
            // Seleccion de Ambito
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaCaracteristicasAMBITO")))
                CommonActions.seleccionarEnCampoDesplegable("Producto.PestañaCaracteristicasAMBITO", "Urbano");


            // Seleccionar FTTT de desplegable
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.SelectUsoLineaNegocioNew")))
                CommonActions.waitAndClick("Producto.SelectUsoLineaNegocioNew");
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.SelectUsoLineaNegocioOpcionFTTE")))
                CommonActions.waitAndClick("Producto.SelectUsoLineaNegocioOpcionFTTE");
            
            // Direcciones y coordenadas: provincia Madrid
            CommonActions.waitAndClick("Producto.PestañaDireccionesYcoordenadas");
            CommonActions.rellenarCampoConDesplegable("Producto.PestañaDireccionesYcoordenadasPROVINCIAORIGEN", "Madrid");
            Thread.Sleep(2000);


            // Pestaña Jira: 
            CommonActions.waitAndClick("Producto.PestañaJira");

            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraPLAZOPROVISION")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraPLAZOPROVISION", "1");

            // Fibra: 
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraCOSTEFIBRAOPTICA")))                
                CommonActions.rellenarCampoInput("Producto.PestañaJiraCOSTEFIBRAOPTICA", "1");

            // Capex: 456
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraCAPEXSATURACION")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraCAPEXSATURACION", "11");


            // Equipos:
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.PestañaJiraEQUIPOS")))
                CommonActions.rellenarCampoInput("Producto.PestañaJiraEQUIPOS", "1");

            // Opex 456
            CommonActions.rellenarCampoInput("Producto.PestañaJiraOPEXTOTAL", "11");
            // Guardar y Cerrar
            CommonActions.waitAndClick("BarraHerramientas.buttonGuardaryCerrar");
            }
            
        }

    }
