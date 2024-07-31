using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFramework.Base
{
    public class Base
    {
        private IWebDriver _driver { get; set; }
        public BasePage CurrentPage { get; set; }

        protected TPage GetInstance<TPage>() where TPage : BasePage, new()
        {
            TPage pageInstance = new TPage()
            {
                _driver = DriverContext.Driver


            };
            PageFactory.InitElements(DriverContext.Driver, pageInstance);

            return pageInstance;
        }
        public TPage As<TPage>() where TPage : BasePage
        {
            return (TPage)this;

        }
        public T AfterPageLoad<T>() where T : BasePage, new()
        {
            bool wait = true;
            int counter = 0;
            while (wait)
            {
                if (DriverContext.Driver.FindElements(By.TagName("body")).Count() > 0)
                    break;
                Thread.Sleep(50);
                wait = counter++ < 100;
            }
            return GetInstance<T>();
        }

        //================================================================================================================================
        //-----------------------------------------------------Extend Report Set up--------------------------------------------------------------
        //================================================================================================================================

        public static ExtentReports extent;
        public static ExtentTest test;
        public static string HTMLReportPath;
        public static string EnvironmentName = ConfigurationManager.AppSettings["Environment"];
        public static string ReportsPath = ConfigurationManager.AppSettings["ReportsPath"];

        public static String datetime()
        {
            var time = DateTime.Now;
            string formattedTime = time.ToString("MM, dd, yyyy, hh, mm, ss");
            formattedTime = formattedTime.Replace(",", "_");
            Console.WriteLine(formattedTime);
            return formattedTime;
        }

        public static string GetParentDirectory()
        {
            System.IO.DirectoryInfo myDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            myDirectory = myDirectory.Parent;
            String parentDirectory = myDirectory.Parent.FullName;
            return parentDirectory;
        }

        public static void ExtentManager()
        {
            String currentdatetime = datetime();

            HTMLReportPath = ReportsPath + "\\AutomationTestReport" + "_" + currentdatetime + ".html";
            extent = new ExtentReports(HTMLReportPath, true, DisplayOrder.OldestFirst);

            extent.AddSystemInfo("Host Name", "Test Host").AddSystemInfo("Environment", EnvironmentName);
        }



        //================================================================================================================================
        //-----------------------------------------------------E-mail Set up--------------------------------------------------------------
        //================================================================================================================================

        public static string FromEmail = "";
        public static string To = "";
        public static string Cc = "";
        public static string Subj = "";

        public static int PassCount = 0;
        public static int FailCount = 0;
        public static int TotalCount = 0;

        private static string PopulateBody(string projectName, string passCount, string failCount, string totalCount)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + "\\Input\\report.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("${ReportHeader}", projectName);
            body = body.Replace("${PassedTestCases}", passCount);
            body = body.Replace("${FailedTestCases}", failCount);
            body = body.Replace("${TotalTestCases}", totalCount);
            return body;
        }

        public static string emailhost = ConfigurationManager.AppSettings["EmailHost"];
        public static string emailssl = ConfigurationManager.AppSettings["EmailSsl"];
        public static string emailpwd = ConfigurationManager.AppSettings["EmailPwd"];
        public static string emailport = ConfigurationManager.AppSettings["EmailPort"];
        public static string username_email = ConfigurationManager.AppSettings["Username_Email"];
        public static string envionment_ProjectName = ConfigurationManager.AppSettings["Environment"];
        public static string emailfilepath = ConfigurationManager.AppSettings["EmailFilePath"];

        public static bool email_send(string filepath, string projectName)
        {

            try
            {
                List<String> to = new List<string>();
                List<String> cc = new List<string>();
                List<String> bcc = new List<string>();
                List<String> Attachment = new List<string>();

                var lines = File.ReadLines(emailfilepath);
                string line = lines.ElementAtOrDefault(1);
                string[] values = line.Split(';');

                FromEmail = values[0].ToString();
                To = values[1].ToString();
                Cc = values[2].ToString();
                Subj = envionment_ProjectName + " Regression Cycle";

                string Message = string.Empty;

                System.Text.StringBuilder body = new System.Text.StringBuilder();
                body.Clear();
                body.Length = 0;
                string bodyText = PopulateBody(projectName, PassCount.ToString(), FailCount.ToString(), (PassCount + FailCount).ToString());

                body.Append(bodyText);
                Message = body.ToString();

                if (!string.IsNullOrEmpty(To))
                {
                    string[] ToMuliId = To.Split(',');
                    foreach (string ToEMailId in ToMuliId)
                    {
                        if (ToEMailId != "")
                            to.Add(ToEMailId);
                    }
                }

                if (!string.IsNullOrEmpty(Cc))
                {
                    string[] CCId = Cc.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        if (CCEmail != "")
                            cc.Add(CCEmail);
                    }
                }

                if (!string.IsNullOrEmpty(filepath))
                {
                    string[] mailAttachments = filepath.Split(',');
                    foreach (string mailAttach in mailAttachments)
                    {
                        if (mailAttach != "")
                            Attachment.Add(mailAttach);
                    }
                }

                bool success;
                string ErrMsg = "";
                try
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage = SetMailParameters(to, FromEmail, Subj, Message, mailMessage);
                    mailMessage = SetCopyParameters(cc, bcc, mailMessage);
                    mailMessage = SetAttachmentPath(Attachment, mailMessage);
                    mailMessage.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient();

                    string UserName = username_email;
                    string Host = emailhost;
                    string Ssl = emailssl;
                    string pwd = emailpwd;
                    string Port = emailport;

                    client = AuthenticateCredentials(Host, Convert.ToInt16(Port), Convert.ToBoolean(Ssl), UserName, pwd, client); ;

                    client.Send(mailMessage);
                    success = true;
                    Console.WriteLine("Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    success = false;
                    ErrMsg = ex.Message;
                }
                return success;
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.Message);
                return false;
            }
        }

        #region Email Supporting Methods

        public static SmtpClient AuthenticateCredentials(string server, int port, bool requireSsl, string userName, string password, SmtpClient smtpClient)
        {
            SmtpClient client;
            try
            {
                smtpClient.Host = server;
                smtpClient.Port = port;
                smtpClient.EnableSsl = requireSsl;
                if ((userName.Length > 0) && (password.Length > 0))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(userName, password);
                }
                else
                {
                    //   Console.WriteLine("failed");
                }
                client = smtpClient;
            }
            catch (Exception es)
            {
                Console.WriteLine("exception3" + es.Message);
                throw;
            }
            return client;
        }

        public static MailMessage SetAttachmentPath(List<string> attachmentPath, MailMessage mailMessage)
        {
            MailMessage message;
            try
            {
                Console.WriteLine("In setattachment path.");
                if ((attachmentPath != null) && (attachmentPath.Count > 0))
                {
                    foreach (string str in attachmentPath)
                    {
                        mailMessage.Attachments.Add(new Attachment(str));
                    }
                }
                message = mailMessage;
            }
            catch (Exception e1)
            {

                Console.WriteLine("Exception4" + e1.Message);
                throw;
            }
            return message;
        }

        public static MailMessage SetCopyParameters(List<string> cc, List<string> bcc, MailMessage mailMessage)
        {
            MailMessage message;
            try
            {
                // Console.WriteLine("In SetCopyParameters.");
                if ((cc != null) && (cc.Count > 0))
                {
                    foreach (string str in cc)
                    {
                        mailMessage.CC.Add(new MailAddress(str));
                    }
                }
                if ((bcc != null) && (bcc.Count > 0))
                {
                    foreach (string str in bcc)
                    {
                        mailMessage.Bcc.Add(new MailAddress(str));
                    }
                }
                message = mailMessage;
            }
            catch (Exception e2)
            {
                Console.WriteLine("Exception5" + e2.Message);
                throw;
            }
            return message;
        }

        private static MailMessage SetMailParameters(List<string> to, string from, string subject, string messageBody, MailMessage mailMessage)
        {
            MailMessage message;
            try
            {
                Console.WriteLine("SetMailParameters");
                if ((to == null) || (to.Count <= 0))
                {
                    throw new Exception("The 'To-Address' was not specified");
                }
                foreach (string str in to)
                {
                    mailMessage.To.Add(new MailAddress(str));
                }
                if (string.IsNullOrEmpty(from))
                {
                    throw new Exception("The 'From-Address' was not specified");
                }
                MailAddress address = new MailAddress(from);
                mailMessage.From = address;
                if (!string.IsNullOrEmpty(subject))
                {
                    mailMessage.Subject = subject;
                }
                else
                {
                    mailMessage.Subject = string.Empty;
                }
                if (!string.IsNullOrEmpty(messageBody))
                {
                    mailMessage.Body = messageBody;
                }
                else
                {
                    mailMessage.Body = string.Empty;
                }
                message = mailMessage;
            }
            catch (Exception e3)
            {
                Console.WriteLine("Exception6" + e3.Message);
                throw;
            }
            return message;
        }


        #endregion
    }
}
