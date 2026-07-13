namespace JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Result;

public record JobApplicationInfoResult(
    string FullName,
    string PhoneNumber,
    string Gender,
    string CityName,
    DateTime BirthDate
    
    );

