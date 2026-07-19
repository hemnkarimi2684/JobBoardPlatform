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

    public async Task<string?> GetUserFullNameByUserIdAsync(Guid userId)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(up => up.UserId == userId && !up.IsDeleted && up.DeletedAt == null)
                          .Select(up => up.FirstName + " " + up.LastName)
                          .FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetUserProfileInfoAsync<TResult>(Expression<Func<UserProfile, TResult>> projection, Guid userId)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(up => up.UserId == userId && !up.IsDeleted && up.DeletedAt == null)
                           .Select(projection)
                           .FirstOrDefaultAsync();
    }

    public async Task<bool> IsDuplicateUserProfileAsync(Guid userId) => await AnyAsync(up => up.UserId == userId);

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserProfile updateProfile)
    {
        var userProfile = await Entities.FirstOrDefaultAsync(up => up.UserId == userId && !up.IsDeleted && up.DeletedAt == null);

        if (userProfile == null)
            return false;

        userProfile.UpdateUserInfo(updateProfile);

        return true;
    }
}
