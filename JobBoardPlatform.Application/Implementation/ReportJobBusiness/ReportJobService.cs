using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.ReportJobBusiness;
using JobBoardPlatform.Core.Entities.Common.Data;
using Microsoft.Extensions.Logging;

namespace JobBoardPlatform.Application.Implementation.ReportJobBusiness;

public class ReportJobService : IReportJobService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<ReportJobService> _logger;

    public ReportJobService(IUnitOfWork unitOfWork, ILogger<ReportJobService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task DemoteAdvertisementsAsync(CancellationToken cancellationToken = default)
    {
        await _unitOfWork.AdvertisementRepository.DemoteAdvertisementsAsync();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Expired featured advertisements have been successfully demoted at {DateTime}.", DateTime.UtcNow);
    }
}
