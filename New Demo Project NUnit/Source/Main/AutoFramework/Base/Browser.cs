using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFramework.Base
{
    public class Browser
    {
        private readonly IWebDriver _driver;

        public Browser(IWebDriver driver)
        {
            _driver = driver;

        }
        public void GotoUrl(string url)
        {
            Thread.Sleep(5000);
            DriverContext.Driver.Manage().Window.Maximize();
            DriverContext.Driver.Url = url;
            Thread.Sleep(5000);
        }

    }
    public enum BrowserType
    {
        InternetExplorer,
        FireFox,
        Chrome
    }
}
