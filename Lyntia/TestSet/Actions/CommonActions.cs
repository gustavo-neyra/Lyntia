using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Allure.Commons;
using Lyntia.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lyntia.TestSet.Actions
{
    public class CommonActions
    {
        private static IWebDriver driver;
        private static WebDriverWait wait;        
        readonly Utils utils = new Utils();        

        public CommonActions()
        {
            driver = Utils.driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));            
        }

        /// <summary>
        /// Acceso a la Gestión de Cliente
        /// </summary>
        public void acceder_A_GestionCliente()
        {
            try
            {
                
                driver.SwitchTo().Frame(Utils.SearchWebElement("Modulo.frameModulos")); // Cambiar al frame de Apps

                waitAndClick("Modulo.gestionCliente"); //modulo gestion de clientes

                TestContext.WriteLine("Se accede correctamente a Gestión de Cliente");

                waitAndClick("Oferta.clientesKAM");
                waitAndClick("Oferta.clientesLyntia");

            }
            catch (Exception e)
            {
                CapturadorExcepcion(e, "AccesoGestionCliente.png", "No se pudo acceder a Gestión de Cliente");
                throw e;
            }
        }

        /// <summary>
        /// Acceso a la sección Ofertas en la barra lateral izquierda
        /// </summary>
        public void AccesoOferta()
        {
            try
            {
                driver.SwitchTo().DefaultContent();
                waitAndClick("Oferta.ofertaSection");
                TestContext.WriteLine("Se accede correctamente a sección de Ofertas");
            }
            catch (Exception e)
            {
                CapturadorExcepcion(e, "AccesoOfertas.png", "No se pudo acceder a la sección de Ofertas");
                throw e;
            }
        }

        /// <summary>
        /// Login a Lyntia, incluyendo el acceso previo
        /// </summary>
        public void Login()
        {
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(Utils.getByElement("Login.firstInput")));
                Utils.SearchWebElement("Login.firstInput").Clear();
                Utils.SearchWebElement("Login.firstInput").SendKeys(Utils.getDatoEntorno("usuario"));
                waitAndClick("Login.submitButton");
                //Thread.Sleep(1000);
                wait.Until(ExpectedConditions.ElementToBeClickable(Utils.getByElement("Login.secondInput")));
                Utils.SearchWebElement("Login.secondInput").SendKeys(Utils.getDatoEntorno("password")); //pass de entorno lyntia
                waitAndClick("Login.submitButton");
                waitAndClick("Login.notPersistanceButton"); //Desea mantener la sesion iniciada NO

                TestContext.WriteLine("Se realiza login de manera correcta");
            }
            catch (Exception e)
            {
                CapturadorExcepcion(e, "Login.png", "No se pudo realizar el login de forma correcta");
                throw e;
            }
        }

        /// <summary>
        /// Método capturador de excepciones, extensible a toda la aplicación.
        /// Realiza captura en formato .png del momento del error, histórico de
        /// la prueba y la excepción completa.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fileName"></param>
        /// <param name="message"></param>
        public static void CapturadorExcepcion(Exception e, String fileName, String message)
        {
            string fullFileName = TestContext.CurrentContext.Test.Name + "-" + fileName;

            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(@"..\..\..\..\Lyntia\Screenshots\" + fullFileName);
            AllureLifecycle.Instance.AddAttachment(@"..\..\..\..\Lyntia\Screenshots\" + fullFileName);
            TestContext.AddTestAttachment(@"..\..\..\..\Lyntia\Screenshots\" + fullFileName);

            TestContext.WriteLine(message);

            TestContext.WriteLine("-------------------- - ");
            TestContext.WriteLine("-------------------- - ");            
            TestContext.WriteLine("Excepción: " + e);
        }

        public static void checkAlert()
        {
            
            IAlert alert = ExpectedConditions.AlertIsPresent().Invoke(driver);
            if (alert != null)
            {
                driver.SwitchTo().Alert().Accept();                                

            }
        }

        public static void seleccionarElementoTabla(int row, int col)
        {            
            driver.FindElement(By.XPath("//i[@data-icon-name='CheckMark']")).Click();                        
        }

        //public void seleccionarElementoTabla(string nameElement)
        //{
        //    if (Utils.EncontrarElemento(By.CssSelector("label[title='" + nameElement + "']")))
        //        waitAndClick(By.CssSelector("label[title='" + nameElement + "']"));

        //    else if (Utils.EncontrarElemento(By.CssSelector("label[title='" + nameElement + "']")))
        //        waitAndClick(By.CssSelector("button[title='" + nameElement + "']"));
        //    else
        //        throw new NullReferenceException();
        //}

        public static void seleccionarPrimeraFilaGrid()
        {
            waitAndClick("Oferta.Grid.selectLine2");
        }

        public static void editarPrimeraFilaGrid()
        {
            seleccionarPrimeraFilaGrid();
            Thread.Sleep(1000);
            waitAndClick("Oferta.buttonEditOferta");
        }

        public static void rellenarCampoInput(string elementoKey, string text)
        {
            waitAndClick(elementoKey);
            limpiarCampo(elementoKey);
            getElement(elementoKey).SendKeys(text);
            getElement(elementoKey).SendKeys(Keys.Enter);
            Thread.Sleep(1000);
        }

        public static void rellenarCampoConDesplegable(string elementoKey, string text)
        {
            rellenarCampoInput(elementoKey, text);
            clickarBotonBuscarDesplegable(elementoKey);
            Thread.Sleep(500);
            seleccionarDesplegable(text);
            Thread.Sleep(1000);
        }

        public static void seleccionarEnCampoDesplegable(string elementoKey, string text)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(Utils.getByElement(elementoKey)));
            SelectElement drop = new SelectElement(getElement(elementoKey));
            drop.SelectByText(text);
            Thread.Sleep(1000);
        }

        public static void limpiarCampo(string keyDiccionario)
        {
            waitAndClick(keyDiccionario);
            Thread.Sleep(500);
            Utils.SearchWebElement(keyDiccionario).Clear();            
            Utils.SearchWebElement(keyDiccionario).SendKeys(Keys.Control + "a");
            Utils.SearchWebElement(keyDiccionario).SendKeys(Keys.Delete);

        }

        public static void seleccionarDesplegable(string textoBusqueda)
        {            
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li[aria-label*='" + textoBusqueda + "']")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li[aria-label*='" + textoBusqueda + "']")));
            string id = driver.FindElement(By.CssSelector("li[aria-label ^='" + textoBusqueda + "']")).GetAttribute("id");
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("document.getElementById('" + id + "').click()");
        }

        //public static void seleccionarDesplegable(string textoBusqueda, string dicctionaryKey)
        //{                        
        //    limpiarCampo(dicctionaryKey);

        //    //Click en input
        //    waitAndClick(dicctionaryKey);

        //    //Click boton buscar
        //    string ariaLabel = driver.FindElement(Utils.getByElement(dicctionaryKey)).GetAttribute("aria-label");
        //    string idButton = getElement(By.CssSelector("input[aria-label*='" + ariaLabel + "']+button")).GetAttribute("id");
        //    getElement(dicctionaryKey).SendKeys(textoBusqueda);
        //    Thread.Sleep(500);
        //    waitAndClick(By.Id(idButton));
        //    Thread.Sleep(500);
        //    waitAndClick(By.CssSelector("li[aria-label *='" + textoBusqueda + "']"));
        //    waitAndClick(By.Id(idButton));
        //}

        public static void clickarBotonBuscarDesplegable(string dicctionaryKey)
        {
            string ariaLabel = driver.FindElement(Utils.getByElement(dicctionaryKey)).GetAttribute("aria-label");
            string idButton = getElement(By.CssSelector("input[aria-label*='" + ariaLabel + "']+button")).GetAttribute("id");
            Thread.Sleep(500);
            waitAndClick(By.Id(idButton));
            Thread.Sleep(500);
        }

        public static IWebElement getElement(string keyDictionary)
        {
            waitElement(keyDictionary);
            return Utils.SearchWebElement(keyDictionary);
        }

        public static IWebElement getElement(By locator)
        {
            waitElement(locator);
            return driver.FindElement(locator);
        }

        public static IList<IWebElement> getElements(By locator)
        {            
            waitElement(locator);
            return driver.FindElements(locator);
        }

        public static void waitAndClick(String elementKey)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Utils.getByElement(elementKey)));
            getElement(elementKey).Click();
            Thread.Sleep(1000);
        }

        public static void waitAndClick(By byElement)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(byElement));
            getElement(byElement).Click();
            Thread.Sleep(1000);
        }

        public static void waitElement(By locator)
        {            
           wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));

            if (!driver.FindElement(locator).Displayed)
                Thread.Sleep(2000);
        }

        public static void waitElement(string keyDictionary)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("section[id='appProgressIndicatorView_popupContainer']")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Utils.getByElement(keyDictionary)));

            if (!Utils.SearchWebElement(keyDictionary).Displayed)
                Thread.Sleep(2000);
        }

        public static void waitInvisibilityOfElement(By locator)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);            
        }
        
        public static void esperarGuardado()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='presentation']//span[text()='Guardando...']")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
        }

        public static void agregarFiltroAvanzado(string nombreFiltro, string valorFiltro)
        {
            try
            {
                waitAndClick(By.CssSelector("input[value='']:not([value*='igual'])+button"));
                waitAndClick("Filtro.buttonFiltro");
                waitAndClick("Filtro.agregarFiltro");
                waitAndClick("Filtro.agregarFilaFiltro");
                waitAndClick("Filtro.tipoFiltro");
                waitAndClick(By.XPath("//span[text()='" + nombreFiltro + "']"));
                Thread.Sleep(500);
                waitAndClick("Filtro.valorFiltro");
                Thread.Sleep(500);
                rellenarCampoInput("Filtro.valorFiltro", valorFiltro);
                waitAndClick("Filtro.aplicarFiltro");
            }
            catch(Exception e)
            {
                CapturadorExcepcion(e, "AgregarFiltroAvanzado.png", "No se pudo agregar un filtro avanzado");
            }

        }
    }
}
