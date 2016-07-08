using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports;
//using CrystalDecisions.ReportSource;
//using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Net;
namespace ACG.Common
{
    public class Mailer
    {
        private const string errorMessage = "Error in sending email. Probable cause: Bad email format. System Message = '{0}'";
        public static string toEmail { get; set; }
        public static string fromEmail { get; set; }
        public static string subject { get; set; }
        public static string message { get; set; }
        public static string attachment { get; set; }
        private static string smtpServer;
        private static int smtpPort;
        private static string smtpuser;
        private static string smtppassword;
        //public MailController(string toEmail, string subject, string message, string attachmentFile)
        //{
        //    this.toEmail = toEmail;
        //    this.attachment = attachmentFile;
        //    this.subject = subject;
        //    this.message = message;
        //}


        private static void LoadConfigInfo()
        {
            smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];
            smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpport"]);
            fromEmail = System.Configuration.ConfigurationManager.AppSettings["smtpfrom"];
            smtpuser = System.Configuration.ConfigurationManager.AppSettings["smtpuser"];
            smtppassword = System.Configuration.ConfigurationManager.AppSettings["smtppassword"];
        }

        private static SmtpClient getSmtpObject()
        {
            LoadConfigInfo();
            SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
            NetworkCredential basicCredential = new NetworkCredential(smtpuser, smtppassword);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basicCredential;
            return smtp;
        }
        public static string SendMail(string toEmail, string subject, string content, string attachment)
        {
            return SendMail(toEmail, subject, content, attachment, false);
        }
        public static string SendMail(string toEmail, string subject, string content, string attachment, bool isHTML)
        {
            try
            {
                SmtpClient smtp = getSmtpObject();
                MailAddress from = new MailAddress(fromEmail);
                MailAddress to = new MailAddress(toEmail);
                MailMessage mess = new MailMessage(from, to);
                mess.IsBodyHtml = isHTML;
                mess.Subject = subject;
                mess.Body = content;
                if (!string.IsNullOrEmpty(attachment))
                {
                    Attachment at = new Attachment(attachment);
                    mess.Attachments.Add(at);
                }
                smtp.Send(mess);
            }
            catch (Exception ex)
            {
                return string.Format(errorMessage, CommonFunctions.getInnerException(ex).Message);
            }
            return "OK";
        }

        public static string SendMail(string toEmail, string subject, string content, Stream attachment, string fileName, string mime)
        {
            return SendMail(toEmail, subject, content, attachment, fileName, mime, false);
        }
        public static string SendMail(string toEmail, string subject, string content, Stream attachment, string fileName, string mime, bool isHTML)
        {
            try
            {
                SmtpClient smtp = getSmtpObject();
                MailAddress from = new MailAddress(fromEmail);
                MailAddress to = new MailAddress(toEmail);
                MailMessage mess = new MailMessage(from, to);
                mess.IsBodyHtml = isHTML;
                mess.Subject = subject;
                mess.Body = content;
                Attachment at = new Attachment(attachment, fileName, mime);
                mess.Attachments.Add(at);
                smtp.Send(mess);
            }
            catch (Exception ex)
            {
                return string.Format(errorMessage, CommonFunctions.getInnerException(ex).Message);
            }
            return "OK";
        }

        /// <summary>
        /// Load an email template
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <returns></returns>
        public static string getEmailTemplate(string TemplateName)
        {
            string path = string.Concat(CommonFunctions.getCurrentPath(), "\\..\\EmailTemplates\\", TemplateName);
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                StringBuilder sb = new StringBuilder();
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }

                return sb.ToString();
            }

        }
    }
}
  
