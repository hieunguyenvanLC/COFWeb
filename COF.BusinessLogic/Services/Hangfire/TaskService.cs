using COF.BusinessLogic.Services.Email;
using COF.BusinessLogic.Services.Reports;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Hangfire
{
    public interface ITaskService
    {
        Task RunExportDailyRevenue();
    }
    public class TaskService : ITaskService
    {
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;

        public TaskService(
            IReportService reportService,
            IEmailService emailService
        )
        {
            _reportService = reportService;
            _emailService = emailService;
        }

        public async Task RunExportDailyRevenue()
        {
            var emails = ConfigurationManager.AppSettings["DailyOrderReportEmails"].Split(',').ToList();
            var fileName = $"Danh_sach_hoa_don_{DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy")}.xlsx";
            var fileContent = _reportService.ExportDailyOrderReport(fileName);
            var request = new SendEmailRequest
            {
                Subject = $"Doanh thu {DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy")}",

                Recipients = emails.Select(x => new EmailRecipient()
                {
                    Email = x
                }).ToList(),
                Body = $"Doanh thu {DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy")}"

            };
            request.Attachments = new List<Attachment>
            {
                new Attachment()
                {
                    Content = System.Convert.ToBase64String(fileContent),
                    Filename = fileName
                }
            };
            await _emailService.SendEmailAsync(request); ;
        }
    }
}
