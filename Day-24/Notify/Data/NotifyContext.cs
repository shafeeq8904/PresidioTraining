using Microsoft.EntityFrameworkCore;
using Notify.Models;

namespace Notify.Data
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions<NotifyContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();


            modelBuilder.Entity<Document>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById);

            base.OnModelCreating(modelBuilder);
        }
    }
}
