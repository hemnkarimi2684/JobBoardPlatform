using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
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

    public async Task<bool> AddSkillsToUserAsync(Guid userId, List<Guid> skillsId)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        if (skillsId is not null && skillsId.Any())
        {
            foreach (var skillId in skillsId.Distinct())
            {
                var userSkill = new UserSkill(userId, skillId, _currentUser.UserId);

                await _unitOfWork.UserSkillRepository.AddAsync(userSkill);
            }
        }

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateSkillAsync(string name)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var isDuplicateSkill = await _unitOfWork.SkillRepository.IsDuplicateSkillAsync(name);

        if (isDuplicateSkill)
            throw new ConflictException($"the skill with name {name} is already exist");

        var skill = new Skill(name, _currentUser.UserId);

        await _unitOfWork.SkillRepository.AddAsync(skill);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<SkillDetailResponseDto>> GetAllSkillsAsync(string text, PagingRequestDto pagingCommand)
    {
        var (skills, totalDataCount) = await _unitOfWork.SkillRepository.GetAllSkillsAsync(us => new SkillDetailResponseDto
                                                                                (
                                                                                  us.Id,
                                                                                  us.Name
                                                                                ),
                                                                                 text, pagingCommand.PageNumber, pagingCommand.PageSize
                                                                                );

        return Pagination<SkillDetailResponseDto>.GetPagination(skills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }

    public async Task<Pagination<UserSkillDetailResponseDto>> GetUserSkillsAsync(Guid userId, PagingRequestDto pagingCommand)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (userSkills, totalDataCount) = await _unitOfWork.UserSkillRepository.GetUserSkillsAsync(us => new UserSkillDetailResponseDto
                                                                                (
                                                                                  us.Id,
                                                                                  us.Skill.Name,
                                                                                  us.UserId
                                                                                ),
                                                                                userId, pagingCommand.PageNumber, pagingCommand.PageSize
                                                                                );

        return Pagination<UserSkillDetailResponseDto>.GetPagination(userSkills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }
}
