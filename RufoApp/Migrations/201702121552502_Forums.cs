namespace RufoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forums : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentDiscussions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimId = c.Int(),
                        ForumThreadId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Claims", "ClaimId", c => c.Int(nullable: false));
            AddColumn("dbo.ForumThreads", "ForumThreadId", c => c.Int(nullable: false));
            CreateIndex("dbo.Claims", "ClaimId");
            CreateIndex("dbo.ForumThreads", "ForumThreadId");
            AddForeignKey("dbo.Claims", "ClaimId", "dbo.CommentDiscussions", "Id");
            AddForeignKey("dbo.ForumThreads", "ForumThreadId", "dbo.CommentDiscussions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumThreads", "ForumThreadId", "dbo.CommentDiscussions");
            DropForeignKey("dbo.Claims", "ClaimId", "dbo.CommentDiscussions");
            DropIndex("dbo.ForumThreads", new[] { "ForumThreadId" });
            DropIndex("dbo.Claims", new[] { "ClaimId" });
            DropColumn("dbo.ForumThreads", "ForumThreadId");
            DropColumn("dbo.Claims", "ClaimId");
            DropTable("dbo.CommentDiscussions");
        }
    }
}
