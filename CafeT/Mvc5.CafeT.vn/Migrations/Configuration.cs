namespace Mvc5.CafeT.vn.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;


    internal sealed class Configuration : DbMigrationsConfiguration<Mvc5.CafeT.vn.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Mvc5.CafeT.vn.Models.ApplicationDbContext";
        }

        
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var roleStore = new RoleStore<ApplicationRole, string, ApplicationUserRole>(db);
            var roleManager = new RoleManager<ApplicationRole, string>(roleStore);
            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(db);
            var userManager = new UserManager<ApplicationUser, string>(userStore);

            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, EmailConfirmed = true, FirstName = "A", LastName="B" };
                var result = userManager.Create(user, password);
                
                result = userManager.SetLockoutEnabled(user.Id, false);
                userManager.AddToRole(user.Id, roleName);
            }
            
            //db.ArticleCategories.AddOrUpdate(new ArticleCategory() { Id = Guid.NewGuid(), Name = "Tất cả" });            
        }        
    }
}
