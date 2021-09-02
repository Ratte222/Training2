using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IEmailService
    {
        void FillBodyAndSendEmail(string pathToHtmlFile, string subject, string email, string header, params string[] other);
        void SendConfirmationEmail(string email, string header, string href);
        void SendForgotPasswordEmail(string email, string header, string token);
    }
}
