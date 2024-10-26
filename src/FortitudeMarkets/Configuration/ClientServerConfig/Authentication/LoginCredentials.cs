#region

using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig.Authentication;

public class LoginCredentials : ILoginCredentials
{
    public LoginCredentials(string loginId, string password)
    {
        Password = password;
        LoginId = loginId;
    }

    public bool RequestingLoginToken { get; set; }

    public byte[]? LoginAttemptToken { get; set; }

    public byte[]? PasswordHashToken { get; set; }

    public string Password { get; set; }

    public string LoginId { get; set; }
}
