using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.PaymentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.PaymentInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Enums;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.PaymentBusiness;

public class PaymentService : IPaymentService
{
    /// <summary>
    /// مدت زمان های مجاز برای ویژه بودن اگهی قیمت هر بسته از جدول FeaturedPackages خوانده میشود
    /// </summary>
    private static readonly int[] AllowedFeaturedDurations = { 7, 15, 30 };

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public PaymentService(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    public async Task<Guid> CreateFeaturedPaymentAsync(
        CreateFeaturedPaymentRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(createCommand.AdvertisementId, cancellationToken);

        if (advertisementOwnerId is null)
            throw new NotFoundException("Advertisement was not found.");

        _accessControlService.EnsureOwnerEmployer(advertisementOwnerId.Value, _currentUser);

        if (!AllowedFeaturedDurations.Contains(createCommand.DurationInDays))
            throw new ValidationException("Allowed featured durations are 7, 15 or 30 days.");

        var package = await _unitOfWork.FeaturedPackageRepository.GetByDurationAsync(createCommand.DurationInDays, cancellationToken);

        if (package is null)
            throw new ValidationException("The price for this featured duration has not been set yet.");

        var payment = new Payment(package.Price, createCommand.DurationInDays, PaymentStatus.Pending, createCommand.AdvertisementId, _currentUser.UserId, _currentUser.UserId);

        await _unitOfWork.PaymentRepository.AddAsync(payment, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }

    #region Get Methods 

    public async Task<List<FeaturedOptionResponseDto>> GetFeaturedOptionsAsync(
    CancellationToken cancellationToken)
    {
        var packages = await _unitOfWork.FeaturedPackageRepository.GetAllPackagesAsync(
            package => new FeaturedOptionResponseDto
            {
                DurationInDays = package.DurationInDays,
                Price = package.Price
            },
            cancellationToken);

        return packages;
    }

    public async Task<PaymentResponseDto> GetPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        var paymentDetail = await _unitOfWork.PaymentRepository.GetPaymentDetailAsync(paymentId, cancellationToken);

        if (paymentDetail is null)
            throw new NotFoundException("Payment was not found.");

        _accessControlService.EnsureOwnerEmployer(paymentDetail.UserId, _currentUser);

        return PaymentResponseDto.MapToResponseDto(paymentDetail);
    }

    #endregion

    #region Update Methods

    public async Task ConfirmSuccessfulPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId, cancellationToken, true);

            if (payment is null)
                throw new NotFoundException("Payment was not found.");

            _accessControlService.EnsureOwnerEmployer(payment.UserId, _currentUser);

            if (payment.Status != PaymentStatus.Pending)
                throw new ConflictException("The payment has already been processed.");

            if (payment.DurationInDays < 1)
                throw new ValidationException("The payment has no featured duration.");

            var advertisement = await _unitOfWork.AdvertisementRepository.GetByIdAsync(payment.AdvertisementId, cancellationToken, true);

            if (advertisement is null)
                throw new NotFoundException("Advertisement was not found.");

            var now = DateTime.UtcNow;

            var baseDate = advertisement.IsFeatured && advertisement.FeaturedUntil.HasValue && advertisement.FeaturedUntil.Value > now
                ? advertisement.FeaturedUntil.Value
                : now;

            advertisement.UpdateFeatured(true, baseDate.AddDays(payment.DurationInDays), _currentUser.UserId);

            payment.UpdatePaymentStatus(PaymentStatus.Success, _currentUser.UserId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task ConfirmFailedPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        await UpdatePaymentStatusAsync(paymentId, PaymentStatus.Failed, cancellationToken);
    }

    public async Task CancelPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        await UpdatePaymentStatusAsync(paymentId, PaymentStatus.Cancelled, cancellationToken);
    }

    #endregion

    #region Private Methods

    private async Task UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId, cancellationToken, true);

        if (payment is null)
            throw new NotFoundException("Payment was not found.");

        _accessControlService.EnsureOwnerEmployer(payment.UserId, _currentUser);

        if (payment.Status != PaymentStatus.Pending)
            throw new ConflictException("The payment has already been processed.");

        payment.UpdatePaymentStatus(status, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
