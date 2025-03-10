// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols;

public interface IVersionedMessage : IReusableObject<IVersionedMessage>
{
    [JsonIgnore] uint MessageId { get; }

    [JsonIgnore] byte Version { get; }
}

public abstract class VersionedMessage : ReusableObject<IVersionedMessage>, IVersionedMessage
{
    protected VersionedMessage() { }

    protected VersionedMessage(IVersionedMessage toClone) => Version = toClone.Version;

    protected VersionedMessage(byte version) => Version = version;

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Version = source.Version;
        return this;
    }

    public abstract uint MessageId { get; }
    public          byte Version   { get; set; }

    protected bool Equals(IVersionedMessage other) => Version == other.Version && MessageId == other.MessageId;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IVersionedMessage)obj);
    }

    public override int GetHashCode() => Version.GetHashCode();
}
