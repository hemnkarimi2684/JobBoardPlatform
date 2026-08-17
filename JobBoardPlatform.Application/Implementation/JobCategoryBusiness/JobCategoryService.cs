using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.RedisKeys;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Application.Interfaces.RedisInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.JobCategoryBusiness;

public class JobCategoryService : IJobCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    private readonly IRedisService _redisService;

    public JobCategoryService(
        IUnitOfWork unitOfWork, 
        ICurrentUser currentUser, 
        IAccessControlService accessControlService,
        IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _redisService = redisService;
    }

    #region Create Methods

    public async Task CreateJobCategoryAsync(CreateJobCategoryRequestDto jobCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var isDuplicateName = await _unitOfWork.JobCategoryRepository.IsDuplicateNameAsync(jobCategoryRequestDto.Name, cancellationToken);

        if (isDuplicateName)
            throw new ConflictException("job category is already exist");

        var jobCategory = new JobCategory(jobCategoryRequestDto.Name, _currentUser.UserId);

        await _unitOfWork.JobCategoryRepository.AddAsync(jobCategory, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _redisService.RemoveAsync(RedisCacheKeys.JobCategoriesSelect);
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<JobCategoryResponseDto>> GetAllJobCategoriesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.JobCategoryRepository.GetAllJobCategoriesAsync(jc => new JobCategoryResponseDto
        {
            JobCategoryId = jc.Id,
            Name = jc.Name
        },
          textRequestDto.Text,
          cancellationToken,
          pagingCommand.PageNumber,
          pagingCommand.PageSize);

        return Pagination<JobCategoryResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    public async Task<JobCategoryDetailResponseDto> GetJobCategoryByIdAsync(
        Guid jobCategoryId,
        CancellationToken cancellationToken = default)
    {
        var jobCategory = await _unitOfWork.JobCategoryRepository.GetJobCategoryByProjectionAsync(jc => new JobCategoryDetailResponseDto
        {
            Name = jc.Name,
            JobCategoryId = jc.Id,

            Jobs = jc.Jobs.Select(j => new JobListItemResponseDto
            {
                JobId = j.Id,
                Name = j.Name
            }).ToList()
        },
            jobCategoryId, cancellationToken);

        if (jobCategory == null)
            throw new NotFoundException($"job category was not found.");

        return jobCategory;
    }

    public async Task<List<JobCategoryResponseDto>> GetAllForSelectAsync(CancellationToken cancellationToken = default)
    {
        var cached = await _redisService.GetAsync<List<JobCategoryResponseDto>>(RedisCacheKeys.JobCategoriesSelect);

        if (cached is not null)
            return cached;

        var result = await _unitOfWork.JobCategoryRepository.GetAllForSelectAsync(jc => new JobCategoryResponseDto
        {
            JobCategoryId = jc.Id,
            Name = jc.Name
        }, cancellationToken);

        await _redisService.SetAsync(RedisCacheKeys.JobCategoriesSelect, result);

        return result;
    }

    #endregion

    #region Delete Methods

    public async Task SoftDeleteAsync(
        Guid jobCategoryId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var result = await _unitOfWork.JobCategoryRepository.SoftDeleteAsync(jobCategoryId, _currentUser.UserId, cancellationToken);

        if (!result)
            throw new ValidationException("Could not delete job category");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _redisService.RemoveAsync(RedisCacheKeys.JobCategoriesSelect);
    }

    #endregion
}
