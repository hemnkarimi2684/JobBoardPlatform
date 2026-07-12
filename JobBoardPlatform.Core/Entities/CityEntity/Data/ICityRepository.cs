using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;

namespace JobBoardPlatform.Core.Entities.CityEntity.Data;

public interface ICityRepository : IGenericRepository<City>
{
    /// <summary>
    /// چک کردن اینکه ایای شهر وجود دارد یا نه
    /// </summary>
    /// <param name="cityId"></param>
    /// <returns></returns>
    Task<bool> IsCityExistAsync(Guid cityId);
}
