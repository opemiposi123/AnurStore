using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _hostenv;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly string _apiKey;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IWebHostEnvironment hostenv, IOptions<EmailConfiguration> emailConfiguration, IConfiguration configuration, ILogger<EmailService> logger)
        {
            _hostenv = hostenv;
            _emailConfiguration = emailConfiguration.Value;
            _apiKey = configuration.GetValue<string>("MailConfig:mailApikey");
            _logger = logger;
        }



        public async Task SendEmailClient(string msg, string title, string email)
        {
         
            if (string.IsNullOrEmpty(msg))
            {
                throw new ArgumentNullException(nameof(msg), "Email message content cannot be null or empty");
            }

            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(new MailboxAddress(_emailConfiguration.EmailSenderName, _emailConfiguration.EmailSenderAddress));
            message.Subject = title;

            message.Body = new TextPart("html")
            {
                Text = msg
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfiguration.SMTPServerAddress, _emailConfiguration.SMTPServerPort, true);
                    client.Authenticate(_emailConfiguration.EmailSenderAddress, _emailConfiguration.EmailSenderPassword);
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending email to: {Email}", email);
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public async Task<bool> SendEmailAsync(MailReceiverDto model, MailRequests request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.HtmlContent))
                {
                    throw new ArgumentNullException(nameof(request.HtmlContent), "Email content cannot be null or empty");
                }

                await SendEmailClient(request.HtmlContent, request.Title, model.Email);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while sending email", ex);
            }
        }
    }
}
