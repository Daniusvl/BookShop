using BookShop.Core.Abstract.Features.EmailSender;
using BookShop.Features.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace BookShop.Features.EmailSender
{
    public class GmailEmailSender : IEmailSender
    {
        private readonly Email email;
        private readonly ILogger<GmailEmailSender> logger;

        public GmailEmailSender(IOptions<Email> email, ILogger<GmailEmailSender> logger)
        {
            this.email = email.Value;
            this.logger = logger;
        }

        public async Task SendAsync(string email_to_send, string subject, string html_message)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("BookShop", email.UserName));
            message.To.Add(new MailboxAddress("", email_to_send));

            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = html_message };

            using SmtpClient client = new();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587);
                await client.AuthenticateAsync(email.UserName, email.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Gmail error");
            }
        }
    }
}
