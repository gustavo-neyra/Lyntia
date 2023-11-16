using System;
using System.Collections.Generic;
using System.Threading;
using Lyntia.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Lyntia.TestSet.Conditions;
using SeleniumExtras.WaitHelpers;
using System.Linq;

namespace Lyntia.TestSet.Actions
{
    public class ProductoActions
    {
        readonly Utils utils = new Utils();

        private static IWebDriver driver;
        private static WebDriverWait wait;
        private static OpenQA.Selenium.Interactions.Actions accionesSelenium;

        private static ProductoConditions productoConditions;

        public ProductoActions()
        {
            driver = Utils.driver;
            accionesSelenium = new OpenQA.Selenium.Interactions.Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
            productoConditions = Utils.GetProductoConditions();
        }


        public void CreacionProducto(Dictionary<string, string> datosRepositorio)
        {
            string unidadVenta = datosRepositorio["unidadVenta"];
            try
            {
                CommonActions.waitAndClick("Oferta.buttonAgregarProducto");

                CommonActions.waitInvisibilityOfElement(By.XPath("//div[@role='presentation']//span[text()='Cargando...']"));

                // Seleccionar Producto existente del desplegable
                CommonActions.rellenarCampoConDesplegable("Producto.inputProductoExistente", datosRepositorio["productoExistente"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");

                // Seleccionar Uso(Línea de negocio)
                if (!datosRepositorio["uso"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Producto.SelectUsoLineaNegocio", datosRepositorio["uso"]);
                }
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AddProducto.png", "No se añaden correctamente los datos del Producto: " + datosRepositorio["productoExistente"] + ", " + datosRepositorio["uso"] + ", " + datosRepositorio["unidadVenta"]);
                throw e;
            }

            // Seleccion de Operador ultima milla
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputOperador")))
            {
                CommonActions.rellenarCampoConDesplegable("Producto.inputOperador", datosRepositorio["operador"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            //Rellenar Unidad Venta            
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputUnidaddeVenta")) && Utils.EncontrarElemento(By.XPath(Utils.GetIdentifier("Producto.inputUnidaddeVenta"))))
            {
                // Seleccionar Producto existente del desplegable si esta vacio                
                unidadVenta = CommonActions.getElement("Producto.inputUnidaddeVenta").GetAttribute("aria-required") == "true" ? datosRepositorio["unidadVenta"] : "";

                CommonActions.rellenarCampoConDesplegable("Producto.inputUnidaddeVenta", unidadVenta);
                Thread.Sleep(2000);
                //Click para perder el foco del desplegable y cerrarlo
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            // Introduccir metros
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputMetros")) && !datosRepositorio["metros"].Equals(""))
            {
                CommonActions.rellenarCampoInput("Producto.inputMetros", datosRepositorio["metros"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            // Introduccir Modalidad de Contratacion
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.SelectModalidadContratacion")) && !datosRepositorio["modalidadContratacion"].Equals(""))
            {
                CommonActions.seleccionarEnCampoDesplegable("Producto.SelectModalidadContratacion", datosRepositorio["modalidadContratacion"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            // Introduccir NRC
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.selectNRC")) && !datosRepositorio["nrc"].Equals(""))
            {
                CommonActions.rellenarCampoInput("Producto.selectNRC", datosRepositorio["nrc"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            //Introducir Precio Metro/Anual
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputPrecioMetroAnual")) && !datosRepositorio["precioAnual"].Equals(""))
            {
                CommonActions.rellenarCampoInput("Producto.inputPrecioMetroAnual", datosRepositorio["precioAnual"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            //Introducir ARC
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputARC")) && !datosRepositorio["arc"].Equals(""))
            {
                CommonActions.rellenarCampoInput("Producto.inputARC", datosRepositorio["arc"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            // Introduccir Duración de Contrato            
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputDuracionContrato")) && !datosRepositorio["duracionContrato"].Equals(""))
            {                
                CommonActions.rellenarCampoInput("Producto.inputDuracionContrato", datosRepositorio["duracionContrato"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
            }

            // Introducir precio mensual
            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputPrecioMensual2")) && !datosRepositorio["precioMensual"].Equals(""))
            {
                CommonActions.rellenarCampoInput("Producto.inputPrecioMensual2", datosRepositorio["precioMensual"]);
                CommonActions.waitAndClick("Producto.titleCreacionRapida");
             }

            try
            {
                // Guardar y Cerrar Producto actual
                CommonActions.waitAndClick("Producto.GuardarYCerrar_producto");
                CommonActions.esperarGuardado();
                CommonActions.waitAndClick("Oferta.saveOferta");
                TestContext.WriteLine("Producto guardado correctamente: " + datosRepositorio["productoExistente"] + ", " + datosRepositorio["uso"] + ", " + unidadVenta + ", " + datosRepositorio["metros"] + ", " + datosRepositorio["nrc"] + ", " + datosRepositorio["modalidadContratacion"]);
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "GuardarProducto.png", "El producto no fue creado: " + datosRepositorio["productoExistente"] + ", " + datosRepositorio["uso"] + ", " + unidadVenta + ", " + datosRepositorio["metros"] + ", " + datosRepositorio["nrc"] + ", " + datosRepositorio["modalidadContratacion"]);
                throw e;
            }
            
        }

        public void Añadirproducto_vistarapida()
        {
            try
            {
                CommonActions.waitAndClick("Producto.buttonCrearRegistroNuevo");
                //Thread.Sleep(3000);
                driver.FindElements(Utils.getByElement("Producto.SelectCrearRegistroNuevoProductoDeOferta"))[5].Click();
                //Thread.Sleep(3000);

                TestContext.WriteLine("Se añade el producto en la vista rapida correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Añadir_producto_vista_rapida.png", "El producto en vista rapida no se ha añadido correctamente");
                throw e;
            }
        }


        //Metodo en el que agregamos un producto a un servicio tipo cambio de capacidad, seleccionamos un producto heredado con campos obligatorios sin rellenar y se guarda.
        public void Agregar_servicio_heredado_y_guardar()
        {
            try
            {
                //Thread.Sleep(3000);
                CommonActions.waitAndClick("Producto.buttonAgregarProducto"); //pulsamos sobre agregar producto
                //Thread.Sleep(4000);
                CommonActions.rellenarCampoConDesplegable("Producto.inputServicioexistente", "Circuitos de capacidad");
                CommonActions.waitAndClick("Producto.GuardarYCerrar_producto"); //Guarda y cierra

                TestContext.WriteLine("Se agrega un producto heredado correctamente y se guarda");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Agregar_servicio_heredado_y_guarda.png", "No se agrega un producto heredado correctamente y no se guarda");
                throw e;
            }
        }
        //Metodo para agregar producto heredado
        public void HeredarProducto(Dictionary<string, string> datosTestCase)
        {
            try
            {
                // Click en "+ Agregar producto"
                CommonActions.waitAndClick("Producto.buttonAgregarProducto");

                if (!datosTestCase["productoHeredado"].Equals(""))
                {
                    CommonActions.rellenarCampoConDesplegable("Producto.inputServicioHeredado", datosTestCase["productoHeredado"]);
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@aria-label='Cód. admin. servicio heredado']")));
                    CommonActions.getElement(By.XPath("//input[@aria-label='Cód. admin. servicio heredado']")).SendKeys(Keys.PageDown);
                    Thread.Sleep(6000);

                }

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.selectPuntoAModificar")))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Producto.selectPuntoAModificar", datosTestCase["puntoAModificar"]);
                }

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.SelectUsoLineaNegocio")) && !datosTestCase["uso"].Equals(""))
                {
                    CommonActions.seleccionarEnCampoDesplegable("Producto.SelectUsoLineaNegocio", datosTestCase["uso"]);
                }

                if (!datosTestCase["precioMensual"].Equals(""))
                {
                    CommonActions.rellenarCampoInput("Producto.selectPrecioMensual", datosTestCase["precioMensual"]);
                }

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputMetros")) && !datosTestCase["metros"].Equals(""))
                {
                    CommonActions.rellenarCampoInput("Producto.inputMetros", datosTestCase["metros"]);
                }

                if (!datosTestCase["precioMetroAnual"].Equals(""))
                {
                    CommonActions.rellenarCampoInput("Producto.inputPrecioMetroAnual", datosTestCase["precioMetroAnual"]);
                }

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputUnidaddeVenta")) && !datosTestCase["unidadVenta"].Equals(""))
                {
                    CommonActions.rellenarCampoConDesplegable("Producto.inputUnidaddeVenta", datosTestCase["unidadVenta"]);
                }

                if (!datosTestCase["duracionContrato"].Equals(""))
                {
                    CommonActions.rellenarCampoInput("Producto.selectDuracionContrato", datosTestCase["duracionContrato"]);
                }

                if (!datosTestCase["nrc"].Equals(""))
                {
                    CommonActions.rellenarCampoInput("Producto.selectNRC", datosTestCase["nrc"]);
                }

                CommonActions.waitAndClick("Producto.GuardarYCerrar_producto");

                IWebElement frame = null;
                bool frameExists = driver.FindElements(By.Id("FullPageWebResource")).Any();

                if (frameExists)
                {
                    frame = driver.FindElement(By.Id("FullPageWebResource"));
                    driver.SwitchTo().Frame(frame);
                    CommonActions.waitAndClick("Oferta.modificacionProcesar");
                    TestContext.WriteLine("El tipo de producto heredado con sus parametros se guarda correctamente");
                }
                else
                {

                    TestContext.WriteLine("El tipo de producto heredado con sus parametros se guarda correctamente");
                }
                Thread.Sleep(10000);
            }

            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Producto heredado.png", "***productoHeredado, precioMensual, duracionContrato, nrc, metros, precioMetroAnual no se guarda correctamente");
                throw e;
            }

        }


        //Metodo en el que una vez agregado un producto heredado se cumplimentan los campos obligatorios, se guarda y se cierra
        public void Cumplimentar_campos_y_guardar(Dictionary<string,string> datosTestCase)
        {
            try
            {
                // Seleccionar Uso(Línea de negocio)
                CommonActions.seleccionarEnCampoDesplegable("Producto.SelectUsoLineaNegocio", datosTestCase["uso"]);

                if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputUnidaddeVenta")))
                {
                    CommonActions.rellenarCampoConDesplegable("Producto.inputUnidaddeVenta", datosTestCase["unidadVenta"]);
                }

                CommonActions.rellenarCampoInput("Producto.selectNRC", "4");
                CommonActions.rellenarCampoInput("Producto.selectPrecioMensual", datosTestCase["precioMensual"]);

                CommonActions.getElement("Producto.GuardarYCerrar_producto").Click();//Guarda y cierra
                Thread.Sleep(10000);

                TestContext.WriteLine("Precio mensual, duracion del contrato y NRC se guardan correctamente");

            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Precio mensual, duracion del contrato y NRC.png", "Precio mensual, duracion del contrato y NRC no se guardan correctamente");
                throw e;
            }
        }

        public void Agregar_Producto_tipo_circuito_de_capacidad(String productoExistente)
        {
            try
            {
                // Click en "+ Agregar producto"
                CommonActions.waitAndClick("Producto.buttonAgregarProducto");
                Thread.Sleep(5000);

                // Seleccionar Producto existente del desplegable
                CommonActions.rellenarCampoConDesplegable("Producto.inputProductoExistente", productoExistente);

                CommonActions.waitAndClick("Producto.GuardarYCerrar_producto");
                Thread.Sleep(3000);

                TestContext.WriteLine("El producto existente tipo circuito de capacidad se ha añadido correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Agregar_Producto_tipo_circuito_de_capacidad.png", "El producto existente tipo circuito de capacidad no se ha añadido correctamente");
                throw e;
            }
        }

        public void Agregar_Linea_de_negocio_y_Unidad_de_venta(Dictionary<string, string> datosTestCase)
        {
            // Seleccionar Uso(Línea de negocio)
            CommonActions.seleccionarEnCampoDesplegable("Producto.SelectUsoLineaNegocio", datosTestCase["uso"]);

            // Seleccionar unidad de venta

            if (Utils.EncontrarElemento(Utils.getByElement("Producto.inputUnidaddeVenta")))
            {
                CommonActions.rellenarCampoConDesplegable("Producto.inputUnidaddeVenta", datosTestCase["unidadVenta"]);
            }

            try
            {
                CommonActions.rellenarCampoInput("Producto.selectNRC", datosTestCase["nrc"]);

                CommonActions.waitAndClick("Producto.GuardarYCerrar_producto"); //guardamos y cerramos
                Thread.Sleep(18000);

                TestContext.WriteLine("***El producto se guarda de manera correcta.");             
            }

            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "AddLineaNegocioProducto.png", "La linea de negocio y la unidad de venta no se han agregado correctamente.");
                throw e;
            }
            
        }

        public void Borrado_de_producto()//metodo por el cual borramos una linea de producto que anteriormente hemos dado de alta en añadir producto.
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(Utils.getByElement("Oferta.gridSelecionarTodo")));
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.gridSelecionarTodo");
                Thread.Sleep(3000);
                CommonActions.waitAndClick("Producto.buttonEliminarProductodeOferta");
                Thread.Sleep(2000);
                CommonActions.waitAndClick("Oferta.confirmDeleteOferta");
                Thread.Sleep(3000);

                TestContext.WriteLine("El reestablecimiento de la prueba se ha realizado correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "ReestablecimientoPrueba.png", "El reestablecimiento de la prueba no se ha realizado correctamente");
                throw e;
            }
        }

        /// <summary>
        /// Método para Agregar producto
        /// </summary>
        public void Editar_añadir_producto()
        {
            try
            {
                CommonActions.getElement("Producto.buttonAgregarProducto").Click(); //pulsamos sobre agregar producto
                if (Utils.EncontrarElemento(By.XPath(Utils.GetIdentifier("Producto.GuardarYCerrar_producto"))))
                {
                    //Thread.Sleep(2000);
                    CommonActions.waitAndClick("Producto.GuardarYCerrar_producto"); //guardamos
                    //Thread.Sleep(3000);
                    CommonActions.waitAndClick("Producto.GuardarYCerrar_producto"); //guardamos y cerramos
                    TestContext.WriteLine("Se pulsa correctamente sobre agregar producto");
                }
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "Editar_añadir_producto.png", "No se pulsa correctamente sobre agregar producto");
                throw e;
            }
        }

        /// <summary>
        /// Método para copiar y buscar el codigo administrativo de la primera linea de los productos contratados
        /// </summary>
        public void BuscarCodigo_administrativo1()
        {
            List<string> listaCodigos = new List<string>();

            for (int i = 0; i < 1; i++)
            {
                String codigo = CommonActions.getElement(By.XPath("//div[@row-index = 0]//div[@aria-colindex = 3]//label")).Text; //Crear un metodo propio para la interaccion con tablas
                listaCodigos.Add(codigo);
            }

            //CommonActions.waitAndClick("Oferta.GridServiciosContratados");
            Thread.Sleep(3000);
            for (int i = 0; i < listaCodigos.Count; i++)
            {
                wait.Until(ExpectedConditions.ElementExists(Utils.getByElement("Oferta.inputFilter")));
                //CommonActions.waitAndClick("Oferta.botonKAM");
                //CommonActions.waitAndClick("Oferta.clickPrueba");
                CommonActions.getElement("Oferta.inputFilter").SendKeys(listaCodigos[i]);
                Thread.Sleep(3000);
                CommonActions.waitAndClick("Oferta.buttonQuickFindOferta");
                Thread.Sleep(3000);
                Assert.AreEqual("En construcción", CommonActions.getElement("Producto.labelEnconstruccion").Text);
                //productoConditions.ResBuscarCodigo_administrativo();
                Thread.Sleep(3000);
                CommonActions.limpiarCampo("Oferta.inputFilter");
            }
        }

        /// <summary>
        /// Método para copiar y buscar el codigo administrativo de la segunda linea de los productos contratados
        /// </summary>
        public void BuscarCodigo_administrativo2()
        {
            List<string> listaCodigos = new List<string>();
            CommonActions.waitElement(By.XPath("//div[@aria-rowindex='2']//div[@aria-colindex='3']//label"));            
            listaCodigos.Add(CommonActions.getElement(By.XPath("//div[@aria-rowindex='2']//div[@aria-colindex='3']//label")).Text);

            CommonActions.waitAndClick("Oferta.GridServiciosContratados");
            Thread.Sleep(3000);
            for (int i = 0; i < listaCodigos.Count; i++)
            {
                CommonActions.waitElement(Utils.getByElement("Oferta.inputFilter"));
                CommonActions.getElement("Oferta.inputFilter").SendKeys(listaCodigos[i]);
                CommonActions.waitAndClick("Oferta.buttonQuickFindOferta");
                Thread.Sleep(3000);
                //Assert.AreEqual("En construcción", CommonActions.getElement("Producto.labelEnservicio").Text);
                CommonActions.limpiarCampo("Oferta.inputFilter");
            }
        }

        /// <summary>
        /// Método para copiar codigo de la oferta y buscarlo en la vista rapida
        /// </summary>
        public void BuscarOferta_desde_servicio_contratado(string nombreOferta)

        {
            try
            {                               
                CommonActions.getElement("Oferta.inputFilter").SendKeys(nombreOferta);
                CommonActions.waitAndClick("Oferta.buttonQuickFindOferta");
                Thread.Sleep(2000);

                TestContext.WriteLine("Se busca la oferta correctamente");
            }
            catch (Exception e)
            {
                CommonActions.CapturadorExcepcion(e, "BuscarOferta_desde_servicio_contratado.png", "No se busca la oferta correctamente");
                throw e;
            }
        }

        ///// <summary>
        ///// Método para crear producto tipo fibra oscura (IRU)
        ///// </summary>
        //public void creacionproductofibraoscuraIRU(Dictionary<string, string> datosTestCase)
        //{
        //    CommonActions.waitAndClick("Oferta.buttonAgregarProducto");

        //    // Seleccionar Producto existente del desplegable
        //    CommonActions.rellenarCampoConDesplegable("Producto.inputProductoExistente", datosTestCase["productoExistente"]);

        //    // Seleccionar Uso(Línea de negocio)
        //    if (!datosTestCase["uso"].Equals(""))
        //    {
        //        CommonActions.seleccionarEnCampoDesplegable("Producto.SelectUsoLineaNegocio", datosTestCase["uso"]);
        //    }

        //    // Introduccir metros
        //    if (!datosTestCase["metros"].Equals(""))
        //    {
        //        CommonActions.rellenarCampoInput("Producto.inputMetros", datosTestCase["metros"]);
        //    }

        //    // Introduccir Modalidad de Contratacion
        //    if (!datosTestCase["modalidadContratacion"].Equals(""))
        //    {
        //        CommonActions.seleccionarEnCampoDesplegable("Producto.SelectModalidadContratacion", datosTestCase["modalidadContratacion"]);
        //    }

        //    // Introduccir NRC
        //    if (!datosTestCase["nrc"].Equals(""))
        //    {
        //        CommonActions.rellenarCampoInput("Producto.selectNRC", datosTestCase["nrc"]);
        //    }
            
        //    //Introducir precio anual
        //    if (!datosTestCase["precioAnual"].Equals(""))
        //    {
        //        CommonActions.rellenarCampoInput("Producto.inputPrecioMetroAnual", datosTestCase["precioAnual"]);
        //    }

        //    //Introducir ARC
        //    if (!datosTestCase["arc"].Equals(""))
        //    {
        //        CommonActions.rellenarCampoInput("Producto.inputARC", datosTestCase["arc"]);
        //    }

        //    // Guardar y Cerrar Producto actual
        //    CommonActions.waitAndClick("Producto.GuardarYCerrar_producto");
        //    CommonActions.esperarGuardado();
        //}

    }
}
