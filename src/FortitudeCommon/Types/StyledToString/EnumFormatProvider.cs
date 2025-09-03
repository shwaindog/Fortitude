using System.Collections.Concurrent;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes;

// ReSharper disable InconsistentlySynchronizedField

namespace FortitudeCommon.Types.StyledToString;

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
        return formatter.AsStructEnumFormatProvider<TEnumValue>()!;
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
        return formatter.AsEnumFormatProvider<TEnum>()!;
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

public interface IEnumFormatter : ICustomFormattableProvider
{
    IStructEnumFormatProvider<TEnum>? AsStructEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable;

    IEnumFormatProvider<TEnum>? AsEnumFormatProvider<TEnum>() where TEnum : Enum;
}

public interface IStructEnumFormatProvider<in TEnum> : IEnumFormatter, ICustomTypeStylerProvider<TEnum>, ICustomSpanFormattableProvider<TEnum> { }

public interface IEnumFormatProvider<in TEnum> : IEnumFormatter, ICustomTypeStylerProvider<TEnum>, ICustomSpanFormattableProvider<TEnum>
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

    private ConcurrentDictionary<long, string>? longRangeEnumMaterializedNames;

    private readonly string enumName;

    public EnumFormatProvider()
    {
        var enumType = typeof(TEnumValue);
        enumName = enumType.Name;

        enumValues = (TEnumValue[])Enum.GetValues(enumType);
        TEnumValue lowest  = enumValues.Min();
        TEnumValue highest = enumValues.Max();

        var range = (highest.ToInt64(null) - lowest.ToInt64(null));
        if (range < 1024L)
        {
            isShortRangeEnum = true;
            lowestOffset     = lowest.ToInt64(null);

            shortRangeEnumMaterializedNames = new string[range + 1];

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

            longRangeEnumMaterializedNames = new ConcurrentDictionary<long, string>();
            foreach (TEnumValue item in enumValues)
            {
                var name       = Enum.GetName(enumType, item)!;
                var nameLength = name.Length;
                if (longestNameCharCount < nameLength)
                {
                    longestNameCharCount = nameLength;
                }
                allValuesCharCount += nameLength + 2;
                longRangeEnumMaterializedNames.TryAdd(item.ToInt64(null), name);
            }
        }
        CustomTypeStyler      = EnumStyler;
        CustomSpanFormattable = EnumExtendedSpanFormattable;

        isFlagsEnum = enumType.GetCustomAttributes<FlagsAttribute>().Any();
        ForType     = enumType;
    }

    public bool SupportSpanFormattable => true;

    public bool SupportStyleToString => true;

    public Type ForType { get; }

    public IStructEnumFormatProvider<TEnum>? AsStructEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable =>
        typeof(TEnum) == ForType ? (IStructEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<TEnum>? AsEnumFormatProvider<TEnum>() where TEnum : Enum =>
        typeof(TEnum) == ForType ? (IEnumFormatProvider<TEnum>)this : null;

    public CustomTypeStyler<TEnumValue> CustomTypeStyler { get; }

    public StyledTypeBuildResult EnumStyler(TEnumValue toFormatEnum, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(toFormatEnum);
        using (var sb = tb.StartDelimitedStringBuilder())
        {
            var buildNames = stackalloc char[allValuesCharCount].ResetMemory();
            SourceEnumNamesFromEnum(toFormatEnum, buildNames, ReadOnlySpan<char>.Empty);
            sb.Append(buildNames, 0, buildNames.PopulatedLength());
        }

        return tb.Complete();
    }

    private int SourceEnumNamesFromEnum(TEnumValue enumValue, Span<char> buildNames, ReadOnlySpan<char> format, IFormatProvider? provider = null)
    {
        if (!isFlagsEnum)
        {
            return SourceSingleNameFromEnum(enumValue, buildNames, format, provider);
        }
        var countChars = 0;
        foreach (var eachEnumValue in enumValues)
        {
            if (eachEnumValue.CompareTo(enumValue) != 0) continue;
            countChars += SourceSingleNameFromEnum(eachEnumValue, buildNames, format, provider);
            return countChars;
        }
        // else check each flag combination
        var hasWrittenValues = false;
        foreach (var eachEnumValue in enumValues)
            if (enumValue.HasFlag(eachEnumValue))
            {
                if (hasWrittenValues)
                {
                    countChars += 2;
                    buildNames.Append(", ");
                }
                countChars += SourceSingleNameFromEnum(eachEnumValue, buildNames, format, provider);
            }
        return countChars;
    }

    private int SourceSingleNameFromEnum(TEnumValue singleEnumValue, Span<char> buildNames, ReadOnlySpan<char> format
      , IFormatProvider? provider = null)
    {
        
        if (format.Length == 0)
        {
            if (isShortRangeEnum)
            {
                var enumValue     = singleEnumValue.ToInt64(null);
                if (enumValue < lowestOffset || enumValue > (lowestOffset + shortRangeEnumMaterializedNames!.Length))
                {
                    longRangeEnumMaterializedNames ??= new ConcurrentDictionary<long, string>();
                    if (!longRangeEnumMaterializedNames!.TryGetValue(enumValue, out var outOfRangeValue))
                    {
                        outOfRangeValue = enumValue.ToString();
                        longRangeEnumMaterializedNames.TryAdd(enumValue, enumValue.ToString());
                    }
                    buildNames.Append(outOfRangeValue);
                    return outOfRangeValue.Length;
                }
                var valueShort    = (short)enumValue - lowestOffset;
                var enumValueName = shortRangeEnumMaterializedNames![valueShort];
                buildNames.Append(enumValueName);
                return enumValueName.Length;
            }
            var underlying = singleEnumValue.ToInt64(null);
            if (longRangeEnumMaterializedNames!.TryGetValue(underlying, out var value))
            {
                buildNames.Append(value);
                return value.Length;
            }
        }
        else
        {
            try
            {
                if (singleEnumValue.TryFormat(buildNames, out var charsWritten, format, provider))
                {
                    return charsWritten;
                }
                else
                {
                    return EnumExtendedSpanFormattable(singleEnumValue, buildNames, null, provider);
                }
            }
            catch (FormatException)
            {
                return EnumExtendedSpanFormattable(singleEnumValue, buildNames, null, provider);
            }
        }
        return 0;
    }

    public CustomSpanFormattable<TEnumValue> CustomSpanFormattable { get; }

    protected virtual string? CachedResult(TEnumValue toFormat, ReadOnlySpan<char> format, IFormatProvider? provider) => null;

    private int EnumExtendedSpanFormattable(TEnumValue toFormat, Span<char> destination, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var cachedResult = CachedResult(toFormat, format, provider);
        if (cachedResult != null)
        {
            destination.Append(cachedResult);
            return cachedResult.Length;
        }

        var buildVanillaName = stackalloc char[allValuesCharCount].ResetMemory();
        var vanillaSize      = SourceEnumNamesFromEnum(toFormat, buildVanillaName, format, provider);
        var vanillaEnumNames = buildVanillaName[..vanillaSize];
        format.ExtractStringFormatStages(out _, out var layout, out _);

        return destination.PadAndAlign(vanillaEnumNames, layout);
    }
}
