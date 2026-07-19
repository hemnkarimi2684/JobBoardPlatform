using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
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

    public async Task<User?> FindByPhoneNumberAsync(string phoneNumber)
                             => await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && !u.IsDeleted && u.DeletedAt == null);

    public async Task<bool> IsDuplicateEmailOrPhoneNumberAsync(string email, string phoneNumber)
        => await _context.Users.AnyAsync(u => u.Email == email || u.PhoneNumber == phoneNumber && !u.IsDeleted && u.DeletedAt == null);

    public async Task<bool> IsUserExistAsync(Guid userId)
                            => await _context.Users.AnyAsync(u => u.Id == userId && !u.IsDeleted && u.DeletedAt == null);
}

