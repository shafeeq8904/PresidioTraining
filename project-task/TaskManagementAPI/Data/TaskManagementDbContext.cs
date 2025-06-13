using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using TaskManagementAPI.Enums;

namespace TaskManagementAPI.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaskItem> TaskItems { get; set; } = null!;
        public DbSet<TaskStatusLog> TaskStatusLogs { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  //user model
                  modelBuilder.Entity<User>(entity =>
                  {
                        entity.HasKey(u => u.Id);

                        entity.HasIndex(u => u.Email).IsUnique();

                        entity.Property(u => u.Role)
                        .HasConversion<string>()
                        .IsRequired();

                        entity.Property(u => u.FullName).IsRequired();
                        entity.Property(u => u.Email).IsRequired();
                        entity.Property(u => u.PasswordHash).IsRequired();

                        // entity.Property(u => u.ProfilePicturePath)
                        //       .HasMaxLength(255);

                        entity.HasMany(u => u.CreatedTasks)
                        .WithOne(t => t.CreatedByUser)
                        .HasForeignKey(t => t.CreatedById)
                        .OnDelete(DeleteBehavior.Restrict);

                        //User-taskitem
                        entity.HasMany(u => u.AssignedTasks)
                        .WithOne(t => t.AssignedTo)
                        .HasForeignKey(t => t.AssignedToId)
                        .OnDelete(DeleteBehavior.SetNull);
                  });

                  //taskitem model
                  modelBuilder.Entity<TaskItem>(entity =>
                  {
                        entity.HasKey(t => t.Id);

                        entity.Property(t => t.Title).IsRequired();

                        entity.Property(t => t.Description).IsRequired();

                        entity.Property(t => t.Status)
                        .HasConversion<string>()
                        .IsRequired();

                        // 1-to many relation
                        entity.HasMany(t => t.StatusLogs)
                        .WithOne(s => s.TaskItem)
                        .HasForeignKey(s => s.TaskItemId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(t => t.UpdatedByUser)
                       .WithMany()
                       .HasForeignKey(t => t.UpdatedById)
                       .OnDelete(DeleteBehavior.SetNull);
                  });

                  // taskstatuslog model
                  modelBuilder.Entity<TaskStatusLog>(entity =>
                  {
                        entity.HasKey(s => s.Id);

                        entity.Property(s => s.PreviousStatus)
                        .HasConversion<string>()
                        .IsRequired();

                        entity.Property(s => s.NewStatus)
                        .HasConversion<string>()
                        .IsRequired();

                        // taskstatuslog - user 
                        entity.HasOne(s => s.ChangedBy)
                        .WithMany()
                        .HasForeignKey(s => s.ChangedById)
                        .OnDelete(DeleteBehavior.Restrict);


                  });
                  
                  //refresh token
                  modelBuilder.Entity<RefreshToken>(entity =>
                  {
                  entity.HasKey(rt => rt.Id);
                  entity.HasOne(rt => rt.User)
                        .WithMany(u => u.RefreshTokens)
                        .HasForeignKey(rt => rt.UserId);
                  });




            }
    }
}
