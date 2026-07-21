using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Data;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Data;
using JobBoardPlatform.Core.Entities.CityEntity.Data;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Data;
using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using JobBoardPlatform.Core.Entities.SkillEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Data;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Data;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.AdvertisementRepo;
using JobBoardPlatform.Infrastructure.Repositories.AdvertisementSkillRepo;
using JobBoardPlatform.Infrastructure.Repositories.AttachmentRepo;
using JobBoardPlatform.Infrastructure.Repositories.CityRepo;
using JobBoardPlatform.Infrastructure.Repositories.CompanyCityRepo;
using JobBoardPlatform.Infrastructure.Repositories.CompanyRepo;
using JobBoardPlatform.Infrastructure.Repositories.EducationDetailRepo;
using JobBoardPlatform.Infrastructure.Repositories.ExperienceDetailRepo;
using JobBoardPlatform.Infrastructure.Repositories.JobApplicationRepo;
using JobBoardPlatform.Infrastructure.Repositories.JobRepo;
using JobBoardPlatform.Infrastructure.Repositories.PaymentRepo;
using JobBoardPlatform.Infrastructure.Repositories.ProvinceRepo;
using JobBoardPlatform.Infrastructure.Repositories.ResumeRepo;
using JobBoardPlatform.Infrastructure.Repositories.SkillRepo;
using JobBoardPlatform.Infrastructure.Repositories.UserProfileRepo;
using JobBoardPlatform.Infrastructure.Repositories.UserRepo;
using JobBoardPlatform.Infrastructure.Repositories.UserSkillRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobBoardPlatform.Infrastructure.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        AdvertisementRepository = new AdvertisementRepository(_context);
        AdvertisementSkillRepository = new AdvertisementSkillRepository(_context);
        AttachmentRepository = new AttachmentRepository(_context);
        CityRepository = new CityRepository(_context);
        CompanyCityRepository = new CompanyCityRepository(_context);
        CompanyRepository = new CompanyRepository(_context);
        EducationDetailRepository = new EducationDetailRepository(_context);
        ExperienceDetailRepository = new ExperienceDetailRepository(_context);
        JobApplicationRepository = new JobApplicationRepository(_context);
        JobRepository = new JobRepository(_context);
        PaymentRepository = new PaymentRepository(_context);
        ProvinceRepository = new ProvinceRepository(_context);
        ResumeRepository = new ResumeRepository(_context);
        SkillRepository = new SkillRepository(_context);
        UserRepository = new UserRepository(_context);
        UserProfileRepository = new UserProfileRepository(_context);
        UserSkillRepository = new UserSkillRepository(_context);
    }

    private IDbContextTransaction? _transaction;

    public IAdvertisementRepository AdvertisementRepository { get; }

    public IAdvertisementSkillRepository AdvertisementSkillRepository { get; }

    public IAttachmentRepository AttachmentRepository { get; }

    public ICityRepository CityRepository { get; }

    public ICompanyCityRepository CompanyCityRepository { get; }

    public ICompanyRepository CompanyRepository { get; }

    public IEducationDetailRepository EducationDetailRepository { get; }

    public IExperienceDetailRepository ExperienceDetailRepository { get; }

    public IJobApplicationRepository JobApplicationRepository { get; }

    public IJobRepository JobRepository { get; }

    public IPaymentRepository PaymentRepository { get; }

    public IProvinceRepository ProvinceRepository { get; }

    public IResumeRepository ResumeRepository { get; }

    public ISkillRepository SkillRepository { get; }

    public IUserRepository UserRepository { get; }

    public IUserProfileRepository UserProfileRepository { get; }

    public IUserSkillRepository UserSkillRepository { get; }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            throw new DomainException("No active transaction.", "Transaction_NoActive");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollBackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
