using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFramework.Base;
using System.Threading;
using RelevantCodes.ExtentReports;
using NUnit.Framework;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenQA.Selenium.Chrome;
using AutoFrameworkTest.Data;
//using static MongoDB.Driver.WriteConcern;

namespace AutoFrameworkTest.Pages
{
    internal class HarmonixFrontPage : HarmonixLogInPage
    {
        
        [FindsBy(How = How.XPath, Using = "//*[@id='menu']/li[1]")]
        IWebElement ParentHome { get; set; }

        [FindsBy(How = How.XPath, Using = ".//a[@href='/FrontPage']")]
        IWebElement  ChildHome { get; set; }

        [FindsBy(How = How.Id, Using = "idTabMyTimeSheet")]
        IWebElement MytimesheetLabel { get; set; }

        
        [FindsBy(How = How.XPath, Using = "(.//a[@href='#tabTimesheet'])[1]")]
        IWebElement Select_TimeSheetDate { get; set; }

        [FindsBy(How = How.Id, Using = "txttimeIn")]
        IWebElement TimeIN { get; set; }

        [FindsBy(How = How.Id, Using = "txtTimeOut")]
        IWebElement TimeOut { get; set; }

        [FindsBy(How = How.Id, Using = "DailyDiary")]
        IWebElement DailyDiary { get; set; }

        [FindsBy(How = How.Id, Using = "tblprojectbody")]
        IWebElement ProjectsName { get; set; }

        [FindsBy(How = How.Id, Using = "btnprojects")]
        IWebElement AddProject { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='mdlTimesheetProject']//h3")]
        IWebElement FindProject { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='mCSB_5_container']//tr[1]//td[5]")]
        IWebElement SelectProject { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='mCSB_5_container']//tr[2]//td[5]")]
        IWebElement SelectProject2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='btn btn-success classAddProjectTimeSheet']")]
        IWebElement AddTimeSheet { get; set; }

        [FindsBy(How = How.XPath, Using = "//tbody[@id='tblprojectbody']//tr[1]//td[2]")]
        IWebElement Project1 { get; set; }
        [FindsBy(How = How.XPath, Using = "//tbody[@id='tblprojectbody']//tr[2]//td[2]")]
        IWebElement Project2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//tbody[@id='tblprojectbody']//tr[1]//td[5]//input")]
        IWebElement ProjectTimeOFF { get; set; }

        [FindsBy(How = How.XPath, Using = "//tbody[@id='tblprojectbody']//tr[2]//td[4]//input")]
        IWebElement Project2TimeON { get; set; }

        [FindsBy(How = How.XPath, Using = "//tbody[@id='tblprojectbody']//tr[2]//td[5]//input")]
        IWebElement Project2TimeOFF { get; set; }

        [FindsBy(How = How.Id, Using = "TaskCommentsforProject")]
        IWebElement AddProjectcmt { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='btn btn-success classAddTaskCommentsTimeSheet']")]
        IWebElement AddTOTask { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@class='btn btn-success btn-large btnSaveTimesheet']")]
        IWebElement SaveProjects { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='listingTable']//tbody//tr")]
        IList<IWebElement>  ProjectRow { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='table table-striped tblprojects table-bordered']//tbody//tr")]
        IList<IWebElement> AddedProjectRow { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='Timesheetdate']//li")]
        IList<IWebElement> SelectDate { get; set; }


        [FindsBy(How = How.XPath, Using = "//div[@class='bootbox modal fade bootbox-confirm in']//button[@class='btn btn-success']")]
        IWebElement Removeconfirm { get; set; }

        public void Goto_Home() 
        {

            WaitForElement(DriverContext.Driver, ParentHome, 20);
            ParentHome.Click();
            Thread.Sleep(3000);

            WaitForElement(DriverContext.Driver, ChildHome, 20);
            ChildHome.Click();
            Thread.Sleep(3000);

            test.Log(LogStatus.Pass, "Go to Home", "User successfully navigated to Home->home for Time sheet");

        }
        public void Fill_TimeSheet()
        {
            test.Log(LogStatus.Pass, "Fill Timesheet", "Timesheet filling started");

            SelectCurrentDate();

            Thread.Sleep(7000);
            WaitForElement(DriverContext.Driver, TimeIN, 40);
            TimeIN.Click();
            TimeIN.Clear();
            TimeIN.SendKeys("09:00");


            WaitForElement(DriverContext.Driver, TimeOut, 20);
            TimeOut.Click();
            TimeOut.Clear();
            TimeOut.SendKeys("18:30");
            Thread.Sleep(2000);

            WaitForElement(DriverContext.Driver, DailyDiary, 20);
            DailyDiary.Click();
            DailyDiary.Clear();                                 
            DailyDiary.SendKeys("Test Time sheet");
            Thread.Sleep(2000);

            WaitForElement(DriverContext.Driver, AddProject, 20);
            AddProject.Click();
            Thread.Sleep(2000);



        }

 

        public void SelectProjects(String P1, String P2)
        {
            test.Log(LogStatus.Pass, "Select Project", "Adding Project in Timesheet started");

            string ActualText = FindProject.GetAttribute("innerText");
            string ExpectedText = "Find Project";
            Assert.AreEqual(ExpectedText, ActualText);
            test.Log(LogStatus.Pass, "Projects popup", "Verified: Projects popup is opened.");


            int rowcount = ProjectRow.Count;

            for (int i = 1; i <= rowcount; i++)
            {

                string ProjectName = DriverContext.Driver.FindElement(By.XPath("(//*[@class='listingTable']//tbody//tr[" + i + "]//td[3]//table//tr//td[2]//strong)[1]")).Text;
                
                if (ProjectName == P1)
                {
                    String Expected_ProjectName = "UnitTest_24-06-2024";
                    //String Expected_ProjectName = "UnitTest_24-06-2024_Test";
                    Assert.AreEqual(Expected_ProjectName, ProjectName);
                    IWebElement selectProject = DriverContext.Driver.FindElement(By.XPath("(//*[@class='listingTable']//tbody//tr[" + i  + "]//td[5]//a)[1]"));
                    selectProject.Click();
                    Thread.Sleep(3000);

                }
                else if (ProjectName == P2)
                {
                    IWebElement selectProject = DriverContext.Driver.FindElement(By.XPath("(//*[@class='listingTable']//tbody//tr[" + i + "]//td[5]//a)[1]"));
                    selectProject.Click();
                    Thread.Sleep(3000);
                    break;
                }
            }

            WaitForElement(DriverContext.Driver, AddTimeSheet, 20);
            AddTimeSheet.Click();
            Thread.Sleep(5000);

            test.Log(LogStatus.Pass, "Project Added", "This two project added successfully: Project1 :  " + P1 + " Project2 :  " + P2);

        }
        public void VerifyAddedProjects(String P1, String P2)
        {          

            int Addedrowcount = AddedProjectRow.Count;

            for (int i = 1; i <= Addedrowcount; i++)
            {
                //IWebElement proname = (IWebElement)DriverContext.Driver.FindElement(By.XPath("//*[@class='listingTable']//tbody//tr["+i+"]//td[3]"));
                //string ProjectName = proname.GetAttribute("Project name");
                string AddedProjectName = DriverContext.Driver.FindElement(By.XPath("//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[2]")).Text;

                if (AddedProjectName == P1)
                {
                    IWebElement ProjecttimeON = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[4]//input)[1]"));
                    ProjecttimeON.Click();
                    ProjecttimeON.SendKeys("10:00");

                    IWebElement ProjecttimeOFF = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[5]//input)[1]"));
                    ProjecttimeOFF.Click();
                    ProjecttimeOFF.SendKeys("12:00");
                    ProjecttimeOFF.SendKeys(Keys.Tab);
                    Thread.Sleep(2000);

                    WaitForElement(DriverContext.Driver, AddProjectcmt, 20);
                    AddProjectcmt.Click();
                    AddProjectcmt.SendKeys("Task Completed");

                    WaitForElement(DriverContext.Driver, AddTOTask, 20);
                    AddTOTask.Click();
                }
                else if (AddedProjectName == P2)
                {
                    IWebElement Project2timeON = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[4]//input)[1]"));
                    Project2timeON.Click();
                    Project2timeON.SendKeys("13:00");

                    IWebElement Project2timeOFF = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[5]//input)[1]"));
                    Project2timeOFF.Click();
                    Project2timeOFF.SendKeys("18:00");
                    Project2timeOFF.SendKeys(Keys.Tab);
                    Thread.Sleep(2000);

                    WaitForElement(DriverContext.Driver, AddProjectcmt, 20);
                    AddProjectcmt.Click();
                    AddProjectcmt.SendKeys("Task Completed");

                    WaitForElement(DriverContext.Driver, AddTOTask, 20);
                    AddTOTask.Click();

                    break;
                }
            }

            //Save Project after verify
            WaitForElement(DriverContext.Driver, SaveProjects, 20);
            SaveProjects.Click();
            Thread.Sleep(8000);

            //Verify Project added
            test.Log(LogStatus.Pass, "Project Saved", " Project saved successfully with working hours:");


            test.Log(LogStatus.Pass, "Fill Timesheet", "Timesheet filled successfully.");
        }

        

  
        public void RemoveProjects(String P2)
        {
            int rowremovecount = AddedProjectRow.Count;

            for (int i = 1; i <= rowremovecount; i++)
            {
                string AddedProjectName = DriverContext.Driver.FindElement(By.XPath("//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[2]")).Text;
               
                if (AddedProjectName == P2)
                {
                    IWebElement ProjectRemove = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[7]//img)[1]"));
                    ProjectRemove.Click();

                    WaitForElement(DriverContext.Driver, Removeconfirm, 20);
                    Removeconfirm.Click();
                    Thread.Sleep(10000);

                    IWebElement Project2Remove = DriverContext.Driver.FindElement(By.XPath("(//*[@class='table table-striped tblprojects table-bordered']//tbody//tr[" + i + "]//td[7]//img)[1]"));
                    Project2Remove.Click();

                    WaitForElement(DriverContext.Driver, Removeconfirm, 20);
                    Removeconfirm.Click();

                    break;
                }
           
            }
            WaitForElement(DriverContext.Driver, SaveProjects, 20);
            SaveProjects.Click();
            Thread.Sleep(8000);

            test.Log(LogStatus.Pass, "Remove Projects", " Removed Project successfully from Timesheet.");
        }
        public void WriteProject()
        {


            var table = DriverContext.Driver.FindElement(By.XPath("//tbody[@id='tblprojectbody']"));

            List<List<string>> tableData = new List<List<string>>();

            var rows = table.FindElements(By.TagName("tr"));
            foreach (var row in rows)
            {
                List<string> rowData = new List<string>();

                // Iterate through each cell of the row
                var cells = row.FindElements(By.TagName("td"));
                foreach (var cell in cells)
                {
                    rowData.Add(cell.Text);
                }

                tableData.Add(rowData);
            }
            // Write data to a CSV file
            string csvFilePath = "D:\\Projects\\New Demo Project NUnit\\Source\\Main\\AutoFrameworkTest\\TestData\\TableData.csv";
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                foreach (var rowData in tableData)
                {
                    writer.WriteLine(string.Join(",", rowData));
                }
            }

        }
        public void SelectCurrentDate()

        {
            DateTime currentDate = DateTime.Now;

            string formattedDate = currentDate.ToString("dd/MM/yy");

            IReadOnlyCollection<IWebElement> dateElements = DriverContext.Driver.FindElements(By.XPath("//*[@id='Timesheetdate']//li//a"));
            TestContext.Progress.WriteLine(dateElements);
            foreach (IWebElement dateElement in dateElements)
            {
                string dateText = dateElement.Text.Trim();

                string[] dateParts = dateText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string extractedDate = dateParts[1];
                string date = extractedDate.Replace("/", "-");
                


                if (date == formattedDate)
                    {
                        // Click on the element (assuming it's clickable)
                        dateElement.Click();
                        break; // Exit loop once clicked
                    }
                }

            test.Log(LogStatus.Pass, "Current Date", " Today's date selected in Timesheet.");

        }
          
        }
    
}
