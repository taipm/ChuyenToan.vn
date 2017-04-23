using CafeT.BusinessObjects;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class ProjectManager : ObjectManager
    {

        #region Articles
        public IEnumerable<ArticleModel> GetArticles(Guid id)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<ArticleModel>().Query()
                            .Select()
                            .Where(c => c.ProjectId != null && c.ProjectId.HasValue && c.ProjectId.Value == id);

            return _objects.AsEnumerable();
        }
        #endregion

        public ProjectManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public ProjectModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<ProjectModel>().Find(id);
            return _object;
        }
        public bool Update(ProjectModel model)
        {
            _unitOfWorkAsync.Repository<ProjectModel>().Update(model);
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
        public bool Delete(ProjectModel model)
        {
            _unitOfWorkAsync.Repository<ProjectModel>().Delete(model);
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
        public bool Insert(ProjectModel model)
        {
            _unitOfWorkAsync.RepositoryAsync<ProjectModel>().Insert(model);
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

        public bool AddFile(Guid id, FileModel model)
        {
            if (model.ProjectId != null && model.ProjectId.HasValue && model.ProjectId.Value == id)
            {
                _unitOfWorkAsync.RepositoryAsync<FileModel>().Insert(model);
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
            return false;
        }

        public IEnumerable<FileModel> GetFiles(Guid id)
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<FileModel>().Query()
                            .Select()
                            .Where(c => c.ProjectId != null && c.ProjectId.HasValue && c.ProjectId.Value == id);

            return _models.AsEnumerable();
        }

        public IEnumerable<ProjectModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<ProjectModel>().Query()
                            .Select().OrderByDescending(t=>t.CreatedDate);

            return _models.AsEnumerable();
        }
    }

    
}