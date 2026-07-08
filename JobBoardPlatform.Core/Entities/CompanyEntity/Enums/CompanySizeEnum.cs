namespace JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

/// <summary>
/// اینام اندازه سازمان
/// </summary>
public enum CompanySizeEnum
{
    /// <summary>
    /// کمتر از 10 نفر
    /// </summary>
    FewerThan10People = 1,

    /// <summary>
    /// بین 11 تا 50
    /// </summary>
    Between11To50,

    /// <summary>
    /// بین 51 تا 200
    /// </summary>
    Between51To200,

    /// <summary>
    /// بین 201 تا 500
    /// </summary>
    Between201To500,

    /// <summary>
    /// بین 501 تا 1000
    /// </summary>
    Between501To1000,

    /// <summary>
    /// بین 1001 تا 5000
    /// </summary>
    Between1001To5000,

    /// <summary>
    /// بیشتر از 5000
    /// </summary>
    MoreThan5000

}
