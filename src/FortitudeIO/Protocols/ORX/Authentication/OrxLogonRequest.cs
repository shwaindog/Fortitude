#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.Authentication;

public sealed class OrxLogonRequest : OrxVersionedMessage
{
    public OrxLogonRequest(ILoginCredentials loginCredentials, MutableString account, MutableString clientVersion
        , uint hbinterval)
    {
        Login = loginCredentials.LoginId;
        Password = loginCredentials.Password;
        Account = account;
        ClientVersion = clientVersion;
        HbInterval = hbinterval;
    }

    public OrxLogonRequest() { }

    private OrxLogonRequest(OrxLogonRequest toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)AuthenticationMessages.LogonRequest;

    [OrxMandatoryField(10)] public MutableString? Login { get; set; }

    [OrxMandatoryField(11)] public MutableString? Password { get; set; }

    [OrxMandatoryField(12)] public MutableString? Account { get; set; }

    [OrxMandatoryField(13)] public MutableString? ClientVersion { get; set; }

    [OrxMandatoryField(14)] public uint HbInterval { get; set; }

    public override void Reset()
    {
        Login?.DecrementRefCount();
        Login = null;
        Password?.DecrementRefCount();
        Password = null;
        Account?.DecrementRefCount();
        Account = null;
        ClientVersion?.DecrementRefCount();
        ClientVersion = null;
        HbInterval = 0;
        base.Reset();
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxLogonRequest logonRequest)
        {
            Login = logonRequest.Login.SyncOrRecycle(Login);
            Password = logonRequest.Password.SyncOrRecycle(Password);
            Account = logonRequest.Account.SyncOrRecycle(Account);
            ClientVersion = logonRequest.ClientVersion.SyncOrRecycle(ClientVersion);
            HbInterval = logonRequest.HbInterval;
        }

        return this;
    }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<OrxLogonRequest>().CopyFrom(this) ?? new OrxLogonRequest(this);
}
