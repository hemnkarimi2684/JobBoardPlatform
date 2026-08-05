using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Company;

public class CompanyIndexViewModel : Pagination<CompanyDetailResponseDto>
{
    public static CompanyIndexViewModel FromResponseDto(Pagination<CompanyDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
