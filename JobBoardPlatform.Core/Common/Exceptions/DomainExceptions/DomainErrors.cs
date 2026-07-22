using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;

/// <summary>
/// تمام ارور های به لایه دامین
/// </summary>
public static class DomainErrors
{
    #region User Errors
    public static Error EmailIsRequired => new Error("email is required", "Email_Is_Required");
    public static Error EmailInvalidFormat => new Error("imvalid email format", "Email_Invalid_Format");
    public static Error PhoneNumberIsRequired => new Error("phone number is required", "PhoneNumber_Is_Required");
    public static Error PhoneNumberInvalidFormat => new Error("invalid phone number format", "PhoneNumber_Invalid_Format");
    public static Error PasswordHashIsRequired => new Error("password hash is required", "PasswordHash_Is_Required");
    #endregion

    #region UserProfile Errors
    public static Error FirstNameIsRequired => new Error("user first name is required", "User_FirstName_Is_Required");
    public static Error LastNameIsRequired => new Error("user last name is required", "User_LastName_Is_Required");
    public static Error FirstNameInvalidLength => new Error("user first name length must be between 3 and 100 characters", "User_FirstName_Invalid_Length");
    public static Error LastNameInvalidLength => new Error("user last name length must be between 3 and 100 characters", "User_LastName_Invalid_Length");
    public static Error BioIsRequired => new Error("user bio is required", "User_Bio_Is_Required");
    public static Error AddressIsRequired => new Error("user address is required", "User_Address_Is_Required");
    public static Error BioInvalidLength => new Error("user bio length must be between 3 and 250 characters", "User_Bio_Invalid_Length");
    public static Error AddressInvalidLength => new Error("user address length must be between 3 and 250 characters", "User_Address_Invalid_Length");
    public static Error UserMustBeAtLeast18YearsOld => new Error("user must be at least 18 years old", "User_Age_AtLeast18");
    public static Error SubjectIsRequired => new Error("user first name is required", "Subject_Is_Required");
    public static Error UserProfileCityIdIsRequired => new Error("user first name is required", "User_CityId_Is_Required");
    public static Error UserProfileUserIdIsRequired => new Error("user first name is required", "User_UserId_Is_Required");
    public static Error SubjectAllCharactersNotLetter => new Error("the subject all characters must be letter", "Subject_Invalid_Format");
    public static Error SubjectAllCharactersNotDigit => new Error("the subject all characters must be digit", "Subject_Invalid_Format");
    #endregion

    #region Province Errors
    public static Error ProvinceCodeInvalidRange => new Error("province code cannot be negative ", "Province_Code_Invalid_Range");
    public static Error ProvinceNameIsRequired => new Error("province name is required", "Province_Name_Is_Required");
    public static Error ProvinceNameInvalidLength => new Error("province name length must be between 2 and 100 characters", "Province_Name_Invalid_Length");
    #endregion

    #region City Errors
    public static Error CityNameInvalidLength => new Error("city name length must be between 2 and 100 characters", "City_Name_Invalid_Length");
    public static Error CityNameIsRequired => new Error("city name is required", "City_Name_Is_Required");
    public static Error CityProvinceIdIsRequired => new Error("City ProvinceId is required", "City_ProvinceId_Is_Required");
    public static Error CityCodeInvalidRange => new Error("city code cannot be negative ", "City_Code_Invalid_Range");
    public static Error CityProvinceCodeInvalidRange => new Error("province code of city cannot be negative ", "City_ProvinceCode_Invalid_Range");
    #endregion

    #region Skill Errors
    public static Error SkillNameInvalidLength => new Error("skill name length must be between 2 and 100 characters", "Skill_Name_Invalid_Length");
    public static Error SkillNameIsRequired => new Error("skill name is required", "Skill_Name_Is_Required");
    #endregion

    #region Job Errors
    public static Error JobNameInvalidLength => new Error("job name length must be between 2 and 100 characters", "Job_Name_Invalid_Length");
    public static Error JobNameIsRequired => new Error("job name is required", "Job_Name_Is_Required");
    public static Error JobCategoryIdIsRequired => new Error("job job category id is required", "Job_JobCategoryId_Is_Required");
    #endregion

    #region Company Errors
    public static Error CompanyNameIsRequired => new Error("company name is required", "Company_Name_Is_Required");
    public static Error CompanyOwnedByUserIdIsRequired => new Error("company OwnedByUserId is required", "Company_OwnedByUserId_Is_Required");
    public static Error CompanyNameInvalidLength => new Error("company name length must be between 2 and 120 characters", "Company_Name_Invalid_Length");
    public static Error CompanyIndustryIsRequired => new Error("company industry is required", "Company_Industry_Is_Required");
    public static Error CompanyIndustryInvalidLength => new Error("company industry length must be between 2 and 200 characters", "Company_Industry_Invalid_Length");
    public static Error CompanyYearOfEstablishmentInvalidRange => new Error("At least one year must have elapsed since the company's establishment.", "Company_YearOfEstablishment_Invalid_Range");
    public static Error CompanyAboutUsIsRequired => new Error("company about us is required", "Company_AboutUs_Is_Required");
    public static Error CompanyAboutUsInvalidLength => new Error("company about us length must be between 50 and 1_500 characters", "Company_AboutUs_Invalid_Length");
    public static Error CompanyWebSiteAddressIsRequired => new Error("company web site address is required", "Company_WebSiteAddress_Is_Required");
    public static Error CompanyWebSiteAddressInvalidLength => new Error("company web site address length must be between 2 and 100 characters", "Company_WebSiteAddress_Invalid_Length");
    public static Error CompanyActivityTypeInvalidLength => new Error("company activity type length must be between 2 and 120 characters", "Company_ActivityType_Invalid_Length");
    #endregion

    #region CompanyCity Errors
    public static Error CompanyCityLocationIsRequired => new Error("company location is required", "CompanyCity_Location_Is_Required");
    public static Error CompanyCityCityIdIsRequired => new Error("company CityId is required", "CompanyCity_CityId_Is_Required");
    public static Error CompanyCityCompanyIdIsRequired => new Error("company CompanyId is required", "CompanyCity_CompanyId_Is_Required");
    public static Error CompanyCityLocationInvalidLength => new Error("company location length must be between 2 and 200 characters", "CompanyCity_Location_Invalid_Length");
    #endregion

    #region Resume Errors
    public static Error ResumeTitleIsRequired => new Error("resume title is required", "Resume_Title_Is_Required");
    public static Error ResumeUserIdIsRequired => new Error("resume UserId is required", "Resume_UserId_Is_Required");
    public static Error ResumeTitleInvalidLength => new Error("resume title length must be between 2 and 100 characters", "Resume_Title_Invalid_Length");
    #endregion

    #region EducationDetail Errors
    public static Error EducationDetailMajorIsRequired => new Error("education detail major is required", "EducationDetail_Major_Is_Required");
    public static Error EducationDetailUserIdIsRequired => new Error("education detail UserId is required", "EducationDetail_UserId_Is_Required");
    public static Error EducationDetailMajorInvalidLength => new Error("education detail major length must be between 2 and 120 characters", "EducationDetail_Major_Invalid_Length");
    public static Error EducationDetailUniversityIsRequired => new Error("education detail university is required", "EducationDetail_University_Is_Required");
    public static Error EducationDetailUniversityInvalidLength => new Error("education detail university length must be between 2 and 100 characters", "EducationDetail_University_Invalid_Length");
    public static Error EducationDetailUniversityStartDateTooFarInFuture => new Error("University start date cannot be more than one year from today.", "EducationDetail_StartDate_Invalid_Time");
    public static Error EducationDetailUniversityDurationTooShort => new Error("The education duration must be at least one year.", "EducationDetail_CompletionDate_Invalid_Time");
    public static Error EducationDetailFinalGradeTooLow => new Error("The final grade must be at least 12.", "EducationDetail_Percentage_TooLow");
    #endregion

    #region ExperienceDetail Errors
    public static Error ExperienceDetailLastJobTitleIsRequired => new Error("experience detail last job title is required", "ExperienceDetail_LastJobTitle_Is_Required");
    public static Error ExperienceDetailUserIdIsRequired => new Error("experience detail UserId is required", "ExperienceDetail_UserId_Is_Required");
    public static Error ExperienceDetailLastJobTitleInvalidLength => new Error("experience detail last job title length must be between 2 and 120 characters", "ExperienceDetail_LastJobTitle_Invalid_Length");
    public static Error ExperienceDetailJobCategoryIsRequired => new Error("experience detail job category is required", "ExperienceDetail_JobCategory_Is_Required");
    public static Error ExperienceDetailJobCategoryInvalidLength => new Error("experience detail job category length must be between 2 and 100 characters", "ExperienceDetail_JobCategory_Invalid_Length");
    public static Error ExperienceDetailCityIsRequired => new Error("experience detail city is required", "ExperienceDetail_City_Is_Required");
    public static Error ExperienceDetailCityInvalidLength => new Error("experience detail city length must be between 2 and 120 characters", "ExperienceDetail_City_Invalid_Length");
    public static Error ExperienceDetailStartDateTooFarInFuture => new Error("start date cannot be more than 2 year from today.", "ExperienceDetail_StartDate_Invalid_Time");
    public static Error ExperienceDetailJobEndTimeLowerThanStartTime => new Error("The Experience end date cannot be lower than start date. ", "ExperienceDetail_EndDate_Invalid_DurationTime");
    #endregion

    #region Advertisement Errors
    public static Error DescriptionIsRequired => new Error("Description is required.", "Advertisement_Description_Is_Required");
    public static Error DescriptionInvalidLength => new Error("Description must be between 100 and 2000 characters long.", "Advertisement_Description_Invalid_Length");
    public static Error MinimumAgeOutOfRange => new Error("Minimum age must be between 18 and 55.", "Advertisement_MinimumAge_Out_Of_Range");
    public static Error MaximumAgeOutOfRange => new Error("Maximum age must be between 18 and 65.", "Advertisement_MaximumAge_Out_Of_Range");
    public static Error MinimumAgeCannotExceedMaximumAge => new Error("Minimum age cannot be greater than maximum age.", "Advertisement_MinimumAge_Cannot_Exceed_MaximumAge");
    public static Error MaximumSalaryOutOfRange => new Error("Maximum salary must be between 1,000,000 and 600,000,000.", "Advertisement_MaximumSalary_Out_Of_Range");
    public static Error ExperienceLevelOutOfRange => new Error("Experience level must be Positive", "Advertisement_ExperienceLevel_Out_Of_Range");
    public static Error AdvertisementJobIdIsRequired => new Error("JobId is required.", "Advertisement_JobId_Is_Required");
    public static Error AdvertisementCityIdIsRequired => new Error("CityId is required.", "Advertisement_CityId_Is_Required");
    public static Error AdvertisementCompanyIdIsRequired => new Error("CompanyId is required.", "Advertisement_CompanyId_Is_Required");
    #endregion

    #region Role Errors
    public static Error RoleNameIsRequired => new Error("Name is required.", "Role_Name_Is_Required");
    public static Error RoleNameInvalidLength => new Error("Name must be between 12 and 100 characters long.", "Role_Name_Invalid_Length");
    public static Error RoleDescriptionInvalidLength => new Error("Role description must be between 2 and 100 characters.", "Role_Description_Invalid_Length");
    #endregion

    #region Attachment Errors
    public static Error AttachmentFileNameIsRequired => new Error("Attachment file name is required.", "Attachment_FileName_Is_Required");
    public static Error AttachmentContentTypeIsRequired => new Error("Attachment content type is required.", "Attachment_ContentType_Is_Required");
    #endregion

    #region Payment Errors
    public static Error PayemntAmountOutOfRange => new Error("payment amount must be Positive", "Payemnt_Amount_Out_Of_Range");
    public static Error PaymentUserIdIsRequired => new Error("Payment UserId is required", "Payment_UserId_Is_Required");
    public static Error PaymentAdvertisementIdIsRequired => new Error("Payment AdvertisementId is required", "Payment_AdvertisementId_Is_Required");

    #endregion

    #region Notifier Errors
    public static Error CodeIsRequired => new Error("phone number is required.", "Notifier_Code_Is_Required");
    public static Error PhoneNumberOrEmailIsRequired => new Error("phone number or email must have value", "Notifier_PhoneNumberOrEmail_Is_Required");
    public static Error CodeAlreadyExpired => new Error("the code is already expired!", "Notifier_Code_Is_Expired");
    public static Error InvalidCodeFormatException => new Error("the code must have 6 characters", "Notifier_Code_Invalid_Format");
    #endregion

    #region JobApplication Errors
    public static Error JobTitleIsRequired => new Error("JobTitle is required.", "JobApplication_JobTitle_Is_Required");
    public static Error JobApplicationAdvertisementIdIsRequired => new Error("AdvertisementId is required.", "JobApplication_AdvertisementId_Is_Required");
    public static Error JobApplicationUserIdIsRequired => new Error("UserId is required.", "JobApplication_UserId_Is_Required");
    public static Error JobApplicationResumeIdIsRequired => new Error("ResumeId is required.", "JobApplication_ResumeId_Is_Required");
    public static Error JobTitleInvalidLength => new Error("JobTitle must be between 100 and 2000 characters long.", "JobApplication_JobTitle_Invalid_Length");
    public static Error JobApplicationCompanyNameIsRequired => new Error("company name is required", "JobApplication_CompanyName_Is_Required");
    public static Error JobApplicationCompanyNameInvalidLength => new Error("company name length must be between 2 and 120 characters", "JobApplication_CompanyName_Invalid_Length");
    public static Error JobApplicationCityNameIsRequired => new Error("city name is required", "JobApplication_CityName_Is_Required");
    public static Error JobApplicationCityNameInvalidRange => new Error("city code cannot be negative ", "JobApplication_CityName_Invalid_Range");
    public static Error FullNameIsRequired => new Error("user full name is required", "JobApplication_UserFullName_Is_Required");
    public static Error FullNameInvalidLength => new Error("user full name length must be between 3 and 200 characters", "JobApplication_UserFullName_Invalid_Length");
    public static Error JobApplicationExperienceLevelOutOfRange => new Error("Experience level must be Positive", "JobApplication_ExperienceLevel_Out_Of_Range");
    #endregion

    #region Notifier Errors
    public static Error NotifierTitleIsRequired => new Error("Title is required.", "Notifier_Title_Is_Required");
    public static Error NotifierRecipientUserIdIsRequired => new Error("RecipientUserId is required.", "Notifier_RecipientUserId_Is_Required");
    public static Error NotifierTitleInvalidLength => new Error("Title must be between 2 and 150 characters long.", "Notifier_Title_Invalid_Length");
    public static Error NotifierMessageIsRequired => new Error("Message is required.", "Notifier_Message_Is_Required");
    public static Error NotifierMessageInvalidLength => new Error("Message must be between 10 and 250 characters long.", "Notifier_Message_Invalid_Length");
    #endregion

    #region AdvertisementSkill Errors
    public static Error AdvertisementSkillAdvertisementIdIsRequired => new Error("AdvertisementId is required.", "AdvertisementSkill_AdvertisementId_Is_Required");
    public static Error AdvertisementSkillSkillIdIsRequired => new Error("SkillId is required.", "AdvertisementSkill_SkillId_Is_Required");
    #endregion

    #region UserSkills
    public static Error UserSkillUserIdIsRequired => new Error("UserSkill UserId is required.", "UserSkill_UserId_Is_Required");
    public static Error UserSkillSkillIdIsRequired => new Error("UserSkill SkillId is required.", "UserSkill_SkillId_Is_Required");
    #endregion

    #region JobCategory Errors
    public static Error JobCategoryNameIsRequired => new Error("job category name UserId is required.", "JobCategory_Name_Is_Required");
    public static Error JobCategoryNameInvalidLength => new Error("job categoty name must be between 2 and 150 characters long.", "JobCategory_Name_Invalid_Length");

    #endregion
}
