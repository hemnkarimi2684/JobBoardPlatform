using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

namespace JobBoardPlatform.Mvc.Models.User;

public class UserEditViewModel : UpdateProfileRequestDto
{
    public static UserEditViewModel FromResponseDto(UserProfileResponseDto source)
        => new()
        {
            FirstName = source.FullName?.Split(' ', 2)[0],
            LastName = source.FullName?.Contains(' ') == true ? source.FullName.Split(' ', 2)[1] : null,
            Bio = source.Bio,
            Address = source.Address,
            BirthDate = source.BirthDate,
            Gender = source.Gender
        };
}
