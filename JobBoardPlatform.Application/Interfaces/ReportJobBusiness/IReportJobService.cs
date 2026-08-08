namespace JobBoardPlatform.Application.Interfaces.ReportJobBusiness;

public interface IReportJobService
{
    /// <summary>
    /// منقضی کردن اگهی ایی که از تاریخ ویژه بودنشون گذشته 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DemoteAdvertisementsAsync(CancellationToken cancellationToken = default);
}
