using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Descriptify.Bll.Abstract;
using Descriptify.Bll.Dtos;
using Descriptify.Contracts.Options;
using Descriptify.Dal.Entities;
using Descriptify.Dal.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Descriptify.Bll;

public class AuthenticateService : IAuthenticateService
{
    private readonly IUserProvider _userProvider;
    private readonly SecretOptions _secretOptions;

    public AuthenticateService(IUserProvider userProvider, IOptions<SecretOptions> secretOptions)
    {
        _userProvider = userProvider;
        _secretOptions = secretOptions.Value;
    }
    
    public async Task<string> SignUp(UserSignUpDto userSignUpDto)
    {
        if (await _userProvider.GetByLoginOrDefault(userSignUpDto.Login) is not null)
        {
            throw new ArgumentException("User with requested Login is already exists");
        }

        await _userProvider.Create(new UserEntity
        {
            Login = userSignUpDto.Login,
            Username = userSignUpDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userSignUpDto.Password)
        });
        
        return GenerateToken(userSignUpDto.Login, userSignUpDto.Username);
    }

    public async Task<string> SignIn(UserSignInDto userSignInDto)
    {
        var user = await _userProvider.GetByLoginOrDefault(userSignInDto.Login);
        if (user is null || !BCrypt.Net.BCrypt.Verify(userSignInDto.Password, user.PasswordHash))
        {
            throw new ArgumentException("No user with such login or password");
        }

        return GenerateToken(user.Login, user.Username);
    }

    private string GenerateToken(string login, string username)
    {
        var key = Encoding.ASCII.GetBytes(_secretOptions.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, login)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}