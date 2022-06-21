using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Liverpol.ConfigFiles
{
    public class BasePage
    {
        protected static IWebDriver Driver { get; set; }
        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler, false);
        }

        


    }
}
