namespace FortitudeIO.Protocols.Authentication;

public interface ILoginCredentials
{
    bool RequestingLoginToken { get; set; }
    byte[]? LoginAttemptToken { get; set; }
    byte[]? PasswordHashToken { get; set; }
    string LoginId { get; set; }
    string Password { get; set; }
}
