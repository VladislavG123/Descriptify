namespace Descriptify.Bll.Dtos;

public record UserSignInDto(string Login, string Password);

public record UserSignUpDto(string Login, string Username, string Password);

public record UserDto(Guid Id, string Login, string Username);