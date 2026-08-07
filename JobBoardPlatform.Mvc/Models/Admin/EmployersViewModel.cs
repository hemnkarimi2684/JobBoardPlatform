using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class EmployersViewModel
{
    public List<EmployerDetailResponseDto> ApprovedEmployers { get; set; } = new();

    public List<EmployerDetailResponseDto> UnapprovedEmployers { get; set; } = new();

    public int ApprovedCurrentPage { get; set; }

    public int ApprovedTotalPages { get; set; }

    public int UnapprovedCurrentPage { get; set; }

    public int UnapprovedTotalPages { get; set; }

    public static EmployersViewModel FromResponseDto(
        Pagination<EmployerDetailResponseDto> approved,
        Pagination<EmployerDetailResponseDto> unapproved)
    {
        return new EmployersViewModel
        {
            ApprovedEmployers = approved.Data ?? new(),

            ApprovedCurrentPage = approved.PageNumber,

            ApprovedTotalPages = approved.TotalPageCount,

            UnapprovedEmployers = unapproved.Data ?? new(),

            UnapprovedCurrentPage = unapproved.PageNumber,

            UnapprovedTotalPages = unapproved.TotalPageCount
        };
    }
}