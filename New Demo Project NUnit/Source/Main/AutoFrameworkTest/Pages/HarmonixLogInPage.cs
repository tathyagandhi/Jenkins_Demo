using AutoFramework.Base;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RelevantCodes.ExtentReports;
using System;
using System.Threading;

namespace AutoFrameworkTest.Pages
{
    class HarmonixLogInPage : BasePage
    {

        [FindsBy(How = How.XPath, Using = ".//input[@type='submit']")]
        IWebElement ClickNext { get; set; }

        [FindsBy(How = How.XPath, Using = ".//input[@type='email']")]
        IWebElement UserName_Email { get; set; }

        [FindsBy(How = How.XPath, Using = ".//input[@type='password']")]
        IWebElement Password { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='grid-form1 customGrid']//h3")]
        IWebElement DashboardLabel { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='/Account/SignOut']")]
        IWebElement Logout { get; set; }




        public void Login(String uname, String pass)
        {
            WaitForElement(DriverContext.Driver, UserName_Email, 20);
            UserName_Email.SendKeys(uname);
            ClickNext.Click();

            WaitForElement(DriverContext.Driver, Password, 20);
            Password.SendKeys(pass);
            ClickNext.Click(); //Clicking Signin

            WaitForElement(DriverContext.Driver, ClickNext, 20);
            ClickNext.Click(); //Stay Signin popup: Clicking Yes

            test.Log(LogStatus.Pass, "Login", "User has Logged in with user: " + uname);
        }

        public void LoginVerify()
        {
            string ActualText = DashboardLabel.GetAttribute("innerText");
            string ExpectedText = "Welcome to Harmonix-IT Ltd";
            //string ExpectedText = "Welcome to Harmonix-IT Ltd test";
            Assert.AreEqual(ExpectedText, ActualText);
            test.Log(LogStatus.Pass, "Verify Login", "Verified: User has been logged in successfully.");
        }
       
        public void LogOut()
        {
            WaitForElement(DriverContext.Driver, Logout, 20);
            Logout.Click();
            
        }

    }
}
