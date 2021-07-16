using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace webui.Services
{
    public interface IEmailService
    {
        Task Send(string from, string to, string subject, string html);
    }
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly string _host;
        private readonly int _port;

        private readonly string _smtpUserName;

        private readonly string _smptPassword;

        public EmailService(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _configuration = configuration;


            _host = configuration.GetSection("SmtpSettings").GetValue<string>("Host");
            _port = configuration.GetSection("SmtpSettings").GetValue<int>("Port");
            _smtpUserName = configuration.GetSection("SmtpSettings").GetValue<string>("SmtpUserName");
            _smptPassword = configuration.GetSection("SmtpSettings").GetValue<string>("SmtpPassword");
        }

        public async Task Send(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };


            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpUserName, _smptPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
