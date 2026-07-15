using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.SkillDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
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

    public SkillService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> AddSkillsToUserAsync(Guid userId, List<Guid> skillsId)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        CheckSelfOrAdminPermission(userId, _currentUser);

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
        CheckAdminPermission(_currentUser);

        var isDuplicateSkill = await _unitOfWork.SkillRepository.IsDuplicateSkillAsync(name);

        if (isDuplicateSkill)
            throw new ConflictException($"the skill with name {name} is already exist");

        var skill = new Skill(name, _currentUser.UserId);

        await _unitOfWork.SkillRepository.AddAsync(skill);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<UserSkillDetailResult>> GetAllSkillsAsync(string text, PagingCommand pagingCommand)
    {
        var (skills, totalDataCount) = await _unitOfWork.SkillRepository.GetAllSkillsAsync(us => new UserSkillDetailResult
                                                                                (
                                                                                  us.Name
                                                                                ),
                                                                                 text, pagingCommand.PageNumber, pagingCommand.PageSize
                                                                                );

        return Pagination<UserSkillDetailResult>.GetPagination(skills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }

    public async Task<Pagination<UserSkillDetailResult>> GetUserSkillsAsync(Guid userId, PagingCommand pagingCommand)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var (userSkills, totalDataCount) = await _unitOfWork.UserSkillRepository.GetUserSkillsAsync(us => new UserSkillDetailResult
                                                                                (
                                                                                  us.Skill.Name
                                                                                ),
                                                                                userId, pagingCommand.PageNumber, pagingCommand.PageSize
                                                                                );

        return Pagination<UserSkillDetailResult>.GetPagination(userSkills,
                                                               pagingCommand.PageNumber,
                                                               pagingCommand.PageSize,
                                                               totalDataCount
                                                               );
    }

    #region Private Methods

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this skill.");
    }

    private void CheckAdminPermission(ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isAdminOrEmployer = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        if (!isAdminOrEmployer)
            throw new ForbiddenException("You do not have sufficient access to manage a skill.");
    }

    #endregion
}
