using JobBoardPlatform.Application.Implementation.CompanyBusiness;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;

public record UpdateCompanyInfoRequestDto(

                                         [MinLength(2, ErrorMessage = "the Name characteers cannot be lower than 2")]
                                         [MaxLength(120, ErrorMessage = "the Name characteers cannot be higher than 120")]
                                         string? Name,

                                         [DataType(DataType.Date)]
                                         DateTime? YearOfEstablishment,

                                         [MinLength(2, ErrorMessage = "the Industry characteers cannot be lower than 2")]
                                         [MaxLength(200, ErrorMessage = "the Industry characteers cannot be higher than 200")]
                                         string? Industry,

                                         [MinLength(50, ErrorMessage = "the AboutUs characteers cannot be lower than 50")]
                                         [MaxLength(1500, ErrorMessage = "the AboutUs characteers cannot be higher than 1500")]
                                         string? AboutUs,

                                         [MinLength(2, ErrorMessage = "the WebSiteAddress characteers cannot be lower than 2")]
                                         [MaxLength(100, ErrorMessage = "the WebSiteAddress characteers cannot be higher than 100")]
                                         string? WebSiteAddress,

                                         [MinLength(1, ErrorMessage = "the OwnershipType characteers cannot be lower than 1")]
                                         [MaxLength(25, ErrorMessage = "the OwnershipType characteers cannot be higher than 25")]
                                         string? OwnershipType,

                                         [MinLength(1, ErrorMessage = "the CompanySize characteers cannot be lower than 1")]
                                         [MaxLength(25, ErrorMessage = "the CompanySize characteers cannot be higher than 25")]
                                         string? CompanySize,

                                         [MinLength(2, ErrorMessage = "the ActivityType characteers cannot be lower than 100")]
                                         [MaxLength(120, ErrorMessage = "the ActivityType characteers cannot be higher than 2000")]
                                         string? ActivityType
);
