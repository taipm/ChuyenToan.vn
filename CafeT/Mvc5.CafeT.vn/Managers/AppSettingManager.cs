using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class AppSettingManager : ObjectManager
    {

        public AppSettingManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public ApplicationSetting GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ApplicationSetting>().Find(id);
            return _object;
        }

        public bool Update(ApplicationSetting model)
        {
            _unitOfWorkAsync.Repository<ApplicationSetting>().Update(model);
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
        public bool Delete(ApplicationSetting model)
        {
            _unitOfWorkAsync.RepositoryAsync<ApplicationSetting>().Delete(model);
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
        public bool Insert(ApplicationSetting model)
        {
            _unitOfWorkAsync.RepositoryAsync<ApplicationSetting>().Insert(model);
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

        public IEnumerable<ApplicationSetting> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ApplicationSetting>().Query()
                            .Select()
                            .OrderBy(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}