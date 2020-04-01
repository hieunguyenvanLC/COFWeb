using COF.API.Core;
using COF.BusinessLogic.Services.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using COF.BusinessLogic.Services.Email;
using SendGrid.Helpers.Mail;

namespace COF.API.Api
{
    [RoutePrefix("api/export")]
    [Authorize]
    public class ExportController : ApiControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;
        public ExportController(
            IReportService reportService,
            IEmailService emailService
            )
        {
            _reportService = reportService;
            _emailService = emailService;
        }

        [Route("daily-order")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Export()
        {
            try
            {
                if (DateTime.UtcNow.AddHours(7).Hour == 11)
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
                    await _emailService.SendEmailAsync(request);
                }
                
                return SuccessResult();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("daily-order-test")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ExportTest(string email)
        {
            try
            {
                var emails = new List<string> {email};
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
                await _emailService.SendEmailAsync(request);

                return SuccessResult();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("daily-order-force")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ExportForce()
        {
            try
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
                await _emailService.SendEmailAsync(request);
                return SuccessResult();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    
}
