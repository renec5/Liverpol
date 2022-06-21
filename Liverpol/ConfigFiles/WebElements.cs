using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Liverpol.ConfigFiles
{
    public class WebElements : BasePage
    {
        public WebElements(IWebDriver driver) : base(driver) { }

        private static WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        public static IWebElement element = null;

        public static IWebElement GetWE(string xpath, string elementToGet)
        {
            element = null;
            try
            {
                element = wait.Until(EC.ElementExists(By.XPath(xpath)));
                ReportResult.Log("Pass", "Element: " + elementToGet +" has been found successfully");

            }
            catch 
            {
                ReportResult.Log("Fail", "Element: " + elementToGet + " was not found");
            }
            return element;
        }

        public static void ClickElement(string xpath, string elementToClick)
        {
            element = null;
            try
            {
                element = GetWE(xpath, elementToClick);
                element.Click();
                ReportResult.Log("Pass", "Click on element: " + elementToClick + " has been performed correctly");
            }
            catch
            {
                ReportResult.Log("Fail", "Click on element: " + elementToClick + " could not be performed");
            }
        }

        public static void Send_Keys(IWebElement element, string fieldToSendData, string data)
        {
            try
            {
                element.SendKeys(data);
                ReportResult.Log("pass", "Data: <b> " + data + "</b> has been sent to field: <b>" + fieldToSendData + "</b>");
            }
            catch 
            {
                ReportResult.Log("fail", "Data: <b> " + data + "</b> could not be sent to field: <b>" + fieldToSendData + "</b>");
            }
        }

        public static void SwitchToIframe(string iframeXpath)
        {
            element = null;
            try
            {
                element = GetWE(iframeXpath, "Iframe");
                Driver.SwitchTo().Frame(element);
                ReportResult.Log("Pass", "Switched to iframe correctly");
            }
            catch 
            {
                ReportResult.Log("Fail", "Switch to iframe could not be done");
            }
        }

        public static void SwitchToDefaultContent()
        {
            try
            {
                Driver.SwitchTo().DefaultContent();
                ReportResult.Log("pass", "Switch back to default window correctly");
            }
            catch
            {
                ReportResult.Log("fail", "Could not switch back to default window");
            }
        }

        public static bool IsElementDisplayed(string locatorType, string locator, string elementToCheck)
        {
            element = null;
            bool check = false;
            switch (locatorType.ToUpper())
            {
                case "CSS":
                    element = wait.Until(EC.ElementExists(By.CssSelector(locator)));
                    break;
                case "XPATH":
                    element = GetWE(locator, elementToCheck);
                    break;
            }
            check = ((element != null) && (element.Displayed)) ? true : false;
            return check;

        }


    }
}
