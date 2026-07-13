using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Command;
using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Result;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Implementation.JobApplicationBusiness;

public class JobApplicationService : IJobApplicationService
{
    public Task<bool> CreateJobApplicationAsync(CreateJobApplicationCommand createCommand)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<JobApplicationInfoResult>> GetAdvertisementJobApplicationsAsync(Guid advertisementId)
    {
        throw new NotImplementedException();
    }

    public Task<JobApplicationInfoResult> GetJobApplicationByIdAsync(Guid jobApplicationId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateJobApplicationStatusAsync(Guid jobApplicationId, string statusName)
    {
        throw new NotImplementedException();
    }
}
