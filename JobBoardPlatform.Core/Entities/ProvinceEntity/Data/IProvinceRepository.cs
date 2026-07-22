using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;

namespace JobBoardPlatform.Core.Entities.ProvinceEntity.Data;

public interface IProvinceRepository : IGenericRepository<Province>
{
    /// <summary>
    /// گرفتن کد استان 
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetProvinceCodeAsync(
        Guid provinceId,
        CancellationToken cancellationToken);
}
