using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Liverpol.ConfigFiles
{
    class CreateAccountPage : BasePage
    {
        public CreateAccountPage(IWebDriver driver) : base(driver) { }
        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        private IWebElement CreateAccountButton => Driver.FindElement(By.XPath("//a[contains(text(),'Crear cuenta')]"));
        private IWebElement EmailField => wait.Until(EC.ElementExists(By.XPath("//input[@id='email']")));
        private IWebElement PasswordField => wait.Until(EC.ElementExists(By.XPath("//input[@id='password']")));
        private IWebElement SubmitNewAccountButton => Driver.FindElement(By.XPath("//button[contains(text(),'Crear cuenta')]"));
        private IWebElement Nombres => wait.Until(EC.ElementExists(By.XPath("//input[@id='input-user__name']")));
        private IWebElement LastName => Driver.FindElement(By.XPath("//input[@id='input-user__apaterno']"));
        private IWebElement MotherLastName => Driver.FindElement(By.XPath("//input[@id='input-user__amaterno']"));
        private SelectElement DaySelector => new SelectElement(Driver.FindElement(By.XPath("//select[@id='daySelectorLabel']")));
        private SelectElement MonthSelector => new SelectElement(Driver.FindElement(By.XPath("//select[@id='monthSelectorLabel']")));
        private SelectElement YearSelector => new SelectElement(Driver.FindElement(By.XPath("//select[@id='yearSelectorLabel']")));
        private IWebElement SexoMujer => Driver.FindElement(By.XPath("//input[@id='female']"));
        private IWebElement SexoHombre => Driver.FindElement(By.XPath("//input[@id='male']"));
        private IWebElement CellphoneField => wait.Until(EC.ElementExists(By.XPath("//input[@id='phone']")));
        private IWebElement VerificationCodeField => wait.Until(EC.ElementExists(By.XPath("//input[@id='code']")));
        private IWebElement ContinuarButton => Driver.FindElement(By.XPath("//button[contains(text(),'Continuar')]"));
        public string ErrorMessage => wait.Until(EC.ElementExists(By.XPath("//span[@id='error-element-code']"))).Text.Trim();



        internal void ClickCreateAccount()
        {
            try
            {
                CreateAccountButton.Click();
                ReportResult.Log("info", "Click on Crear cuenta perfomed correctly");
            }
            catch 
            {
                ReportResult.Log("fail", "Click on Crear cuenta could not be performed");
            }
        }
        internal void FillCreateAccountForm()
        {
            try
            {
                EmailField.SendKeys(TestContext.Parameters.Get("testEmail"));
                PasswordField.SendKeys(TestContext.Parameters.Get("testPassword"));
                ReportResult.Log("pass", "email and password have been entered correctly", true);
                SubmitNewAccountButton.Click();
                ReportResult.Log("info", "Click on Crear cuenta perfomed correctly to request new account");
            }
            catch 
            {
                ReportResult.Log("fail", "Click on Crear cuenta could not be performed to request new account");
            }
        }

        internal void FillPersonalDataForm()
        {
            Nombres.SendKeys(TestContext.Parameters.Get("name"));
            LastName.SendKeys(TestContext.Parameters.Get("lastName"));
            MotherLastName.SendKeys(TestContext.Parameters.Get("motherLastName"));
            DaySelector.SelectByText(TestContext.Parameters.Get("dayOfBirth"));
            MonthSelector.SelectByText(TestContext.Parameters.Get("monthOfBirth"));
            YearSelector.SelectByText(TestContext.Parameters.Get("yearOfBirth"));
            SexoHombre.Click();
            SubmitNewAccountButton.Click();
        }

        internal void EnterCellPhone()
        {
            try
            {
                CellphoneField.SendKeys(TestContext.Parameters.Get("cellPhone"));
                ReportResult.Log("pass", "Cellphone number has been entered correctly", true);
                ContinuarButton.Click();
            }
            catch (Exception)
            {
                ReportResult.Log("pass", "Cellphone number could not be entered");
            }
        }

        internal void EnterVerificationCode()
        {
            try
            {
                VerificationCodeField.SendKeys(TestContext.Parameters.Get("verificationCode"));
                ReportResult.Log("pass", "Verification code has been entered successfully", true);
                ContinuarButton.Click();
            }
            catch 
            {
                ReportResult.Log("pass", "Verification code could not be entered");
            }
        }
    }
}
