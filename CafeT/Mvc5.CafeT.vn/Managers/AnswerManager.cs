using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class AnswerManager : ObjectManager
    {
        public AnswerManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public AnswerModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<AnswerModel>().Find(id);
            return _object;
        }

        public bool Update(AnswerModel model)
        {
            _unitOfWorkAsync.Repository<AnswerModel>().Update(model);
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

        public bool Insert(AnswerModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<AnswerModel>().Insert(model);
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

        public bool Delete(AnswerModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<AnswerModel>().Delete(model);
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

        public void AddReview(Guid id, AnswerReviewModel model)
        {
            model.AnswerId = id;
            _unitOfWorkAsync.RepositoryAsync<AnswerReviewModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteAllReviews(Guid id, AnswerReviewModel model)
        {
            var _reviews = _unitOfWorkAsync.RepositoryAsync<AnswerReviewModel>().Query().Select()
                .Where(t => t.AnswerId != null && t.AnswerId.HasValue && t.AnswerId.Value == id);
            if(_reviews != null && _reviews.Count()>0)
            {
                foreach(AnswerReviewModel _review in _reviews)
                {
                    _unitOfWorkAsync.Repository<AnswerReviewModel>().Delete(_review);
                }
                
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteAll()
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<AnswerModel>().Query().Select();
            if (_objects != null && _objects.Count() > 0)
            {
                foreach (AnswerModel _review in _objects)
                {
                    _unitOfWorkAsync.Repository<AnswerModel>().Delete(_review);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<AnswerModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<AnswerModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }

        public IEnumerable<AnswerReviewModel> GetAnswerReviews(Guid answerId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<AnswerReviewModel>().Query().Select()
                .Where(t=>t.AnswerId != null && t.AnswerId.HasValue && t.AnswerId.Value == answerId)
                .OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}