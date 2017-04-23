using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class MenuItemManager : ObjectManager
    {

        public MenuItemManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public MenuItemModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<MenuItemModel>().Find(id);
            return _object;
        }

        public bool Update(MenuItemModel model)
        {
            _unitOfWorkAsync.Repository<MenuItemModel>().Update(model);
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
        public bool Delete(MenuItemModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<MenuItemModel>().Delete(model);
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
        public bool Insert(MenuItemModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<MenuItemModel>().Insert(model);
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

        public IEnumerable<MenuItemModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<MenuItemModel>().Query()
                            .Select()
                            .OrderByDescending(t=>t.CreatedBy);

            return _models.AsEnumerable();
        }
    }
}