using Educonnect.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Educonnect.Infrastructure.Data;

public class ApplicationDbContext : IdentityUserContext<User, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        RenameIdentityTables(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureProfile(modelBuilder);
        ConfigurePost(modelBuilder);
        ConfigureComment(modelBuilder);
        ConfigureGroup(modelBuilder);
        ConfigureReaction(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }

    private static void RenameIdentityTables(ModelBuilder m)
    {
        m.Entity<User>().ToTable("Users");
        m.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        m.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        m.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
    }

    private static void ConfigureUser(ModelBuilder m)
    {
        m.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.HasQueryFilter(u => !u.IsDeleted);

            e.HasOne(u => u.Profile)
             .WithOne(p => p.User)
             .HasForeignKey<Profile>("UserId")
             .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureProfile(ModelBuilder m)
    {
        m.Entity<Profile>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Username).IsUnique();
            e.HasQueryFilter(p => !p.IsDeleted);

            e.HasMany(p => p.Posts)
             .WithOne(p => p.Author)
             .HasForeignKey(p => p.AuthorId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany<Reaction>()
             .WithOne(r => r.Profile)
             .HasForeignKey(r => r.ProfileId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePost(ModelBuilder m)
    {
        m.Entity<Post>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.AuthorId);
            e.HasIndex(p => p.CreatedAt);
            e.HasQueryFilter(p => !p.IsDeleted);

            e.HasMany(p => p.Reactions)
             .WithOne(r => r.Post)
             .HasForeignKey(r => r.PostId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureComment(ModelBuilder m)
    {
        m.Entity<Comment>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasQueryFilter(c => !c.IsDeleted);

            e.HasOne(c => c.Author)
             .WithMany(p => p.Comments)
             .HasForeignKey("AuthorId")
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.Post)
             .WithMany(p => p.Comments)
             .HasForeignKey("PostId")
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(c => c.Reactions)
             .WithOne(r => r.Comment)
             .HasForeignKey(r => r.CommentId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureGroup(ModelBuilder m)
    {
        m.Entity<Group>(e =>
        {
            e.HasKey(g => g.Id);
            e.HasIndex(g => g.GroupId).IsUnique();
            e.HasQueryFilter(g => !g.IsDeleted);

            e.Ignore(g => g.PostId);

            e.HasOne(g => g.Admin)
             .WithMany()
             .HasForeignKey(g => g.AdminId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(g => g.Memebres)
             .WithMany(p => p.Groups)
             .UsingEntity("GroupMembers");

            e.HasMany(g => g.Posts)
             .WithOne(p => p.Group);
        });
    }

    private static void ConfigureReaction(ModelBuilder m)
    {
        m.Entity<Reaction>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.ProfileId);
            e.HasIndex(r => r.PostId);
            e.HasIndex(r => r.CommentId);
        });
        m.Entity<Reaction>()
            .HasIndex(r => new { r.ProfileId, r.PostId })
            .IsUnique()
            .HasFilter("[PostId] IS NOT NULL");

        m.Entity<Reaction>()
            .HasIndex(r => new { r.ProfileId, r.CommentId })
            .IsUnique()
            .HasFilter("[CommentId] IS NOT NULL");
    }

    private static void ConfigureRefreshToken(ModelBuilder m)
    {
        m.Entity<RefreshToken>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.Token).IsUnique();

            e.HasOne(r => r.User)
             .WithMany()
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
