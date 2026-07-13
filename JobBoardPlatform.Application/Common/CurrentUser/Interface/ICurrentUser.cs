namespace JobBoardPlatform.Application.Common.CurrentUser.Interface;

public interface ICurrentUser
{
    public Guid? UserId { get; }

    List<string> UserRoles { get; }
}
