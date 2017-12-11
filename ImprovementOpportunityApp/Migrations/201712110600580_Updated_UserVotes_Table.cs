namespace ImprovementOpportunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_UserVotes_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserVote", "HasVoted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserVote", "HasUpVoted", c => c.Boolean());
            DropColumn("dbo.UserVote", "HasDownVoted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserVote", "HasDownVoted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserVote", "HasUpVoted", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserVote", "HasVoted");
        }
    }
}
