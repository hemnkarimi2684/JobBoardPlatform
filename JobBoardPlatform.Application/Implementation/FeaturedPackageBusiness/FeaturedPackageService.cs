using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.FeaturedPackageDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.FeaturedPackageInterface;
using JobBoardPlatform.Core.Entities.Common.Data;

namespace JobBoardPlatform.Application.Implementation.FeaturedPackageBusiness;

public class FeaturedPackageService : IFeaturedPackageService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public FeaturedPackageService(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<List<FeaturedPackageResponseDto>> GetFeaturedPackagesAsync(
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.FeaturedPackageRepository.GetAllPackagesAsync(
            package => new FeaturedPackageResponseDto
            {
                PackageId = package.Id,
                DurationInDays = package.DurationInDays,
                Price = package.Price
            },
            cancellationToken);
    }

    public async Task UpdateFeaturedPackagePriceAsync(
        Guid packageId,
        decimal price,
        CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.FeaturedPackageRepository.GetByIdAsync(packageId, cancellationToken, true);

        if (package is null)
            throw new NotFoundException("Featured package was not found.");

        package.UpdatePrice(price, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
