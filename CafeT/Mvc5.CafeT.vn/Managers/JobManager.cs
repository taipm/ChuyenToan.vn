using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class JobManager : ObjectManager
    {

        public JobManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public JobModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<JobModel>().Find(id);
            return _object;
        }

        public CompanyModel GetCompany(Guid companyId)
        {
            return _unitOfWorkAsync.Repository<CompanyModel>().Find(companyId);
        }

        public bool Update(JobModel model)
        {
            _unitOfWorkAsync.Repository<JobModel>().Update(model);
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
        public bool Delete(JobModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<JobModel>().Delete(model);
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
        public bool Insert(JobModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<JobModel>().Insert(model);
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

        public IEnumerable<JobModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<JobModel>().Query()
                            .Select()
                            .OrderByDescending(t=>t.CreatedBy);

            return _models.AsEnumerable();
        }

        public IEnumerable<JobModel> GetHotJobs(int? n)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<JobModel>().Query().Select()
                .OrderByDescending(t => t.CountViews);

            if (n != null && n <= _models.Count())
            {
                _models.Take(n.Value);
            }

            return _models.AsEnumerable();
        }
    }

    
}