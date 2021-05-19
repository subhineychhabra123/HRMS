using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EMPMGMT.Utility
{
    public class MailHelper
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtmlBody { get; set; }
        public bool ReadBodyFromFile { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool SendEmail()
        {
            try
            {
                string toEmail = this.ToEmail;
                string subject = this.Subject;
                string messageBody = this.Body;
                bool isBodyHtml = this.IsHtmlBody;
                if (this.ReadBodyFromFile == true && !string.IsNullOrEmpty(this.FileName) && !string.IsNullOrEmpty(this.FilePath))
                {
                    string filePath = (this.FilePath + this.FileName);
                    messageBody = CommonFunctions.ReadFile(filePath);
                }
                EMPMGMT.Utility.EmailService.Service1Client emailService = new EMPMGMT.Utility.EmailService.Service1Client();
                emailService.SendEmail(toEmail, subject, messageBody, isBodyHtml, ReadConfiguration.EMailName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
