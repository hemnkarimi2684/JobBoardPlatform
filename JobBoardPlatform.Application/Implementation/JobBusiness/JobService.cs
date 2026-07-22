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

    public async Task CreateJobAsync(CreateJobRequestDto jobRequestDto, CancellationToken cancellationToken = default)
    {
        var normalizedName = jobRequestDto.Name.Trim();

        var categoryExist = await _unitOfWork.JobCategoryRepository.ExistAsync(jobRequestDto.JobCategoryId, cancellationToken);

        if (!categoryExist)
            throw new NotFoundException("Job category was not found.");

        var isDuplicateJob = await _unitOfWork.JobRepository
            .IsDuplicateJobAsync(normalizedName, jobRequestDto.JobCategoryId, cancellationToken);

        if (isDuplicateJob)
            throw new ConflictException("A job with the same name already exists in this category.");

        var job = new Job(normalizedName, jobRequestDto.JobCategoryId, _currentUser.UserId);

        await _unitOfWork.JobRepository.AddAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<JobResponseDto> GetAllJobsAsync(
        string text,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<JobAdvertisementListItemResponseDto>> GetJobAdvertisementsAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
