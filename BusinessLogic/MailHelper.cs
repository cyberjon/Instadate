using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication9.ViewModels;
using System.Net.Mail;

namespace WebApplication9.BusinessLogic
{
    public class MailHelper
    {
        public const string SUCCESS
        = "Success! Your email has been sent.  Please allow up to 48 hrs for a reply.";

        const string BY = "noreply@noreply.zaichaopan.com";

        public string EmailFromArvixe(Message message)
        {
            const string FROM = "noreply@zaichaopan.com";
            const string FROM_PWD = "123noreply";
            const bool USE_HTML = true;

            const string SMTP_SERVER = "143.95.249.35";
            try
            {
                MailMessage mailMsg = new MailMessage(FROM, message.Sender);
                mailMsg.Subject = message.Subject;
                mailMsg.Body = message.Body + "<br/>sent by: " + BY;
                mailMsg.IsBodyHtml = USE_HTML;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 25;
                smtp.Host = SMTP_SERVER;
                smtp.Credentials = new System.Net.NetworkCredential(FROM, FROM_PWD);
                smtp.Send(mailMsg);
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            return SUCCESS;
        }

    }
}