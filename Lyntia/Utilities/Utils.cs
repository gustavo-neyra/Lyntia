using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Lyntia.TestSet.Actions;
using Lyntia.TestSet.Conditions;
using System.Linq;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Firefox;
using System.Xml;
using System.Collections.Generic;

namespace Lyntia.Utilities
{
    public class Utils
    {
        public static IWebDriver driver;
        public static ObjectRepositoryUtils objRep;
        private static TestDataUtils dataRep;
        private static OfertaActions ofertaActions;
        private static OfertaConditions ofertaCondition;
        private static ProductoActions productoActions;
        private static ProductoConditions productoCondition;
        private static CommonActions commonActions;
        private static CommonConditions commonCondition;
        private static String randomString;        
        private static readonly Random random = new Random();
        static Dictionary<String, String> database = new Dictionary<String, String>();
        static Dictionary<String, String> datosEntorno = new Dictionary<String, String>();
        private static string fechaHoraActualFormateada = "";

        public void Instanciador()
        {
            try
            {
                getProjectProperties(@"..\..\..\..\Lyntia\Utilities\ProjectProperties.xml");
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(database["windowsSize"]);
                if (database["headless"].ToLower() == "true")
                    chromeOptions.AddArguments("headless");

                driver = new ChromeDriver(chromeOptions);//chromeOption                
                objRep = ObjectRepositoryUtils.Instance;
                objRep.TestDataReader("ObjectRepository.csv");
                dataRep = TestDataUtils.Instance;                
                ofertaActions = new OfertaActions();
                ofertaCondition = new OfertaConditions();
                productoActions = new ProductoActions();
                productoCondition = new ProductoConditions();
                commonActions = new CommonActions();
                commonCondition = new CommonConditions();
                randomString = RandomString(15);

                driver.Navigate().GoToUrl(datosEntorno["url"]);
                
                if (database["headless"].ToLower() != "true") {
                    driver.Manage().Window.Position = new System.Drawing.Point(-1000, 100);
                    driver.Manage().Window.Maximize();
                }
                
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Int32.Parse(database["tiempoEsperaMaximo"]));
                inicializarDiccionarioDatos();

                TestContext.WriteLine("Instanciador iniciado correctamente");
                TestContext.WriteLine("-------------------- - ");
            } catch(Exception e) {
                TestContext.WriteLine("Error al iniciar el instaciador : " + e);
                throw e;
            }  
        }

        public static OfertaActions GetOfertaActions()
        {
            return ofertaActions;
        }

        public static String GetRandomString()
        {
            return randomString;
        }

        public static OfertaConditions GetOfertaConditions()
        {
            return ofertaCondition;
        }

        public static ProductoActions GetProductoActions()
        {
            return productoActions;
        }

        public static ProductoConditions GetProductoConditions()
        {
            return productoCondition;
        }

        public static CommonActions GetCommonActions()
        {
            return commonActions;
        }

        public static CommonConditions GetCommonConditions()
        {
            return commonCondition;
        }

        public static string getDatoRepositorio(string nombreDato)
        {
            return dataRep.getData(nombreDato);
        }       
        
        public void inicializarDiccionarioDatos()
        {
            //Cargamos el diccionario de objetos
            if (TestContext.CurrentContext.Test.ClassName.Contains("Oferta"))
                loadData(@"..\..\..\..\Lyntia\Utilities\DataRepositoryOferta.xml", TestContext.CurrentContext.Test.Name);
            if (TestContext.CurrentContext.Test.ClassName.Contains("Producto"))
                loadData(@"..\..\..\..\Lyntia\Utilities\DataRepositoryProducto.xml", TestContext.CurrentContext.Test.Name);
        }

        public static bool EncontrarElemento(By by)
        {
            IWebElement element;
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                element = driver.FindElement(by);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(45);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789*";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static String eliminarAcentos(String texto)
        {
            if (texto.Contains("á"))
                texto = texto.Replace("á", "a");
            if (texto.Contains("é"))
                texto = texto.Replace("é", "e");
            if (texto.Contains("í"))
                texto = texto.Replace("í", "i");
            if (texto.Contains("ó"))
                texto = texto.Replace("ó", "o");
            if (texto.Contains("ú"))
                texto = texto.Replace("ú", "u");
            if (texto.Contains("Á"))
                texto = texto.Replace("Á", "A");
            if (texto.Contains("É"))
                texto = texto.Replace("É", "E");
            if (texto.Contains("Í"))
                texto = texto.Replace("Í", "I");
            if (texto.Contains("Ó"))
                texto = texto.Replace("Ó", "O");
            if (texto.Contains("Ú"))
                texto = texto.Replace("Ú", "U");
            return texto;
        }

        public void BorradoScreenshots()
        {
            DirectoryInfo Dr = new DirectoryInfo(@"..\..\..\..\Lyntia\Screenshots\");
            FileInfo[] files = Dr.GetFiles("*.png").Where(p => p.Extension == ".png").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
        }

        public static IWebElement SearchWebElement(String identificador)
        {
            try
            {
                return (objRep.TypeObjectID(identificador)) switch
                {
                    "XPATH" => driver.FindElement(By.XPath(objRep.ObjectID(identificador))),
                    "ID" => driver.FindElement(By.Id(objRep.ObjectID(identificador))),
                    "CSS" => driver.FindElement(By.CssSelector(objRep.ObjectID(identificador))),
                    _ => null,
                };
            }
            catch(NoSuchElementException e)
            {
                Console.WriteLine("No se pudo interactuar con el elemento " + identificador + " de tipo " + objRep.TypeObjectID(identificador));
                Console.WriteLine("Excepción : " + e);

                return null;
            }  
        }

        public static By getByElement(String identificador)
        {            
            try
            {
                return (objRep.TypeObjectID(identificador)) switch
                {
                    "XPATH" => By.XPath(objRep.ObjectID(identificador)),
                    "ID" => By.Id(objRep.ObjectID(identificador)),
                    "CSS" => By.CssSelector(objRep.ObjectID(identificador)),
                    _ => null,
                };
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("No se pudo interactuar con el elemento " + identificador + " de tipo " + objRep.TypeObjectID(identificador));
                Console.WriteLine("Excepción : " + e);

                return null;
            }
        }

        public static String GetIdentifier(String identificador)
        {
            String ident = objRep.ObjectID(identificador);
            return ident;
        }

        public string modificarMesFecha(string fecha, int sumMes)
        {
            var parseDate = DateTime.Parse(fecha);
            DateTime fechaModificada = parseDate.AddMonths(sumMes);
            return fechaModificada.ToString("dd/MM/yyyy");
        }

        public static string getFechaActual()
        {
            DateTime hoy = DateTime.Now;
            return hoy.ToString("dd/MM/yyyy");
        }

        public static string getFechaHoraActualFormateada()
        {
            DateTime hoy = DateTime.Now;
            return (hoy.ToString("dd/MM/yy") + hoy.ToString("HH/mm")).Replace("/", "");
        }

        public static void scrollToElementByXpath(string xPath)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].scrollIntoView(false);", driver.FindElement(By.XPath(xPath)));
        }

        public static void scrollToElementByCss(string cssSelector)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].scrollIntoView(false);", driver.FindElement(By.CssSelector(cssSelector)));
        }

        public void marcarElementoSeleccionadoByID(string id)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('" + id + "').style = 'border: 4px solid #F2C524;'");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("El elemento al que intenta acceder no existe");
            }
        }

        public void marcarElementoSeleccionadoByName(string name)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementByName('" + name + "').style = 'border: 4px solid #F2C524;'");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("El elemento al que intenta acceder no existe");
            }
        }

        public void marcarElementoSeleccionadoByCSS(string byIdentificador)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("document.querySelector('" + byIdentificador + "').style = 'border: 4px solid #F2C524;'");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("El elemento al que intenta acceder no existe");
            }
        }

        public static void clickWithJavascript(string keyDicctionary)
        {
            string idRelacionados = CommonActions.getElement(keyDicctionary).GetAttribute("id");
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("document.getElementById('" + idRelacionados + "').click()");
        }

        public void getProjectProperties(string filename)
        {
            //Create the XmlDocument.
            XmlDocument doc;
            doc = new XmlDocument();
            doc.Load(filename);

            //Display all the book titles.
            XmlNodeList propiedades = doc.GetElementsByTagName("Properties");
            XmlNodeList datos = propiedades[0].ChildNodes;

            foreach (XmlNode dato in datos)
            {
                //Guardamos los datos del entorno
                if (dato.Name != "datosEntorno")         
                    database.TryAdd(dato.Name, dato.InnerText);
            }
            getDatosEntorno(doc);
        }

        public void getDatosEntorno(XmlDocument doc)
        {
            XmlNodeList propiedades = doc.GetElementsByTagName(database["entorno"]);
            XmlNodeList datos = propiedades[0].ChildNodes;

            foreach (XmlNode dato in datos)            
                datosEntorno.TryAdd(dato.Name, dato.InnerText);            
        }

        public static string getDatoEntorno(string dato)
        {
            return datosEntorno[dato];
        }

        public static void loadData(string fileName, string testCaseName)
        {
            dataRep.testDataReader(fileName, testCaseName);
        }
        
        public static Dictionary<String, String> generarDiccionario(XmlNodeList datos)
        {
            return dataRep.generarDiccionario(datos);
        }

        public static Dictionary<string,string> getDatosTestCase(string nodoRepositorio)
        {            
            return dataRep.getDatosRepositorio(nodoRepositorio);            
        }
    }
}

