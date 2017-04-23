using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class UrlManager : ObjectManager
    {
        public IEnumerable<UrlModel> Urls { set; get; }
        public IEnumerable<string> Errors { set; get; }

        public UrlManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public UrlModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<UrlModel>().Find(id);
            return _object;
        }

        public void AddAll()
        {
            if(Urls != null && Urls.Count()>0)
            {
                foreach(UrlModel url in Urls)
                {
                    try
                    {
                        Insert(url);
                    }
                    catch(Exception ex)
                    {
                        Errors.ToList().Add(ex.Message);
                    }
                }
            }
        }

        public void AddUrls(List<UrlModel> urls)
        {
            if (urls != null && urls.Count() > 0)
            {
                foreach (UrlModel url in urls)
                {
                    try
                    {
                        Insert(url);
                    }
                    catch (Exception ex)
                    {
                        Errors.ToList().Add(ex.Message);
                    }
                }
            }
        }

        public void Update(UrlModel model)
        {
            _unitOfWorkAsync.Repository<UrlModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Insert(UrlModel model)
        {
            var _urls = this.GetAll().Select(t => t.Url).ToList();
            if(!_urls.Contains(model.Url))
            {
                _unitOfWorkAsync.RepositoryAsync<UrlModel>().Insert(model);
            }
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

        public IEnumerable<UrlModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<UrlModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }
    }
}