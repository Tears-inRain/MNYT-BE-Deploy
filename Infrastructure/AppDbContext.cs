using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountMembership> AccountMemberships { get; set; }

    public virtual DbSet<BlogBookmark> BlogBookmarks { get; set; }

    public virtual DbSet<BlogLike> BlogLikes { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<FetusRecord> FetusRecords { get; set; }

    public virtual DbSet<Media> Media { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

    public virtual DbSet<Pregnancy> Pregnancies { get; set; }

    public virtual DbSet<PregnancyStandard> PregnancyStandards { get; set; }

    public virtual DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }

    public virtual DbSet<ScheduleUser> ScheduleUsers { get; set; }

    public virtual DbSet<Fetus> Fetus { get; set; }

 
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
