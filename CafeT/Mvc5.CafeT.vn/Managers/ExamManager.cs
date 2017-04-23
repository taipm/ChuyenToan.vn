using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class ExamManager:ObjectManager
    {

        public ExamManager(IUnitOfWorkAsync unitOfWorkAsync):base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public IEnumerable<ExamModel> GetAll()
        {
            return _unitOfWorkAsync.RepositoryAsync<ExamModel>().Query().Select();
        }

        public ExamModel GetById(Guid id)
        {
            return _unitOfWorkAsync.RepositoryAsync<ExamModel>().Find(id);
        }

        public bool Update(ExamModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<ExamModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        public bool Insert(ExamModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<ExamModel>().Insert(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                Notify(new string[] { "taipm.vn@gmail.com", "taipm.vn@outlook.com" });
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<QuestionModel> GetQuestions(Guid examId)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query().Select()
                .Where(t=>t.ExamId == examId).OrderByDescending(t=>t.CreatedDate);

            return _objects;
        }

        public void Notify(string[] users)
        {
            foreach(string user in users)
            {
                Notify(user);   
            }
        }

        public void Notify(string user)
        {

        }
    }
}