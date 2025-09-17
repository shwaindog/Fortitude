// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface IExtractKeyExpressionConfig : IFLogConfig, IInterfacesComparable<IExtractKeyExpressionConfig>
  , IConfigCloneTo<IExtractKeyExpressionConfig>, IStringBearer
{
    string KeyName { get; }
    string ExtractRegEx { get; }
    int ExtractGroupNumber { get; }
}

public interface IMutableExtractKeyExpressionConfig : IExtractKeyExpressionConfig, IMutableFLogConfig
{
    new string KeyName { get; set; }
    new string ExtractRegEx { get; set; }
    new int ExtractGroupNumber { get; set; }
}

public class ExtractKeyExpressionConfig : FLogConfig, IMutableExtractKeyExpressionConfig
{
    public ExtractKeyExpressionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public ExtractKeyExpressionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public ExtractKeyExpressionConfig
        (string keyName, string extractRegEx, int extractGroupNumber = 0)
        : this(InMemoryConfigRoot, InMemoryPath, keyName, extractRegEx, extractGroupNumber) { }

    public ExtractKeyExpressionConfig
        (IConfigurationRoot root, string path, string keyName, string extractRegEx, int extractGroupNumber = 0)
        : base(root, path)
    {
        KeyName            = keyName;
        ExtractRegEx       = extractRegEx;
        ExtractGroupNumber = extractGroupNumber;
    }

    public ExtractKeyExpressionConfig(IExtractKeyExpressionConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        KeyName            = toClone.KeyName;
        ExtractRegEx       = toClone.ExtractRegEx;
        ExtractGroupNumber = toClone.ExtractGroupNumber;
    }

    public ExtractKeyExpressionConfig(IExtractKeyExpressionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public int ExtractGroupNumber
    {
        get =>
            int.TryParse(this[nameof(ExtractGroupNumber)], out var timePart)
                ? timePart
                : 0;
        set => this[nameof(ExtractGroupNumber)] = value.ToString();
    }

    public string ExtractRegEx
    {
        get => this[nameof(ExtractRegEx)] ?? "";
        set => this[nameof(ExtractRegEx)] = value;
    }

    public string KeyName
    {
        get => this[nameof(KeyName)] ?? "";
        set => this[nameof(KeyName)] = value;
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IExtractKeyExpressionConfig IConfigCloneTo<IExtractKeyExpressionConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public ExtractKeyExpressionConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IExtractKeyExpressionConfig ICloneable<IExtractKeyExpressionConfig>.Clone() => Clone();

    public virtual ExtractKeyExpressionConfig Clone() => new(this);

    public virtual bool AreEquivalent(IExtractKeyExpressionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var extractGrpSame = ExtractGroupNumber == other.ExtractGroupNumber;
        var regExSame      = ExtractRegEx == other.ExtractRegEx;
        var keyNameSame    = KeyName == other.KeyName;

        var allAreSame = extractGrpSame && regExSame && keyNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IExtractKeyExpressionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = ExtractGroupNumber;
        hashCode = (hashCode * 397) ^ ExtractRegEx.GetHashCode();
        hashCode = (hashCode * 397) ^ KeyName.GetHashCode();
        return hashCode;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(KeyName), KeyName)
           .Field.AlwaysAdd(nameof(ExtractRegEx), ExtractRegEx)
           .Field.AlwaysAdd(nameof(ExtractGroupNumber), ExtractGroupNumber)
           .Complete();
}
