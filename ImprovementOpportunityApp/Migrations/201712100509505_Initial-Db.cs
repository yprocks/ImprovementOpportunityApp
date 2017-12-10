namespace ImprovementOpportunityApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Forum",
                c => new
                    {
                        ForumId = c.Int(nullable: false, identity: true),
                        SuggestionId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        TopicId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUpdated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        UpVotes = c.Int(nullable: false),
                        DownVotes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForumId)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Suggestion", t => t.SuggestionId, cascadeDelete: true)
                .ForeignKey("dbo.Topic", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.SuggestionId)
                .Index(t => t.DepartmentId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.ForumMessage",
                c => new
                    {
                        ForumMessageId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ForumId = c.Int(nullable: false),
                        ReplyMessageId = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUpdated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Links = c.String(),
                        Images = c.String(),
                    })
                .PrimaryKey(t => t.ForumMessageId)
                .ForeignKey("dbo.Forum", t => t.ForumId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ForumId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(nullable: false, maxLength: 128),
                        DepartmentId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUpdated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: false)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Suggestion",
                c => new
                    {
                        SuggestionId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DepartmentId = c.Int(nullable: false),
                        TopicId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 512),
                        Description = c.String(nullable: false),
                        Images = c.String(nullable: false),
                        Links = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUpdated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        HasReviewed = c.Boolean(nullable: false),
                        HasConsidered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SuggestionId)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Topic", t => t.TopicId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserVote",
                c => new
                    {
                        UserVoteId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ForumId = c.Int(nullable: false),
                        HasUpVoted = c.Boolean(nullable: false),
                        HasDownVoted = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserVoteId)
                .ForeignKey("dbo.Forum", t => t.ForumId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ForumId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserVote", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVote", "ForumId", "dbo.Forum");
            DropForeignKey("dbo.Suggestion", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topic", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suggestion", "TopicId", "dbo.Topic");
            DropForeignKey("dbo.Forum", "TopicId", "dbo.Topic");
            DropForeignKey("dbo.Forum", "SuggestionId", "dbo.Suggestion");
            DropForeignKey("dbo.Suggestion", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ForumMessage", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ForumMessage", "ForumId", "dbo.Forum");
            DropForeignKey("dbo.Forum", "DepartmentId", "dbo.Department");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UserVote", new[] { "ForumId" });
            DropIndex("dbo.UserVote", new[] { "UserId" });
            DropIndex("dbo.Topic", new[] { "UserId" });
            DropIndex("dbo.Suggestion", new[] { "TopicId" });
            DropIndex("dbo.Suggestion", new[] { "DepartmentId" });
            DropIndex("dbo.Suggestion", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.ForumMessage", new[] { "ForumId" });
            DropIndex("dbo.ForumMessage", new[] { "UserId" });
            DropIndex("dbo.Forum", new[] { "TopicId" });
            DropIndex("dbo.Forum", new[] { "DepartmentId" });
            DropIndex("dbo.Forum", new[] { "SuggestionId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserVote");
            DropTable("dbo.Topic");
            DropTable("dbo.Suggestion");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ForumMessage");
            DropTable("dbo.Forum");
            DropTable("dbo.Department");
        }
    }
}
