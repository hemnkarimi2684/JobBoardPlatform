using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserExistAsync(Guid userId)
                            => await _context.Users.AnyAsync(u => u.Id == userId);

}

