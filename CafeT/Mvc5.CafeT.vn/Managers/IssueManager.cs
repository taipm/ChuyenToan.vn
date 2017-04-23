using CafeT.DateTimes;
using CafeT.Objects;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Mvc5.CafeT.vn.Managers
{
    public class IssueManager : ObjectManager
    {
        public Timer Clock { set; get; }

        public int? ReminderBefore { set; get; }
        public int? MaxCountReminder { set; get; }
        private int CountRemider { set; get; }

        public IssueManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            ReminderBefore = 0;
            //Clock = new Timer();
            //Clock.Enabled = true;
            //Clock.Interval = 1000; //1 hour
            //Clock.Elapsed += Clock_Elapsed;
            //MaxCountReminder = 1;
            //CountRemider = 0;
        }

        private void Clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.IsStartDay())
            {
                this.Restart();
            }
            Notify();
        }

        public void Restart()
        {
            StartDay();
            CountRemider = 0;
        }

        public void StartDay()
        {
            var _dailyObjects = GetAll().Where(t => t.IsDaily.HasValue && t.IsDaily.Value);
            foreach (var _object in _dailyObjects)
            {
                var _copy = this.Clone(_object.Id);
                if (_dailyObjects.Where(t => t.Title == _copy.Title && t.Start == _copy.Start && t.End == _copy.End) == null)
                {
                    Insert(_copy);
                }
            }
        }

        public void Notify()
        {
            if (CountRemider <= MaxCountReminder)
            {
                var _objects = GetAllInToday();
                if (_objects != null)
                {
                    var _emailService = new EmailService();
                    foreach (var _object in _objects)
                    {
                        _emailService.SendAsync(_object);
                    }
                    CountRemider = CountRemider + 1;
                }
            }
        }
        
        public IssueModel Clone(Guid id)
        {
            var _object = GetById(id);
            IssueModel _copy = (IssueModel)_object.CloneObject();
            _copy.Id = Guid.NewGuid();
            _copy.Start = _copy.Start.Value.AddDays(1);
            _copy.End = _copy.End.Value.AddDays(1);
            return _copy;
        }

        public IssueModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<IssueModel>().Find(id);
            return _object;
        }

        public bool Update(IssueModel model)
        {
            _unitOfWorkAsync.Repository<IssueModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Insert(IssueModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<IssueModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(IssueModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<IssueModel>().Delete(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<IssueModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query()
                            .Select().OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllVerifyBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query()
                            .Select()
                            .Where(t => t.VerifyBy.Contains(userName))
                            .OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllExcuteBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query()
                            .Select()
                            .Where(t=>t.ExcuteBy.Contains(userName))
                            .OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllInToday()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query().Select()
                .Where(t=>t.IsToday()).OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllInTomorrow()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query().Select()
                .Where(t => t.IsTomorrow()).OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllInYesterday()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query().Select()
                .Where(t => t.IsYesterday()).OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }

        public IEnumerable<IssueModel> GetAllOverdued()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<IssueModel>().Query()
                            .Select()
                            .Where(t=>t.IsOverdued())
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}