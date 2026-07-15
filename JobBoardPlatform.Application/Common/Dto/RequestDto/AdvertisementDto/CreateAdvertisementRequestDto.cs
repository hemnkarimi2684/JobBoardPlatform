using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public record CreateAdvertisementRequestDto(
                                            [Required(ErrorMessage = "the Description is required", AllowEmptyStrings = false)]
                                            [MinLength(100, ErrorMessage = "the Description characteers cannot be lower than 100")]
                                            [MaxLength(2000, ErrorMessage = "the Description characteers cannot be higher than 2000")]
                                            string Description,

                                            [Range(18, 55, ErrorMessage = "Minimum age must be between 18 and 55.")]
                                            int MinimumAge,

                                            [Range(18, 65, ErrorMessage = "Maximum age must be between 18 and 65.")]
                                            int MaximumAge,

                                            [Range(0, double.MaxValue, ErrorMessage = "the MinimumSalary must be in the range")]
                                            decimal MinimumSalary,

                                            [Range(0, double.MaxValue, ErrorMessage = "the MaximumSalary must be in the range")]
                                            decimal MaximumSalary,

                                            [Range(0, 50, ErrorMessage = "the ExperienceLevel must be in the range")]
                                            int ExperienceLevel,

                                            [Required(ErrorMessage = "the DescriptiCollaborationTypeon is required", AllowEmptyStrings = false)]
                                            [MinLength(0, ErrorMessage = "the CollaborationType characteers cannot be lower than 0")]
                                            [MaxLength(25, ErrorMessage = "the CollaborationType characteers cannot be higher than 25")]
                                            string CollaborationType,

                                            [Required(ErrorMessage = "the JobId is required", AllowEmptyStrings = false)]
                                            Guid JobId,

                                            [Required(ErrorMessage = "the CityId is required", AllowEmptyStrings = false)]
                                            Guid CityId,

                                            [Required(ErrorMessage = "the CompanyId is required", AllowEmptyStrings = false)]
                                            Guid CompanyId,

                                            [Required(ErrorMessage = "the SkillsId is required")]
                                            List<Guid> SkillsId);
