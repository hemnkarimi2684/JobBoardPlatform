using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.SkillBusiness;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public SkillService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Craete Methods

    public async Task<bool> AddSkillsToUserAsync(
        Guid userId,
        List<Guid> skillsId,
        CancellationToken cancellationToken = default)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        _accessControlService.EnsureApplicant(userId, _currentUser);

        if (skillsId is not null && skillsId.Any())
        {
            foreach (var skillId in skillsId.Distinct())
            {
                var isDuplicateSkillForUser = await _unitOfWork.UserSkillRepository.IsDuplicateSkillForUserAsync(userId, skillId,cancellationToken);

                if (isDuplicateSkillForUser)
                    throw new ConflictException($"the user with id {userId} already has skill with id {skillId}");

                var userSkill = new UserSkill(userId, skillId, _currentUser.UserId);

                await _unitOfWork.UserSkillRepository.AddAsync(userSkill, cancellationToken);
            }
        }

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> CreateSkillAsync(
        CreateSkillRequestDto skillRequestDto,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var isDuplicateSkill = await _unitOfWork.SkillRepository.IsDuplicateSkillAsync(skillRequestDto.Name, cancellationToken);

        if (isDuplicateSkill)
            throw new ConflictException($"the skill with name {skillRequestDto.Name} is already exist");

        var skill = new Skill(skillRequestDto.Name, _currentUser.UserId);

        await _unitOfWork.SkillRepository.AddAsync(skill, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<SkillDetailResponseDto>> GetAllSkillsAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (skills, totalDataCount) = await _unitOfWork.SkillRepository.GetAllSkillsAsync(us => new SkillDetailResponseDto
        {
            SkillId = us.Id,
            SkillName = us.Name
        },
          textRequestDto.Text,
          cancellationToken,
          pagingCommand.PageNumber,
          pagingCommand.PageSize
        );

        return Pagination<SkillDetailResponseDto>.GetPagination(skills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }

    public async Task<SkillDetailResponseDto> GetSkillByIdAsync(
        Guid skillId,
        CancellationToken cancellationToken = default)
    {
        var skill = await _unitOfWork.SkillRepository.GetByIdAsync(skillId, cancellationToken);

        if (skill is null)
            throw new NotFoundException($"the skill with id {skillId} was not found.");

        return new SkillDetailResponseDto
        {
            SkillId = skillId,
            SkillName = skill.Name,
        };
    }

    public async Task<Pagination<UserSkillResponseDto>> GetUserSkillsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (userSkills, totalDataCount) = await _unitOfWork.UserSkillRepository.GetUserSkillsAsync(us => new UserSkillResponseDto
        {
            SkillId = us.Id,
            SkillName = us.Skill.Name,
            UserId = us.UserId
        },
        userId,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);

        return Pagination<UserSkillResponseDto>.GetPagination(userSkills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }

    #endregion
}
