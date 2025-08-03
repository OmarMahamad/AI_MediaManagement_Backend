using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using HelperLayer.Notifecation.Email.Interface;

namespace HelperLayer.Notifecation.Email.Service
{
    public class EmailService : IEmail
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;

        public EmailService(IConfiguration configuration)
        {
            smtpServer = configuration["EmailSettings:SmtpServer"];
            smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            smtpUser = configuration["EmailSettings:SmtpUser"];
            smtpPass = configuration["EmailSettings:SmtpPassword"];
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Fahd app test", smtpUser));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Sand Email: {ex.Message}");
                return false;
            }
        }
    }
}
