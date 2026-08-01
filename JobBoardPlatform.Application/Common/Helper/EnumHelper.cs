using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Helper;

public static class EnumHelper
{
    public static List<EnumResponseDto> GetEnumValues<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
                          .Cast<TEnum>()
                          .Select(x => new EnumResponseDto
                          {
                              Id = Convert.ToInt32(x),
                              Title = x.ToString()
                          })
                          .ToList();
    }
}
