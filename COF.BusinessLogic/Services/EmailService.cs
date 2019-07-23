using COF.BusinessLogic.Settings;
using CapstoneProjectServer.DataAccess.EF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Email;
using GemBox.Email.Smtp;
using CapstoneProjectServer.Models.dto;
using CapstoneProjectServer.DataAccess.EF.Models;

namespace COF.BusinessLogic.Services
{
    public interface IEmailService
    {
        BusinessLogicResult<bool> SendEmail(Customer customer, string emailContent);
    }
    public class EmailService : IEmailService
    {
        public EmailService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepositoryHelper RepositoryHelper;

        public BusinessLogicResult<bool> SendEmail(Customer customer, string emailContent)
        {
            try
            {
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                MailMessage message = new MailMessage(new MailAddress("acsappvietnam@gmail.com", "ACSApp"),
                                             new MailAddress(customer.Email, "First receiver"));

                // Add second receiver to CC and set subject
                // message.Cc.Add(new MailAddress("second.receiver@example.com", "Second receiver"));
                message.Subject = "Xác thực tài khoản ACS App";

                message.BodyHtml = "<html>" +
                                      "<body>" +
                                         $"<p>Xin chào {customer.Name}!<br/><br/>" +
                                            "<br/>" +
                                            $"<a href='http://acsa-capstone.azurewebsites.net/System/User/Confirm?customerId={customer.CustomerId}'>Hãy nhấn vào đường link dưới đây để xác thực tài khoản của bạn </a>" +
                                         "</p>" +
                                      "</body>" +
                                   "</html>";
                // Initialize new SMTP client and send an email message
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Connect();
                    Console.WriteLine("Connected.");

                    smtp.Authenticate("acsappvietnam@gmail.com", "12345678x@X");

                    smtp.SendMessage(message);
                }
                return new BusinessLogicResult<bool>()
                {
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResult<bool>
                {
                    Success = false,
                    Validations = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("SendEmail", ex.Message) })
                };
            }

        }
    }
}
