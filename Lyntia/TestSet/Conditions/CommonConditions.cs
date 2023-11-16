using Lyntia.TestSet.Actions;
using Lyntia.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Lyntia.TestSet.Conditions
{
	public class CommonConditions
	{
        
        private static IWebDriver driver;
        private static WebDriverWait wait;
        private static Utils utils;

        public CommonConditions()
        {
            driver = Utils.driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
        }

        public void validar_Acceso_A_GestionCliente()
        {
            Assert.AreEqual(true, driver.FindElement(By.LinkText("Gestión del Cliente")).Enabled);
            Assert.AreEqual("Gestión del Cliente", driver.FindElement(By.LinkText("Gestión del Cliente")).Text);
        }

        public void validar_AccesoOferta()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Utils.getByElement("Oferta.ofertasLyntia")));
            Assert.AreEqual("OFERTAS LYNTIA", Utils.SearchWebElement("Oferta.ofertasLyntia").Text);
        }
    }
}
