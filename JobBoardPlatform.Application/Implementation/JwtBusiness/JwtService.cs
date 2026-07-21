using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.RoleEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoardPlatform.Application.Implementation.JwtBusiness;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    private readonly IUnitOfWork _unitOfWork;

    private readonly RoleManager<Role> _roleManager;

    private readonly UserManager<User> _userManager;

    public JwtService(IOptions<JwtSettings> options, IUnitOfWork unitOfWork, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _jwtSettings = options.Value;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<TokenLoginResponseDto> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
                continue;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims);
        }

        claims = claims.DistinctBy(c => (c.Type, c.Value)).ToList();

        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifeTime);

        var sigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.EncryptKey));

        var sigingCredentials = new SigningCredentials(sigingKey, SecurityAlgorithms.HmacSha256Signature);
        var EncryptCredentials = new EncryptingCredentials(encryptKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Expires = expires,
            SigningCredentials = sigingCredentials,
            EncryptingCredentials = EncryptCredentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return new TokenLoginResponseDto
        {
            AccessToken = handler.WriteToken(token),
            ExpiryTime = TimeSpan.FromMinutes(_jwtSettings.TokenLifeTime),
            TokenType = "Bearer"
        };
    }
}
