using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.EducationDetailBusiness;

public class EducationDetailService : IEducationDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public EducationDetailService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateEducationDetailAsync(CreateEducationDetailCommand createCommand)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!isUserExist)
            throw new NotFoundException($"user with id {createCommand.UserId} was not found");

        var educationDetail = new EducationDetail(
                                           createCommand.CertificateDegreeName,
                                           createCommand.Major,
                                           createCommand.University,
                                           createCommand.StartDate,
                                           createCommand.CompletionDate,
                                           createCommand.Percentage,
                                           createCommand.IsCurrentlyStudying,
                                           createCommand.UserId,
                                           _currentUser.UserId
                                           );

        await _unitOfWork.EducationDetailRepository.AddAsync(educationDetail);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<UserEducationDetailResult>> GetUserEducationDetailsAsync(Guid userId,PagingCommand pagingCommand)
    {
        //var userEducationDetails = await _unitOfWork.EducationDetailRepository
        //                                                               .GetUserEducationDetailsAsync(ed =>
        //                                                                                   new UserEducationDetailResult
        //                                                                                   (
        //                                                                                       ed.CertificateDegreeName,
        //                                                                                       ed.Major,
        //                                                                                       ed.University

        //                                                                                   ),
        //                                                                                   userId,
        //                                                                                   pagingCommand.PageNumber,
        //                                                                                   pagingCommand.PageSize);

        //return userEducationDetails;

        throw new NotImplementedException();
    }

    public Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetailCommand updateCommand)
    {
        throw new NotImplementedException();
    }
}
