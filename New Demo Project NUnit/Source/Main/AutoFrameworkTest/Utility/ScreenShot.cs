using AutoFramework.Base;
using OpenQA.Selenium;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFrameworkTest.Utility
{
    public class ScreenShot : Base
    {
        public static String Screenshotpath = CreateDateTimeFolder();
        public static String Imagepath;
        public static string imgFormat;

        public static void CaptureScreenshot(IWebDriver driver, string screenShotName)
        {
            try
            {
                string dateScreen = DateTime.Now.ToString("MMddyyyy_hhmmss_tt");
                screenShotName = screenShotName + "_" + dateScreen;

                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile(Screenshotpath + "\\" + screenShotName + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                Imagepath = Screenshotpath + "\\" + screenShotName + ".png";
                //Convert image in base64 format to open in Extent Report.
                Image image = Image.FromFile(Imagepath);
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        string base64String;
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                        imgFormat = "data:image/png;base64," + base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
                test.Log(LogStatus.Fail, ex.Message, test.AddBase64ScreenCapture(ScreenShot.imgFormat));
            }
        }

        public static String CreateDateTimeFolder()
        {

            DateTime TestStart = DateTime.Now;
            string OutputResultPath = Path.Combine(Base.GetParentDirectory(), "Screenshots\\");
            string datefold = TestStart.ToString("MMddyyyy"), timefold = TestStart.ToString("HHmmss");

            //Create Folder With datefold (if not exists) and under datefold folder create folder with time fold (if not exists)
            string CreateResultFolder = @"\" + Convert.ToString(Directory.CreateDirectory(Path.Combine(OutputResultPath, datefold))) + @"\";
            OutputResultPath = OutputResultPath + CreateResultFolder;
            CreateResultFolder = Convert.ToString(Directory.CreateDirectory(Path.Combine(OutputResultPath, timefold)));
            OutputResultPath += CreateResultFolder + "\\";

            return OutputResultPath;
        }

    }
}
