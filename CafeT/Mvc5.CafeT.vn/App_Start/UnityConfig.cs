using System;
using Microsoft.Practices.Unity;
using Repository.Pattern.DataContext;
using Repository.Pattern.UnitOfWork;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.Ef6;
using Mvc5.CafeT.vn.Controllers;
using Repository.Pattern.Repositories;
using Mvc5.CafeT.vn.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mvc5.CafeT.vn.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            //container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container
               .RegisterType<IDataContext, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, ApplicationDbContext>(new PerRequestLifetimeManager())

               .RegisterType<IRepositoryAsync<ApplicationSetting>, Repository<ApplicationSetting>>()

               .RegisterType<IArticleService, ArticleService>()
               .RegisterType<ICommentService, CommentService>()

               .RegisterType<IRepositoryAsync<MenuModel>, Repository<MenuModel>>()
               .RegisterType<IRepositoryAsync<MenuItemModel>, Repository<MenuItemModel>>()
               .RegisterType<IRepositoryAsync<CompanyModel>, Repository<CompanyModel>>()
               .RegisterType<IRepositoryAsync<JobModel>, Repository<JobModel>>()
               .RegisterType<IRepositoryAsync<WorkModel>, Repository<WorkModel>>()
               .RegisterType<IRepositoryAsync<EventModel>, Repository<EventModel>>()
               .RegisterType<IRepositoryAsync<ArticleModel>, Repository<ArticleModel>>()
               .RegisterType<IRepositoryAsync<ArticleCategory>, Repository<ArticleCategory>>()
               .RegisterType<IRepositoryAsync<CommentModel>, Repository<CommentModel>>()
               .RegisterType<IRepositoryAsync<QuestionModel>, Repository<QuestionModel>>()
               .RegisterType<IRepositoryAsync<AnswerModel>, Repository<AnswerModel>>()
               .RegisterType<IRepositoryAsync<AnswerReviewModel>, Repository<AnswerReviewModel>>()
               .RegisterType<IRepositoryAsync<ProductModel>, Repository<ProductModel>>()
               .RegisterType<IRepositoryAsync<ComplainModel>, Repository<ComplainModel>>()
               .RegisterType<IRepositoryAsync<ProjectModel>, Repository<ProjectModel>>()
               .RegisterType<IRepositoryAsync<CrawlerModel>, Repository<CrawlerModel>>()
               .RegisterType<IRepositoryAsync<UrlModel>, Repository<UrlModel>>()
               .RegisterType<IRepositoryAsync<WebPageModel>, Repository<WebPageModel>>()
               

               .RegisterType<IUserStore<IdentityUser>, UserStore<IdentityUser>>()
                .RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager())
                .RegisterType<RoleManager<ApplicationRole>>(new HierarchicalLifetimeManager())
                .RegisterType<ApplicationUserManager>()
                .RegisterType<ApplicationRoleManager>()
                .RegisterType<ApplicationRoleStore>()
                .RegisterType<GroupStoreBase>()
                .RegisterType<ApplicationGroupManager>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<RolesAdminController>(new InjectionConstructor());
            container.RegisterType<UsersAdminController>(new InjectionConstructor());
        }
    }
}
