namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "AvatarPath", c => c.String());
            AddColumn("dbo.FileModels", "AvatarPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileModels", "AvatarPath");
            DropColumn("dbo.Articles", "AvatarPath");
        }
    }
}
