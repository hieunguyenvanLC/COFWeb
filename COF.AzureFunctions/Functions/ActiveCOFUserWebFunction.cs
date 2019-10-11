using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace COF.AzureFunctions.Functions
{
    public static class ActiveCOFUserWebFunction
    {
        [FunctionName("ActiveCOFUserWebFunction")]
        public async static void Run([TimerTrigger("0/50 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            HttpClient client = new HttpClient();
            await client.GetAsync("http://cof.coffee/");
            await client.GetAsync("http://quantri.cof.coffee");
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
