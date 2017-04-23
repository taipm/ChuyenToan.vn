using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    
    public class FileManager : ObjectManager
    {
        public Timer Clock { set; get; }

        public int? ReminderBefore { set; get; }
        public int? MaxCountReminder { set; get; }
        private int CountRemider { set; get; }

        private List<FileModel> ActiveFiles { set; get; }


        private void Clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (IsRemider())
            //{
            //    Notify();
            //}
        }

        public void Start()
        {
            ActiveFiles = new List<FileModel>();
        }
        
        public FileManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            ReminderBefore = 0;
            Clock = new Timer();
            Clock.Enabled = true;
            Clock.Interval = 300000;
            Clock.Elapsed += Clock_Elapsed;
            MaxCountReminder = 2;
            CountRemider = 0;
            Start();
        }

        public IEnumerable<FileModel> GetAll()
        {
            return _unitOfWorkAsync.RepositoryAsync<FileModel>().Query().Select().AsEnumerable();
        }

        //public int CountDownloads(Guid id)
        //{
        //    var _file = GetById(id);
        //    if (_file != null) return _file.CountDownloads.Value;
        //    return 0;
        //}
        public void DailyNotify()
        {
            foreach(var file in ActiveFiles)
            {
                new EmailService().SendAsync(file);
            }
        }


        public FileModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            return _object;
        }

        public IEnumerable<FileModel> GetTopDownloads(int? n)
        {
            var _objects = _unitOfWorkAsync.Repository<FileModel>().Query().Select()
                .OrderByDescending(t=>t.CountDownloads);
            if(n.HasValue && _objects.Count()>=n)
            {
                return _objects.AsEnumerable().Take(n.Value);
            }
            return _objects.AsEnumerable();
        }

        public IEnumerable<FileModel> GetTopViews(int? n)
        {
            var _objects = _unitOfWorkAsync.Repository<FileModel>().Query().Select()
                .OrderByDescending(t => t.CountViews);
            if (n.HasValue && _objects.Count() >= n)
            {
                return _objects.AsEnumerable().Take(n.Value);
            }
            return _objects.AsEnumerable();
        }

        public IEnumerable<FileModel> GetNews(int? n)
        {
            var _objects = _unitOfWorkAsync.Repository<FileModel>().Query().Select()
                .OrderByDescending(t => t.CreatedDate);
            if (n.HasValue && _objects.Count() >= n)
            {
                return _objects.AsEnumerable().Take(n.Value);
            }
            return _objects.AsEnumerable();
        }

        public bool Update(FileModel model)
        {
            _unitOfWorkAsync.Repository<FileModel>().Update(model);
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

        public bool Insert(FileModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<FileModel>().Insert(model);
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

        public IEnumerable<FileModel> GetFiles(int? n)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<FileModel>().Query()
                            .Select();

            if (n.HasValue && _models.Count() >= n)
            {
                return _models.AsEnumerable().Take(n.Value);
            }
            return _models.AsEnumerable();
        }

        public bool Delete(Guid id)
        {
            var _object = GetById(id);
            if(_object != null)
            {
                try
                {
                    _unitOfWorkAsync.Repository<FileModel>().Delete(_object);
                    _unitOfWorkAsync.SaveChanges();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }
    }
}