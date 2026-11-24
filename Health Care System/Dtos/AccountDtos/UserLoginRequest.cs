namespace Health_Care_System.Dtos.AccountDtos;

/// <summary>Login body.</summary>
/// <param name="Email">Registered email address.</param>
/// <param name="Password">Account password.</param>
public record UserLoginRequest(string Email, string Password);
