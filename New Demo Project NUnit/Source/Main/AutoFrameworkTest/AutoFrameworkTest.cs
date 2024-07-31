using AutoFramework.Base;
using AutoFrameworkTest.Data;
using AutoFrameworkTest.Pages;
using AutoFrameworkTest.Utility;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using RelevantCodes.ExtentReports;
using System;
using System.Configuration;
using System.Threading;

namespace AutoFrameworkTest
{
    [TestFixture]
    public class AutoFrameworkTest : Base
    {
        #region Fields
        public TestContext TestContext { get; set; }
        string url = ConfigurationManager.AppSettings["URL"];
        string chromePath = ConfigurationManager.AppSettings["ChromePath"];
        string fileDownloadPath = ConfigurationManager.AppSettings["FileDownloadPath"];
        #endregion

        [OneTimeSetUp]
        public static void TestClassInitialize()
        {
            ExtentManager();
        }
        
        #region private methods
        public void OpenBrowser(BrowserType browserType = BrowserType.FireFox)
        {
            switch (browserType)
            {

                case BrowserType.InternetExplorer:
                    DriverContext.Driver = new InternetExplorerDriver();
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;

                case BrowserType.FireFox:
                    DriverContext.Driver = new FirefoxDriver();
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;

                case BrowserType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("no-sandbox");
                    var FileDownloadPath = fileDownloadPath;
                    chromeOptions.AddUserProfilePreference("download.default_directory", FileDownloadPath);
                    chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    DriverContext.Driver = new ChromeDriver(chromePath, chromeOptions, TimeSpan.FromMinutes(30));
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;

            }

        }
        #endregion

        [Test, TestCaseSource(typeof(ObjectMother), nameof(ObjectMother.ReadTestData1))]
        public void Verify_Fill_DailyTimeSheet(string[] testdata)
        {
            try
            {
                test = extent.StartTest("Verify Fill Daily TimeSheet");
                var data = ObjectMother.GetTestData(testdata[0]);
                OpenBrowser(BrowserType.Chrome);
                test.Log(LogStatus.Pass, "Open Browser", "Browser has been opened");
                DriverContext.Browser.GotoUrl(url);
                test.Log(LogStatus.Pass, "Navigate URL", "Navigated on this :" + DriverContext.Driver.Url);
                Thread.Sleep(5000);
                CurrentPage = GetInstance<HarmonixLogInPage>();
                CurrentPage.As<HarmonixLogInPage>().Login(data.UserName, data.Password);
                CurrentPage.As<HarmonixLogInPage>().LoginVerify();

                CurrentPage = GetInstance<HarmonixFrontPage>();
                CurrentPage.As<HarmonixFrontPage>().Goto_Home();
                CurrentPage.As<HarmonixFrontPage>().Fill_TimeSheet();
                CurrentPage.As<HarmonixFrontPage>().SelectProjects(data.Projects, data.Projects2);
                CurrentPage.As<HarmonixFrontPage>().VerifyAddedProjects(data.Projects, data.Projects2);
                CurrentPage.As<HarmonixFrontPage>().RemoveProjects(data.Projects2);
                CurrentPage.As<HarmonixLogInPage>().LogOut();
                


                test.Log(LogStatus.Pass, "Verify Fill Daily TimeSheet test case status", "Passed");
                PassCount = PassCount + 1;
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                ScreenShot.CaptureScreenshot(DriverContext.Driver, "Verify_Fill_DailyTimeSheet");
                test.Log(LogStatus.Fail, ex.Message, test.AddBase64ScreenCapture(ScreenShot.imgFormat));
                FailCount = FailCount + 1;
            }
        }
        

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                DriverContext.Driver.Quit();
                extent.EndTest(test);
            }
            catch (System.Exception)
            {
            }
        }

        [OneTimeTearDown]
        public static void ClassCleanup()
        {
            extent.Flush();
            extent.Close();
            //email_send(HTMLReportPath, envionment_ProjectName);
        }
    }
}