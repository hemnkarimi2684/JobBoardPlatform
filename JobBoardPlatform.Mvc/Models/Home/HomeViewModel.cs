using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Home;

public class HomeViewModel
{
    public string? SearchTerm { get; set; }

    public Guid? JobCategoryId { get; set; }

    public CollaborationType? CollaborationType { get; set; }

    public decimal? MinimumSalary { get; set; }

    public decimal? MaximumSalary { get; set; }

    public List<AdvertisementDetailResponseDto> Data { get; set; } = new();

    public int PageNumber { get; set; } = 1;

    public int TotalPageCount { get; set; } = 1;

    public bool HasActiveCriteria
        => !string.IsNullOrWhiteSpace(SearchTerm)
        || JobCategoryId.HasValue
        || CollaborationType.HasValue
        || MinimumSalary.HasValue
        || MaximumSalary.HasValue;

    public static HomeViewModel FromResponseDto(
        Pagination<AdvertisementDetailResponseDto> source,
        string? searchTerm,
        Guid? jobCategoryId,
        CollaborationType? collaborationType,
        decimal? minimumSalary,
        decimal? maximumSalary)
        => new()
        {
            SearchTerm = searchTerm,
            JobCategoryId = jobCategoryId,
            CollaborationType = collaborationType,
            MinimumSalary = minimumSalary,
            MaximumSalary = maximumSalary,
            Data = source.Data,
            PageNumber = source.PageNumber,
            TotalPageCount = source.TotalPageCount
        };
}
