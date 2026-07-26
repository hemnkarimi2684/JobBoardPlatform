using JobBoardPlatform.Core.Entities.UserProfileEntity.Data;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.UserProfileRepo;

public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserProfile?> GetProfileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);
    }

    public async Task<string?> GetUserFullNameByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(up => up.UserId == userId)
                          .Select(up => up.FirstName + " " + up.LastName)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetUserProfileByUserIdAsync<TResult>(
        Expression<Func<UserProfile, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(up => up.UserId == userId)
                           .Select(projection)
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateUserProfileAsync(
        Guid userId,
        CancellationToken cancellationToken) => await AnyAsync(up => up.UserId == userId, cancellationToken);

    public async Task<bool> IsUserHasProfileAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(up => up.UserId == userId, cancellationToken);
    }

    public async Task<bool> UpdateProfileAsync(
        Guid userId,
        CancellationToken cancellationToken,
        UpdateUserProfile updateProfile)
    {
        var userProfile = await Entities.FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);

        if (userProfile == null)
            return false;

        userProfile.UpdateUserInfo(updateProfile);

        return true;
    }
}
