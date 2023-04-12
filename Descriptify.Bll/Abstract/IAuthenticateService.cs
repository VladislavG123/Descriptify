using Descriptify.Bll.Dtos;

namespace Descriptify.Bll.Abstract;

public interface IAuthenticateService
{
    Task<string> SignUp(UserSignUpDto userSignUpDto);
    Task<string> SignIn(UserSignInDto userSignInDto);
}