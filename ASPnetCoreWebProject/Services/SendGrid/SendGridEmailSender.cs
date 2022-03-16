using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace PizzaWebsite.Services.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(IOptions<SendGridEmailSenderOptions> options)
        {
            Options = options.Value;
        }

        private SendGridEmailSenderOptions Options { get; set; }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            await Execute(Options.ApiKey, subject, message, email);
        }
        public async Task<Response> Execute(
            string apiKey,
            string subject,
            string message,
            string email)
        {
            
            var client = new SendGridClient(apiKey);
            var senderEmail = new EmailAddress(Options.SenderEmail, Options.SenderName);
            var msg = new SendGridMessage()
            {
                From = senderEmail,
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            var recipientEmail = new EmailAddress(email);

            // send the message to the user
            msg.AddTo(recipientEmail);
            msg.ReplyTo = recipientEmail;

            // send the message to the sender
            msg.AddTo(senderEmail);
            // send the message to the company email
            msg.AddTo(Options.CompanyEmail);

            msg.SetClickTracking(true, true);
            msg.SetOpenTracking(true);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(true);

            return await client.SendEmailAsync(msg);
        }
    }
}
