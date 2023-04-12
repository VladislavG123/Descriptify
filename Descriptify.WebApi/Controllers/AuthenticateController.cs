using Descriptify.Bll;
using Descriptify.Bll.Abstract;
using Descriptify.Bll.Dtos;
using Descriptify.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Descriptify.Controllers;

public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticateService _authenticateService;
    private readonly IUserService _userService;

    public AuthenticateController(IAuthenticateService authenticateService, IUserService userService)
    {
        _authenticateService = authenticateService;
        _userService = userService;
    }
    
    [HttpPost("api/auth/signup")]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpViewModel viewModel)
    {
        try
        {
            return Ok(await _authenticateService
                .SignUp(new UserSignUpDto(viewModel.Login, viewModel.Username, viewModel.Password)));
        }
        catch (ArgumentException)
        {
            return BadRequest("User with the same login is already exists");
        }
    }
    
    [HttpPost("api/auth/signin")]
    public async Task<IActionResult> SignIn([FromBody] UserSignInViewModel viewModel)
    {
        try
        {
            return Ok(await _authenticateService.SignIn(new UserSignInDto(viewModel.Login, viewModel.Password)));
        }
        catch (ArgumentException)
        {
            return BadRequest("Wrong login or password");
        }
    }

    [HttpGet("api/auth/")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            return Ok(await _authenticateService.GetUserByHeaders(Request.Headers[HeaderNames.Authorization]!));
        }
        catch (ArgumentException e)
        {
            return NotFound("User is not found, wrong token");
        }
    }
}