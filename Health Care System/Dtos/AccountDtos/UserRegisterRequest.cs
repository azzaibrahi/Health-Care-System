namespace Health_Care_System.Dtos.AccountDtos;

/// <summary>Registration body.</summary>
/// <param name="Email">Account email, also used as the user name.</param>
/// <param name="Password">Password checked by ASP.NET Core Identity.</param>
public record UserRegisterRequest(string Email, string Password);
