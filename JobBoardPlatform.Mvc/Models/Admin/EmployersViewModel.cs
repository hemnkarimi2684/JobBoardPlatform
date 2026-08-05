using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class EmployersViewModel
{
    public List<EmployerDetailResponseDto> ApprovedEmployers { get; set; } = new();

    public List<EmployerDetailResponseDto> UnapprovedEmployers { get; set; } = new();

    public static EmployersViewModel FromResponseDto(
        List<EmployerDetailResponseDto> approved,
        List<EmployerDetailResponseDto> unapproved)
        => new()
        {
            ApprovedEmployers = approved,
            UnapprovedEmployers = unapproved
        };
}
