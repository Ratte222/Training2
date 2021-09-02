using BLL.Helpers;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BLL.Services
{
    public class EmailService:IEmailService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly string CompanyName = "VRealSoft";
        public EmailService(SmtpConfig smtpConfig)
        {
            _smtpConfig = smtpConfig;
        }

        public void SendEmail(string email, string subject, string message, bool isHtml = true)
        {
            var from = new MailAddress(_smtpConfig.Email, CompanyName);
            var to = new MailAddress(email);
            var msg = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };
            using (SmtpClient smtp = new SmtpClient(_smtpConfig.SmtpHost, _smtpConfig.SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(_smtpConfig.Email, _smtpConfig.Password);
                smtp.EnableSsl = true;
                smtp.Send(msg);
            }
        }

        public void FillBodyAndSendEmail(string pathToHtmlFile, string subject, string email, string header, params string[] other)
        {

            string htmlBody;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToHtmlFile))
            {
                htmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(htmlBody,
                header,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now), email, other);
            SendEmail(email, subject, messageBody);
        }

        public void SendConfirmationEmail(string email, string header, string href)
        {
            var current = Path.Combine(Directory.GetCurrentDirectory(),
                    "template", "Confirm_Account_Registration.html");
            var pathToFile = current;
            //var builder = new BodyBuilder();
            string htmlBody;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                htmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(htmlBody,
                header,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                email, href);
            SendEmail(email, "Confirm", messageBody);
        }

        public void SendForgotPasswordEmail(string email, string header, string token)
        {
            var current = Path.Combine(Directory.GetCurrentDirectory(),
                    "template", "ResetPassword.html");
            var pathToFile = current;
            //var builder = new BodyBuilder();
            string htmlBody;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                htmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(htmlBody,
                header,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                email, token);
            SendEmail(email, "Continue to recover password", messageBody);
        }


    }
}

