using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public record struct StyledTypeBuildResult(string TypeName, StyledTypeStringAppender TypeStringAppender, Range AppendRange)
{
    public static implicit operator StyledTypeBuildResult(StyledTypeBuilder stb) => stb.Complete();

    public static implicit operator StyledTypeStringAppender(StyledTypeBuildResult stbr) => stbr.TypeStringAppender;

    public static readonly StyledTypeBuildResult EmptyAppend = 
        new ("Empty", null!, new Range(Index.FromStart(0), Index.FromStart(0)));
}

public abstract class StyledTypeBuilder : ExplicitRecyclableObject, IDisposable
{
    protected StyleTypeBuilderPortableState PortableState = new();

    protected   int                           StartIndex;

    protected void InitializeStyledTypeBuilder(IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        PortableState.OwningAppender   = owningAppender;
        PortableState.TypeName         = typeName;
        PortableState.AppenderSettings = typeSettings;  
        PortableState.CompleteResult    = null;
        
        StartIndex                         = owningAppender.WriteBuffer.Length;
    }

    public bool IsComplete => PortableState.IsComplete;

    public abstract void Start();

    public string TypeName => PortableState.TypeName;

    public abstract StyledTypeBuildResult Complete();

    public StringBuildingStyle Style => PortableState.OwningAppender.Style;

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

        StartIndex     = -1;

        MeRecyclable.StateReset();
    }

    public class StyleTypeBuilderPortableState
    {
        public TypeAppendSettings AppenderSettings;

        public string             TypeName { get; set; } = null!;

        public  IStyleTypeAppenderBuilderAccess OwningAppender { get; set; } = null!;

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

    public static IStringBuilder AddIndents(this IStringBuilder sb, string indentString, int indentLevel)
    {
        for (var i = 0; i < indentLevel; i++) sb.Append(indentString);
        return sb;
    }

    public static TExt Next<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder => stb.StyleTypeBuilder;

    public static TExt AddGoToNext<TExt>(this IStringBuilder sb, IStyleTypeBuilderComponentAccess<TExt> returnStyledComplexAppender)
        where TExt : StyledTypeBuilder
    {
        if (returnStyledComplexAppender.Style.IsPretty())
        {
            sb.Append(",\n");
            returnStyledComplexAppender.IncrementIndent();
        }
        else
        {
            sb.Append(", ");
        }
        return returnStyledComplexAppender.StyleTypeBuilder;
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
            stb.Sb.Append(", ");
        }
        return stb.StyleTypeBuilder;
    }

    public static IStringBuilder AppendOrNull(this IStringBuilder sb, string? value, bool inQuotes = false) =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendOrNull<T>(this IStringBuilder sb, T? value, bool inQuotes = false) where T : struct =>
        value != null ? sb.Qt(inQuotes).Append(value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder Append<T>(this IStringBuilder sb, T value, bool inQuotes = false) where T : struct =>
        sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append(this IStringBuilder sb, string value, bool inQuotes = false) => sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder Append<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string value, bool inQuotes = false)
        where TExt : StyledTypeBuilder => stb.Sb.Qt(inQuotes).Append(value).Qt(inQuotes);

    public static IStringBuilder AppendFormattedOrNull<T>
    (this IStringBuilder sb, T? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : struct =>
        value != null ? sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes) : sb.Append(Null);

    public static IStringBuilder AppendFormatted<T>
    (this IStringBuilder sb, T value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false) where T : struct
    {
        return sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder AppendFormatted
    (this IStringBuilder sb, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString, bool inQuotes = false)
    {
        return sb.Qt(inQuotes).AppendFormat(formatString, value).Qt(inQuotes);
    }

    public static IStringBuilder Qt(this IStringBuilder sb, bool writeQuote) => writeQuote ? sb.Append("\"") : sb;

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stsa, ICharSequence? value, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var sb        = stsa.Sb;
        var addQuotes = inQuotes ?? stsa.Style.IsJson();
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

    public static IStringBuilder AppendOrNull<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, object? value, bool? inQuotes = null)
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
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            sb.Qt(addQuotes).Append(value).Qt(addQuotes);
            if (addQuotes) sb.Append("\"");
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
      , IStyledToStringObject? value, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
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
        return sb;
    }

    public static IStringBuilder AppendOrNull<TStruct, TExt> (this IStyleTypeBuilderComponentAccess<TExt> stb, TStruct? value
      , StructStyler<TStruct> styledToStringAction, bool? inQuotes = null) where TStruct : struct where TExt : StyledTypeBuilder
    {
        var sb        = stb.Sb;
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
        {
            if (addQuotes) sb.Append("\"");
            styledToStringAction(value.Value, stb.OwningAppender);
            if (addQuotes) sb.Append("\"");
        }
        else
        {
            sb.Append(Null);
        }
        return stb.Sb;
    }

    public static IStringBuilder AppendFormattedOrNull<TValue, TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb
      , TValue value, string formatString, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        var sb        = stb.Sb;
        if (value != null)
            switch (value)
            {
                case bool valueBool:        stb.AppendFormattedOrNull(valueBool, formatString, addQuotes); break;
                case byte valueByte:        stb.AppendFormattedOrNull(valueByte, formatString, addQuotes); break;
                case sbyte valueSByte:      stb.AppendFormattedOrNull(valueSByte, formatString, addQuotes); break;
                case char valueChar:        stb.AppendFormattedOrNull(valueChar, formatString, addQuotes); break;
                case short valueShort:      stb.AppendFormattedOrNull(valueShort, formatString, addQuotes); break;
                case ushort valueUShort:    stb.AppendFormattedOrNull(valueUShort, formatString, addQuotes); break;
                case int valueInt:          stb.AppendFormattedOrNull(valueInt, formatString, addQuotes); break;
                case uint valueUInt:        stb.AppendFormattedOrNull(valueUInt, formatString, addQuotes); break;
                case float valueFloat:      stb.AppendFormattedOrNull(valueFloat, formatString, addQuotes); break;
                case long valueLong:        stb.AppendFormattedOrNull(valueLong, formatString, addQuotes); break;
                case ulong valueULong:      stb.AppendFormattedOrNull(valueULong, formatString, addQuotes); break;
                case double valueDouble:    stb.AppendFormattedOrNull(valueDouble, formatString, addQuotes); break;
                case decimal valueDecimal:  stb.AppendFormattedOrNull(valueDecimal, formatString, addQuotes); break;
                case char[] valueCharArray: stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:    sb.AppendFormat(formatString, valueString); break;

                case IFrozenString valueFrozenString:              stb.AppendFormattedOrNull(valueFrozenString, formatString, addQuotes); break;
                case IStringBuilder valueStringBuilder:            stb.AppendFormattedOrNull(valueStringBuilder, formatString, addQuotes); break;
                case StringBuilder valueSb:                        stb.AppendFormattedOrNull(valueSb, formatString, addQuotes); break;
                case IStyledToStringObject valueStyledToStringObj: stb.AppendOrNull(valueStyledToStringObj, addQuotes); break;
                default:                                           sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return sb;
    }

    public static IStringBuilder AppendOrNull<TValue, TExt>(this IStringBuilder sb, TValue value
      ,IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null) where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            switch (value)
            {
                case bool valueBool:        sb.Append(valueBool, addQuotes); break;
                case byte valueByte:        sb.Append(valueByte, addQuotes); break;
                case sbyte valueSByte:      sb.Append(valueSByte, addQuotes); break;
                case char valueChar:        sb.Append(valueChar, addQuotes); break;
                case short valueShort:      sb.Append(valueShort, addQuotes); break;
                case ushort valueUShort:    sb.Append(valueUShort, addQuotes); break;
                case int valueInt:          sb.Append(valueInt, addQuotes); break;
                case uint valueUInt:        sb.Append(valueUInt, addQuotes); break;
                case float valueFloat:      sb.Append(valueFloat, addQuotes); break;
                case long valueLong:        sb.Append(valueLong, addQuotes); break;
                case ulong valueULong:      sb.Append(valueULong, addQuotes); break;
                case double valueDouble:    sb.Append(valueDouble, addQuotes); break;
                case decimal valueDecimal:  sb.Append(valueDecimal, addQuotes); break;
                case char[] valueCharArray: stb.AppendOrNull(valueCharArray, addQuotes); break;
                case string valueString:    stb.AppendOrNull(valueString, addQuotes); break;

                case IFrozenString valueFrozenString:         stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case IStringBuilder valueFrozenString:        stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case StringBuilder valueFrozenString:         stb.AppendOrNull(valueFrozenString, addQuotes); break;
                case IStyledToStringObject valueFrozenString: stb.AppendOrNull(valueFrozenString, addQuotes); break;
                default:                                      sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
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

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, IStringBuilder? value
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

    public static TExt AddNullOrValue<TExt>(this IStringBuilder sb, string? value, int startIndex, int length
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

    public static TExt AddNullOrValue<TValue, TExt>(this IStringBuilder sb, TValue value
      , IStyleTypeBuilderComponentAccess<TExt> stb, bool? inQuotes = null)
        where TExt : StyledTypeBuilder
    {
        var addQuotes = inQuotes ?? stb.Style.IsJson();
        if (value != null)
            switch (value)
            {
                case bool valueBool:        sb.Append(valueBool, addQuotes); break;
                case byte valueByte:        sb.Append(valueByte, addQuotes); break;
                case sbyte valueSByte:      sb.Append(valueSByte, addQuotes); break;
                case char valueChar:        sb.Append(valueChar, addQuotes); break;
                case short valueShort:      sb.Append(valueShort, addQuotes); break;
                case ushort valueUShort:    sb.Append(valueUShort, addQuotes); break;
                case int valueInt:          sb.Append(valueInt, addQuotes); break;
                case uint valueUInt:        sb.Append(valueUInt, addQuotes); break;
                case float valueFloat:      sb.Append(valueFloat, addQuotes); break;
                case long valueLong:        sb.Append(valueLong, addQuotes); break;
                case ulong valueULong:      sb.Append(valueULong, addQuotes); break;
                case double valueDouble:    sb.Append(valueDouble, addQuotes); break;
                case decimal valueDecimal:  sb.Append(valueDecimal, addQuotes); break;
                case char[] valueCharArray: sb.AddNullOrValue(valueCharArray, stb, addQuotes); break;
                case string valueString:    sb.AddNullOrValue(valueString, stb, addQuotes); break;

                case IFrozenString valueFrozenString:         sb.AddNullOrValue(valueFrozenString, stb, addQuotes); break;
                case IStringBuilder valueFrozenString:        sb.AddNullOrValue(valueFrozenString, stb, addQuotes); break;
                case StringBuilder valueFrozenString:         sb.AddNullOrValue(valueFrozenString, stb, addQuotes); break;
                case IStyledToStringObject valueFrozenString: sb.AddNullOrValue(valueFrozenString, stb, addQuotes); break;
                default:                                      sb.Qt(addQuotes).Append(value).Qt(addQuotes); break;
            }
        else
            sb.Append(Null);
        return stb.AddGoToNext();
    }


    public static void StartCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder
    {
        stb.Sb.Append("[");
        if (stb.Style.IsPretty())
        {
            stb.IncrementIndent();
            stb.Sb.Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        }
    }

    public static IStringBuilder FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName) 
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.OwningAppender.Indent);

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").Append(": ");
        else
            stb.Sb.Append(fieldName).Append(": ");

        return stb.Sb;
    }

    public static IStyleTypeBuilderComponentAccess<TExt> FieldNameJoin<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb, string fieldName
      , IStyleTypeBuilderComponentAccess<TExt> toReturn) 
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty()) stb.Sb.Append(stb.OwningAppender.Indent);

        if (stb.Style.IsJson())
            stb.Sb.Append("\"").Append(fieldName).Append("\"").Append(": ");
        else
            stb.Sb.Append(fieldName).Append(": ");

        return toReturn;
    }

    public static void GoToNextCollectionItemStart<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder
    {
        if (stb.Style.IsPretty())
            stb.Sb.Append(",").Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        else
            stb.Sb.Append(", ");
    }

    public static void EndCollection<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("]");
    }

    public static IStringBuilder RemoveLastWhiteSpacedCommaIfFound<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder
    {
        if (stb.Sb[^2] == ',' && stb.Sb[^1] == ' ')
        {
            stb.Sb.Length -= 2;
            stb.Sb.Append(" ");
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
            stb.Sb.Append(stb.OwningAppender.NewLineStyle).AddIndents(stb.OwningAppender.Indent, stb.IndentLevel);
        }
    }

    public static void EndDictionary<TExt>(this IStyleTypeBuilderComponentAccess<TExt> stb) 
        where TExt : StyledTypeBuilder
    {
        stb.RemoveLastWhiteSpacedCommaIfFound();
        stb.Sb.Append("}");
    }
}
