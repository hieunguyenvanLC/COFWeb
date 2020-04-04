using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace COF.AzureFunctions.Functions
{
    public class DailyOrderTestExportFunction
    {
        [FunctionName("DailyOrderTestExportFunction")]
        public async static void Run([TimerTrigger("0 34 10 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
                var client = new HttpClient();
                var result = await client.GetAsync("http://quantri.cof.coffee/api/export/daily-order-test?email=hoang.phannhat1996@gmail.com");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
    }
}
