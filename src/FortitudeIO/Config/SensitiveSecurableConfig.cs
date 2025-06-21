// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;

namespace FortitudeIO.Config;

public interface ISensitiveSecurableConfig<TFlagEnum> where TFlagEnum : struct, IConvertible
{
    TFlagEnum ObscureFieldFlags { get; set; }

    [JsonIgnore] TFlagEnum DefaultObscureFieldFlags { get; }

    bool ObscureToString { get; set; }

    int ShowUpToFirstChars { get; set; }

    int ShowUpToLastChars { get; set; }

    string BuildToString(bool secureSensitive);
}

public abstract class SensitiveSecurableConfig<TFlagEnum> : ConfigSection, ISensitiveSecurableConfig<TFlagEnum>
    where TFlagEnum : struct, IConvertible
{
    protected SensitiveSecurableConfig(IConfigurationRoot root, string path) : base(root, path) { }


    protected SensitiveSecurableConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected SensitiveSecurableConfig
        (bool obscureToString = true, TFlagEnum obscureFieldFlags = default, int showUpToFirstChars = 3, int showUpToLastChars = 3)
    {
        ObscureToString    = obscureToString;
        ObscureFieldFlags  = obscureFieldFlags;
        ShowUpToFirstChars = showUpToFirstChars;
        ShowUpToLastChars  = showUpToLastChars;
    }

    protected SensitiveSecurableConfig(ISensitiveSecurableConfig<TFlagEnum> toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        ObscureToString    = toClone.ObscureToString;
        ObscureFieldFlags  = toClone.ObscureFieldFlags;
        ShowUpToFirstChars = toClone.ShowUpToFirstChars;
        ShowUpToLastChars  = toClone.ShowUpToLastChars;
    }

    protected SensitiveSecurableConfig(ISensitiveSecurableConfig<TFlagEnum> toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool ObscureToString
    {
        get
        {
            var checkValue = this[nameof(ObscureToString)]!;
            return checkValue.IsNullOrEmpty() || bool.Parse(checkValue);
        }

        set => this[nameof(ObscureToString)] = value.ToString();
    }

    public virtual TFlagEnum ObscureFieldFlags
    {
        get
        {
            var checkValue = this[nameof(ObscureFieldFlags)]!;
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<TFlagEnum>(checkValue) : DefaultObscureFieldFlags;
        }

        set => this[nameof(ObscureFieldFlags)] = value.ToString();
    }

    public abstract TFlagEnum DefaultObscureFieldFlags { get; }

    public int ShowUpToFirstChars
    {
        get
        {
            var checkValue = this[nameof(ShowUpToFirstChars)]!;
            return checkValue.IsNotNullOrEmpty() ? int.Parse(checkValue) : 3;
        }

        set => this[nameof(ShowUpToFirstChars)] = value.ToString();
    }

    public int ShowUpToLastChars
    {
        get
        {
            var checkValue = this[nameof(ShowUpToLastChars)]!;
            return checkValue.IsNotNullOrEmpty() ? int.Parse(checkValue) : 3;
        }

        set => this[nameof(ShowUpToLastChars)] = value.ToString();
    }

    public bool AreEquivalent(ISensitiveSecurableConfig<TFlagEnum>? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var obscureSame = ObscureToString == other.ObscureToString;
        var obscureFieldsSame = Equals(ObscureFieldFlags, other.ObscureFieldFlags);
        var showFirstSame = ShowUpToFirstChars == other.ShowUpToFirstChars;
        var showLastSame = ShowUpToLastChars == other.ShowUpToLastChars;

        var allAreSame = obscureSame && obscureFieldsSame && showFirstSame && showLastSame;

        return allAreSame;
    }

    public string Obscure(string? original)
    {
        if (original.IsNullOrEmpty()) return "";
        var length = original.Length;
        switch (length)
        {
            case < 4:  return "***";
            case < 5:  return $"{original[0]}**";
            case < 6:  return $"{original[0]}***{original[^1]}";
            case < 7:  return $"{original[..1]}***{original[^1]}";
            case < 8:  return $"{original[..1]}***{original[^2..]}";
            case < 9:  return $"{original[..2]}***{original[^2..]}";
            case < 10: return $"{original[..3]}***{original[^2..]}";
            case < 11: return $"{original[..3]}***{original[^3..]}";
            default:
                var modifiedShowFirstChars = ShowUpToFirstChars;
                var modifiedShowLastChars = ShowUpToLastChars;

                var totalShow              = ShowUpToFirstChars + ShowUpToLastChars;
                var originalLength         = original.Length;
                for (int i = originalLength; i <= totalShow; i+=2)
                {
                    modifiedShowFirstChars--;
                    modifiedShowLastChars--;
                }
                var sb        = new StringBuilder();
                for (int i = 0; i < modifiedShowFirstChars; i++)
                {
                    sb.Append(original[i]);
                }
                sb.Append("***");
                for (int i = original.Length - modifiedShowLastChars; i < original.Length; i++)
                {
                    sb.Append(original[i]);
                }
                return sb.ToString();
        }
    }

    public virtual string BuildToString(bool secureSensitive)
    {
        var sb = new StringBuilder();
        sb.Append(nameof(ObscureToString)).Append(": ").Append(ObscureToString).Append(", ");
        sb.Append(nameof(ObscureFieldFlags)).Append(": ").Append(ObscureFieldFlags).Append(", ");
        sb.Append(nameof(ShowUpToFirstChars)).Append(": ").Append(ShowUpToFirstChars).Append(", ");
        sb.Append(nameof(ShowUpToLastChars)).Append(": ").Append(ShowUpToLastChars).Append(", ");
        return sb.ToString();
    }
}
