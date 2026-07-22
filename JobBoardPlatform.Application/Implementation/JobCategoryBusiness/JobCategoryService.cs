using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.JobCategoryBusiness;

public class JobCategoryService : IJobCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public JobCategoryService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task CreateJobCategoryAsync(CreateJobCategoryRequestDto jobCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var isDuplicateName = await _unitOfWork.JobCategoryRepository.IsDuplicateNameAsync(jobCategoryRequestDto.Name, cancellationToken);

        if (isDuplicateName)
            throw new ConflictException($"the job category with name {jobCategoryRequestDto.Name} is already exist");

        var jobCategory = new JobCategory(jobCategoryRequestDto.Name, _currentUser.UserId);

        await _unitOfWork.JobCategoryRepository.AddAsync(jobCategory, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<JobCategoryResponseDto>> GetAllJobCategoriesAsync(string text, PagingRequestDto pagingCommand, CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.JobCategoryRepository.GetAllJobCategoriesAsync(jc => new JobCategoryResponseDto
        {
            JobCategoryId = jc.Id,
            Name = jc.Name
        },
          text,
          cancellationToken,
          pagingCommand.PageNumber,
          pagingCommand.PageSize);

        return Pagination<JobCategoryResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    public async Task<JobCategoryDetailResponseDto> GetJobCategoryByIdAsync(Guid jobCategoryId, CancellationToken cancellationToken = default)
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
            throw new NotFoundException($"the job category with id {jobCategoryId} was not found.");

        return jobCategory;
    }

    #endregion
}
