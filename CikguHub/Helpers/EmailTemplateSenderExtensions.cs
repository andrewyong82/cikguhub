using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CikguHub.Helpers
{
    public static class EmailTemplateSenderExtensions
    {
        public static Task SendWelcomeEmail(this IEmailTemplateSender sender, string email, string name)
        {
            return sender.SendEmailAsync(email, "d-b5d8fd8c42cd4c99947c44fa62f16c84", new { name = name });
        }

        public static Task SendSubscribedEmail(this IEmailTemplateSender sender, string email, string name)
        {
            return sender.SendEmailAsync(email, "d-b5d8fd8c42cd4c99947c44fa62f16c84", new { name = name });
        }

        public static Task SendEnrolledEmail(this IEmailTemplateSender sender, string email, string name)
        {
            return sender.SendEmailAsync(email, "d-b5d8fd8c42cd4c99947c44fa62f16c84", new { name = name });
        }

        public static Task SendCompletedEmail(this IEmailTemplateSender sender, string email, string name)
        {
            return sender.SendEmailAsync(email, "d-b5d8fd8c42cd4c99947c44fa62f16c84", new { name = name });
        }
    }
}