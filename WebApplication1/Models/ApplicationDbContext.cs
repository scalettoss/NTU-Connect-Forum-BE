using Microsoft.EntityFrameworkCore;

namespace ForumBE.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ScamDetection> ScamDetections { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportStatus> ReportStatuses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Warning> Warnings { get; set; }
        public DbSet<ScamKeyword> ScamKeywords { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.IsActive);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleId);

            // Configure unique constraints and indexes for UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasIndex(up => up.UserId)
                .IsUnique();

            modelBuilder.Entity<UserProfile>()
                .HasIndex(up => up.IsProfilePublic);

            modelBuilder.Entity<UserProfile>()
                .HasIndex(up => up.LastLoginAt);

            // Configure unique constraints and indexes for Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.UserId);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CreatedAt);

            // Configure indexes for Post
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.CategoryId);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.IsScam);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.IsReviewed);

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.CreatedAt);


            // Configure indexes for Comment
            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.PostId);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.UserId);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.CreatedAt);

            // Configure indexes for ScamDetection
            modelBuilder.Entity<ScamDetection>()
                .HasIndex(sd => sd.PostId);

            modelBuilder.Entity<ScamDetection>()
                .HasIndex(sd => sd.ModelPrediction);

            modelBuilder.Entity<ScamDetection>()
                .HasIndex(sd => sd.AdminReviewed);

            modelBuilder.Entity<ScamDetection>()
                .HasIndex(sd => sd.DetectedAt);

            // Configure indexes for Report
            modelBuilder.Entity<Report>()
                .HasIndex(r => r.UserId);

            modelBuilder.Entity<Report>()
                .HasIndex(r => r.PostId);

            modelBuilder.Entity<Report>()
                .HasIndex(r => r.CommentId);

            modelBuilder.Entity<Report>()
                .HasIndex(r => r.ReportedAt);

            // Configure indexes for ReportStatus
            modelBuilder.Entity<ReportStatus>()
                .HasIndex(rs => rs.ReportId)
                .IsUnique();

            modelBuilder.Entity<ReportStatus>()
                .HasIndex(rs => rs.IsProcessed);

            modelBuilder.Entity<ReportStatus>()
                .HasIndex(rs => rs.HandledBy);

            // Configure indexes for Notification
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.UserId);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.IsRead);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.CreatedAt);

            // Configure indexes for Like
            modelBuilder.Entity<Like>()
                .HasIndex(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => l.PostId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => l.CommentId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => l.CreatedAt);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.PostId })
                .IsUnique()
                .HasFilter("[PostId] IS NOT NULL");

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.CommentId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL");

            // Configure indexes for Bookmark
            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => b.UserId);

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => b.PostId);

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => b.CreatedAt);

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.PostId })
                .IsUnique();

            // Configure indexes for ActivityLog
            modelBuilder.Entity<ActivityLog>()
                .HasIndex(al => al.UserId);

            modelBuilder.Entity<ActivityLog>()
                .HasIndex(al => al.CreatedAt);

            // Configure indexes for Warning
            modelBuilder.Entity<Warning>()
                .HasIndex(w => w.UserId);

            modelBuilder.Entity<Warning>()
                .HasIndex(w => w.IssuedBy);

            modelBuilder.Entity<Warning>()
                .HasIndex(w => w.CreatedAt);

            // Configure indexes for ScamKeyword
            modelBuilder.Entity<ScamKeyword>()
                .HasIndex(sk => sk.Keyword)
                .IsUnique();

            // Configure indexes for Attachment
            modelBuilder.Entity<Attachment>()
                .HasIndex(a => a.PostId);

            modelBuilder.Entity<Attachment>()
                .HasIndex(a => a.CommentId);

            modelBuilder.Entity<Attachment>()
                .HasIndex(a => a.UserId);

            modelBuilder.Entity<Attachment>()
                .HasIndex(a => a.FileType);

            modelBuilder.Entity<Attachment>()
                .HasIndex(a => a.UploadedAt);

            // Configure relationships with cascade delete
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfiles)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Fix for multiple cascade paths - change cascade delete to restrict for these relationships
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure many-to-many relationships
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(a => a.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(a => a.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attachments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Post)
                .WithMany(p => p.Bookmarks)
                .HasForeignKey(b => b.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(al => al.User)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScamDetection>()
                .HasOne(sd => sd.Post)
                .WithMany(p => p.ScamDetections)
                .HasForeignKey(sd => sd.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for Warning
            modelBuilder.Entity<Warning>()
                .HasOne(w => w.User)
                .WithMany(u => u.ReceivedWarnings)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warning>()
                .HasOne(w => w.Issuer)
                .WithMany(u => u.IssuedWarnings)
                .HasForeignKey(w => w.IssuedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship for ReportStatus
            modelBuilder.Entity<ReportStatus>()
                .HasOne(rs => rs.Report)
                .WithOne(r => r.ReportStatus)
                .HasForeignKey<ReportStatus>(rs => rs.ReportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportStatus>()
                .HasOne(rs => rs.Handler)
                .WithMany(u => u.HandledReports)
                .HasForeignKey(rs => rs.HandledBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}