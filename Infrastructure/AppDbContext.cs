using System;
using System.Collections.Generic;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

    public virtual DbSet<Pregnancy> Pregnancies { get; set; }

    public virtual DbSet<PregnancyStandard> PregnancyStandards { get; set; }

    public virtual DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }

    public virtual DbSet<ScheduleUser> ScheduleUsers { get; set; }

    public virtual DbSet<Fetus> Fetus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__B19D418153C218A9");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534C1B03E34").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ExternalProvider)
                .HasMaxLength(255)
                .HasColumnName("External_provider");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("Full_Name");
            entity.Property(e => e.IsExternal).HasColumnName("Is_external");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("User_name");
        });

        modelBuilder.Entity<AccountMembership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Account___CAE49DDDAE6AC4FE");

            entity.ToTable("Account_membership");

            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.EndDate).HasColumnName("End_date");
            entity.Property(e => e.MembershipPlanId).HasColumnName("Membership_plan_id");
            entity.Property(e => e.PaymentAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Payment_amount");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("Payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("Payment_status");
            entity.Property(e => e.StartDate).HasColumnName("Start_date");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountMemberships)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Account_m__Accou__3C69FB99");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.AccountMemberships)
                .HasForeignKey(d => d.MembershipPlanId)
                .HasConstraintName("FK__Account_m__Membe__3D5E1FD2");
        });

        modelBuilder.Entity<BlogBookmark>(entity =>
        {
            entity.HasKey(e => e.BlogBookmarkId).HasName("PK__Blog_boo__43525A619F3E70B6");

            entity.ToTable("Blog_bookmark");

            entity.Property(e => e.BlogBookmarkId).HasColumnName("Blog_bookmark_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Account).WithMany(p => p.BlogBookmarks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Blog_book__accou__49C3F6B7");

            entity.HasOne(d => d.Post).WithMany(p => p.BlogBookmarks)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Blog_book__post___4AB81AF0");
        });

        modelBuilder.Entity<BlogLike>(entity =>
        {
            entity.HasKey(e => e.BlogLikeId).HasName("PK__Blog_lik__3AE47BBD432EF71A");

            entity.ToTable("Blog_like");

            entity.Property(e => e.BlogLikeId).HasColumnName("Blog_like_id");
            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Account).WithMany(p => p.BlogLikes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Blog_like__Accou__45F365D3");

            entity.HasOne(d => d.Post).WithMany(p => p.BlogLikes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Blog_like__post___46E78A0C");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.BlogPostId).HasName("PK__Blog_pos__3FD703BF2C193ECB");

            entity.ToTable("Blog_post");

            entity.Property(e => e.BlogPostId).HasColumnName("Blog_post_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.PublishedDay).HasColumnName("published_day");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Blog_post__autho__4316F928");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__99D3E6C3A6984A5C");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("Comment_id");
            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.BlogPostId).HasColumnName("Blog_post_id");
            entity.Property(e => e.ReplyId).HasColumnName("Reply_id");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Comment__Account__5AEE82B9");

            entity.HasOne(d => d.BlogPost).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogPostId)
                .HasConstraintName("FK__Comment__Blog_po__5BE2A6F2");
        });

        modelBuilder.Entity<Fetus>(entity =>
        {
            entity.HasKey(e => e.FetusId).HasName("PK__Fetus__F1A3E2A3D3A3D3A3");

            entity.ToTable("Fetus");

            entity.Property(e => e.FetusId).HasColumnName("Fetus_id");
            entity.Property(e => e.PregnancyId).HasColumnName("Pregnancy_id");
            entity.Property(e => e.Name);
            entity.Property(e => e.gender).HasMaxLength(50);

            entity.HasOne(d => d.Pregnancy).WithMany(p => p.Fetus)
                .HasForeignKey(d => d.PregnancyId)
                .HasConstraintName("FK__Fetus__Pregnancy__4D94879B");
            entity.HasMany(d => d.FetusRecords).WithOne(p => p.Fetus).HasForeignKey(d => d.FetusId);
        });

        modelBuilder.Entity<FetusRecord>(entity =>
        {
            
            entity.HasKey(e => e.FetusRecordId).HasName("PK__Fetus_Re__11E3575A507B3844");

            entity.ToTable("Fetus_Record");

            entity.Property(e => e.FetusRecordId).HasColumnName("Fetus_Record_id");
            entity.Property(e => e.Bpd)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("BPD");
            entity.Property(e => e.FetusId).HasColumnName("Fetus_Id");
            entity.Property(e => e.Hc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("HC");
            entity.Property(e => e.InputPeriod).HasColumnName("Input_Period");
            entity.Property(e => e.Length).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__Media__278CBDD334309D6C");

            entity.Property(e => e.MediaId).HasColumnName("Media_id");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.HasKey(e => e.MembershipPlanId).HasName("PK__Membersh__0A7DEA259F8CD130");

            entity.ToTable("Membership_plan");

            entity.Property(e => e.MembershipPlanId).HasColumnName("Membership_plan_id");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentMethod__DA638B192282CC45");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethod_id");
            entity.Property(e => e.AccountMembershipId).HasColumnName("Account_id");
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(1)
                .HasColumnName("Transaction_code");
            entity.Property(e => e.Via).HasMaxLength(50);

            entity
                .HasOne(d => d.AccountMembership)
                .WithOne(p => p.PaymentMethods)
                .IsRequired(false)
                .HasForeignKey<PaymentMethod>(d => d.AccountMembershipId)
                .HasConstraintName("FK__Payment__Account__403A8C7D");
        });

        modelBuilder.Entity<Pregnancy>(entity =>
        {
            entity.HasKey(e => e.PregnancyId).HasName("PK__Pregnanc__9BB559690F4E4C0D");

            entity.ToTable("Pregnancy");

            entity.Property(e => e.PregnancyId).HasColumnName("Pregnancy_id");
            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.EndDate).HasColumnName("End_date");
            entity.Property(e => e.StartDate).HasColumnName("Start_date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Pregnancies)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Pregnancy__Accou__5165187F");
            entity.HasMany(d => d.ScheduleUser).WithOne(p => p.Pregnancy).HasForeignKey(d => d.PregnancyId);
            entity.HasMany(d => d.Fetus).WithOne(p => p.Pregnancy).HasForeignKey(d => d.PregnancyId);
        });

        modelBuilder.Entity<PregnancyStandard>(entity =>
        {
            entity.HasKey(e => e.PregnancyStandardId).HasName("PK__Pregnanc__41CCD4377106EC7A");

            entity.ToTable("Pregnancy_Standard");

            entity.Property(e => e.PregnancyStandardId).HasColumnName("Pregnancy_Standard_id");
            entity.Property(e => e.Maximum).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Minimum).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<ScheduleTemplate>(entity =>
        {
            entity.HasKey(e => e.ScheduleTemplateId).HasName("PK__Schedule__112A047641EEFD5D");

            entity.ToTable("Schedule_Template");

            entity.Property(e => e.ScheduleTemplateId).HasColumnName("Schedule_Template_id");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<ScheduleUser>(entity =>
        {
            entity.HasKey(e => e.ScheduleUserId).HasName("PK__Schedule__CEFD2C9D1F53F754");

            entity.ToTable("Schedule_User");

            entity.Property(e => e.ScheduleUserId).HasColumnName("Schedule_User_id");
            entity.Property(e => e.PregnancyId).HasColumnName("Pregnancy_id");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
        }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
