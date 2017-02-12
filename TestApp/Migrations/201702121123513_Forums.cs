namespace TestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forums : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ForumThreads");
            CreateTable(
                "dbo.CommentDiscussions",
                c => new
                    {
                        CommentDiscussionId = c.Int(nullable: false, identity: true),
                        ClaimId = c.Int(),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentDiscussionId)
                .ForeignKey("dbo.Claims", t => t.ClaimId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ClaimId)
                .Index(t => t.ProjectId);
            
            AlterColumn("dbo.ForumThreads", "ForumThreadId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ForumThreads", "ForumThreadId");
            CreateIndex("dbo.ForumThreads", "ForumThreadId");
            AddForeignKey("dbo.ForumThreads", "ForumThreadId", "dbo.CommentDiscussions", "CommentDiscussionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentDiscussions", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ForumThreads", "ForumThreadId", "dbo.CommentDiscussions");
            DropForeignKey("dbo.CommentDiscussions", "ClaimId", "dbo.Claims");
            DropIndex("dbo.CommentDiscussions", new[] { "ProjectId" });
            DropIndex("dbo.CommentDiscussions", new[] { "ClaimId" });
            DropIndex("dbo.ForumThreads", new[] { "ForumThreadId" });
            DropPrimaryKey("dbo.ForumThreads");
            AlterColumn("dbo.ForumThreads", "ForumThreadId", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.CommentDiscussions");
            AddPrimaryKey("dbo.ForumThreads", "ForumThreadId");
        }
    }
}
