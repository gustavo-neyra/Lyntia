using System;
using System.Collections.Generic;
using System.Threading;
using Lyntia.TestSet.Actions;
using Lyntia.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lyntia.TestSet.Conditions
{
    public class OfertaConditions
    {
        private Utils utils = new Utils();
        private static IWebDriver driver;
        private static WebDriverWait wait;

        public OfertaConditions()
        {
            driver = Utils.driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
        }

        public void validar_CondicionesCreacionOferta()
        {
            try
            {
                // Assert de título "Nuevo Oferta" del formulario
                Assert.AreEqual("Crear Oferta: Sin guardar", CommonActions.getElement(By.XPath("//h1[@data-id='header_title']")).Text);

                // Assert de tab por defecto "General"
                Assert.IsTrue(CommonActions.getElement(By.XPath("//li[@aria-label='General']")).GetAttribute("aria-selected").Equals("true"));                

                // Assert de Tipo de Oferta Nuevo Servicio                
                Assert.AreEqual(CommonActions.getElement(By.XPath("//select[contains(@aria-label,'Tipo de oferta')]")).GetAttribute("title"), "Nuevo servicio");

                // Assert de Divisa
                Utils.scrollToElementByXpath("//h2[@title='Información adicional']");   
                //Utils.scrollToElementByXpath("//div[contains(@data-id,'transactioncurrencyid_selected_tag_text')]");
               
                //Assert.AreEqual("Euro", CommonActions.getElement(By.XPath("//div[contains(@data-id,'transactioncurrencyid_selected_tag_text')]")).GetAttribute("title"));

                TestContext.WriteLine("***Se cumplen las condiciones de crear oferta");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "CreaOferta.png", "***No se cumplen las condiciones para crear oferta correctamente");
                throw e;
            }

        }

        public void validar_FechasSinInformar()
        {
            try
            {
                // Assert de Fecha de creación vacía
                Assert.IsTrue(driver.FindElement(By.XPath("//input[contains(@data-id,'createdon')]")).Text.Equals(""));
                TestContext.WriteLine("***Se cumplen las condiciones con fechas sin informar");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "FechasSinInformar.png", "***No se cumplen las condiciones con fechas sin informar correctamente");
                throw e;
            }
        }

        public void validar_CondicionesOfertaNoCreada()
        {
            try
            {
                // Assert de alerta con los campos obligatorios sin informar                
                Assert.IsTrue(CommonActions.getElements(By.XPath("//div[@data-id='notificationWrapper']")).Count > 0);                

                TestContext.WriteLine("***Se cumplen las condiciones de una oferta no creada");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "OfertaNoCreada.png", "***No se cumplen las condiciones de una oferta creada");
                throw e;
            }
        }

        public void validar_FechasInformadasCorrectamente()
        {
            try
            {
                // Assert de Fecha de creación vacía
                Assert.IsFalse(driver.FindElement(By.XPath("//input[contains(@data-id,'createdon')]")).GetAttribute("value").Equals(""));

                // Assert de Hora de creación vacía
                Assert.IsFalse(driver.FindElement(By.XPath("//input[contains(@aria-label,'Hora de Fecha creación')]")).GetAttribute("value").Equals(""));

                if (Utils.EncontrarElemento(By.XPath("//input[contains(@data-id,'modifiedon')]")))
                {
                    // Assert de Fecha de modificación vacía
                    Assert.IsFalse(driver.FindElement(By.XPath("//input[contains(@data-id,'modifiedon')]")).GetAttribute("value").Equals(""));

                    // Assert de Hora de modificación vacía
                    Assert.IsFalse(driver.FindElement(By.XPath("//input[contains(@aria-label,'Hora de Fecha de modificación')]")).GetAttribute("value").Equals(""));
                }                

                TestContext.WriteLine("***Las condiciones de fechas informadas correctamente han sido OK");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "FechasInformadascorrectamente.png", "***No se cumplen las condiciones de fechas informadas");
                throw e;
            }
        }

        public void validar_OfertaGuardadaCorrectamente(Dictionary<string, string> datosTestCase)
        {
            try
            {
                // Nombre de Oferta
                Thread.Sleep(6000);
                Assert.AreEqual(datosTestCase["nombreOferta"] + Oferta.getFechaHoraActual(), CommonActions.getElement(By.XPath("//input[@aria-label='Nombre oferta']")).GetAttribute("value"));

                // Cliente 
                Assert.AreEqual(datosTestCase["cliente"], CommonActions.getElement(By.XPath("//div[contains(@data-id,'customerid_selected_tag_text')]")).Text);

                // Razon para el estado
                Assert.AreEqual("En elaboración", CommonActions.getElement(By.XPath("//div[@aria-label='Razón para el estado']")).Text);

                CommonActions.getElement(By.XPath("//div[contains(@data-id,'customerid_selected_tag')]")).SendKeys(Keys.PageDown);

                Assert.AreEqual(datosTestCase["tipoOferta"], CommonActions.getElement(By.XPath("//select[@aria-label='Tipo de oferta']")).GetAttribute("title"));
                Thread.Sleep(1000);

                TestContext.WriteLine("***Se cumplen las condiciones de guardado de oferta correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "OfertaGuardacorrectamente.png", "***No se cumplen las condiciones para guardar la oferta");
                throw e;
            }
        }

        public void validar_OfertaGuardadaCorrectamenteEnGrid()
        {
            try
            {                
                // Se encuentra en Razon para el estado En elaboracion
                Assert.AreEqual("En elaboración", CommonActions.getElement(Utils.getByElement("Producto.Grid.celdaRazonEstadoElaboracion")).Text);
                TestContext.WriteLine("***Se cumple la condición de Oferta guardada Correctamente.");
            } catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "CondicionOfertaGuardada.png", "***No se cumple la condición de Oferta guardada Correctamente.");
                throw e;                
            }
        }

        public void validar_OfertaAdjudicada()
        {
            try
            {
                // Se encuentra en Razon para el estado En elaboracion
                Assert.AreEqual("Adjudicada", CommonActions.getElement(Utils.getByElement("Producto.Grid.celdaRazonEstado")).Text);
                TestContext.WriteLine("***La oferta se ha adjudicado correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "CondicionOfertaGuardada.png", "***No se cumple la condición de Oferta guardada Correctamente.");
                throw e;
            }
        }


        public void validar_Acceso_A_SeleccionOferta()
        {
            try
            {
                Assert.AreEqual(true, CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Enabled);//la pestaña general esta activa
                Assert.AreEqual("General", CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Text);
                TestContext.WriteLine("***Se cumple la condicion");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AccederSeleccionOferta.png", "***No se cumple la condicion");
                throw e;
            }
        }

        public void validar_IntroduccionDatos(string nombreOferta)//Comprobamos que todos los campos se introducen correctamente
        {
            try
            {

                Assert.IsTrue(Utils.EncontrarElemento(By.XPath("//span[text()='" + nombreOferta + "']")));
                
                TestContext.WriteLine(CommonActions.getElement(By.XPath("//span[text()='" + nombreOferta + "']")).Text);//muestra por consola la nueva oferta modificada      
                TestContext.WriteLine("***La condicion se cumple");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Introducir datos.png", "***No se ha modificado la oferta correctamente");
                throw e;
            }
        }        

        public void validar_EdicionCamposOferta(Dictionary<string,string> datosTestCase)
        {
            Assert.AreEqual(datosTestCase["nombreOferta"] + Oferta.getFechaHoraActual(), CommonActions.getElement("Oferta.inputNameOferta").GetAttribute("title"));
            Utils.scrollToElementByXpath(Utils.GetIdentifier("Oferta.inputReferenceOferta"));
            Assert.AreEqual(datosTestCase["referenciaOferta"], CommonActions.getElement("Oferta.inputReferenceOferta").GetAttribute("title"));
            Utils.scrollToElementByXpath(Utils.GetIdentifier("Oferta.inputGVAL"));
            
            Utils.scrollToElementByXpath(Utils.GetIdentifier("Oferta.inputCampoDescripcion"));
            Assert.AreEqual(datosTestCase["descripcion"], CommonActions.getElement("Oferta.inputCampoDescripcion").GetAttribute("title"));
            Assert.AreEqual(datosTestCase["GVAL"], CommonActions.getElement("Oferta.inputGVAL").GetAttribute("title"));
        }

        public void validar_AvisoCambioCapacidad()//mensaje por el tipo de oferta
        {
            try
            {
                if(!Utils.EncontrarElemento(By.XPath("//span[contains(text(),'Cambio de capacidad')]")))
                    CommonActions.getElement(By.Id("notificationIcon")).Click();
                Assert.AreEqual("La oferta de tipo “Cambio de capacidad” requiere envío a construcción, pero no cambia el código administrativo", CommonActions.getElement(By.XPath("//span[contains(text(),'Cambio de capacidad')]")).Text);                
                TestContext.WriteLine("***La condicion de aviso cambio de capacidad se cumple");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Aviso cambio de capacidad.png", "***No se cumple la condicion de cambio de capacidad");
                throw e;
            }

        }

        public void validar_AvisoCambioPrecio()//mensaje por el tipo de oferta
        {
            try
            {
                Thread.Sleep(3000);
                if (!Utils.EncontrarElemento(By.XPath("//span[contains(text(),'Cambio de precio')]")))
                    CommonActions.getElement(By.Id("notificationIcon")).Click();
                Assert.AreEqual("La oferta de tipo “Cambio de precio” no requiere envío a construcción ni cambiar el código administrativo", CommonActions.getElement(By.XPath("//span[contains(text(),'Cambio de precio')]")).Text);
                Thread.Sleep(3000);
                CommonActions.getElement(By.XPath("//button[contains(@aria-label, 'Guardar')]")).Click();//Guardar

                TestContext.WriteLine("***La condicion aviso cambio de precio se cumple correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Aviso cambio de precio.png", "***No se cumple la condicion de cambio de precio");
                throw e;
            }
        }

        public void validar_AvisoCambioSolucion()//mensaje por el tipo de oferta
        {
            try
            {
                Thread.Sleep(3000);
                if (!Utils.EncontrarElemento(By.XPath("//span[contains(text(),'Cambio de configuración')]")))
                    CommonActions.getElement(By.Id("notificationIcon")).Click();
                Assert.AreEqual("La oferta de tipo “Cambio de configuración” requiere el envío a construcción pero no cambia el código administrativo", CommonActions.getElement(By.XPath("//span[contains(text(),'Cambio de configuración')]")).Text);
                CommonActions.getElement(By.XPath("//button[contains(@aria-label, 'Guardar')]")).Click();//Guardar

                TestContext.WriteLine("***Se cumple la condicion de aviso cambio de solucion");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Aviso cambio solucion.png", "***No se cumple la condicion de aviso cambio de solucion");
                throw e;
            }
        }

        public void validar_AvisoCambiodedireccion()//mensaje por el tipo de oferta
        {
            try
            {
                Thread.Sleep(3000);
                if (!Utils.EncontrarElemento(By.XPath("//span[contains(text(),'Migración')]")))
                    CommonActions.getElement(By.Id("notification_icon_La oferta de tipo “Cambio de dirección” requiere el envío a construcción pero no cambia el código administrativo")).Click();
                Assert.AreEqual("La oferta de tipo “Cambio de dirección” requiere el envío a construcción pero no cambia el código administrativo", CommonActions.getElement(By.XPath("//span[contains(text(),'Cambio de dirección')]")).Text);
                TestContext.WriteLine("***La condicion de aviso cambio de direccion es correcta");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Aviso cambio de direccion.png", "***No se cumple la condicion de aviso cambio de direccion");
                throw e;
            }

        }

        public void validar_Acceso_A_OfertaAdjudicadas()
        {
            try
            {               
                TestContext.WriteLine("***La condicion resultado acceso oferta adjudicada es correcta");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado acceder oferta estado adjudicada.png", "***No se cumple la condicion para acceder a oferta en estado adjudicada");
                throw e;
            }
        }


        public void validar_EliminacionDesdeBarraMenu()
        {
            try
            {
                Assert.AreEqual("Confirmar eliminación", CommonActions.getElement(By.XPath("//h1[contains(@aria-label, 'Confirmar eliminación')]")).Text);//se comprueba texto de la ventana emergente
                TestContext.WriteLine("***La condicion de confirmar borrado se cumple");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado eliminar barra menu.png", "***No se cumple la condicion de confirmar borrado");
                throw e;
            }
        }

        public void validar_CancelacionOferta()
        {
            try
            {
                Assert.AreEqual("Ganada", Utils.SearchWebElement("Oferta.labelOfertaestadoGanada").Text);//la oferta esta en estado ganada
                TestContext.WriteLine("***Se cumple la condicion de cancelar");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado cancela.png", "***No se cumple la condicion de cancelar");
                throw e;
            }
        }

        public void validar_EliminacionPopUp()
        {
            try
            {
                Assert.IsTrue(CommonActions.getElement(By.XPath("//h1[contains(@aria-label, 'No se pudo eliminar')]")).Text.Contains("No se pudo eliminar"));                
                String AvisoPriv = CommonActions.getElement(By.XPath("//h1[contains(@aria-label, 'No se pudo eliminar')]")).Text;//imprime en consola el texto
                TestContext.WriteLine(AvisoPriv);
                String AvisoPriv2 = CommonActions.getElement(By.XPath("/html/body/section/div/div/div/div/div/div/div[1]/div[2]/h2")).Text;//imprime en consola el texto
                TestContext.WriteLine(AvisoPriv2);
                CommonActions.getElement("Oferta.cancelDeleteOferta").Click();
                Thread.Sleep(2000);

                TestContext.WriteLine("***La condicion resultado eliminar pop up se cumple correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado Eliminar popup.png", "***La condicion resultado eliminar pop up no se cumple correctamente");
                throw e;
            }
        }

        public void validar_OFertaCerrada()
        {
            try
            {                
                Assert.AreEqual(false, Utils.EncontrarElemento(By.XPath("//span[contains(@aria-label, 'Cerrar Oferta')]")));//se comprueba que el elemento no esta presente
                TestContext.WriteLine("***La condicion seleccion oferta adjudicada se cumple correctamente");
            }

            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado seleccion oferta adjudicada.png", "***No se cumple la condicion seleccion oferta adjudicada");
                throw e;
            }
        }


        public void validar_CerrarOfertaNoVisible()
        {
            try
            {
                Assert.AreEqual(false, Utils.EncontrarElemento(Utils.getByElement("Oferta.buttonCerrar")));
                TestContext.WriteLine("***La condicion cerrar oferta no visible es correcta");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Cerrar oferta no visible.png", "***La condicion cerrar oferta no visible no es correcta");
                throw e;
            }
        }


        public void validar_OfertaNoCerrada()
        {
            try
            {
                driver.SwitchTo().Frame(CommonActions.getElement("Oferta.frameCerrarOferta"));
                Assert.AreEqual("Por favor, completa los campos obligatorios", CommonActions.getElement(By.XPath("//p[@id='error']")).Text);
                driver.SwitchTo().DefaultContent();

                TestContext.WriteLine("***La condicion oferta no cerrada se cumple correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta no cerrada.png", "***No se cumple la condicion oferta no cerrada");
                throw e;
            }
        }


        public void validar_OfertaCerradaCorrectamenteEnGrid(String razonEstado)
        {
            try
            {   
                // Se encuentra en Razon para el estado En elaboracion
                Thread.Sleep(2000);
                Assert.AreEqual(razonEstado, CommonActions.getElement(Utils.getByElement("Producto.Grid.celdaRazonEstadoCancelada")).Text);

                TestContext.WriteLine("***La condicion oferta cerrada correctamente en grid funciona correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta cerrada correctamente en grid.png", "***No se cumple la condicion oferta cerrada en grid");
                throw e;
            }
        }
        public void validar_OfertaPerdidaCorrectamenteEnGrid(String razonEstado)
        {
            try
            {
                // Se encuentra en Razon para el estado En elaboracion
                Thread.Sleep(2000);
                Assert.AreEqual(razonEstado, CommonActions.getElement(Utils.getByElement("Producto.Grid.celdaRazonEstadoPerdida")).Text);

                TestContext.WriteLine("***La condicion oferta cerrada correctamente en grid funciona correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta cerrada correctamente en grid.png", "***No se cumple la condicion oferta cerrada en grid");
                throw e;
            }
        }



        public void validar_OfertaPresentada()
        {
            try
            {
                Assert.AreEqual("Solo lectura: estado de este registro: Bloqueada", CommonActions.getElement(By.XPath("//span[@data-id='warningNotification']")).Text);
                TestContext.WriteLine("***Se cumple la condicion de oferta presentada");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta presentada.png", "***No se cumple la condicion de oferta presentada");
                throw e;

            }
        }

        public void validar_OFertaRevisadaCorrectamente()
        {
            try
            {                
                Assert.AreEqual("2", CommonActions.getElement(By.XPath("//div[@role='grid']")).GetAttribute("aria-rowcount"));
                TestContext.WriteLine("***Se cumple la condicion oferta revisada");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta revisada correctamente.png", "***No se cumple la condicion oferta revisada correctamente");
                throw e;
            }
        }

        public void validar_SeleccionOfertaBorrador()
        {
            try
            {
                Assert.AreEqual(true, CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Enabled);//la pestaña general esta activa
                Assert.AreEqual("General", CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Text);
                TestContext.WriteLine("***Se cumple la condicion resultado seleccion oferta en borrador");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado seleccion oferta en borrador.png", "***No se cumple la condicion de resultado seleccionar oferta en borrador");
                throw e;
            }
        }
        public void validar_AgregarProducto()
        {
            try
            {
                Assert.AreEqual(true, CommonActions.getElement(By.XPath("//input[contains(@aria-label, 'Producto existente, Búsqueda')]")).Enabled);//el campo producto existente esta habilitado
                TestContext.WriteLine("***Se cumple la condicion de producto agregado");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado agregar producto.png", "***No se cumple la condicion de producto agregado");
                throw e;
            }
        }
        public void validar_EdicionOFerta()
        {
            try
            {
                Assert.AreEqual(true, CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Enabled);//la pestaña general esta activa
                Assert.AreEqual("General", CommonActions.getElement(By.XPath("//li[contains(@aria-label, 'General')]")).Text);
                TestContext.WriteLine("***Se cumple la condicion resultado edicion de una oferta");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Resultado edicion de una oferta.png", "***No se cumple la condicion resultado edicion de una ofera");
                throw e;
            }

        }
        public void validar_OfertaPresentadaCorrectamente()
        {
            try
            {                
                // Se encuentra en Razon para el estado En elaboracion
                Assert.AreEqual("Presentada", CommonActions.getElement(Utils.getByElement("Producto.Grid.celdaRazonEstado")).Text);

                TestContext.WriteLine("***Se cumple la condicion de oferta presentada correctamente");

            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Oferta presentada correctamente.png", "***No se cumple la condicion de oferta presentada correctamente");
                throw e;
            }

        }

        public void validar_AdjudicacionOferta()
        {
            try
            {                
                Assert.AreEqual("Crear Proyecto", CommonActions.getElement("Oferta.labelCrearpedido").Text);               
                TestContext.WriteLine("***Se cumple la condicion de oferta adjudicada correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ResAdjudicarOferta.png", ("*** No Se cumple la condicion de oferta adjudicada correctamente"));
                throw e;
            }
        }       

        public void validar_FechaAdjudicacionCorrecta()
        {
            try
            {
                Assert.AreEqual("Error al introducir fecha de adjudicación: no puede ser anterior a fecha de presentación.", CommonActions.getElement("Oferta.labelMensajeCreapedidofechainferior").Text);
                TestContext.WriteLine("***Se cumple la condicion de advertencia, fecha inferior");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ResAdjudicarOferta.png", ("*** No cumple la condicion de advertencia, fecha inferior"));
                throw e;
            }
        }
        /// <summary>
        /// Método para comprobar que no hay datos disponibles de una oferta
        /// </summary>
        public void Datos_disponibles()
        {
            try
            {
               // string textoAssert = CommonActions.getElement("Oferta.labelNOhayDatosDisponibles").Text.Contains('.') ? "No hay datos disponibles." : "No hay datos disponibles";
               // Assert.AreEqual(textoAssert, CommonActions.getElement("Oferta.labelNOhayDatosDisponibles").Text);
                //TestContext.WriteLine("*** Se cumple la condicion de que no existe el dato");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Datos no disponibles.png", " ***NO se cumple la condicion de que no existe el dato");
                throw e;
            }
        }

        /// <summary>
        /// Método para comprobar que se ha guardado un producto y queda registrado
        /// </summary>
        public void ResultadResVentanaCrearPedidofechaposterior()
        {
            try
            {
                Thread.Sleep(11000);
                wait.Until(ExpectedConditions.ElementExists(Utils.getByElement("Oferta.ServiciosContratados.labelEnconstruccion")));
                Assert.AreEqual("En construcción", CommonActions.getElement("Oferta.ServiciosContratados.labelEnconstruccion").Text);                                                                            
                TestContext.WriteLine("Existe en la oferta el producto contratado");
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ResultadResVentanaCrearPedidofechaposterior.png", "No existe en la oferta el producto contratado");
                throw e;
            }
        }

        public void ResultadResVentanaCrearPedidofechaposterior(string servicio)
        {
            try
            {               
                Thread.Sleep(2000);
                Assert.AreEqual("En construcción", CommonActions.getElement("Producto.labelEnconstruccion").Text);
                TestContext.WriteLine("Existe en la oferta el producto contratado");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ResultadResVentanaCrearPedidofechaposterior.png", "No existe en la oferta el producto contratado");
                throw e;
            }
        }

        /// <summary>
        /// Método para comprobar que se ha generado una oferta y queda adjudicada
        /// </summary>
        public void ResBuscarOferta_desde_servicio_contratado()
        {
            try
            {
                Assert.AreEqual("Adjudicada", CommonActions.getElement("Oferta.labelAdjudicada").Text);
                TestContext.WriteLine("Existe la oferta en estado adjudicada");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ResultBuscarOfertaServicioContratado.png", "No existe ninguna oferta adjudicada");
                throw e;
            }

        }


        /// <summary>
        /// Método para comprobar que se envia correctamente a Jira
        /// </summary>
        public void ResBuscarOferta_enviarJira()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Utils.getByElement("Producto.EnviaraJiraenviocorrecto")));
            Assert.AreEqual("Envío correcto.", Utils.SearchWebElement("Producto.EnviaraJiraenviocorrecto").Text);
        }

        /// <summary>
        /// Método para comprobar que se envia correctamente a Jira y el servicio queda cancelado
        /// </summary>
        public void Resultado_Enviar_A_Jira_cancelar()
        {
            Thread.Sleep(7000);
            Assert.AreEqual("Cancelado", CommonActions.getElement("Oferta.condicionCancelado").Text);            
        }

        public void validarFacturaEnviada_A_Nav(string nombreGeneradorFacturacion)
        {
            try
            {
                Assert.IsTrue(Utils.EncontrarElemento(By.XPath("//div[@title='" + nombreGeneradorFacturacion + "']/..//div[@title='Enviada a NAV']")));
            }
            catch(Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Factura_Enviada_A_NAV.png", "***No se ha encontrado la factura con estado Enviada a NAV");
            }
            
        }
    }

}