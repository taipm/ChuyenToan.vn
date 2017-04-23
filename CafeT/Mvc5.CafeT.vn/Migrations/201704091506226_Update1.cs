namespace Mvc5.CafeT.vn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobModels", "AvatarPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobModels", "AvatarPath");
        }
    }
}
