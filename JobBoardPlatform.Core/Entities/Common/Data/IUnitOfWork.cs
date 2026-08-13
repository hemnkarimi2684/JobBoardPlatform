using JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Data;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Data;
using JobBoardPlatform.Core.Entities.CityEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Data;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Data;
using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Data;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Data;
using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Data;
using JobBoardPlatform.Core.Entities.SkillEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Data;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Data;

namespace JobBoardPlatform.Core.Entities.Common.Data;

public interface IUnitOfWork
{
    IAdvertisementRepository AdvertisementRepository { get; }
    IAdvertisementSkillRepository AdvertisementSkillRepository { get; }
    IAttachmentRepository AttachmentRepository { get; }
    ICityRepository CityRepository { get; }
    ICompanyCityRepository CompanyCityRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IEducationDetailRepository EducationDetailRepository { get; }
    IExperienceDetailRepository ExperienceDetailRepository { get; }
    IJobApplicationRepository JobApplicationRepository { get; }
    IJobRepository JobRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IProvinceRepository ProvinceRepository { get; }
    IResumeRepository ResumeRepository { get; }
    ISkillRepository SkillRepository { get; }
    IUserRepository UserRepository { get; }
    IUserProfileRepository UserProfileRepository { get; }
    IUserSkillRepository UserSkillRepository { get; }
    IJobCategoryRepository JobCategoryRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    IFeaturedPackageRepository FeaturedPackageRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitTransactionAsync(CancellationToken cancellationToken);

    Task RollBackTransactionAsync(CancellationToken cancellationToken);
}
