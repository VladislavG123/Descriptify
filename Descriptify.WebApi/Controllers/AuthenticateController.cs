using Descriptify.Bll;
using Descriptify.Bll.Abstract;
using Descriptify.Bll.Dtos;
using Descriptify.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Descriptify.Controllers;

public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }
    
    [HttpPost("api/user/signup")]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpViewModel viewModel)
    {
        return Ok(await _authenticateService.SignUp(new UserSignUpDto
        {
            Login = viewModel.Login,
            Password = viewModel.Password,
            Username = viewModel.Username
        }));
    }
    
    [HttpPost("api/user/signin")]
    public async Task<IActionResult> SignIp([FromBody] UserSignInViewModel viewModel)
    {
        return Ok(await _authenticateService.SignIn(new UserSignInDto
        {
            Login = viewModel.Login,
            Password = viewModel.Password
        }));
    }
}