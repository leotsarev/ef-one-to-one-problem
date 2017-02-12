namespace TestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        ClaimId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClaimId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.ForumThreads",
                c => new
                    {
                        ForumThreadId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForumThreadId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumThreads", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Claims", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ForumThreads", new[] { "ProjectId" });
            DropIndex("dbo.Claims", new[] { "ProjectId" });
            DropTable("dbo.ForumThreads");
            DropTable("dbo.Projects");
            DropTable("dbo.Claims");
        }
    }
}
