using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace COF.BusinessLogic.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailRequest request);
    }
    public class EmailService : IEmailService
    {
        private readonly string _sendgridKey;
        private readonly string _sendgridEmail;
        public EmailService()
        {
            _sendgridKey = ConfigurationManager.AppSettings["SendGridKey"];
            _sendgridEmail = ConfigurationManager.AppSettings["SendGridEmail"];
        }


        public async Task SendEmailAsync(SendEmailRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var sg = new SendGridClient(_sendgridKey);
            EmailAddress from = new EmailAddress(_sendgridEmail);
            string subject = request.Subject;
            EmailAddress to = new EmailAddress(request.Recipients.Select(p => p.Email).FirstOrDefault());
            var mail = MailHelper.CreateSingleEmail(from, to, subject, request.Body, request.Body);
            if (request.Attachments != null && request.Attachments.Any())
            {
                mail.Attachments = request.Attachments;
            }
            Response response = await sg.SendEmailAsync(mail);
            var content = await response.Body.ReadAsStringAsync();
        }
    }

    public class SendEmailRequest
    {
        public List<EmailRecipient> Recipients { get; set; }
        public bool SeparateEmail { get; set; }
        public List<Attachment> Attachments { get; set; }

        public string Body { get; set; }
        public string Subject { get; set; }


    }

    public class EmailRecipient
    {
        public string Email { get; set; }
        public Dictionary<string, string> EmailTemplateToken { get; set; }
    }
}
