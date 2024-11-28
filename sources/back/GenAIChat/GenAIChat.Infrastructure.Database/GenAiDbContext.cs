using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure
{
    public class GenAiDbContext(DbContextOptions<GenAiDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nom des tables
            modelBuilder.Entity<ProjectDomain>().ToTable("Projects");
            modelBuilder.Entity<DocumentDomain>().ToTable("Documents");
            modelBuilder.Entity<DocumentMetadataDomain>().ToTable("DocumentMetadatas");
            modelBuilder.Entity<UserStoryGroupDomain>().ToTable("UserStoryGroups");
            modelBuilder.Entity<UserStoryDomain>().ToTable("UserStories");
            modelBuilder.Entity<TaskDomain>().ToTable("Tasks");
            modelBuilder.Entity<TaskCostDomain>().ToTable("TaskCosts");
            modelBuilder.Entity<UserStoryPromptDomain>().ToTable("UserStoryPrompts");

            // Relation ProjectDomain -> DocumentDomain
            modelBuilder.Entity<ProjectDomain>()
                .HasMany(p => p.Documents)
                .WithOne()
                .HasForeignKey(d => d.ProjectId);

            // Relation ProjectDomain -> UserStoryGroupDomain
            modelBuilder.Entity<ProjectDomain>()
                .HasMany(p => p.Stories)
                .WithOne()
                .HasForeignKey(ug => ug.ProjectId);

            // Relation DocumentDomain -> DocumentMetadataDomain
            modelBuilder.Entity<DocumentDomain>()
                .HasOne(d => d.Metadata)
                .WithOne()
                .HasForeignKey<DocumentMetadataDomain>(m => m.DocumentId);

            // Relation UserStoryGroupDomain -> UserStoryPromptDomain
            modelBuilder.Entity<UserStoryGroupDomain>()
                .HasOne(ug => ug.Prompt)
                .WithOne()
                .HasForeignKey<UserStoryPromptDomain>(p => p.GroupId);

            // Relation UserStoryGroupDomain -> UserStoryDomain
            modelBuilder.Entity<UserStoryGroupDomain>()
                .HasMany(ug => ug.UserStories)
                .WithOne()
                .HasForeignKey(us => us.GroupId);

            // Relation UserStoryDomain -> TaskDomain
            modelBuilder.Entity<UserStoryDomain>()
                .HasMany(us => us.Tasks)
                .WithOne()
                .HasForeignKey(t => t.UserStoryId);

            // Relation TaskDomain -> TaskCostDomain
            modelBuilder.Entity<TaskDomain>()
                .HasMany(t => t.WorkingCosts)
                .WithOne()
                .HasForeignKey(tc => tc.TaskId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
