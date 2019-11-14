using System;
using System.Net.Http;
using COF.AzureFunctions.Ioc;
using COF.BusinessLogic.Services.Reports;
using CommonServiceLocator;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace COF.AzureFunctions.Functions
{
    public static class DailyOrderExportFunction
    {
        public static IServiceLocator ServiceLocator { get; set; } = new ServiceLocatorBuilder()
            .RegisterModule<FuncModule>()
            .Build();

        [FunctionName("DailyOrderExportFunction")]
        public async static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
                var client = new HttpClient();
                var result = await client.GetAsync("http://cof-dev.azurewebsites.net/api/export/daily-order");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }  
        }
    }
}
