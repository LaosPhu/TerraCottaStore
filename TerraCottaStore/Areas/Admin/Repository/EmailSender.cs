﻿using System.Net.Mail;
using System.Net;

namespace TerraCottaStore.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("leprosperities@gmail.com", "upjtnlxtthwfaxig")
            };

            return client.SendMailAsync(
                new MailMessage(from: "leprosperities@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
