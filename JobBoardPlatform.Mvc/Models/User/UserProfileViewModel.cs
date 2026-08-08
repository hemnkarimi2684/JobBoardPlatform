using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

namespace JobBoardPlatform.Mvc.Models.User;

public class UserProfileViewModel : UserProfileResponseDto
{
    public static UserProfileViewModel FromResponseDto(UserProfileResponseDto source)
        => new()
        {
            UserId = source.UserId,
            FullName = source.FullName,
            Bio = source.Bio,
            Address = source.Address,
            BirthDate = source.BirthDate,
            CityName = source.CityName,
            Gender = source.Gender,
            UserImageFileId = source.UserImageFileId
        };
}
