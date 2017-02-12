using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RufoApp
{
  public class Claim
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public CommentDiscussion CommentDiscussion { get; set; }
  }

  public class ForumThread
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public CommentDiscussion CommentDiscussion { get; set; }
  }

  public class CommentDiscussion
  {
    public int Id { get; set; }
    public int? ClaimId { get; set; }
    public Claim Claim { get; set; }
    public int? ForumThreadId { get; set; }
    public ForumThread ForumThread { get; set; }
  }

  public class ClaimConfiguration : EntityTypeConfiguration<Claim>
  {
    public ClaimConfiguration()
    {
      HasKey(e => e.Id);
      Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(100);
    }
  }

  public class ForumThreadConfiguration : EntityTypeConfiguration<ForumThread>
  {
    public ForumThreadConfiguration()
    {
      HasKey(e => e.Id);
      Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(100);
    }
  }

  public class CommentDiscussionConfiguration : EntityTypeConfiguration<CommentDiscussion>
  {
    public CommentDiscussionConfiguration()
    {
      HasKey(e => e.Id);
      HasOptional(e => e.Claim)
          .WithRequired(m => m.CommentDiscussion)
          .Map(cfg => cfg.MapKey(nameof(CommentDiscussion.ClaimId)));
      HasOptional(e => e.ForumThread)
          .WithRequired(m => m.CommentDiscussion)
          .Map(cfg => cfg.MapKey(nameof(CommentDiscussion.ForumThreadId)));
    }
  }

  public class ModelContext : DbContext
  {
    // Der Kontext wurde für die Verwendung einer ModelContext-Verbindungszeichenfolge aus der
    // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die
    // Datenbank 'ConsoleApp7.Model.ModelContext' auf der LocalDb-Instanz als Ziel.
    //
    // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die ModelContext-Zeichenfolge
    // in der Anwendungskonfigurationsdatei.
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new ClaimConfiguration());
      modelBuilder.Configurations.Add(new ForumThreadConfiguration());
      modelBuilder.Configurations.Add(new CommentDiscussionConfiguration());

      base.OnModelCreating(modelBuilder);
    }

    protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
    {
      return base.ShouldValidateEntity(entityEntry);
    }

    protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
    {
      if (entityEntry.Entity is CommentDiscussion)
      {
        if (!entityEntry.CurrentValues.GetValue<int?>(nameof(CommentDiscussion.ClaimId)).HasValue && !entityEntry.CurrentValues.GetValue<int?>(nameof(CommentDiscussion.ForumThreadId)).HasValue)
        {
          var list = new List<System.Data.Entity.Validation.DbValidationError>();
          list.Add(new System.Data.Entity.Validation.DbValidationError(nameof(CommentDiscussion.Claim), "Claim or ForumThread is required"));
          list.Add(new System.Data.Entity.Validation.DbValidationError(nameof(CommentDiscussion.ForumThread), "Claim or ForumThread is required"));

          return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
        }

        if (entityEntry.CurrentValues.GetValue<int?>(nameof(CommentDiscussion.ClaimId)).HasValue && entityEntry.CurrentValues.GetValue<int?>(nameof(CommentDiscussion.ForumThreadId)).HasValue)
        {
          var list = new List<System.Data.Entity.Validation.DbValidationError>();
          list.Add(new System.Data.Entity.Validation.DbValidationError(nameof(CommentDiscussion.Claim), "Only Claim or ForumThread is possible, not both"));
          list.Add(new System.Data.Entity.Validation.DbValidationError(nameof(CommentDiscussion.ForumThread), "Only Claim or ForumThread is possible, not both"));

          return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
        }
      }
      return base.ValidateEntity(entityEntry, items);
    }
  }
  class Program
  {
    static void Main(string[] args)
    {
      var ctx = new ModelContext();
      ctx.Database.CreateIfNotExists();
    }
  }
}
