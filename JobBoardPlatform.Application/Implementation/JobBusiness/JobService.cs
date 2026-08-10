using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.JobBusiness;

public class JobService : IJobService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public JobService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task CreateJobAsync(CreateJobRequestDto jobRequestDto, CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var trimmedName = jobRequestDto.Name.Trim();

        var categoryExist = await _unitOfWork.JobCategoryRepository.ExistAsync(jobRequestDto.JobCategoryId, cancellationToken);

        if (!categoryExist)
            throw new NotFoundException("Job category was not found.");

        var isDuplicateJob = await _unitOfWork.JobRepository
            .IsDuplicateJobAsync(trimmedName, jobRequestDto.JobCategoryId, cancellationToken);

        if (isDuplicateJob)
            throw new ConflictException("A job with the same name already exists in this category.");

        var job = new Job(trimmedName, jobRequestDto.JobCategoryId, _currentUser.UserId);

        await _unitOfWork.JobRepository.AddAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<JobResponseDto>> GetAllJobsAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.JobRepository.GetAllJobsAsync(
                                                                                        textRequestDto.Text,
                                                                                        j => new JobResponseDto
                                                                                        {
                                                                                            JobId = j.Id,
                                                                                            Name = j.Name
                                                                                        },
                                                                                        cancellationToken,
                                                                                        pagingCommand.PageNumber,
                                                                                        pagingCommand.PageSize);

        return Pagination<JobResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    public async Task<List<JobResponseDto>> GetAllForSelectAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.JobRepository.GetAllForSelectAsync(j => new JobResponseDto
        {
            JobId = j.Id,
            Name = j.Name
        }, cancellationToken);
    }

    public async Task<Pagination<JobAdvertisementListItemResponseDto>> GetJobAdvertisementsAsync(
    Guid jobId,
    PagingRequestDto pagingCommand,
    CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.AdvertisementRepository
            .GetJobAdvertisementsAsync(
            a => new JobAdvertisementListItemResponseDto
            {
                Description = a.Description,
                AboutCompany = a.Company.AboutUs,
                AdvertisementId = a.Id,
                CreatedAt = a.CreatedAt,
                MaximumAge = a.MaximumAge,
                MinimumAge = a.MinimumAge,
                MinimumSalary = a.MinimumSalary,
                MaximumSalary = a.MaximumSalary,
                CityId = a.CityId,
                CityName = a.City.Name,
                CollaborationType = a.CollaborationType,
                CompanyId = a.CompanyId,
                CompanyName = a.Company.Name,
                ExperienceLevel = a.ExperienceLevel,
                CompanyJobCategoryId = a.Company.JobCategoryId,
                CompanyJobCategoryName = a.Company.JobCategory.Name,
                JobId = a.JobId,
                JobName = a.Job.Name,
                SkillNames = a.AdvertisementSkills.Select(x => x.Skill.Name).ToList()
            },
            jobId,
            cancellationToken,
            pagingCommand.PageNumber,
            pagingCommand.PageSize);

        return Pagination<JobAdvertisementListItemResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    public async Task SoftDeleteAsync(
        Guid jobId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var result = await _unitOfWork.JobRepository.SoftDeleteAsync(jobId, _currentUser.UserId, cancellationToken);

        if (!result)
            throw new ValidationException("Could not delete job");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion


}
