using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class MenuManager : ObjectManager
    {

        public MenuManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public MenuModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<MenuModel>().Find(id);
            return _object;
        }

        public bool Update(MenuModel model)
        {
            _unitOfWorkAsync.Repository<MenuModel>().Update(model);
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
        public bool Delete(MenuModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<MenuModel>().Delete(model);
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
        public bool Insert(MenuModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<MenuModel>().Insert(model);
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

        public IEnumerable<MenuModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<MenuModel>().Query()
                            .Select()
                            .OrderBy(t=>t.Order);

            return _models.AsEnumerable();
        }
    }
}