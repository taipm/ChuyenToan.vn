using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System.Data.Entity;

namespace Mvc5.CafeT.vn.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDataContextAsync
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<ApplicationSetting> Settings { get; set; }
        public DbSet<ApplicationMessage> Messages { get; set; }

        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<MenuItemModel> MenuItems { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<JobModel> Jobs { get; set; }
        public DbSet<WorkModel> Works { get; set; }
        public DbSet<CuriculumModel> Curiculums { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<ExamModel> Exams { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<ComplainModel> Complains { get; set; }
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<AnswerReviewModel> AnswerReviews { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<IssueModel> Issues { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<CrawlerModel> Crawlers { get; set; }
        public DbSet<UrlModel> Urls { get; set; }
        public DbSet<WebPageModel> WebPages { get; set; }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");

        }

        //public System.Data.Entity.DbSet<Mvc5.CafeT.vn.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}