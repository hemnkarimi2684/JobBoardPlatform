
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Data;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
    /// <summary>
    /// اپدیت پروفایل کاربر
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateProfileAsync(Guid userId, UpdateUserProfile updateProfile);

    /// <summary>
    /// دریافت اطلاعات پروفایل کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<TResult?> GetUserProfileInfoAsync<TResult>(Expression<Func<UserProfile, TResult>> projection, Guid userId);

    /// <summary>
    /// چک کردن اینکه کاربر پروفایل تکراری نداشته باشد
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateUserProfileAsync(Guid userId);

    /// <summary>
    /// دریافت نام و نام خانوداگی کاربر مورد نظر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string?> GetUserFullNameByUserIdAsync(Guid userId);

    /// <summary>
    /// ایا کاربر ورد نظر پروفایل دارد یا نه
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsUserHasProfileAsync(Guid userId);
}
