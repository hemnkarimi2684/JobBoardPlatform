
namespace JobBoardPlatform.Core.Entities.UserEntity.Data;

public interface IUserRepository
{
    /// <summary>
    /// چک کردن اینکه ایا کاربر وجود دارد یا نه 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsUserExistAsync(Guid userId);
}
