using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Interfaces;

namespace WebApplication4.Services.BusinessLogic
{
    public class EmailService : ISender
    {
        private readonly IConfiguration _configuration;
        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("У Артемки в хатке", _configuration.GetSection("DataForMail")["Login"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 25, false);
                await client.AuthenticateAsync(_configuration.GetSection("DataForMail")["Login"], _configuration.GetSection("DataForMail")["Password"]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
