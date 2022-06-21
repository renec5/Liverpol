using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Liverpol.ConfigFiles
{
    class ReportResult : BasePage
    {
        public ReportResult(IWebDriver driver) : base(driver) { }

        private static ExtentHtmlReporter HtmlReport;
        public static ExtentReports ReportManager;
        private static ExtentTest CurrentTest;
        private static string ReportFolder;

        private static void CreateDirectories()
        {
            ReportFolder = @"C:\reports\" + TestContext.CurrentContext.Test.Name + " - " +DateTime.Now.ToString("MMddyyyy-HHmmss");
            if (!Directory.Exists(ReportFolder))
            {
                Directory.CreateDirectory(ReportFolder);
            }
            if (!Directory.Exists(ReportFolder + "\\Screenshots"))
            {
                Directory.CreateDirectory(ReportFolder + "\\Screenshots");
            }
        }

        public void StartReport()
        {
            CreateDirectories();
            HtmlReport = new ExtentHtmlReporter(ReportFolder + "\\");
            ReportManager = new ExtentReports();
            ReportManager.AttachReporter(HtmlReport);
            CurrentTest = ReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        private static string TakeScreenshot()
        {
            string ssName = "Screenshot-" + DateTime.Now.ToString("MMddyyyy-HHmmss");
            Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            screenshot.SaveAsFile(ReportFolder + "\\Screenshots\\" + ssName + ".png");
            return ReportFolder + "\\Screenshots\\" + ssName + ".png";
        } 

        public static void Log(string status, string description, bool takeScreenshot = false)
        {
            if (takeScreenshot)
            {
                string ssPath = TakeScreenshot();
                switch (status.ToUpper())
                {
                    case "PASS":
                        CurrentTest.Log(Status.Pass, description + "<br>", MediaEntityBuilder.CreateScreenCaptureFromPath(ssPath).Build());
                        break;
                    case "FAIL":
                        CurrentTest.Log(Status.Fail, description + "<br>", MediaEntityBuilder.CreateScreenCaptureFromPath(ssPath).Build());
                        break;
                    case "INFO":
                        CurrentTest.Log(Status.Info, description + "<br>", MediaEntityBuilder.CreateScreenCaptureFromPath(ssPath).Build());
                        break;
                }
            }
            else
            {
                switch (status.ToUpper())
                {
                    case "PASS":
                        CurrentTest.Log(Status.Pass, description);
                        break;
                    case "FAIL":
                        CurrentTest.Log(Status.Fail, description);
                        break;
                    case "INFO":
                        CurrentTest.Log(Status.Info, description);
                        break;
                }
            }
        }





    }
}
