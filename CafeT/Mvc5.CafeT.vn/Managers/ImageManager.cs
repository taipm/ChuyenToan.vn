using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class ImageManager : ObjectManager
    {

        public ImageManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public ImageModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ImageModel>().Find(id);
            return _object;
        }

        public bool Update(ImageModel model)
        {
            _unitOfWorkAsync.Repository<ImageModel>().Update(model);
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

        public bool Insert(ImageModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<ImageModel>().Insert(model);
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

        public bool Delete(ImageModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<ImageModel>().Delete(model);
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

        public IEnumerable<ImageModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ImageModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }

        public IEnumerable<ImageModel> GetByArticleId(Guid articleId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ImageModel>().Query(m => m.ArticleId == articleId)
                            .Select();

            return _models.AsEnumerable();
        }
    }

    
}