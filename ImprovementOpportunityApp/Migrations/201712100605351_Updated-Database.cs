namespace ImprovementOpportunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Topic", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Topic", new[] { "UserId" });
            AlterColumn("dbo.ForumMessage", "ReplyMessageId", c => c.Int());
            DropColumn("dbo.Topic", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topic", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ForumMessage", "ReplyMessageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Topic", "UserId");
            AddForeignKey("dbo.Topic", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
