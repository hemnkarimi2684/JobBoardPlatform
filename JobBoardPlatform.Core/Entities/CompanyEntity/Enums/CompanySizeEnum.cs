using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

/// <summary>
/// ا اندازه سازمان
/// </summary>
public enum CompanySizeEnum
{
    /// <summary>
    /// کمتر از 10 نفر
    /// </summary>
    [Display(Name = "Fewer than 10")]
    FewerThan10People = 1,

    /// <summary>
    /// بین 11 تا 50
    /// </summary>
    [Display(Name = "11 - 50")]
    Between11To50,

    /// <summary>
    /// بین 51 تا 200
    /// </summary>
    [Display(Name = "51 - 200")]
    Between51To200,

    /// <summary>
    /// بین 201 تا 500
    /// </summary>
    [Display(Name = "201 - 500")]
    Between201To500,

    /// <summary>
    /// بین 501 تا 1000
    /// </summary>
    [Display(Name = "501 - 1000")]
    Between501To1000,

    /// <summary>
    /// بین 1001 تا 5000
    /// </summary>
    [Display(Name = "1001 - 5000")]
    Between1001To5000,

    /// <summary>
    /// بیشتر از 5000
    /// </summary>
    [Display(Name = "More than 5000")]
    MoreThan5000
}
