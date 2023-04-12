using Descriptify.Bll.Dtos;

namespace Descriptify.Bll.Abstract;

public interface IAuthenticateService
{
    Task<string> SignUp(UserSignUpDto userSignUpDto);
    Task<string> SignIn(UserSignInDto userSignInDto);
    
    /// <summary>
    /// Gets User by headers from Request
    /// Usage in controllers: 
    /// GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    Task<UserDto> GetUserByHeaders(string[] headers);
}