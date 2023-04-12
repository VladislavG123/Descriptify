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
    private readonly IUserService _userService;
    private readonly SecretOptions _secretOptions;

    public AuthenticateService(IUserProvider userProvider, IOptions<SecretOptions> secretOptions, IUserService userService)
    {
        _userProvider = userProvider;
        _userService = userService;
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
                new Claim(ClaimTypes.NameIdentifier, login),
                new Claim(ClaimTypes.Name, username)
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
    
    /// <inheritdoc /> 
    public async Task<UserDto> GetUserByHeaders(string[] headers)
    {
        var token = headers[0].Replace("Bearer ", "");
        var login = DecryptToken(token).Login;

        return await _userService.GetUserByLogin(login);
    }
    
    /// <summary>
    ///    Token decryption
    /// </summary>
    /// <param name="token"></param>
    /// <exception cref="ArgumentException">throws when could not parse claims</exception>
    /// <returns>Owner's data</returns>
    private (string Login, string Username) DecryptToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        if (tokenS?.Claims is List<Claim> claims)
        {
            return new ValueTuple<string, string>(claims[0].Value, claims[1].Value);
        }

        throw new ArgumentException();
    }
}