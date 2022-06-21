using Liverpol.ConfigFiles;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Liverpol.Pages
{
    class SearchResultsPage : BasePage
    {
        public SearchResultsPage (IWebDriver driver): base(driver) { }
        public static WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        public static string viewMoreBrandsXpath = "(//div[@class='filter-brands']/parent::div/a)[2]";
        private IWebElement ViewMoreBrands => Driver.FindElement(By.XPath(viewMoreBrandsXpath));

        public static string brandsXpath = "//label[contains(text(),'Marcas')]/ancestor::div[@class='m-plp__filterSection ']/div[@class='plp-filter-options active']/div[@class='filter-brands']/div";
        //label[contains(text(),'Marcas')]/ancestor::div[@class='m-plp__filterSection ']/div[@class='plp-filter-options active']/a
        public static IList<IWebElement> Brands => wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath(brandsXpath)));

        public static string sizeXpath = "//label[contains(text(),'Tamaño')]/ancestor::div[@class='m-plp__filterSection ']/div[@class='plp-filter-options active']/div[@class='filter-brands']/div";

        public static IList<IWebElement> Sizes = wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath(sizeXpath)));

        public static string modelXpath = "//label[contains(text(),'Año del Modelo')]/ancestor::div[@class='m-plp__filterSection ']/div[@class='plp-filter-options active']/div[@class='filter-brands']/div";

        public static IList<IWebElement> Models => wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath(modelXpath)));

        public static string pricesXpath = "//label[contains(text(),'Precios')]/ancestor::div[@class='m-plp__filterSection ']//div[@class='fiterl-prices']/div/div";

        public static IList<IWebElement> Prices => wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath(pricesXpath)));   
        
        public string SelectedFilters { get; set; }

        public string SelectedFiltersXpath ="//div[@class='mdc-chip a-plp__filterSelection']/div";

        internal void FilterBrand(string brand)
        {
            ViewMoreBrands.Click();
            foreach (IWebElement brandElement in Brands)
            {                                
                if (brandElement.FindElement(By.XPath("./label")).Text.Trim().StartsWith(brand))
                {
                    brandElement.FindElement(By.XPath("./div")).Click();
                    ReportResult.Log("pass", $"Selecting <b>\"{brand}\"</b> brand", true);
                    break;
                }
                else
                {
                    if (brandElement == Brands.Last())
                    {
                        ReportResult.Log("fail", $"Brand <b>\"{brand}\"</b> was not found");
                    }
                }
            }
        }

        internal void FilterAll(string filterType, string valueToFilter)
        {
            string filters = "";
            IList<IWebElement> elements = null;
            IList<IWebElement> finalFilters = null;
            IList<IWebElement> ViewMoreButtons = null;
            string finalXpathTag = "";
            string xpathContinuation = "";
            try
            {
                ViewMoreButtons = wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath("//div[@class='filter-brands']/parent::div/a")));
            }
            catch { }
            if (ViewMoreButtons != null)
            {
                foreach (IWebElement button in ViewMoreButtons)
                {
                    if (button.Text.Trim() == "Ver más") button.Click();
                }
            }
            
            switch (filterType.ToUpper())
            {
                case "BRAND":                    
                    //elements = Brands;                    
                    elements = Driver.FindElements(By.XPath(brandsXpath));
                    break;
                case "SIZE":
                    elements = Driver.FindElements(By.XPath(sizeXpath));
                    //elements = Sizes;
                    break;
                case "MODEL":
                    //elements = Models;
                    elements = Driver.FindElements(By.XPath(modelXpath));
                    break;
                case "PRICE":
                    elements = Driver.FindElements(By.XPath(pricesXpath));
                    break;
            }
            finalXpathTag = (filterType.ToUpper() == "BRAND" || filterType.ToUpper() == "SIZE" || filterType.ToUpper() == "MODEL") ? "./div" : "./input";
            xpathContinuation = (filterType.ToUpper() == "BRAND" || filterType.ToUpper() == "SIZE" || filterType.ToUpper() == "MODEL") ? "./label" : "./following::label";
            //Selecting the filter
            foreach (IWebElement brandElement in elements)
            {
                //
                //if (brandElement.FindElement(By.XPath("./label")).Text.Trim().StartsWith(valueToFilter))
                if (brandElement.FindElement(By.XPath(xpathContinuation)).Text.Trim().StartsWith(valueToFilter))
                {
                    
                    //brandElement.FindElement(By.XPath("./div")).Click();
                    brandElement.FindElement(By.XPath(finalXpathTag)).Click();
                    ReportResult.Log("pass", $"Selecting <b>\"{valueToFilter}\"</b> brand", true);
                    break;
                }
                else
                {
                    if (brandElement == Brands.Last())
                    {
                        ReportResult.Log("fail", $"Brand <b>\"{valueToFilter}\"</b> was not found");
                    }
                }
            }

            //finalFilters = Driver.FindElements(By.XPath(SelectedFiltersXpath));
            finalFilters = wait.Until(EC.PresenceOfAllElementsLocatedBy(By.XPath(SelectedFiltersXpath)));
            foreach (IWebElement filter in finalFilters)
            {
                filters = filters + " " + filter.Text.Trim();
            }
            SelectedFilters = filters;


        }
    }
}
