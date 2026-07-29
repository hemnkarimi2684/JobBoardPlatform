
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.UserEntity.Data;

public interface IUserRepository
{
    /// <summary>
    /// چک کردن اینکه ایا کاربر وجود دارد یا نه
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsUserExistAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت کاربر توسط شماره تلفن
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User?> FindByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken);

    /// <summary>
    /// چک کردن اینکه ایا این ایمیل یا شمار تلفن تکراری است یا نه 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateEmailOrPhoneNumberAsync(
        string email,
        string phoneNumber,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت اطلاعات رزومه کاربر 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetResumeDetailAsync<TResult>(
        Expression<Func<User, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت ایمیل کاربر مورد نظر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetUserEmailAsync(Guid userId, CancellationToken cancellationToken);
}
