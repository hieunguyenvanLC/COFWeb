using COF.DataAccess.EF.Models;
using Hangfire;
namespace COF.BusinessLogic.Services.Hangfire
{
    public interface IHangfireService
    {
        void Start();
        void Stop();
    }

    public class HangfireService : IHangfireService
    {
        #region fields
        private readonly IScheduleTaskService _scheduleTaskService;
        //private readonly bool isReportServer = bool.Parse(ConfigurationManager.AppSettings["IsReportServer"]);
        #endregion

        #region ctor
        public HangfireService(IScheduleTaskService scheduleTaskService)
        {
            _scheduleTaskService = scheduleTaskService;
        }
        #endregion


        public void Start()
        {
            var isReportServer = true;
            if (isReportServer)
            {
                RecurringJob.RemoveIfExists(ScheduleTaskName.DailyOrderReport);
                RecurringJob.AddOrUpdate<ITaskService>(ScheduleTaskName.DailyOrderReport, p => p.ImportCustomerData(), Cron.Minutely);
                //BackgroundJob.Enqueue<ITaskService>(x => x.ImportCustomerData());

                var tasks = _scheduleTaskService.GetAll();
                //foreach (var task in tasks)
                //{
                //    if (task.IsEnabled)
                //    {
                //        task.IsRunning = false;
                //        task.LastStartedOnUtc = null;
                //        _scheduleTaskService.Update(task);
                //        switch (task.Name)
                //        {
                //            case "A2000 Import Task":
                //                BackgroundJob.Enqueue<IProductCategoryService>(x => x.Run());
                //                break;
                //            case "Report Task":
                //                BackgroundJob.Enqueue<IReportTaskConverter>(x => x.Run());
                //                break;
                //            case "A2000 Po Import Task":
                //                BackgroundJob.Enqueue<IImportPurchasingTask>(x => x.Run());
                //                break;
                //        }
                //    }
                //}
            }
            else
            {
                RecurringJob.RemoveIfExists(ScheduleTaskName.DailyOrderReport);
              
            }
        }

        public void Stop()
        {
            RecurringJob.RemoveIfExists(ScheduleTaskName.DailyOrderReport);
            RemoveJob();
        }

        private void RemoveJob()
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            var record = monitor.ProcessingCount();
            if (record > 0)
            {
                foreach (var job in monitor.ProcessingJobs(0, (int)record))
                {
                    BackgroundJob.Delete(job.Key);
                }
            }
        }



    }
}
