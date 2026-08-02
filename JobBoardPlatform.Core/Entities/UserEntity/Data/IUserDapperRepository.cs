using JobBoardPlatform.Core.Entities.UserEntity.ReadModels;

namespace JobBoardPlatform.Core.Entities.UserEntity.Data;

public interface IUserDapperRepository
{
    /// <summary>
    /// دریافت تمام کارفرما مورد تایید 
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(IEnumerable<EmployerDetailReadModel> Items, int TotalDataCount)> GetApprovedEmployersAsync(
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// دریافت تمام کارجو ها 
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(IEnumerable<JobSeekerDetailReadModel> Items, int TotalDataCount)> GetJobSeekersAsync(
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// دریافت تمام کارفرما های در حال انتظار 
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(IEnumerable<EmployerDetailReadModel> Items, int totalDataCount)> GetUnapprovedEmployersAsync(
        int pageNumber = 1,
        int pageSize = 10);
}
