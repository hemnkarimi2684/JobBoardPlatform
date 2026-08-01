
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Data;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
    /// <summary>
    /// اپدیت پروفایل کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="updateProfile"></param>
    /// <returns></returns>
    Task<bool> UpdateProfileAsync(
        Guid userId,
        CancellationToken cancellationToken,
        UpdateUserProfile updateProfile);

    /// <summary>
    /// دریافت اطلاعات پروفایل کاربر 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetUserProfileByUserIdAsync<TResult>(
        Expression<Func<UserProfile, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// چک کردن اینکه کاربر پروفایل تکراری نداشته باشد
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateUserProfileAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت نام و نام خانوداگی کاربر مورد نظر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetUserFullNameByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// ایا کاربر مورد نظر پروفایل دارد یا نه
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsUserHasProfileAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت پروفایل کاربر توسط شناسه اش 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserProfile?> GetProfileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
