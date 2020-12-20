using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(userRole => userRole.UserRoles)
                .WithOne(user => user.User)
                .HasForeignKey(userRole => userRole.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(userRole => userRole.UserRoles)
                .WithOne(role => role.Role)
                .HasForeignKey(userRole => userRole.RoleId)
                .IsRequired();

            modelBuilder.Entity<UserLike>().HasKey(key => new {key.SourceUserId, key.LikedUserId});

            modelBuilder.Entity<UserLike>()
                .HasOne(source => source.SourceUser)
                .WithMany(like => like.LikedUsers)
                .HasForeignKey(source => source.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserLike>()
                .HasOne(source => source.LikedUser)
                .WithMany(like => like.LikedByUsers)
                .HasForeignKey(source => source.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(user => user.Recipient)
                .WithMany(message => message.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Message>()
                .HasOne(user => user.Sender)
                .WithMany(message => message.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}