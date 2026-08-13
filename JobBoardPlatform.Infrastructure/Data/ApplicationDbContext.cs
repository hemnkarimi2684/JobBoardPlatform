using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using JobBoardPlatform.Core.Entities.NotifierEntity.Entity;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JobBoardPlatform.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
    public DbSet<AdvertisementSkill> AdvertisementSkills => Set<AdvertisementSkill>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyCity> CompanyCities => Set<CompanyCity>();
    public DbSet<EducationDetail> EducationDetails => Set<EducationDetail>();
    public DbSet<ExperienceDetail> ExperienceDetails => Set<ExperienceDetail>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSkill> UserSkills => Set<UserSkill>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<JobCategory> JobCategories => Set<JobCategory>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Notifier> Notifiers => Set<Notifier>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<FeaturedPackage> FeaturedPackages => Set<FeaturedPackage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ApplyIdentityEntitiesConfiguration(modelBuilder);
    }

    private void ApplyIdentityEntitiesConfiguration(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });
    }
}
