using CafeT.BusinessObjects.ELearning;
using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Services
{
    public interface IArticleService : IService<ArticleModel>
    {
        #region CRUD
        IEnumerable<ArticleModel> GetAll();
        IEnumerable<string> GetAllTags();
        ArticleModel GetById(Guid id);
        void Edit(ArticleModel model);
        void Delete(Guid id);
        #endregion
        #region Searching
        IEnumerable<ArticleModel> SearchBy(string keyWords);
        #endregion
        #region Interaction with another objects

        #endregion

    }

    public class ArticleService : Service<ArticleModel>, IArticleService
    {
        public ArticleService(IRepositoryAsync<ArticleModel> repository) : base(repository)
        {
        }

        public IEnumerable<ArticleModel> GetAll()
        {
            return this.Query().Select().AsEnumerable();
        }
        public IEnumerable<string> GetAllTags()
        {
            return this.Query().Select().Where(a=>a.Tags != null)
                .Select(t=>t.Tags).Distinct().AsEnumerable();
        }

        public ArticleModel GetById(Guid id)
        {
            var _object = this.Find(id);
            return _object;
        }

        public void Edit(ArticleModel model)
        {
            var _object = GetById(model.Id);
            _object.Title = model.Title;
            _object.Summary = model.Summary;
            _object.Content = model.Content;
            _object.CreatedDate = model.CreatedDate;
            _object.LastUpdatedDate = DateTime.Now;

            this.Update(_object);
        }

        public void Delete(Guid id)
        {
            var _object = GetById(id);
            this.Delete(_object);
        }

        public IEnumerable<ArticleModel> SearchBy(string keyWords)
        {
            return this.Query().Select()
                .Where(t =>
                            (t.Title.ToWords().Union(keyWords.ToWords()) != null) ||
                            (t.Summary.ToWords().Union(keyWords.ToWords()) != null) ||
                            (t.Content.ToWords().Union(keyWords.ToWords()) != null))
                .AsEnumerable();
        }
    }
}