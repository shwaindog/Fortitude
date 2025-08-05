using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeIO.Config;

[Flags]
public enum RemoteCredentialsFieldFlags : uint
{
    None                     = 0
  , RemoteAddress            = 0x00_01
  , Port                     = 0x00_02
  , Username                 = 0x00_04
  , Password                 = 0x00_08
  , ApiToken                 = 0x00_10
  , CertificatePath          = 0x00_20
  , CertificatePayload       = 0x00_40
  , HostName                 = 0x00_80
  , DatabaseName             = 0x01_00
  , Schema                   = 0x02_00
  , DatabasePath             = 0x04_00
  , ConnectionString         = 0x08_00
}

public static class RemoteCredentialsFieldFlagsExtensions
{
    public static bool IsSecureRemoteAddress(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.RemoteAddress) > 0;

    public static bool IsSecurePort(this RemoteCredentialsFieldFlags flags)          => (flags & RemoteCredentialsFieldFlags.Port) > 0;

    public static bool IsSecureUsername(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.Username) > 0;

    public static bool IsSecurePassword(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.Password) > 0;

    public static bool IsSecureApiToken(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.ApiToken) > 0;

    public static bool IsSecureCertificatePath(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.CertificatePath) > 0;

    public static bool IsSecureCertificatePayload(this RemoteCredentialsFieldFlags flags) =>
        (flags & RemoteCredentialsFieldFlags.CertificatePayload) > 0;

    public static bool IsSecureHostName(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.Username) > 0;

    public static bool IsSecureDatabaseName(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.DatabaseName) > 0;

    public static bool IsSecureSchema(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.Schema) > 0;

    public static bool IsSecureDatabasePath(this RemoteCredentialsFieldFlags flags) => (flags & RemoteCredentialsFieldFlags.DatabasePath) > 0;

    public static bool IsSecureConnectionString(this RemoteCredentialsFieldFlags flags) =>
        (flags & RemoteCredentialsFieldFlags.ConnectionString) > 0;

    public static bool IsSecureResolvedConnectionString(this RemoteCredentialsFieldFlags flags) => flags > 0;
}

public interface IRemoteCredentialsConfig : ISensitiveSecurableConfig<RemoteCredentialsFieldFlags>
  , IInterfacesComparable<IRemoteCredentialsConfig>, ICloneable<IRemoteCredentialsConfig>
{
    static readonly uint           DefaultRetryAttempts          = 3u;
    static readonly TimeSpanConfig DefaultFirstRetryInterval     = new (seconds: 1);
    static readonly TimeSpanConfig DefaultRetryIntervalIncrement = new (seconds: 2);
    static readonly TimeSpanConfig DefaultMaxRetryIntervalCap    = new (minutes: 5);

    const IntervalExpansionType DefaultIntervalExpansionType = IntervalExpansionType.Quadratic;

    string                         Name { get; set; }

    bool Enabled { get; set; }

    string? RemoteAddress { get; set; }

    ushort? Port { get; set; }

    string? ApiToken { get; set; }

    string? CertificatePath { get; set; }

    string? CertificatePayload { get; set; }

    string? Username { get; set; }

    string? Password { get; set; }

    IRetryConfig ReconnectConfig { get; set; }
}

public class RemoteCredentialsConfig : SensitiveSecurableConfig<RemoteCredentialsFieldFlags>, IRemoteCredentialsConfig
{
    public RemoteCredentialsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public RemoteCredentialsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public RemoteCredentialsConfig(string name, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
    {
        Name               = name;
        Enabled            = enabled;
        RemoteAddress      = remoteAddress;
        Username           = username;
        Password           = password;
        ApiToken           = apiToken;
        CertificatePath    = certificatePath;
        CertificatePayload = CertificatePayload;
    }

    public RemoteCredentialsConfig(IRemoteCredentialsConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        Name               = toClone.Name;
        Enabled            = toClone.Enabled;
        RemoteAddress      = toClone.RemoteAddress;
        Username           = toClone.Username;
        Password           = toClone.Password;
        ApiToken           = toClone.ApiToken;
        CertificatePath    = toClone.CertificatePath;
        CertificatePayload = toClone.CertificatePayload;
    }

    public RemoteCredentialsConfig(IRemoteCredentialsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override RemoteCredentialsFieldFlags DefaultObscureFieldFlags =>
        RemoteCredentialsFieldFlags.Username
      | RemoteCredentialsFieldFlags.Password
      | RemoteCredentialsFieldFlags.ApiToken
      | RemoteCredentialsFieldFlags.CertificatePayload;

    public virtual bool Enabled
    {
        get
        {
            var checkValue = this[nameof(Enabled)]!;
            return checkValue.IsNullOrEmpty() || bool.Parse(checkValue);
        }

        set => this[nameof(Enabled)] = value.ToString();
    }

    public virtual string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    public virtual string? RemoteAddress
    {
        get => this[nameof(RemoteAddress)];
        set => this[nameof(RemoteAddress)] = value;
    }

    public virtual ushort? Port 
    {
        get
        {
            var checkValue = this[nameof(ShowUpToLastChars)]!;
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : null;
        }

        set => this[nameof(ShowUpToLastChars)] = value.ToString();
    }

    public virtual string? Username
    {
        get => this[nameof(Username)];
        set => this[nameof(Username)] = value;
    }

    public virtual string? Password
    {
        get => this[nameof(Password)];
        set => this[nameof(Password)] = value;
    }

    public virtual string? ApiToken
    {
        get => this[nameof(ApiToken)];
        set => this[nameof(ApiToken)] = value;
    }

    public virtual string? CertificatePath
    {
        get => this[nameof(CertificatePath)];
        set => this[nameof(CertificatePath)] = value;
    }

    public virtual string? CertificatePayload
    {
        get => this[nameof(CertificatePayload)];
        set => this[nameof(CertificatePayload)] = value;
    }
    
    public IRetryConfig ReconnectConfig
    {
        get
        {
            if (GetSection(nameof(ReconnectConfig)).GetChildren().SelectMany(cs => cs.GetChildren()).Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new RetryConfig(ConfigRoot, $"{Path}{Split}{nameof(ReconnectConfig)}");
            }
            return new RetryConfig(ConfigRoot, $"{Path}{Split}{nameof(ReconnectConfig)}", IRemoteCredentialsConfig.DefaultRetryAttempts,
                                   IRemoteCredentialsConfig.DefaultFirstRetryInterval, IRemoteCredentialsConfig.DefaultRetryIntervalIncrement
                                  ,IRemoteCredentialsConfig.DefaultMaxRetryIntervalCap, IRemoteCredentialsConfig.DefaultIntervalExpansionType);
        }
        set => _ = value != null ? new RetryConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ReconnectConfig)}") : null;
    }


    object ICloneable.Clone() => Clone();

    IRemoteCredentialsConfig ICloneable<IRemoteCredentialsConfig>.Clone() => Clone();

    public virtual RemoteCredentialsConfig Clone() => new (this);

    public virtual bool AreEquivalent(IRemoteCredentialsConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var nameSame        = Name == other.Name;
        var enabledSame        = Enabled == other.Enabled;
        var remoteAddressSame   = RemoteAddress == other.RemoteAddress;
        var userNameSame       = Username == other.Username;
        var passwordSame       = Password == other.Password;
        var apiTokenSame       = ApiToken == other.ApiToken;
        var certPathSame = CertificatePath == other.CertificatePath;
        var certPayloadSame = CertificatePayload == other.CertificatePayload;

        var secureSensitiveSame = true;
        if (exactTypes)
        {
            secureSensitiveSame = base.AreEquivalent(other, exactTypes);
        }

        var allAreSame = nameSame && enabledSame && remoteAddressSame && userNameSame && passwordSame 
                      && apiTokenSame && certPathSame && certPayloadSame && secureSensitiveSame;

        return allAreSame;
    }

    public override string BuildToString(bool secureSensitive)
    {
        var sb = new StringBuilder();

        var secureFlags = ObscureFieldFlags;
        sb.Append(nameof(Name)).Append(": ").Append(Name).Append(", ");
        sb.Append(nameof(Enabled)).Append(": ").Append(Enabled).Append(", ");
        if (RemoteAddress.IsNotNullOrEmpty())
        {
            sb.Append(nameof(RemoteAddress)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureRemoteAddress() ? Obscure(RemoteAddress) : RemoteAddress).Append(", ");
        }
        if (Username.IsNotNullOrEmpty())
        {
            sb.Append(nameof(Username)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureUsername() ? Obscure(Username) : Username).Append(", ");
        }
        if (Password.IsNotNullOrEmpty())
        {
            sb.Append(nameof(Password)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecurePassword() ? Obscure(Password) : Password).Append(", ");
        }
        if (ApiToken.IsNotNullOrEmpty())
        {
            sb.Append(nameof(ApiToken)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecurePassword() ? Obscure(ApiToken) : ApiToken).Append(", ");
        }
        if (CertificatePath.IsNotNullOrEmpty())
        {
            sb.Append(nameof(CertificatePath)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureCertificatePath() ? Obscure(CertificatePath) : CertificatePath).Append(", ");
        }
        if (CertificatePayload.IsNotNullOrEmpty())
        {
            sb.Append(nameof(CertificatePayload)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureCertificatePath() ? Obscure(CertificatePayload) : CertificatePayload).Append(", ");
        }
        sb.Length -= 2;

        return sb.ToString();
    }

    protected string RemoteConnectionCredentialsToStringMembers => $"{BuildToString(ObscureToString)}";

    public override string ToString() => $"{nameof(RemoteCredentialsConfig)}{{{RemoteConnectionCredentialsToStringMembers}}}";
}
