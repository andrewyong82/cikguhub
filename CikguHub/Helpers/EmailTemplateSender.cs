using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public interface IEmailTemplateSender
    {
        public Task SendEmailAsync(string email, string templateId, object templateData);
    }

    public class EmailTemplateSender : IEmailTemplateSender
    {
        public EmailTemplateSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string templateId, object templateData)
        {
            return Execute(Options.SendGridKey, email, templateId, templateData);
        }

        private Task Execute(string apiKey, string email, string templateId, object templateData)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SendGridUserEmail, Options.SendGridUser)
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}