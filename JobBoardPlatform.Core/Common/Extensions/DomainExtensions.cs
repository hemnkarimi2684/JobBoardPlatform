using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using System.Net.Mail;

namespace JobBoardPlatform.Core.Common.Extensions;

/// <summary>
/// افزونه های در لایه دامین
/// </summary>
public static class DomainExtensions
{
    /// <summary>
    /// اعتبار سنجی ایمیل
    /// </summary>
    /// <param name="email"></param>
    /// <exception cref="DomainException"></exception>
    public static void EmailIsValid(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException(DomainErrors.EmailIsRequired);

        var trimmedEmail = email.Trim();

        try
        {
            var validMailAddress = new MailAddress(trimmedEmail);
        }
        catch (FormatException)
        {
            throw new DomainException(DomainErrors.EmailInvalidFormat);
        }
    }

    /// <summary>
    /// اعتبار سنجی شماره تلفن 
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <exception cref="DomainException"></exception>
    public static void PhoneNumberIsValid(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException(DomainErrors.PhoneNumberIsRequired);

        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.Length != 11)
            throw new DomainException(DomainErrors.PhoneNumberInvalidFormat);

        if (!phoneNumber.All(char.IsDigit))
            throw new DomainException(DomainErrors.PhoneNumberInvalidFormat);
    }

    /// <summary>
    /// تغییر فرمت شماره تلفن 
    /// </summary>
    /// <param name="phoneNumber"></param>
    public static string FixPhoneNumberFormat(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return phoneNumber;

        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.StartsWith("+98"))
            return $"09{phoneNumber.Substring(3)}";

        return phoneNumber;
    }

    /// <summary>
    /// بررسی اینکه ایا همه ی کارکتر های سابجکت مورد نظر حرف است یا نه
    /// </summary>
    /// <param name="subject"></param>
    /// <exception cref="DomainException"></exception>
    public static void IsAllLetter(this string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new DomainException(DomainErrors.SubjectIsRequired);

        subject = subject.Trim();

        if (!subject.All(char.IsLetter))
            throw new DomainException(DomainErrors.SubjectAllCharactersNotLetter);
    }

    /// <summary>
    /// بررسی اینکه ایای همه ی کارکتر های سابجکت مورد نظر شماره است یا نه 
    /// </summary>
    /// <param name="subject"></param>
    /// <exception cref="DomainException"></exception>
    public static void IsAllDigit(this string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new DomainException(DomainErrors.SubjectIsRequired);

        subject = subject.Trim();

        if (!subject.All(char.IsDigit))
            throw new DomainException(DomainErrors.SubjectAllCharactersNotDigit);
    }
}
