namespace ImprovementOpportunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedSuggestion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Suggestion", "Images", c => c.String());
            AlterColumn("dbo.Suggestion", "Links", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Suggestion", "Links", c => c.String(nullable: false));
            AlterColumn("dbo.Suggestion", "Images", c => c.String(nullable: false));
        }
    }
}
