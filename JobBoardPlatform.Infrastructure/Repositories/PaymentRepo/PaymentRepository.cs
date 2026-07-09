using JobBoardPlatform.Core.Entities.PaymentEntity.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.PaymentRepo;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
