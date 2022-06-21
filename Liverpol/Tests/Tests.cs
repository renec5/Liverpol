using Liverpol.ConfigFiles;
using Liverpol.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Threading;

namespace Liverpol
{
    public class Tests
    {        
        [SetUp]
        public void Setup()
        {
            WebBrowser.OpenBrowser("chrome");
            ReportResult RP = new ReportResult(WebBrowser.Driver);
            RP.StartReport();
            WebBrowser.NavigateToURL(TestContext.Parameters.Get("liverpoolURL"));
        }

        [Test]
        [Order(1)]
        [Category("Search")]
        [Author("Rene Cortes")]
        [Description("Searchs for a Pantalla and validates we have outcoming results")]
        public void TestSearchArticle()
        {
            string item = "Pantalla";            
            HomePage HP = new HomePage(WebBrowser.Driver);
            HP.SearchArticle(item);            
            // Validating search result returns items and flow is correct
            if (HP.TotalResults > 0) { ReportResult.Log("pass", "We have " + HP.TotalResults + " products available for item: " + item); }            
            Assert.IsTrue(HP.PantallaSearchResults);                        
            Assert.Greater(HP.TotalResults, 0, "There are no products available for item: " + item);
        }

        [Test]
        [Order(2)]
        [Category("Search")]
        [Author("Rene Cortes")]
        [Description("Searchs for a Pantalla and validates we have outcoming results with small functions that create the hole search and validation process")]
        public void TestSearchWithCustomizedFunctions()
        {
            HomePage HP = new HomePage(WebBrowser.Driver);
            //WebBrowser.NavigateToURL(TestContext.Parameters.Get("liverpoolURL"));
            HP.SearchSmallAPI("Pantalla");
        }

        [Test]
        [Order(3)]
        [Category("Search")]
        [Author("Rene Cortes")]
        [Description("Searchs for an unavailable product to validate we get notified no products are available")]
        public void NoSearchResults()
        {
            string item = "selenium";
            //WebBrowser.NavigateToURL(TestContext.Parameters.Get("liverpoolURL"));
            HomePage HP = new HomePage(WebBrowser.Driver);
            HP.SearchArticle(item);
            // Validating search result returns items and flow is correct
            int result = HP.NoSearchResultsTotal;
            if (HP.NoSearchResultsTotal == 0) { ReportResult.Log("pass", "We have " + HP.NoSearchResultsTotal + " products available for item: " + item); }            
            Assert.AreEqual(0, HP.NoSearchResultsTotal, "There are 1 or more items available for: " + item);
        }

        [Test]
        [Category("Filter")]
        [Author("Rene Cortes")]
        [Description("Searchs for a Smart TV and filter results by brand, size and model")]
        public void FilterBrand_PC_Model()
        {
            string item = "Smart TV";
            HomePage HP = new HomePage(WebBrowser.Driver);
            HP.SearchArticle(item);
            if (HP.TotalResults > 0) { ReportResult.Log("pass", "We have " + HP.TotalResults + " products available for item: " + item); }
            Assert.IsTrue(HP.PantallaSearchResults);
            SearchResultsPage SR = new SearchResultsPage(WebBrowser.Driver);
            //SR.FilterBrand(TestContext.Parameters.Get("Brand"));
            SR.FilterAll("brand", TestContext.Parameters.Get("brandLG"));
            StringAssert.Contains(TestContext.Parameters.Get("brandLG"), SR.SelectedFilters);
            SR.FilterAll("size", TestContext.Parameters.Get("size55"));
            StringAssert.Contains(TestContext.Parameters.Get("size55"), SR.SelectedFilters);
            SR.FilterAll("model", TestContext.Parameters.Get("model"));
            StringAssert.Contains(TestContext.Parameters.Get("model"), SR.SelectedFilters);
        }

        [Test]
        [Category("Filter")]
        [Author("Rene Cortes")]
        [Description("Searchs for a Smart TV and filter results by brand, size and price range")]
        public void FilterBrand_Size_Price()
        {
            string item = "Smart TV";
            HomePage HP = new HomePage(WebBrowser.Driver);
            HP.SearchArticle(item);
            if (HP.TotalResults > 0) { ReportResult.Log("pass", "We have " + HP.TotalResults + " products available for item: " + item); }
            Assert.IsTrue(HP.PantallaSearchResults);
            SearchResultsPage SR = new SearchResultsPage(WebBrowser.Driver);
            // Filter by LG brand
            SR.FilterAll("brand", TestContext.Parameters.Get("brandLG"));
            StringAssert.Contains(TestContext.Parameters.Get("brandLG"), SR.SelectedFilters);
            //Filter by TCL brand
            SR.FilterAll("brand", TestContext.Parameters.Get("brandTCL"));
            StringAssert.Contains(TestContext.Parameters.Get("brandTCL"), SR.SelectedFilters);
            //Filter by Size 43
            SR.FilterAll("size", TestContext.Parameters.Get("size32"));
            StringAssert.Contains(TestContext.Parameters.Get("size32"), SR.SelectedFilters);
            SR.FilterAll("price", TestContext.Parameters.Get("priceG5000"));
            StringAssert.Contains(TestContext.Parameters.Get("priceG5000FilterResult"), SR.SelectedFilters);
        }

        [Test]
        [Category("Account Creation")]
        [Author("Rene Cortes")]
        [Description("Create a new account to be able to be logged when buying")]
        public void CreateAccount()
        {
            HomePage HP = new HomePage(WebBrowser.Driver);
            HP.IniciarSesion();            
            CreateAccountPage CA = new CreateAccountPage(WebBrowser.Driver);
            CA.ClickCreateAccount();
            CA.FillCreateAccountForm();
            CA.FillPersonalDataForm();
            CA.EnterCellPhone();            
            CA.EnterVerificationCode();
            StringAssert.Contains("El código que ingresó es incorrecto", CA.ErrorMessage);
        }

        [TearDown]
        public void CleanUp()
        {
            WebBrowser.CloseBrowser();
            ReportResult.ReportManager.Flush();
        }
    }
}