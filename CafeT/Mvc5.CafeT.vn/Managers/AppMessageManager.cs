using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class AppMessageManager : ObjectManager
    {

        public AppMessageManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public ApplicationMessage GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ApplicationMessage>().Find(id);
            return _object;
        }

        public bool Update(ApplicationMessage model)
        {
            _unitOfWorkAsync.Repository<ApplicationMessage>().Update(model);
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
        public bool Delete(ApplicationMessage model)
        {
            _unitOfWorkAsync.RepositoryAsync<ApplicationMessage>().Delete(model);
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

        public bool Insert(ApplicationMessage model)
        {
            _unitOfWorkAsync.RepositoryAsync<ApplicationMessage>().Insert(model);
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

        public IEnumerable<ApplicationMessage> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ApplicationMessage>().Query()
                            .Select()
                            .OrderBy(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}