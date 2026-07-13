namespace JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Command;

public record UpdateAdvertisementCommand(
string? Description,
int? MinimumAge,
int? MaximumAge,
decimal? MinimumSalary,
decimal? MaximumSalary,
int? ExperienceLevel,
string? CollaborationType);  
