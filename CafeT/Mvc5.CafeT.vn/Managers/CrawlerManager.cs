using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class CrawlerManager : ObjectManager
    {

        public CrawlerManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public CrawlerModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<CrawlerModel>().Find(id);
            return _object;
        }

        public void Update(CrawlerModel model)
        {
            _unitOfWorkAsync.Repository<CrawlerModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Insert(CrawlerModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<CrawlerModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<CrawlerModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<CrawlerModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }

        public IEnumerable<CrawlerModel> GetEnables()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<CrawlerModel>().Query().Select()
                .Where(t=>t.Enable)
                .OrderByDescending(t => t.CreatedDate);
            return _models.AsEnumerable();
        }
    }

    
}