using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Dto;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.UserRepo;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken) => await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.IsActive, cancellationToken);

    public async Task<TResult?> GetResumeDetailAsync<TResult>(
        Expression<Func<User, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.Users
                                .AsNoTracking()
                                .Where(u => u.Id == userId)
                                .Select(projection)
                                .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDisplayDto?> GetUserEmailAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users
                                .AsNoTracking()
                                .Where(u => u.Id == userId && u.IsActive)
                                .Select(u => new UserDisplayDto
                                {
                                    Email = u.Email!,
                                    FullName = u.UserProfile!.FirstName + " " + u.UserProfile.LastName
                                })
                                .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateEmailOrPhoneNumberAsync(
        string email,
        string phoneNumber,
        CancellationToken cancellationToken) => await _context.Users.AnyAsync(u => u.Email == email || u.PhoneNumber == phoneNumber && u.IsActive, cancellationToken);

    public async Task<bool> IsUserExistAsync(
        Guid userId,
        CancellationToken cancellationToken) => await _context.Users.AnyAsync(u => u.Id == userId && u.IsActive, cancellationToken);
}

