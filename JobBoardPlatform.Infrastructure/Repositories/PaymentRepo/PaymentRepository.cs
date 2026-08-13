using JobBoardPlatform.Core.Entities.PaymentEntity.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Dto;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.PaymentRepo;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaymentDetail?> GetPaymentDetailAsync(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        return await Entities
                       .AsNoTracking()
                       .Where(p => p.Id == paymentId)
                       .Select(p => new PaymentDetail
                       {
                           PaymentId = p.Id,
                           Amount = p.Amount,
                           DurationInDays = p.DurationInDays,
                           Status = p.Status,
                           AdvertisementId = p.AdvertisementId,
                           UserId = p.UserId,
                           AdvertisementTitle = p.Advertisement.Job.Name,
                           CompanyName = p.Advertisement.Company.Name
                       })
                       .FirstOrDefaultAsync(cancellationToken);
    }
}
