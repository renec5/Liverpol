using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Liverpol.ConfigFiles
{

    public class WebBrowser { 
        public static IWebDriver Driver { get; set; }
        
        public static void OpenBrowser(string browser)
        {
            switch (browser.ToUpper())
            {
                case "CHROME":
                    string outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Driver = new ChromeDriver(outputDirectory);                    
                    break;
            }
        }

        public static void NavigateToURL(string url)
        {
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(url);
            if (Driver.Url == url)
            {
                ReportResult.Log("pass", "WebPage loaded successfully", true);
            }
        }

        public static void CloseBrowser()
        {
            Driver.Close();
            Driver.Quit();
        }
    }
}
