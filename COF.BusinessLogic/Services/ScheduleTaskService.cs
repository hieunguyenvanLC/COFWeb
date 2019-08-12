using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using COF.DataAccess.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services
{
    public interface IScheduleTaskService
    {
        List<ScheduleTask> GetAll();
        ScheduleTask GetByName(string name);
        ScheduleTask GetById(int id);
        void Create(ScheduleTask entity);
        void Update(ScheduleTask entity);
    }
    public class ScheduleTaskService : IScheduleTaskService
    {
        #region fields

        private readonly IScheduleTaskRepository _scheduleTaskRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ctor

        public ScheduleTaskService(
            IScheduleTaskRepository scheduleTaskRepository,
            IUnitOfWork unitOfWork)
        {
            _scheduleTaskRepository = scheduleTaskRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region public methods
        public List<ScheduleTask> GetAll()
        {
            var res = _scheduleTaskRepository.GetAll();
            return res;
        }
        public ScheduleTask GetByName(string name)
        {
            var res = _scheduleTaskRepository.GetSingle(p => p.Name == name);
            return res;
        }

        public void Create(ScheduleTask entity)
        {
            try
            {
                _scheduleTaskRepository.Add(entity);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        public void Update(ScheduleTask entity)
        {
            try
            {
                _scheduleTaskRepository.Update(entity);
                _unitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        public ScheduleTask GetById(int id)
        {
            return _scheduleTaskRepository.GetById(id);
        }
        #endregion
    }
}
