using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using System.Security.Cryptography;

namespace JobBoardPlatform.Core.Entities.NotifierEntity.Entity;

public class Notifier : BaseEntity
{
    private Notifier() { }

    public Notifier(string? toEmail, string? toPhoneNumber, string code)
    {
        ToEmail = toEmail;
        ToPhoneNumber = toPhoneNumber;
        Code = code;
        IsUsed = false;

        Validate();
        SetExpiredAt();
    }

    /// <summary>
    /// فرستاده شده برا ایمیل 
    /// </summary>
    public string? ToEmail { get; private set; }

    /// <summary>
    /// فرستاده شده برای شماره تلفن
    /// </summary>
    public string? ToPhoneNumber { get; private set; }

    /// <summary>
    /// کد جنریت شده برای کاربر 
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// تایم اکسپایر
    /// </summary>
    public DateTime ExpiredAt { get; private set; }

    /// <summary>
    /// ایا استفاده شده یا نه
    /// </summary>
    public bool IsUsed { get; private set; }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Code))
            throw new DomainException(DomainErrors.CodeIsRequired);

        //if (Code.Length != 6)
        //    throw new DomainException(DomainErrors.InvalidCodeFormatException);

        if (string.IsNullOrWhiteSpace(ToPhoneNumber) && string.IsNullOrWhiteSpace(ToEmail))
            throw new DomainException(DomainErrors.PhoneNumberOrEmailIsRequired);

        if (!string.IsNullOrWhiteSpace(ToPhoneNumber))
            ToPhoneNumber.PhoneNumberIsValid();

        if (!string.IsNullOrWhiteSpace(ToEmail))
            ToEmail.EmailIsValid();
    }

    private void SetExpiredAt() => ExpiredAt = DateTime.UtcNow.AddMinutes(5);

    //public void GenerateCode()
    //{
    //    var random = RandomNumberGenerator.GetInt32(100000, 1000000);

    //    Code = random.ToString();
    //}
}
