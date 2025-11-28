using System.Collections.Concurrent;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentlySynchronizedField

namespace FortitudeCommon.Types.StringsOfPower;

public static class EnumFormatterRegistry
{
    private static readonly object SyncLock = new();

    private static readonly ConcurrentDictionary<Type, IEnumFormatter> MaterializedFormatters = new();

    public static IStructEnumFormatProvider<TEnumValue> GetOrCreateStructEnumFormatProvider<TEnumValue>()
        where TEnumValue : ISpanFormattable
    {
        var type = typeof(TEnumValue);
        if (!type.IsEnum)
        {
            throw new ArgumentException($"Type '{typeof(TEnumValue).FullName}' is not an enum.");
        }
        if (!MaterializedFormatters.TryGetValue(type, out var formatter))
        {
            lock (SyncLock)
            {
                if (!MaterializedFormatters.TryGetValue(type, out formatter))
                {
                    formatter = (IEnumFormatter)ReflectionHelper.InstantiateGenericType(typeof(EnumFormatProvider<>), [type]);
                    MaterializedFormatters.TryAdd(type, formatter);
                }
            }
        }
        return formatter.AsSpanFormattableEnumFormatProvider<TEnumValue>()!;
    }

    public static IEnumFormatProvider<TEnum> GetOrCreateEnumFormatProvider<TEnum>()
        where TEnum : Enum
    {
        var type = typeof(TEnum);
        if (!type.IsEnum)
        {
            throw new ArgumentException($"Type '{typeof(TEnum).FullName}' is not an enum.");
        }
        if (!MaterializedFormatters.TryGetValue(type, out var formatter))
        {
            lock (SyncLock)
            {
                if (!MaterializedFormatters.TryGetValue(type, out formatter))
                {
                    formatter = (IEnumFormatter)ReflectionHelper.InstantiateGenericType(typeof(EnumFormatProvider<>), [type]);
                    MaterializedFormatters.TryAdd(type, formatter);
                }
            }
        }
        return formatter.AsTypedEnumFormatProvider<TEnum>()!;
    }

    public static IEnumFormatter GetOrCreateEnumFormatProvider(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException($"Type '{enumType.FullName}' is not an enum.");
        }
        IEnumFormatter? formatter;
        lock (SyncLock)
        {
            if (!MaterializedFormatters.TryGetValue(enumType, out formatter))
            {
                formatter = (IEnumFormatter)ReflectionHelper.InstantiateGenericType(typeof(EnumFormatProvider<>), [enumType]);
                MaterializedFormatters.TryAdd(enumType, formatter);
            }
        }
        return formatter;
    }
}

public interface IEnumFormatter : IStringBearerFormattableProvider
{
    IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable?;

    IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum;
    IEnumFormatProvider<Enum>? AsEnumFormatProvider();
}

public interface IStructEnumFormatProvider<in TEnum> : IEnumFormatter, IStringBearerRevelStateProvider<TEnum>
  , IStringBearerSpanFormattableProvider<TEnum>
    where TEnum : notnull
{ }

public interface IEnumFormatProvider<in TEnum> : IEnumFormatter, IStringBearerRevelStateProvider<TEnum>, IStringBearerSpanFormattableProvider<TEnum>
    where TEnum : Enum { }

public class EnumFormatProvider<TEnumValue> : IStructEnumFormatProvider<TEnumValue>, IEnumFormatProvider<TEnumValue>
    where TEnumValue : struct, Enum, IConvertible, ISpanFormattable
{
    private readonly bool isFlagsEnum;
    private readonly bool isShortRangeEnum;
    private readonly long lowestOffset;

    private readonly int allValuesCharCount;
    private readonly int longestNameCharCount;

    private readonly string[]? shortRangeEnumMaterializedNames;

    private readonly TEnumValue[] enumValues;

    private ConcurrentDictionary<Int128, string>? veryLongRangeEnumMaterializedNames;

    private IEnumFormatProvider<Enum>? enumAdapterCompanion;

    private readonly PalantírReveal<Enum>              asEnumTypeStyler;
    private readonly StringBearerSpanFormattable<Enum> asEnumSpanFormattable;

    public EnumFormatProvider()
    {
        var enumType = typeof(TEnumValue);

        enumValues = (TEnumValue[])Enum.GetValues(enumType);
        TEnumValue lowest  = enumValues.Min();
        TEnumValue highest = enumValues.Max();

        decimal range = highest.ToDecimal(null) - lowest.ToDecimal(null);
        // switch (firstValue)
        // {
        //     case ulong: range = highest.ToDecimal(null) - lowest.ToDecimal(null); break;
        //     case long: range = highest.ToDecimal(null) - lowest.ToDecimal(null); break;
        //     default:    range = highest.ToInt64(null) - lowest.ToInt64(null); break;
        // }

        if (range < 1024m)
        {
            isShortRangeEnum = true;
            lowestOffset     = lowest.ToInt64(null);

            shortRangeEnumMaterializedNames = new string[(int)range + 1];

            foreach (TEnumValue item in enumValues)
            {
                var valueShort = (ushort)(item.ToInt64(null) - lowestOffset);
                var name       = Enum.GetName(enumType, item)!;
                var nameLength = name.Length;
                if (longestNameCharCount < nameLength)
                {
                    longestNameCharCount = nameLength;
                }
                allValuesCharCount                          += nameLength + 2;
                shortRangeEnumMaterializedNames[valueShort] =  name;
            }
        }
        else
        {
            isShortRangeEnum = false;

            veryLongRangeEnumMaterializedNames = new ConcurrentDictionary<Int128, string>();
            foreach (TEnumValue item in enumValues)
            {
                var name       = Enum.GetName(enumType, item)!;
                var nameLength = name.Length;
                if (longestNameCharCount < nameLength)
                {
                    longestNameCharCount = nameLength;
                }
                allValuesCharCount += nameLength + 2;
                veryLongRangeEnumMaterializedNames.TryAdd((Int128)item.ToDecimal(null), name);
            }
        }
        EnumPalantír     = EnumStyler;
        StringBearerSpanFormattable = EnumExtendedSpanFormattable;

        asEnumTypeStyler      = EnumStyler;
        asEnumSpanFormattable = EnumExtendedSpanFormattable;

        isFlagsEnum    = enumType.GetCustomAttributes<FlagsAttribute>().Any();
        ForType        = enumType;
        UnderlyingType = enumType.GetEnumUnderlyingType();
    }

    public bool SupportSpanFormattable => true;

    public bool SupportStyleToString => true;

    public IEnumFormatProvider<Enum> EnumAdapterCompanion
    {
        get => enumAdapterCompanion ??= new CompanionEnumAdapter(this);
        set => enumAdapterCompanion = value;
    }

    public Type ForType { get; }
    
    public Type UnderlyingType { get; }

    public IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable =>
        typeof(TEnum) == ForType ? (IStructEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum =>
        typeof(TEnum).IsAssignableFrom(ForType) ? (IEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<Enum>? AsEnumFormatProvider() => typeof(Enum).IsAssignableFrom(ForType) ? EnumAdapterCompanion : null;

    public PalantírReveal<TEnumValue> EnumPalantír { get; }

    public StateExtractStringRange EnumStyler(TEnumValue toFormatEnum, ITheOneString stsa)
    {
        var tb = stsa.StartSimpleValueType(toFormatEnum);
        using (var sb = tb.StartDelimitedStringBuilder())
        {
            var buildNames = stackalloc char[allValuesCharCount].ResetMemory();
            SourceEnumNamesFromEnum(stsa.CurrentStyledTypeFormatter.StringEncoder, toFormatEnum, buildNames, ReadOnlySpan<char>.Empty);
            sb.Append(buildNames, 0, buildNames.PopulatedLength());
        }

        return tb.Complete();
    }

    public StateExtractStringRange EnumStyler(Enum toFormatEnum, ITheOneString stsa)
    {
        return EnumStyler((TEnumValue)toFormatEnum, stsa);
    }

    private int SourceEnumNamesFromEnum(IEncodingTransfer encoder, TEnumValue enumValue, Span<char> buildNames, ReadOnlySpan<char> format
      , IFormatProvider? provider = null, FormattingHandlingFlags formattingFlags = FormattingHandlingFlags.DefaultCallerTypeFlags)
    {
        if (!isFlagsEnum)
        {
            return SourceSingleNameFromEnum(encoder, enumValue, buildNames, format, provider, formattingFlags);
        }
        var countChars = 0;
        foreach (var eachEnumValue in enumValues)
        {
            if (eachEnumValue.CompareTo(enumValue) != 0) continue;
            countChars += SourceSingleNameFromEnum(encoder, eachEnumValue, buildNames, format, provider, formattingFlags);
            return countChars;
        }
        // else check each flag combination
        var hasWrittenValues = false;
        if (enumValue.CompareTo(default(TEnumValue)) != 0)
        {
            foreach (var eachEnumValue in enumValues)
                if (enumValue.HasFlag(eachEnumValue) && (eachEnumValue.CompareTo(default(TEnumValue)) != 0 || enumValue.CompareTo(default(TEnumValue)) == 0))
                {
                    if (hasWrittenValues)
                    {
                        countChars += 2;
                        buildNames.Append(", ");
                    }
                    countChars       += SourceSingleNameFromEnum(encoder, eachEnumValue, buildNames, format, provider, formattingFlags);
                    hasWrittenValues =  true;
                }
        }
        else
        {
            countChars       += SourceSingleNameFromEnum(encoder, enumValue, buildNames, format, provider, formattingFlags);
        }
        return countChars;
    }

    private int SourceSingleNameFromEnum(IEncodingTransfer encoder, TEnumValue singleEnumValue, Span<char> buildNames, ReadOnlySpan<char> format
      , IFormatProvider? provider = null, FormattingHandlingFlags formattingFlags = FormattingHandlingFlags.DefaultCallerTypeFlags)
    {
        if (format.Length == 0)
        {
            if (isShortRangeEnum)
            {
                var enumValue = singleEnumValue.ToInt64(null);
                if (enumValue < lowestOffset || enumValue > (lowestOffset + shortRangeEnumMaterializedNames!.Length))
                {
                    veryLongRangeEnumMaterializedNames ??= new ConcurrentDictionary<Int128, string>();
                    if (!veryLongRangeEnumMaterializedNames!.TryGetValue(enumValue, out var outOfRangeValue))
                    {
                        outOfRangeValue = enumValue.ToString();
                        veryLongRangeEnumMaterializedNames.TryAdd(enumValue, enumValue.ToString());
                    }
                    buildNames.Append(outOfRangeValue);
                    return outOfRangeValue.Length;
                }
                var valueShort    = (short)enumValue - lowestOffset;
                var enumValueName = shortRangeEnumMaterializedNames![valueShort];
                buildNames.Append(enumValueName);
                return enumValueName.Length;
            }
            if (UnderlyingType != typeof(ulong))
            {
                var underlying = singleEnumValue.ToInt64(null);
                if (veryLongRangeEnumMaterializedNames!.TryGetValue(underlying, out var valueLong))
                {
                    buildNames.Append(valueLong);
                    return valueLong.Length;
                }
                return buildNames.AppendLong(underlying);
            }
            var underlyingUlong = singleEnumValue.ToUInt64(null);
            if (veryLongRangeEnumMaterializedNames!.TryGetValue(underlyingUlong, out var valueULong))
            {
                buildNames.Append(valueULong);
                return valueULong.Length;
            }
            return buildNames.AppendULong(underlyingUlong);
        }
        try
        {
            return EnumExtendedSpanFormattable(encoder, singleEnumValue, buildNames, format, provider, formattingFlags);
        }
        catch (FormatException)
        {
            return EnumExtendedSpanFormattable(encoder, singleEnumValue, buildNames, null, provider,  formattingFlags);
        }
    }

    public StringBearerSpanFormattable<TEnumValue> StringBearerSpanFormattable { get; }

    protected virtual string? CachedResult(TEnumValue toFormat, ReadOnlySpan<char> format, IFormatProvider? provider) => null;

    private int EnumExtendedSpanFormattable(IEncodingTransfer encoder,  Enum toFormat, Span<char> destination
      , ReadOnlySpan<char> formatString, IFormatProvider? provider, FormattingHandlingFlags formattingFlags)
    {
        return EnumExtendedSpanFormattable(encoder, (TEnumValue)toFormat, destination, formatString, provider, formattingFlags);
    }

    private int EnumExtendedSpanFormattable(IEncodingTransfer encoder, TEnumValue toFormat, Span<char> destination, ReadOnlySpan<char> formatString
      , IFormatProvider? provider, FormattingHandlingFlags formattingFlags)
    {
        var cachedResult = CachedResult(toFormat, formatString, provider);
        if (cachedResult != null)
        {
            destination.Append(cachedResult);
            return cachedResult.Length;
        }

        var buildVanillaName = stackalloc char[allValuesCharCount].ResetMemory();
        var vanillaSize      = SourceEnumNamesFromEnum(encoder, toFormat, buildVanillaName, null, provider, formattingFlags);
        var vanillaEnumNames = buildVanillaName[..vanillaSize];
        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out _ , out var layout, out _, out _, out var suffix);

        var enumNextSpan = destination;
        var charsAdded   = 0;
        if (prefix.Length > 0)
        {
            charsAdded   += encoder.TransferPrefix(formattingFlags.HasEncodeBoundsFlag(), prefix, enumNextSpan, 0);
            enumNextSpan =  enumNextSpan[charsAdded..];
        }
        
        var lastAdded  = enumNextSpan.PadAndAlign(vanillaEnumNames, layout);
        enumNextSpan =  enumNextSpan[lastAdded..];
        charsAdded += lastAdded;
        
        if (suffix.Length > 0) charsAdded += encoder.TransferSuffix(suffix, enumNextSpan, 0,  formattingFlags.HasEncodeBoundsFlag());

        return charsAdded;
    }

    private class CompanionEnumAdapter(EnumFormatProvider<TEnumValue> parent) : IEnumFormatProvider<Enum>
    {
        public bool SupportSpanFormattable => parent.SupportSpanFormattable;
        public bool SupportStyleToString => parent.SupportStyleToString;
        public Type ForType => typeof(Enum);

        public IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable
        {
            if (typeof(TEnum) == typeof(TEnumValue))
            {
                return (IStructEnumFormatProvider<TEnum>)parent;
            }
            return null;
        }

        public IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum
        {
            if (typeof(TEnum).IsAssignableFrom(typeof(TEnumValue)))
            {
                return (IEnumFormatProvider<TEnum>)parent;
            }
            return null;
        }

        public IEnumFormatProvider<Enum> AsEnumFormatProvider() => this;

        public PalantírReveal<Enum> EnumPalantír => parent.asEnumTypeStyler;

        public StringBearerSpanFormattable<Enum> StringBearerSpanFormattable => parent.asEnumSpanFormattable;
    }
}
