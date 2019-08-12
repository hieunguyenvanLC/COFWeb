using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Hangfire
{
    public interface IHangfireService
    {
        void Start();
        void Stop();
    }

    public class HangfireService : IHangfireService
    {
        
        public HangfireService()
        {
          
        }

        public void Start()
        {
          
        }

        public void Stop()
        {
            RemoveJob();
        }

        private void RemoveJob()
        {
            
        }


    }
}
