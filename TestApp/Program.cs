using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
  public class Project
  {
    public int ProjectId { get; set; }
    public string Name { get; set; }
  }
  public class Claim
  {
    [Key]
    public int ClaimId { get; set; }
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public virtual CommentDiscussion CommentDiscussion { get; set; }
  }

  public class ForumThread
  {
    [Key]
    public int ForumThreadId { get; set; }
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public virtual CommentDiscussion CommentDiscussion { get; set; }
  }

  public class CommentDiscussion
  {
    public int CommentDiscussionId { get; set; }
    public virtual Claim Claim { get; set; }
    public virtual ForumThread ForumThread { get; set; }
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
  }


  public class MyDbContext : DbContext
  {
    public DbSet<Claim> Claims { get; set; }

    public DbSet<ForumThread> ForumThreads { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<CommentDiscussion>().HasOptional(e => e.Claim)
        .WithRequired(m => m.CommentDiscussion)
        .Map(cfg => cfg.MapKey("ClaimId"));

      modelBuilder.Entity<CommentDiscussion>().HasOptional(e => e.ForumThread)
          .WithRequired(m => m.CommentDiscussion)
          .Map(cfg => cfg.MapKey("ForumThreadId")));
    }
  }

  class Program
  {
    
    static void Main(string[] args)
    {
      var ctx = new MyDbContext();
      ctx.Database.CreateIfNotExists();
    }
  }
}
