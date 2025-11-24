using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TaskOrganizer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendRegistrationEmail(string to, string name)
        {
            var fromEmail = _config["EmailSettings:FromEmail"] ?? "";
            var password = _config["EmailSettings:Password"] ?? "";
            var smtp = _config["EmailSettings:SmtpServer"] ?? "";
            var port = int.Parse(_config["EmailSettings:Port"] ?? "587");

            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(to))
                throw new InvalidOperationException("Email settings are missing.");

            var body = $"Hi {name},\n\nYour account has been successfully created.\nYou may now log in using your registered email.\n\nThank you!";

            var client = new SmtpClient(smtp, port)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, to)
            {
                Subject = "Registration Successful",
                Body = body
            };

            client.Send(mail);
        }
    }
}
