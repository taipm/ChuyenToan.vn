using CafeT.DateTimes;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CafeT.Text;
using CafeT.MathText;

namespace Mvc5.CafeT.vn.Managers
{
    public class ArticleCategoryManager : ObjectManager
    {
        public Timer Clock { set; get; }

        public int? ReminderBefore { set; get; }
        public int? MaxCountReminder { set; get; }
        private int CountRemider { set; get; }

        public ArticleCategoryManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            //ReminderBefore = 0;
            //Clock = new Timer();
            //Clock.Enabled = true;
            //Clock.Interval = 1000; //1 hour
            //Clock.Elapsed += Clock_Elapsed;
            //MaxCountReminder = 1;
            //CountRemider = 0;
        }

        private void Clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(DateTime.Now.IsStartDay())
            {
                this.Restart();
            }
            Notify();
        }

        public void Restart()
        {
            CountRemider = 0;
        }

        public void Notify()
        {
            //if(CountRemider <= MaxCountReminder)
            //{
            //    var _objects = GetAllNoAnswers();
            //    if (_objects != null)
            //    {
            //        var _emailService = new EmailService();
            //        foreach (var _object in _objects)
            //        {
            //            _emailService.SendAsync(_object);
            //        }
            //        CountRemider = CountRemider + 1;
            //    }
            //}
        }
        
        public ArticleCategory GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ArticleCategory>().Find(id);
            return _object;
        }

        public bool Update(ArticleCategory model)
        {
            _unitOfWorkAsync.Repository<ArticleCategory>().Update(model);
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

        //public bool PreInsert(QuestionModel model)
        //{
        //    if(!model.Content.IsNullOrEmptyOrWhiteSpace())
        //    {
        //        string[] _mathTexts = model.Content.ExtractAllMathText();
        //        foreach(string _mathText in _mathTexts)
        //        {
        //            WordObject _word = new WordObject();
        //            _word = model.Content.GetWordObject()
        //        }
        //        return true;
        //    }
        //}

        public bool Insert(ArticleCategory model)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Insert(model);
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
        public bool Delete(ArticleCategory model)
        {
            _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Delete(model);
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
        public IEnumerable<ArticleCategory> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
                            .Select();

            return _models.AsEnumerable();
        }
        public IEnumerable<QuestionModel> GetAllByLevel(int level)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<QuestionModel>().Query()
                            .Select()
                            .Where(t=>t.Level == level)
                            .OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
        //public IEnumerable<ArticleCategory> GetAllVerified()
        //{
        //    var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
        //                    .Select()
        //                    .Where(t => t.VerifiedBy != null || (t.VerifiedBy.IsNullOrEmptyOrWhiteSpace()));

        //    return _models.AsEnumerable();
        //}

        public IEnumerable<ArticleCategory> GetAllCreateBy(string userName)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
                            .Select()
                            .Where(t=>t.CreatedBy == userName)
                            .OrderByDescending(t => t.CreatedDate); 

            return _models.AsEnumerable();
        }

        //public IEnumerable<ArticleCategory> GetAllVerifiedBy(string userName)
        //{
        //    var _models = _unitOfWorkAsync.RepositoryAsync<ArticleCategory>().Query()
        //                    .Select()
        //                    .Where(t => t.VerifiedBy == userName)
        //                    .OrderByDescending(t=>t.CreatedDate);

        //    return _models.AsEnumerable();
        //}

        //public IEnumerable<ArticleCategory> GetAllNoAnswers()
        //{
        //    var _all = GetAll();
        //    List<ArticleCategory> _models = new List<ArticleCategory>();
        //    foreach (var _item in _all)
        //    {
        //        _item.Answers = GetAnswers(_item.Id);
        //        if(_item.Answers == null || _item.Answers.Count() == 0)
        //        {
        //            _models.Add(_item);
        //        }
                
        //    }
        //    return _models.Where(t => t.Answers != null).OrderByDescending(t => t.CreatedDate); ;
        //}

        //public IEnumerable<ArticleCategory> GetTopAnswers()
        //{
        //    var _all = GetAll();
        //    List<ArticleCategory> _models = new List<ArticleCategory>();
        //    foreach(var _item in _all)
        //    {
        //        _item.Answers = GetAnswers(_item.Id);
        //        _models.Add(_item);
        //    }
        //    return _models.Where(t => t.Answers != null).OrderByDescending(t => t.Answers.Count()); ;
        //}

        public IEnumerable<ArticleModel> GetArticles(Guid categoryId)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ArticleModel>().Query().Select()
                .Where(t=>t.CategoryId != null && t.CategoryId.HasValue && t.CategoryId.Value == categoryId)
                .OrderByDescending(t => t.CreatedDate);

            return _models.AsEnumerable();
        }
    }
}