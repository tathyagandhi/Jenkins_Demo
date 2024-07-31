using AutoFramework.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFramework.Base
{
    public abstract class BasePage : Base
    {
        public BasePage()
        {
            PageFactory.InitElements(DriverContext.Driver, this);
        }

        public static void WaitForElement(IWebDriver driver, IWebElement ele, long time)
        {
            try
            {
                //string temp = ele.GetAttribute("href");
                if (time > 0)
                {
                    Helper.WaitForPageToLoad(driver);
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(ele));
                    //wait.Until(ExpectedConditions.ElementToBeClickable(ele));
                    //wait.Until(d => ele.Displayed);
                    wait.Until(d => ele.Enabled);
                }
                else
                {
                    Helper.WaitForPageToLoad(driver);
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(ele));
                    //wait.Until(ExpectedConditions.ElementToBeClickable(ele));
                    //wait.Until(d => ele.Displayed);
                    wait.Until(d => ele.Enabled);
                }

            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }

        }

    }
}
