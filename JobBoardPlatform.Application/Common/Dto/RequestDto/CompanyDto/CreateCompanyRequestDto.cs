using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;

public record CreateCompanyRequestDto(

                                       [Required(ErrorMessage = "the Name is required", AllowEmptyStrings = false)]
                                       [MinLength(2, ErrorMessage = "the Name characteers cannot be lower than 2")]
                                       [MaxLength(120, ErrorMessage = "the Name characteers cannot be higher than 120")]
                                       string Name,

                                       [Required(ErrorMessage = "the YearOfEstablishment is required", AllowEmptyStrings = false)]
                                       [DataType(DataType.Date)]
                                       DateTime YearOfEstablishment,

                                       [Required(ErrorMessage = "the Industry is required", AllowEmptyStrings = false)]
                                       [MinLength(2, ErrorMessage = "the Industry characteers cannot be lower than 2")]
                                       [MaxLength(200, ErrorMessage = "the Industry characteers cannot be higher than 200")]
                                       string Industry,

                                       [Required(ErrorMessage = "the AboutUs is required", AllowEmptyStrings = false)]
                                       [MinLength(50, ErrorMessage = "the AboutUs characteers cannot be lower than 50")]
                                       [MaxLength(1500, ErrorMessage = "the AboutUs characteers cannot be higher than 1500")]
                                       string AboutUs,

                                       [Required(ErrorMessage = "the WebSiteAddress is required", AllowEmptyStrings = false)]
                                       [MinLength(2, ErrorMessage = "the WebSiteAddress characteers cannot be lower than 2")]
                                       [MaxLength(100, ErrorMessage = "the WebSiteAddress characteers cannot be higher than 100")]
                                       string WebSiteAddress,

                                       [Required(ErrorMessage = "the OwnershipType is required", AllowEmptyStrings = false)]
                                       [MinLength(1, ErrorMessage = "the OwnershipType characteers cannot be lower than 1")]
                                       [MaxLength(25, ErrorMessage = "the OwnershipType characteers cannot be higher than 25")]
                                       string OwnershipType,

                                       [Required(ErrorMessage = "the OwnedByUserId is required", AllowEmptyStrings = false)]
                                       Guid OwnedByUserId,

                                       [Required(ErrorMessage = "the CompanySize is required", AllowEmptyStrings = false)]
                                       [MinLength(1, ErrorMessage = "the CompanySize characteers cannot be lower than 1")]
                                       [MaxLength(25, ErrorMessage = "the CompanySize characteers cannot be higher than 25")]
                                       string CompanySize,

                                       [Required(ErrorMessage = "the CityId is required", AllowEmptyStrings = false)]
                                       Guid CityId,

                                       [Required(ErrorMessage = "the Location is required", AllowEmptyStrings = false)]
                                       [MinLength(1, ErrorMessage = "the Location characteers cannot be lower than 5")]
                                       [MaxLength(2000, ErrorMessage = "the Location characteers cannot be higher than 200")]
                                       string Location,

                                       [Required(ErrorMessage = "the ActivityType is required", AllowEmptyStrings = false)]
                                       [MinLength(2, ErrorMessage = "the ActivityType characteers cannot be lower than 100")]
                                       [MaxLength(120, ErrorMessage = "the ActivityType characteers cannot be higher than 2000")]
                                       string? ActivityType = null
);


