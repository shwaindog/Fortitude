using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.Options;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public record struct StyledTypeBuildResult(string TypeName, StyledTypeStringAppender TypeStringAppender, Range AppendRange)
{
    public static implicit operator StyledTypeBuildResult(StyledTypeBuilder stb) => stb.Complete();

    public static implicit operator StyledTypeStringAppender(StyledTypeBuildResult stbr) => stbr.TypeStringAppender;

    public static readonly StyledTypeBuildResult EmptyAppend =
        new("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class StyledTypeBuilder : ExplicitRecyclableObject, IDisposable
{
    protected StyleTypeBuilderPortableState PortableState = new();

    protected int StartIndex;

    protected void InitializeStyledTypeBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        PortableState.TypeBeingBuilt   = typeBeingBuilt;
        PortableState.OwningAppender   = owningAppender;
        PortableState.TypeName         = typeName;
        PortableState.TypeFormatting    = typeFormatting;
        PortableState.AppenderSettings = typeSettings;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = existingRefId;

        StartIndex = owningAppender.WriteBuffer.Length;
    }

    public bool IsComplete => PortableState.CompleteResult != null;

    public int ExistingRefId => PortableState.ExistingRefId;

    public abstract void Start();

    public Type TypeBeingBuilt => PortableState.TypeBeingBuilt;

    public StyleOptions Settings => PortableState.OwningAppender.Settings;
    
    public string TypeName => PortableState.TypeName;

    public abstract StyledTypeBuildResult Complete();

    public StyleOptions StyleSettings => PortableState.OwningAppender.Settings;

    public void Dispose()
    {
        if (!IsComplete)
        {
            PortableState.CompleteResult = Complete();
        }
    }

    protected override void InheritedStateReset()
    {
        PortableState.OwningAppender   = null!;
        PortableState.TypeName         = null!;
        PortableState.AppenderSettings = default;
        PortableState.CompleteResult   = null;
        PortableState.ExistingRefId    = 0;

        StartIndex = -1;

        MeRecyclable.StateReset();
    }

    public class StyleTypeBuilderPortableState
    {
        public TypeAppendSettings AppenderSettings;

        public Type TypeBeingBuilt { get; set; } = null!;
        public string TypeName { get; set; } = null!;

        public IStyledTypeFormatting TypeFormatting { get; set; } = null!;
        public int ExistingRefId { get; set; }

        public IStyleTypeAppenderBuilderAccess OwningAppender { get; set; } = null!;

        public StyledTypeBuildResult? CompleteResult;

        public bool IsComplete => CompleteResult != null;
    }
}

public interface ITypeBuilderComponentSource
{
    IStyleTypeBuilderComponentAccess ComponentAccess { get; }
}

public interface ITypeBuilderComponentSource<T> : ITypeBuilderComponentSource where T : StyledTypeBuilder
{
    IStyleTypeBuilderComponentAccess<T> CompAccess { get; }
}

public static class StyledTypeBuilderExtensions
{
    internal const string Null = "null";

    public static IStringBuilder AddIndents(this IStringBuilder sb, char indentChar, int settingsIndentSize, int indentLevel)
    {
        sb.Append(indentChar, indentLevel*settingsIndentSize);
        return sb;
    }

    public static TExt Next<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder =>
        stb.StyleTypeBuilder;

    public static TExt AddGoToNext<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
        {
            sb.Append(",\n");
            stb.IncrementIndent();
        }
        else
        {
            sb.Append(stb.Style.IsCompact() ? "," : ", ");
        }
        return stb.StyleTypeBuilder;
    }

    public static TExt AddGoToNext<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
        {
            stb.Sb.Append(",\n");
            stb.IncrementIndent();
        }
        else
        {
            stb.Sb.Append(stb.Style.IsCompact() ? "," : ", ");
        }
        return stb.StyleTypeBuilder;
    }

    public static IStringBuilder AppendOrNull(this IStringBuilder sb, string? value, bool inQuotes = false) =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : struct, ISpanFormattable =>
        value != null ? sb.Qt(inQuotes).Append(value.Value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull(this IStringBuilder sb, bool? value, bool inQuotes = false) =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder Append(this IStringBuilder sb, bool value, bool inQuotes = false) => 
        sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static TExt Append<TExt, TFmt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TFmt value, bool inQuotes = false)
        where TExt : StyledTypeBuilder where TFmt : ISpanFormattable =>
        stb.Sb.Append(value, stb.StyleFormatter).AnyToTypeBuilder(stb);

    public static IStringBuilder Append<T>(this IStringBuilder sb, T value, bool inQuotes = false) where T : struct, ISpanFormattable =>
        sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append(this IStringBuilder sb, string value, bool inQuotes = false) => sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string value, bool inQuotes = false)
        where TExt : StyledTypeBuilder =>
        stb.Sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder AppendFormatted<T>
    (this IStringBuilder sb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : ISpanFormattable =>
        sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);

    public static TExt AppendFormatted<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder  where T : ISpanFormattable =>
        stb.StyleFormatter.Format(value, stb.Sb, formatString).AnyToTypeBuilder(stb);

    public static IStringBuilder AppendFormattedCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder where T : ISpanFormattable =>
         stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToSb(stb.Sb);

    public static IStringBuilder AppendFormattedCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable =>
         stb.StyleFormatter.CollectionNextItemFormat(value, retrieveCount, stb.Sb, formatString).AnyToSb(stb.Sb);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, string? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendFormattedCollectionItemOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, StringBuilder? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.Format(value, 0, stb.Sb, formatString).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendCollectionItem<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T value, int retrieveCount) where TExt : StyledTypeBuilder =>
        stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToSb(stb.Sb);

    public static IStringBuilder AppendCollectionItemOrNull<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value, int retrieveCount) where TExt : StyledTypeBuilder =>
        value != null 
            ? stb.StyleFormatter.CollectionNextItem(value, retrieveCount, stb.Sb).AnyToSb(stb.Sb)
            : stb.Sb.Append(stb.Settings.NullStyle);

    public static IStringBuilder AppendCollectionItemFormattedOrNull<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value, int retrieveCount
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable =>
        value != null 
            ? stb.StyleFormatter.CollectionNextItemFormat(value.Value, retrieveCount, stb.Sb, formatString).AnyToSb(stb.Sb) 
            : stb.StyleFormatter.CollectionNextItem(Null, retrieveCount, stb.Sb).AnyToSb(stb.Sb);

    public static IStringBuilder                         AnyToSb<T>(this T _, IStringBuilder sbToReturn)                    => sbToReturn;
    
    public static TExt AnyToTypeBuilder<TExt, T>(this T _, IStyleTypeBuilderComponentAccess<TExt> typeBuilder)
        where TExt : StyledTypeBuilder  => typeBuilder.StyleTypeBuilder;
     
    public static IStringBuilder AppendFormattedOrNull<TExt, T>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString)
        where TExt : StyledTypeBuilder where T : struct, ISpanFormattable =>
        stb.StyleFormatter.Format(value, stb.Sb, formatString).AnyToSb(stb.Sb);
    

    public static IStringBuilder AppendFormatted
    (this IStringBuilder sb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
    {
        return sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder Qt(this IStringBuilder sb, bool writeQuote) => writeQuote ? sb.Append("\"") : sb;

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, ICharSequence? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
            if (addQuotes) sb.Append("\"");
        }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , IStringBuilder? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , StringBuilder? value, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendObjectOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, object? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , char[]? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var isJson    = stb.Style.IsJson();
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var hasAdded = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (hasAdded) sb.ItemNext(stb);
                    sb.Qt(true).Append(value[i]).Qt(true);
                }
            }
        }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TExt>
    (this IStyleTypeBuilderComponentAccess<TExt> stb, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).AppendFormat(formatString, value).Qt(addQuotes);
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , IStyledToStringObject? value) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
        {
            value.ToString(stb.OwningAppender);
        }
        else
        {
            sb.Append(Null);
        }
        return sb;
    }

    public static IStringBuilder Append<T, TBase, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, T value
      , CustomTypeStyler<TBase> styledToStringAction, bool? inQuotes = null)
        where T : class, TBase where TBase : class where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (addQuotes) sb.Append("\"");
        styledToStringAction(value, stb.OwningAppender);
        if (addQuotes) sb.Append("\"");
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TFmt, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TFmt? value
      , bool? inQuotes = null) where TFmt : ISpanFormattable where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            var isJson    = stb.Style.IsJson();
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var typeOfTFmt = typeof(TFmt);
                var isNumeric  = typeOfTFmt.IsNumericType();
                var isChar     = typeof(char) == typeOfTFmt;
                addQuotes |= !isNumeric || isChar;
                sb.Qt(addQuotes).Append(value).Qt(addQuotes);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TFmtStruct, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TFmtStruct? value
      , bool? inQuotes = null) where TFmtStruct : struct, ISpanFormattable where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? false;
        if (value != null)
        {
            var isJson    = stb.Style.IsJson();
            if (!isJson)
            {
                if(addQuotes) sb.Qt(addQuotes);
                sb.Append(value);
                if(addQuotes) sb.Qt(addQuotes);
            }
            else
            {
                var typeOfTFmt = typeof(TFmtStruct);
                var isNumeric  = typeOfTFmt.IsNumericType();
                var isChar     = typeof(char) == typeOfTFmt;
                addQuotes |= !isNumeric || isChar;
                sb.Qt(addQuotes).Append(value).Qt(addQuotes);
            }
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendOrNull<TToStyle, TStylerType, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TToStyle? value
      , CustomTypeStyler<TStylerType> styledToStringAction) where TToStyle : TStylerType where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
        {
            styledToStringAction(value, stb.OwningAppender);
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendFormattedCollectionItemMatchOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, int retrieveCount, string formatString) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendCollectionItem(valueBool, retrieveCount); break;
                case byte valueByte:         stb.AppendFormattedCollectionItem(valueByte, retrieveCount, formatString); break;
                case sbyte valueSByte:       stb.AppendFormattedCollectionItem(valueSByte, retrieveCount, formatString); break;
                case char valueChar:         stb.AppendFormattedCollectionItem(valueChar, retrieveCount, formatString); break;
                case short valueShort:       stb.AppendFormattedCollectionItem(valueShort, retrieveCount, formatString); break;
                case ushort valueUShort:     stb.AppendFormattedCollectionItem(valueUShort, retrieveCount, formatString); break;
                case Half valueHalfFloat:    stb.AppendFormattedCollectionItem(valueHalfFloat, retrieveCount, formatString); break;
                case int valueInt:           stb.AppendFormattedCollectionItem(valueInt, retrieveCount, formatString); break;
                case uint valueUInt:         stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case nint valueUInt:         stb.AppendFormattedCollectionItem(valueUInt, retrieveCount, formatString); break;
                case float valueFloat:       stb.AppendFormattedCollectionItem(valueFloat, retrieveCount, formatString); break;
                case long valueLong:         stb.AppendFormattedCollectionItem(valueLong, retrieveCount, formatString); break;
                case ulong valueULong:       stb.AppendFormattedCollectionItem(valueULong, retrieveCount, formatString); break;
                case double valueDouble:     stb.AppendFormattedCollectionItem(valueDouble, retrieveCount, formatString); break;
                case decimal valueDecimal:   stb.AppendFormattedCollectionItem(valueDecimal, retrieveCount, formatString); break;
                case Int128 veryLongInt:     stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case UInt128 veryLongUInt:   stb.AppendFormattedCollectionItem(veryLongUInt, retrieveCount, formatString); break;
                case BigInteger veryLongInt: stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case Complex veryLongInt:    stb.AppendFormattedCollectionItem(veryLongInt, retrieveCount, formatString); break;
                case DateTime valueDateTime: stb.AppendFormattedCollectionItem(valueDateTime, retrieveCount, formatString); break;
                case DateOnly valueDateOnly: stb.AppendFormattedCollectionItem(valueDateOnly, retrieveCount, formatString); break;
                case TimeSpan valueTimeSpan: stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case TimeOnly valueTimeSpan: stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case Rune valueTimeSpan:     stb.AppendFormattedCollectionItem(valueTimeSpan, retrieveCount, formatString); break;
                case Guid valueGuid:         stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString); break;
                case IPNetwork valueIntPtr:  stb.AppendFormattedCollectionItem(valueIntPtr, retrieveCount, formatString); break;
                case char[] valueCharArray:  stb.AppendCollectionItem(valueCharArray, retrieveCount); break;
                case string valueString:     stb.AppendFormattedCollectionItemOrNull(valueString, retrieveCount, formatString); break;
                case Enum valueEnum:         stb.AppendFormattedCollectionItem(valueEnum, retrieveCount, formatString); break;
                case Version valueGuid:      stb.AppendFormattedCollectionItem(valueGuid, retrieveCount, formatString); break;
                case IPAddress valueIntPtr:  stb.AppendFormattedCollectionItem(valueIntPtr,retrieveCount, formatString); break;
                case Uri valueUri:           stb.AppendFormattedCollectionItem(valueUri, retrieveCount, formatString); break;

                case IFrozenString valueFrozenString:   stb.AppendFormattedCollectionItemOrNull(valueFrozenString, retrieveCount, formatString); break;
                case IStringBuilder valueStringBuilder: stb.AppendFormattedCollectionItemOrNull(valueStringBuilder, retrieveCount, formatString); break;
                case StringBuilder valueSb:             stb.AppendFormattedCollectionItemOrNull(valueSb, retrieveCount, formatString); break;

                case IStyledToStringObject styledToStringObj: stb.AppendOrNull(styledToStringObj); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default: stb.AppendCollectionItem(value, retrieveCount); break;
            }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, string formatString, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        var sb        = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:         sb.Append(valueBool, addQuotes); break;
                case byte valueByte:         stb.AppendFormatted(valueByte, formatString); break;
                case sbyte valueSByte:       stb.AppendFormatted(valueSByte, formatString); break;
                case char valueChar:         stb.AppendFormatted(valueChar, formatString); break;
                case short valueShort:       stb.AppendFormatted(valueShort, formatString); break;
                case ushort valueUShort:     stb.AppendFormatted(valueUShort, formatString); break;
                case Half valueHalfFloat:    stb.AppendFormatted(valueHalfFloat, formatString); break;
                case int valueInt:           stb.AppendFormatted(valueInt, formatString); break;
                case uint valueUInt:         stb.AppendFormatted(valueUInt, formatString); break;
                case nint valueUInt:         stb.AppendFormatted(valueUInt, formatString); break;
                case float valueFloat:       stb.AppendFormatted(valueFloat, formatString); break;
                case long valueLong:         stb.AppendFormatted(valueLong, formatString); break;
                case ulong valueULong:       stb.AppendFormatted(valueULong, formatString); break;
                case double valueDouble:     stb.AppendFormatted(valueDouble, formatString); break;
                case decimal valueDecimal:   stb.AppendFormatted(valueDecimal, formatString); break;
                case Int128 veryLongInt:     stb.AppendFormatted(veryLongInt, formatString); break;
                case UInt128 veryLongUInt:   stb.AppendFormatted(veryLongUInt, formatString); break;
                case BigInteger veryLongInt: stb.AppendFormatted(veryLongInt, formatString); break;
                case Complex veryLongInt:    stb.AppendFormatted(veryLongInt, formatString); break;
                case DateTime valueDateTime: stb.AppendFormatted(valueDateTime, formatString); break;
                case DateOnly valueDateOnly: stb.AppendFormatted(valueDateOnly, formatString); break;
                case TimeSpan valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString); break;
                case TimeOnly valueTimeSpan: stb.AppendFormatted(valueTimeSpan, formatString); break;
                case Rune valueTimeSpan:     stb.AppendFormatted(valueTimeSpan, formatString); break;
                case Guid valueGuid:         stb.AppendFormatted(valueGuid, formatString); break;
                case IPNetwork valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:     sb.AppendFormat(formatString, valueString); break;
                case Enum valueEnum:         stb.AppendFormatted(valueEnum, formatString); break;
                case Version valueGuid:      stb.AppendFormatted(valueGuid, formatString); break;
                case IPAddress valueIntPtr:  stb.AppendFormatted(valueIntPtr, formatString); break;
                case Uri valueUri:           stb.AppendFormatted(valueUri, formatString); break;

                case ICharSequence valueCharSequence:   stb.AppendFormattedOrNull(valueCharSequence, formatString, addQuotes); break;
                case StringBuilder valueSb:             stb.AppendFormattedOrNull(valueSb, formatString, addQuotes); break;

                case IStyledToStringObject styledToStringObj: stb.AppendOrNull(styledToStringObj); break;
                case IEnumerator:
                case IEnumerable:
                    var type = typeof(TValue);
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll<TValue>(keyedCollectionBuilder, value, formatString);
                        keyedCollectionBuilder.Complete();
                        break;
                    }
                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll<TValue>(orderedCollectionBuilder, value, formatString);
                    orderedCollectionBuilder.Complete();
                    break;

                default: sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return sb;
    }


    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, IStyledToStringObject? value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        inQuotes |= stb.Style.IsJson();
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            value.ToString(stb.OwningAppender);
            if (addQuotes) sb.Append("\"");
        }
        else
        {
            sb.Append(Null);
        }
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, ICharSequence? value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, string? value, int startIndex, int length, string? formatString
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, char[]? value, int startIndex, int length, string? formatString
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length, formatString).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, ICharSequence? value, int startIndex, int length
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, StringBuilder? value, int startIndex, int length
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            sb.Qt(addQuotes).Append(value, startIndex, length).Qt(addQuotes);
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }

    public static IStringBuilder AppendNullOrValue<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TValue value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb = stb.Sb;
        sb.AddNullOrValue(value, stb, inQuotes);
        return sb;
    }

    public static TExt AppendCollectionItemMatchOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, TValue value, int retrieveCount)
        where TExt : StyledTypeBuilder
    {
        if (value != null)
            switch (value)
            {
                case bool valueBool:         stb.AppendCollectionItem(valueBool, retrieveCount); break;
                case byte valueByte:         stb.AppendCollectionItem(valueByte, retrieveCount); break;
                case sbyte valueSByte:       stb.AppendCollectionItem(valueSByte, retrieveCount); break;
                case char valueChar:         stb.AppendCollectionItem(valueChar, retrieveCount); break;
                case short valueShort:       stb.AppendCollectionItem(valueShort, retrieveCount); break;
                case ushort valueUShort:     stb.AppendCollectionItem(valueUShort, retrieveCount); break;
                case Half valueHalfFloat:    stb.AppendCollectionItem(valueHalfFloat, retrieveCount); break;
                case int valueInt:           stb.AppendCollectionItem(valueInt, retrieveCount); break;
                case uint valueUInt:         stb.AppendCollectionItem(valueUInt, retrieveCount); break;
                case nint valueUInt:         stb.AppendCollectionItem(valueUInt, retrieveCount); break;
                case float valueFloat:       stb.AppendCollectionItem(valueFloat, retrieveCount); break;
                case long valueLong:         stb.AppendCollectionItem(valueLong, retrieveCount); break;
                case ulong valueULong:       stb.AppendCollectionItem(valueULong, retrieveCount); break;
                case double valueDouble:     stb.AppendCollectionItem(valueDouble, retrieveCount); break;
                case decimal valueDecimal:   stb.AppendCollectionItem(valueDecimal, retrieveCount); break;
                case Int128 veryLongInt:     stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case UInt128 veryLongUInt:   stb.AppendCollectionItem(veryLongUInt, retrieveCount); break;
                case BigInteger veryLongInt: stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case Complex veryLongInt:    stb.AppendCollectionItem(veryLongInt, retrieveCount); break;
                case DateTime valueDateTime: stb.AppendCollectionItem(valueDateTime, retrieveCount); break;
                case DateOnly valueDateOnly: stb.AppendCollectionItem(valueDateOnly, retrieveCount); break;
                case TimeSpan valueTimeSpan: stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case TimeOnly valueTimeSpan: stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case Rune valueTimeSpan:     stb.AppendCollectionItem(valueTimeSpan, retrieveCount); break;
                case Guid valueGuid:         stb.AppendCollectionItem(valueGuid, retrieveCount); break;
                case IPNetwork valueIntPtr:  stb.AppendCollectionItem(valueIntPtr, retrieveCount); break;
                case char[] valueCharArray:  stb.AppendCollectionItem(valueCharArray, retrieveCount); break;
                case string valueString:     stb.AppendCollectionItem(valueString, retrieveCount); break;
                case Enum valueEnum:         stb.AppendCollectionItem(valueEnum, retrieveCount); break;
                case Version valueGuid:      stb.AppendCollectionItem(valueGuid, retrieveCount); break;
                case IPAddress valueIntPtr:  stb.AppendCollectionItem(valueIntPtr, retrieveCount); break;
                case Uri valueUri:           stb.AppendCollectionItem(valueUri, retrieveCount); break;

                case IFrozenString valueFrozenString:  stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;
                case IStringBuilder valueFrozenString: stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;
                case StringBuilder valueFrozenString:  stb.AppendCollectionItem(valueFrozenString, retrieveCount); break;

                case IStyledToStringObject styledToStringObject: stb.AppendOrNull(styledToStringObject); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value!);
                    orderedCollectionBuilder.Complete();
                    break;

                default: stb.AppendCollectionItem(value, retrieveCount); break;
            }
        else
            stb.Sb.Append(Null);
        return stb.StyleTypeBuilder;
    }

    public static TExt AddNullOrValue<TValue, TExt>(this IStringBuilder sb, TValue value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            switch (value)
            {
                case bool valueBool:         sb.Append(valueBool, addQuotes); break;
                case byte valueByte:         sb.Append(valueByte, addQuotes); break;
                case sbyte valueSByte:       sb.Append(valueSByte, addQuotes); break;
                case char valueChar:         sb.Append(valueChar, addQuotes); break;
                case short valueShort:       sb.Append(valueShort, addQuotes); break;
                case ushort valueUShort:     sb.Append(valueUShort, addQuotes); break;
                case Half valueHalfFloat:    sb.Append(valueHalfFloat, addQuotes); break;
                case int valueInt:           sb.Append(valueInt, addQuotes); break;
                case uint valueUInt:         sb.Append(valueUInt, addQuotes); break;
                case nint valueUInt:         sb.Append(valueUInt, addQuotes); break;
                case float valueFloat:       sb.Append(valueFloat, addQuotes); break;
                case long valueLong:         sb.Append(valueLong, addQuotes); break;
                case ulong valueULong:       sb.Append(valueULong, addQuotes); break;
                case double valueDouble:     sb.Append(valueDouble, addQuotes); break;
                case decimal valueDecimal:   sb.Append(valueDecimal, addQuotes); break;
                case Int128 veryLongInt:     sb.Append(veryLongInt, addQuotes); break;
                case UInt128 veryLongUInt:   sb.Append(veryLongUInt, addQuotes); break;
                case BigInteger veryLongInt: sb.Append(veryLongInt, addQuotes); break;
                case Complex veryLongInt:    sb.Append(veryLongInt, addQuotes); break;
                case DateTime valueDateTime: sb.Append(valueDateTime, addQuotes); break;
                case DateOnly valueDateOnly: sb.Append(valueDateOnly, addQuotes); break;
                case TimeSpan valueTimeSpan: sb.Append(valueTimeSpan, addQuotes); break;
                case TimeOnly valueTimeSpan: sb.Append(valueTimeSpan, addQuotes); break;
                case Rune valueTimeSpan:     sb.Append(valueTimeSpan, addQuotes); break;
                case Guid valueGuid:         sb.Append(valueGuid, addQuotes); break;
                case IPNetwork valueIntPtr:  sb.Append(valueIntPtr, addQuotes); break;
                case char[] valueCharArray:  stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:     stb.AppendOrNull(valueString, addQuotes); break;
                case Enum valueEnum:         stb.AppendFormatted(valueEnum, "{0}"); break;
                case Version valueGuid:      stb.AppendFormatted(valueGuid, "{0}"); break;
                case IPAddress valueIntPtr:  stb.AppendFormatted(valueIntPtr, "{0}"); break;
                case Uri valueUri:           stb.AppendFormatted(valueUri, "{0}"); break;

                case IFrozenString valueFrozenString:  stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case IStringBuilder valueFrozenString: stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case StringBuilder valueFrozenString:  stb.AppendOrNull(valueFrozenString, addQuotes); break;

                case IStyledToStringObject styledToStringObject: stb.AppendOrNull(styledToStringObject); break;
                case IEnumerator:
                case IEnumerable:
                    var type = value.GetType();
                    if (type.IsGenericType && type.IsKeyedCollection())
                    {
                        var keyedCollectionBuilder = stb.OwningAppender.StartKeyedCollectionType(value, "");
                        KeyedCollectionGenericAddAllInvoker.CallAddAll(keyedCollectionBuilder, value);
                        keyedCollectionBuilder.Complete();
                        break;
                    }

                    var orderedCollectionBuilder = stb.OwningAppender.StartSimpleCollectionType(value, "");
                    SimpleOrderedCollectionGenericAddAllInvoker.CallAddAll(orderedCollectionBuilder, value!);
                    orderedCollectionBuilder.Complete();
                    break;

                default: sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return stb.StyleTypeBuilder;
    }

    public static void StartCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, bool hasElements)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, hasElements);
    }

    public static IStringBuilder FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.Settings.IndentChar, stb.Settings.IndentRepeat(stb.OwningAppender.IndentLevel));

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").FieldEnd(stb);
        else
            stb.Sb.Append(fieldName).FieldEnd(stb);

        return stb.Sb;
    }

    public static IStyleTypeBuilderComponentAccess<TExt> FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName
      , IStyleTypeBuilderComponentAccess<TExt> toReturn)
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.Settings.IndentChar, stb.Settings.IndentRepeat(stb.OwningAppender.IndentLevel));

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").FieldEnd(stb);
        else
            stb.Sb.Append(fieldName).FieldEnd(stb);

        return toReturn;
    }

    public static IStringBuilder FieldEnd(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? ":" : ": ");

    public static void GoToNextCollectionItemStart<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, int elementAt)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.AddCollectionElementSeparator(stb, elementType,  elementAt + 1);
    }

    public static IStringBuilder ItemNext(this IStringBuilder sb, IStyleTypeBuilderComponentAccess stb) =>
        sb.Append(stb.Style.IsCompact() ? "," : ", ");


    public static void EndCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, Type elementType, int numberOfElements)
        where TExt : StyledTypeBuilder
    {
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, numberOfElements);
    }

    public static IStringBuilder RemoveLastWhiteSpacedCommaIfFound<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        if (stb.Sb[^1] == ',')
        {
            stb.Sb.Length -= 1;
            return stb.Sb;
        }
        if (stb.Sb[^2] == ',' && stb.Sb[^1] == ' ')
        {
            stb.Sb.Length -= 2;
            if (stb.Style.IsPretty()) stb.Sb.Append(" ");
            return stb.Sb;
        }
        for (var i = stb.Sb.Length - 1; i > 0 && stb.Sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (stb.Sb[i] == ',')
            {
                stb.Sb.Remove(i, 1);
                break;
            }
        return stb.Sb;
    }


    public static void StartDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.Sb.Append("{");
        if (stb.Style.IsPretty())
        {
            stb.IncrementIndent();
            stb.Sb.Append(stb.Settings.NewLineStyle).AddIndents(stb.Settings.IndentChar, stb.Settings.IndentSize, stb.IndentLevel);
        }
    }

    public static void EndDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb)
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("}");
    }
}
