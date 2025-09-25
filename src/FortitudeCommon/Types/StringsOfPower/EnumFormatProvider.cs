using System.Collections.Concurrent;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

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
    IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable;

    IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum;
    IEnumFormatProvider<Enum>? AsEnumFormatProvider();
}

public interface IStructEnumFormatProvider<in TEnum> : IEnumFormatter, IStringBearerRevelStateProvider<TEnum>, IStringBearerSpanFormattableProvider<TEnum> { }

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

    private ConcurrentDictionary<Int128, string>? verylongRangeEnumMaterializedNames;

    private readonly string enumName;

    private IEnumFormatProvider<Enum>? enumAdapterCompanion;

    private StringBearerRevealState<Enum>     asEnumTypeStyler;
    private StringBearerSpanFormattable<Enum> asEnumSpanFormattable;

    public EnumFormatProvider()
    {
        var enumType = typeof(TEnumValue);
        enumName = enumType.Name;

        enumValues = (TEnumValue[])Enum.GetValues(enumType);
        TEnumValue lowest  = enumValues.Min();
        TEnumValue highest = enumValues.Max();

        var enumValuesUnderlying = Enum.GetValuesAsUnderlyingType(enumType);
        var firstValue           = enumValuesUnderlying.GetValue(0);

        decimal range = highest.ToDecimal(null) - lowest.ToDecimal(null);;
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

            verylongRangeEnumMaterializedNames = new ConcurrentDictionary<Int128, string>();
            foreach (TEnumValue item in enumValues)
            {
                var name       = Enum.GetName(enumType, item)!;
                var nameLength = name.Length;
                if (longestNameCharCount < nameLength)
                {
                    longestNameCharCount = nameLength;
                }
                allValuesCharCount += nameLength + 2;
                verylongRangeEnumMaterializedNames.TryAdd((Int128)item.ToDecimal(null), name);
            }
        }
        StringBearerRevealState     = EnumStyler;
        StringBearerSpanFormattable = EnumExtendedSpanFormattable;

        asEnumTypeStyler      = EnumStyler;
        asEnumSpanFormattable = EnumExtendedSpanFormattable;

        isFlagsEnum = enumType.GetCustomAttributes<FlagsAttribute>().Any();
        ForType     = enumType;
    }

    public bool SupportSpanFormattable => true;

    public bool SupportStyleToString => true;

    public IEnumFormatProvider<Enum> EnumAdapterCompanion
    {
        get => enumAdapterCompanion ??= new CompanionEnumAdapter(this);
        set => enumAdapterCompanion = value;
    }

    public Type ForType { get; }

    public IStructEnumFormatProvider<TEnum>? AsSpanFormattableEnumFormatProvider<TEnum>() where TEnum : ISpanFormattable =>
        typeof(TEnum) == ForType ? (IStructEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<TEnum>? AsTypedEnumFormatProvider<TEnum>() where TEnum : Enum =>
        typeof(TEnum).IsAssignableFrom(ForType) ? (IEnumFormatProvider<TEnum>)this : null;

    public IEnumFormatProvider<Enum>? AsEnumFormatProvider() => typeof(Enum).IsAssignableFrom(ForType) ? EnumAdapterCompanion : null;

    public StringBearerRevealState<TEnumValue> StringBearerRevealState { get; }

    public StateExtractStringRange EnumStyler(TEnumValue toFormatEnum, ITheOneString stsa)
    {
        var tb = stsa.StartSimpleValueType(toFormatEnum);
        using (var sb = tb.StartDelimitedStringBuilder())
        {
            var buildNames = stackalloc char[allValuesCharCount].ResetMemory();
            SourceEnumNamesFromEnum(toFormatEnum, buildNames, ReadOnlySpan<char>.Empty);
            sb.Append(buildNames, 0, buildNames.PopulatedLength());
        }

        return tb.Complete();
    }

    public StateExtractStringRange EnumStyler(Enum toFormatEnum, ITheOneString stsa)
    {
        return EnumStyler((TEnumValue)toFormatEnum, stsa);
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
            if (enumValue.HasFlag(eachEnumValue) && (eachEnumValue.CompareTo((TEnumValue)default) != 0 || enumValue.CompareTo((TEnumValue)default) == 0))
            {
                if (hasWrittenValues)
                {
                    countChars += 2;
                    buildNames.Append(", ");
                }
                countChars       += SourceSingleNameFromEnum(eachEnumValue, buildNames, format, provider);
                hasWrittenValues =  true;
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
                var enumValue = singleEnumValue.ToInt64(null);
                if (enumValue < lowestOffset || enumValue > (lowestOffset + shortRangeEnumMaterializedNames!.Length))
                {
                    verylongRangeEnumMaterializedNames ??= new ConcurrentDictionary<Int128, string>();
                    if (!verylongRangeEnumMaterializedNames!.TryGetValue(enumValue, out var outOfRangeValue))
                    {
                        outOfRangeValue = enumValue.ToString();
                        verylongRangeEnumMaterializedNames.TryAdd(enumValue, enumValue.ToString());
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
            if (verylongRangeEnumMaterializedNames!.TryGetValue(underlying, out var value))
            {
                buildNames.Append(value);
                return value.Length;
            }
        }
        else
        {
            try
            {
                return EnumExtendedSpanFormattable(singleEnumValue, buildNames, format, provider);
            }
            catch (FormatException)
            {
                return EnumExtendedSpanFormattable(singleEnumValue, buildNames, null, provider);
            }
        }
        return 0;
    }

    public StringBearerSpanFormattable<TEnumValue> StringBearerSpanFormattable { get; }

    protected virtual string? CachedResult(TEnumValue toFormat, ReadOnlySpan<char> format, IFormatProvider? provider) => null;

    private int EnumExtendedSpanFormattable(Enum toFormat, Span<char> destination, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return EnumExtendedSpanFormattable((TEnumValue)toFormat, destination, format, provider);
    }

    private int EnumExtendedSpanFormattable(TEnumValue toFormat, Span<char> destination, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var cachedResult = CachedResult(toFormat, format, provider);
        if (cachedResult != null)
        {
            destination.Append(cachedResult);
            return cachedResult.Length;
        }

        var buildVanillaName = stackalloc char[allValuesCharCount].ResetMemory();
        var vanillaSize      = SourceEnumNamesFromEnum(toFormat, buildVanillaName, null, provider);
        var vanillaEnumNames = buildVanillaName[..vanillaSize];
        format.ExtractStringFormatStages(out _, out var layout, out _);

        return destination.PadAndAlign(vanillaEnumNames, layout);
    }

    private class CompanionEnumAdapter : IEnumFormatProvider<Enum>
    {
        private readonly EnumFormatProvider<TEnumValue> parent;

        public CompanionEnumAdapter(EnumFormatProvider<TEnumValue> parent)
        {
            this.parent = parent;
        }

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

        public IEnumFormatProvider<Enum>? AsEnumFormatProvider() => this;

        public StringBearerRevealState<Enum> StringBearerRevealState => parent.asEnumTypeStyler;

        public StringBearerSpanFormattable<Enum> StringBearerSpanFormattable => parent.asEnumSpanFormattable;
    }
}
