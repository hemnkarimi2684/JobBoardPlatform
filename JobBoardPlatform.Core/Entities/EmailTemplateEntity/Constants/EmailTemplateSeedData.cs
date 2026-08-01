namespace JobBoardPlatform.Core.Entities.EmailTemplateEntity.Constants;

public static class EmailTemplateSeedData
{
    public static readonly Guid EmployerApprovedId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000001");

    public static readonly Guid EmployerRejectedId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000002");

    public static readonly Guid NewJobApplicationReceivedId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000003");

    public static readonly Guid JobApplicationReviewingId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000004");

    public static readonly Guid JobApplicationInterviewId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000005");

    public static readonly Guid JobApplicationAcceptedId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000006");

    public static readonly Guid JobApplicationRejectedId =
        Guid.Parse("41f80e91-65d4-4b1a-9c19-000000000007");
}
