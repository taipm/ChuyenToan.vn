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
    public interface ICommentService : IService<CommentModel>
    {
        #region CRUD
        IEnumerable<CommentModel> GetAll();
        CommentModel GetById(Guid id);
        void Edit(CommentModel model);
        void Delete(Guid id);
        #endregion
        #region Searching
        IEnumerable<CommentModel> SearchBy(string keyWords);
        #endregion
    }

    public class CommentService : Service<CommentModel>, ICommentService
    {
        public CommentService(IRepositoryAsync<CommentModel> repository) : base(repository)
        {
        }

        public IEnumerable<CommentModel> GetAll()
        {
            return this.Query().Select().AsEnumerable();
        }

        public CommentModel GetById(Guid id)
        {
            return this.Find(id);
        }

        public void Edit(CommentModel model)
        {
           
            var _object = GetById(model.Id);
            _object.Title = model.Title;
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

        public IEnumerable<CommentModel> SearchBy(string keyWords)
        {
            return this.Query().Select()
                .Where(t => 
                            (t.Title.ToWords().Union(keyWords.ToWords()) != null) || 
                            (t.Content.ToWords().Union(keyWords.ToWords()) != null))
                .AsEnumerable();
        }
    }
}