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
        if (!type.IsEnum) { throw new ArgumentException($"Type '{typeof(TEnumValue).FullName}' is not an enum."); }
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
        if (!type.IsEnum) { throw new ArgumentException($"Type '{typeof(TEnum).FullName}' is not an enum."); }
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
        if (!enumType.IsEnum) { throw new ArgumentException($"Type '{enumType.FullName}' is not an enum."); }
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
    IEnumFormatProvider<Enum>?  AsEnumFormatProvider();
}

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
public interface IStructEnumFormatProvider<in TEnum> : IEnumFormatter, IStringBearerRevelStateProvider<TEnum>
    #pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
  , IStringBearerSpanFormattableProvider<TEnum>
    where TEnum : ISpanFormattable? { }

public interface IEnumFormatProvider<in TEnum> : IEnumFormatter, IStringBearerRevelStateProvider<TEnum>, IStringBearerSpanFormattableProvider<TEnum>
    where TEnum : ISpanFormattable 
{ }

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
                if (longestNameCharCount < nameLength) { longestNameCharCount = nameLength; }
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
                if (longestNameCharCount < nameLength) { longestNameCharCount = nameLength; }
                allValuesCharCount += nameLength + 2;
                veryLongRangeEnumMaterializedNames.TryAdd((Int128)item.ToDecimal(null), name);
            }
        }
        EnumPalantír                = EnumStyler;
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

    public IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable? =>
        typeof(TEnum) == ForType ? (IStructEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum =>
        typeof(TEnum).IsAssignableFrom(ForType) ? (IEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<Enum>? AsEnumFormatProvider() => typeof(Enum).IsAssignableFrom(ForType) ? EnumAdapterCompanion : null;

    public PalantírReveal<TEnumValue> EnumPalantír { get; }

    public AppendSummary EnumStyler(TEnumValue toFormatEnum, ITheOneString tos)
    {
        var tb = tos.StartSimpleContentType(toFormatEnum);
        using (var sb = tb.StartDelimitedStringBuilder())
        {
            var buildNames = stackalloc char[allValuesCharCount].ResetMemory();
            SourceEnumNamesFromEnum(toFormatEnum, buildNames, ReadOnlySpan<char>.Empty, tos.CurrentStyledTypeFormatter.ContentEncoder
                                  , tos.CurrentStyledTypeFormatter.LayoutEncoder);
            sb.Append(buildNames, 0, buildNames.PopulatedLength());
        }

        return tb.Complete();
    }

    public AppendSummary EnumStyler(Enum toFormatEnum, ITheOneString tos)
    {
        return EnumStyler((TEnumValue)toFormatEnum, tos);
    }

    private int SourceEnumNamesFromEnum(TEnumValue enumValue, Span<char> buildNames, ReadOnlySpan<char> format
      , IEncodingTransfer enumEncoder, IEncodingTransfer joinEncoder, IFormatProvider? provider = null
      , FormatSwitches formattingFlags = FormatSwitches.DefaultCallerTypeFlags)
    {
        if (!isFlagsEnum) { return SourceSingleNameFromEnum(enumValue, buildNames, format, enumEncoder, joinEncoder, provider, formattingFlags); }
        var countChars = 0;
        // check if this is a single flag covering all flags set
        foreach (var eachEnumValue in enumValues)
        {
            if (eachEnumValue.CompareTo(enumValue) != 0) continue;
            countChars += SourceSingleNameFromEnum(eachEnumValue, buildNames, format, enumEncoder, joinEncoder, provider, formattingFlags);
            return countChars;
        }
        // check if this is a bit set not in the enum
        var remainingFlags = enumValue.ToUInt64(null);
        foreach (var eachEnumValue in enumValues)
        {
            var flag = eachEnumValue.ToUInt64(null);
            remainingFlags &= ~flag;
        }
        if (remainingFlags != 0)
        {
            if (UnderlyingType != typeof(ulong))
            {
                var underlying = enumValue.ToInt64(null);
                return buildNames.AppendLong(underlying);
            }
            remainingFlags = enumValue.ToUInt64(null);
            return buildNames.AppendULong(remainingFlags);
        }
        // else check each flag combination
        var hasWrittenValues = false;
        remainingFlags = enumValue.ToUInt64(null);
        if (remainingFlags.CompareTo(default(TEnumValue)) != 0)
        {
            foreach (var eachEnumValue in enumValues)
            {
                var flag = eachEnumValue.ToUInt64(null);
                if ((remainingFlags & flag) != 0)
                {
                    if (hasWrittenValues)
                    {
                        countChars += 2;
                        buildNames.Append(", ");
                    }
                    countChars += SourceSingleNameFromEnum(eachEnumValue, buildNames, format, enumEncoder, joinEncoder, provider, formattingFlags);
                    hasWrittenValues = true;
                    remainingFlags &= ~flag;
                    if (remainingFlags.CompareTo(default(TEnumValue)) == 0) break;
                }
            }
        }
        else { countChars += SourceSingleNameFromEnum(enumValue, buildNames, format, enumEncoder, joinEncoder, provider, formattingFlags); }
        return countChars;
    }

    private int SourceSingleNameFromEnum(TEnumValue singleEnumValue, Span<char> buildNames, ReadOnlySpan<char> format
      , IEncodingTransfer enumEncoder, IEncodingTransfer joinEncoder, IFormatProvider? provider = null
      , FormatSwitches formattingFlags = FormatSwitches.DefaultCallerTypeFlags)
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
        try { return EnumExtendedSpanFormattable(singleEnumValue, buildNames, format, enumEncoder, joinEncoder, provider, formattingFlags); }
        catch (FormatException)
        {
            return EnumExtendedSpanFormattable(singleEnumValue, buildNames, null, enumEncoder, joinEncoder, provider, formattingFlags);
        }
    }

    public StringBearerSpanFormattable<TEnumValue> StringBearerSpanFormattable { get; }

    protected virtual string? CachedResult(TEnumValue toFormat, ReadOnlySpan<char> format, IFormatProvider? provider) => null;

    private int EnumExtendedSpanFormattable(Enum toFormat, Span<char> destination
      , ReadOnlySpan<char> formatString, IEncodingTransfer enumEncoder, IEncodingTransfer joinEncoder, IFormatProvider? provider
      , FormatSwitches formattingFlags)
    {
        return EnumExtendedSpanFormattable((TEnumValue)toFormat, destination, formatString, enumEncoder, joinEncoder, provider, formattingFlags);
    }

    private int EnumExtendedSpanFormattable(TEnumValue toFormat, Span<char> destination, ReadOnlySpan<char> formatString
      , IEncodingTransfer enumEncoder, IEncodingTransfer joinEncoder, IFormatProvider? provider, FormatSwitches formattingFlags)
    {
        var cachedResult = CachedResult(toFormat, formatString, provider);
        if (cachedResult != null)
        {
            destination.Append(cachedResult);
            return cachedResult.Length;
        }

        Span<char> buildVanillaName = stackalloc char[allValuesCharCount];

        formatString.ExtractExtendedStringFormatStages
            (out var prefix, out _, out var extendLengthRange, out var layout, out var splitJoinRange,
             out var format, out var suffix);

        int vanillaSize;
        switch (format)
        {
            case "":
            case "G":
            case "g":
            case "F":
            case "f":
            case "D":
            case "d":
            case "X":
            case "x":
                if (!toFormat.TryFormat(buildVanillaName, out vanillaSize, format, provider))
                {
                    vanillaSize = SourceEnumNamesFromEnum(toFormat, buildVanillaName, null, enumEncoder, joinEncoder, provider, formattingFlags);
                }
                break;
            default:
                vanillaSize = SourceEnumNamesFromEnum(toFormat, buildVanillaName, null, enumEncoder, joinEncoder, provider, formattingFlags);
                break;
        }
        var vanillaEnumNames = buildVanillaName[..vanillaSize];

        var enumNextSpan = destination;
        var charsAdded   = 0;
        if (prefix.Length > 0)
        {
            charsAdded += (formattingFlags.HasEncodeBoundsFlag() ? enumEncoder : joinEncoder)
                .TransferPrefix(formattingFlags.HasEncodeBoundsFlag() || joinEncoder.Type != EncodingType.PassThrough, prefix
                                                   , enumNextSpan, 0);
            enumNextSpan = enumNextSpan[charsAdded..];
        }

        var rawSourceFrom   = 0;
        var rawSourceTo     = Math.Clamp(vanillaSize, 0, vanillaEnumNames.Length);
        var rawCappedLength = Math.Min(vanillaSize, rawSourceTo);
        if (!extendLengthRange.IsAllRange())
        {
            extendLengthRange = extendLengthRange.BoundRangeToLength(vanillaSize);
            var start = extendLengthRange.Start;
            if (start.IsFromEnd || start.Value > 0)
            {
                rawSourceFrom = start.IsFromEnd
                    ? Math.Max(0, vanillaSize - start.Value)
                    : Math.Min(vanillaSize, start.Value);
            }
            var end = extendLengthRange.End;
            if (!end.IsFromEnd || end.Value > 0)
            {
                rawSourceTo = end.IsFromEnd
                    ? Math.Max(rawSourceFrom, rawSourceTo - end.Value)
                    : Math.Min(rawSourceTo, rawSourceFrom + end.Value);
            }
            rawCappedLength = Math.Clamp(rawSourceTo - rawSourceFrom, 0, rawCappedLength);
        }

        int lastAdded;
        if (layout.Length == 0 && splitJoinRange.IsNoSplitJoin)
        {
            lastAdded = (formattingFlags.HasEncodeInnerContent() ? enumEncoder : joinEncoder)
                .OverwriteTransfer(vanillaEnumNames, rawSourceFrom, enumNextSpan, 0, rawCappedLength);

            enumNextSpan =  enumNextSpan[lastAdded..];
            charsAdded   += lastAdded;
            if (suffix.Length > 0)
                charsAdded += (formattingFlags.HasEncodeBoundsFlag() ? enumEncoder : joinEncoder)
                    .TransferSuffix(suffix, enumNextSpan, 0, formattingFlags.HasEncodeBoundsFlag() || joinEncoder.Type != EncodingType.PassThrough);
            return charsAdded;
        }
        var        alignedLength = rawCappedLength.CalculatePaddedAlignedLength(layout) + 256;
        Span<char> sourceInSpan  = stackalloc char[rawCappedLength];
        sourceInSpan.Append(vanillaEnumNames, rawSourceFrom, rawCappedLength);
        Span<char> padSpan = stackalloc char[alignedLength];

        int padSize;

        if (!splitJoinRange.IsNoSplitJoin)
        {
            Span<char> splitJoinResultSpan = stackalloc char[alignedLength + 256];

            var size = splitJoinRange.ApplySplitJoin(splitJoinResultSpan, sourceInSpan, enumEncoder, joinEncoder);
            splitJoinResultSpan = splitJoinResultSpan[..size];

            padSize   = padSpan.PadAndAlign(splitJoinResultSpan, layout);
            padSize   = Math.Min(padSize, alignedLength);
            lastAdded = PassThroughEncodingTransfer.FinalEncoder.OverwriteTransfer(padSpan[..padSize], enumNextSpan, 0);
        }
        else
        {
            padSize   = padSpan.PadAndAlign(sourceInSpan, layout);
            padSize   = Math.Min(padSize, alignedLength);
            lastAdded = (formattingFlags.HasEncodeInnerContent() ? enumEncoder : joinEncoder)
                .OverwriteTransfer(padSpan[..padSize], enumNextSpan, 0);
        }

        enumNextSpan =  enumNextSpan[lastAdded..];
        charsAdded   += lastAdded;

        if (suffix.Length > 0)
        {
            charsAdded += (formattingFlags.HasEncodeBoundsFlag() ? enumEncoder : joinEncoder)
                .TransferSuffix(suffix, enumNextSpan, 0
                              , formattingFlags.HasEncodeBoundsFlag() || joinEncoder.Type != EncodingType.PassThrough);
        }

        return charsAdded;
    }

    private class CompanionEnumAdapter(EnumFormatProvider<TEnumValue> parent) : IEnumFormatProvider<Enum>
    {
        public bool SupportSpanFormattable => parent.SupportSpanFormattable;
        public bool SupportStyleToString => parent.SupportStyleToString;
        public Type ForType => typeof(Enum);

        public IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable?
        {
            if (typeof(TEnum) == typeof(TEnumValue)) { return (IStructEnumFormatProvider<TEnum>)parent; }
            return null;
        }

        public IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum
        {
            if (typeof(TEnum).IsAssignableFrom(typeof(TEnumValue))) { return (IEnumFormatProvider<TEnum>)parent; }
            return null;
        }

        public IEnumFormatProvider<Enum> AsEnumFormatProvider() => this;

        public PalantírReveal<Enum> EnumPalantír => parent.asEnumTypeStyler;

        public StringBearerSpanFormattable<Enum> StringBearerSpanFormattable => parent.asEnumSpanFormattable;
    }
}
