using GenAIChat.Domain;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database
{
    public class GenAiDbContext : DbContext
    {

        public DbSet<ProjectDomain> Projects { get; set; }
        public DbSet<UserStoryDomain> UserStories { get; set; }
        public DbSet<UserStoryTaskDomain> Tasks { get; set; }
        public DbSet<DocumentDomain> Documents { get; set; }

        public DbSet<DocumentMetadataDomain> DocumentMetadatas { get; set; }

        public GenAiDbContext(DbContextOptions<GenAiDbContext> contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des entités
            modelBuilder.Entity<ProjectDomain>().ToTable("Projects");
            modelBuilder.Entity<DocumentDomain>().ToTable("Documents");
            modelBuilder.Entity<UserStoryDomain>().ToTable("UserStories");
            modelBuilder.Entity<UserStoryTaskDomain>().ToTable("Tasks");
            modelBuilder.Entity<DocumentMetadataDomain>().ToTable("Metadatas");
            modelBuilder.Entity<PromptDomain>().ToTable("Prompts");

            // Configuration des relations
            modelBuilder.Entity<ProjectDomain>()
                .HasMany(p => p.Documents)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectDomain>()
                .HasMany(p => p.UserStories)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserStoryDomain>()
                .HasMany(us => us.Tasks)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
