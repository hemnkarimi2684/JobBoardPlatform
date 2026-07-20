using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;

namespace JobBoardPlatform.Core.Entities.JobEntity.Data;

public interface IJobRepository : IGenericRepository<Job>
{
    /// <summary>
    /// ایا این کار وجود داره یا نه
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    Task<bool> IsJobExistAsync(Guid jobId);
}
