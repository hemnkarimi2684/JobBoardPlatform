namespace JobBoardPlatform.Application.Common.Dto.Common.Command;

public record PagingCommand(int PageNumber = 1, int PageSize = 10);

