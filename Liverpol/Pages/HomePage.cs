using Liverpol.ConfigFiles;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Liverpol.Pages
{
    class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }
        public static WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

        IWebElement SearchBar => Driver.FindElement(By.XPath("//input[@id='mainSearchbar']"));

        public static string searchButtonXpath = "//i[@class='icon-zoom']";
        IWebElement SearchButton => Driver.FindElement(By.XPath(searchButtonXpath));

        public static string searchResultsCSS = "span.searchNum-result";
        public bool PantallaSearchResults => wait.Until(EC.ElementExists(By.CssSelector(searchResultsCSS))).Displayed;
        public int TotalResults => Convert.ToInt32(wait.Until(EC.ElementExists(By.CssSelector(searchResultsCSS))).Text.Trim().Split('(', ' ')[1]);

        public static string NoSearchResultsCSS = "h1.a-headline__results strong:nth-of-type(2)";
        public bool NoSearchResults => wait.Until(EC.ElementExists(By.CssSelector(NoSearchResultsCSS))).Displayed;
        public int NoSearchResultsTotal => Convert.ToInt32(wait.Until(EC.ElementExists(By.CssSelector(NoSearchResultsCSS))).Text.Replace("\"", string.Empty).Trim());
        private IWebElement IniciarSesionButton => Driver.FindElement(By.XPath("//span[contains(text(),'Iniciar sesión')]"));

        public void SearchArticle(string articleToSearch)
        {
            try
            {
                SearchBar.SendKeys(articleToSearch);
                ReportResult.Log("pass", "Data has been typed succesfully on search bar", true);
                SearchButton.Click();
                ReportResult.Log("info", "Click has been performed correctly on search button");
                if (PantallaSearchResults || NoSearchResults)
                {
                    ReportResult.Log("pass", "Results for " + articleToSearch + " search are displayed now", true);
                }
            }
            catch
            {
                ReportResult.Log("fail", "Data could not be typed on search bar", true);
            }
        }

        public void EnterArticleToSearch(string articleToSearch)
        {
            WebElements.Send_Keys(SearchBar, "Search Bar", articleToSearch);
        }

        public void ClickSearchButton()
        {
            WebElements.ClickElement(searchButtonXpath, "Search Button");
        }

        public static void ValidateSearchResults()
        {
            Assert.IsTrue(WebElements.IsElementDisplayed("css", searchResultsCSS, "Search Results"));
        }

        public void SearchSmallAPI(string itemToSearch)
        {
            EnterArticleToSearch(itemToSearch);
            ClickSearchButton();
            ValidateSearchResults();
        }

        internal void IniciarSesion()
        {
            try 
            {
                IniciarSesionButton.Click();
                ReportResult.Log("pass", "Click on Iniciar sesion has been performed correclty");
            }
            catch
            {
                ReportResult.Log("fail", "Click on Iniciar sesion could not be performed");
            }
            
        }

    }
}
