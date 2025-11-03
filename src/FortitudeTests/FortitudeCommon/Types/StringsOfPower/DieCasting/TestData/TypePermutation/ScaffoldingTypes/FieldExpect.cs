// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface ISingleFieldExpectation : IFormatExpectation
{
    bool IsNullable { get; }

    bool IsStruct { get; }

    string? DefaultAsString(IStyledTypeFormatting styleFormatting);

    string? FormattedDefault { get; }

    bool HasDefault { get; }

    bool HasIndexRangeLimiting { get; }
}

public interface IComplexFieldFormatExpectation : IFormatExpectation
{
    BuildExpectedOutput WhenValueExpectedOutput { get; set; }
}

public class FieldExpect<TInput>
(
    TInput? input
  , string? formatString = null
  , bool hasDefault = false
  , TInput? defaultValue = default
  , int fromIndex = 0
  , int length = Int32.MaxValue)
    : FieldExpect<TInput, TInput>(input, formatString, hasDefault, defaultValue, fromIndex, length);

public class FieldExpect<TInput, TDefault> : FieldExpectBase<TInput?>, ISingleFieldExpectation
{
    public int FromIndex { get; init; }
    public int Length { get; init; }

    public override bool HasIndexRangeLimiting => FromIndex != 0 || Length != int.MaxValue;

    public FieldExpect(TInput? input, string? formatString = null, bool hasDefault = false, TDefault? defaultValue = default) :
        base(input, formatString)
    {
        HasDefault   = hasDefault;
        DefaultValue = !typeof(TInput).IfNullableGetUnderlyingTypeOrThis().ImplementsInterface<IStringBearer>()
            ? defaultValue.IfNullableGetNonNullableUnderlyingDefault()
            : defaultValue;
    }

    public bool HasDefault { get; init; }

    public TDefault? DefaultValue { get; init; }

    public string? FormattedDefault { get; protected set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public FieldExpect(TInput? input, string? formatString = null, bool hasDefault = false
      , TDefault? defaultValue = default, int fromIndex = 0, int length = int.MaxValue) : base(input, formatString)
    {
        HasDefault   = hasDefault;
        DefaultValue = !typeof(TInput).IfNullableGetUnderlyingTypeOrThis().ImplementsInterface<IStringBearer>()
            ? defaultValue.IfNullableGetNonNullableUnderlyingDefault()
            : defaultValue;
        
        FromIndex = fromIndex;
        Length    = length;
    }

    public override bool InputIsEmpty
    {
        get
        {
            switch (Input)
            {
                case string stringValue:
                    return stringValue.Length == 0
                        || FromIndex >= stringValue.Length || Length <= 0;
                case char[] charArrayValue:
                    return charArrayValue.Length == 0
                        || FromIndex >= charArrayValue.Length || Length <= 0;
                case ICharSequence charSeqValue:
                    return charSeqValue.Length == 0
                        || FromIndex >= charSeqValue.Length || Length <= 0;
                case StringBuilder sbValue:
                    return sbValue.Length == 0
                        || FromIndex >= sbValue.Length || Length <= 0;
                default: return Input != null && Equals(Input, default(TInput));
            }
        }
    }

    public override string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(base.ShortTestName);
                if (HasDefault)
                {
                    if (DefaultValue is string defaultString)
                    {
                        if (defaultString.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is char[] defaultCharArray)
                    {
                        if (defaultCharArray.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is ICharSequence defaultCharSeq)
                    {
                        if (defaultCharSeq.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is StringBuilder defaultSb)
                    {
                        if (defaultSb.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else { result.Append("_").Append(DefaultValue); }
                }

                return result.ToString();
            }
        }
    }
    
    public string DefaultAsString(IStyledTypeFormatting styleFormatting)
    {
        // if (!HasDefault && !InputType.IsValueType && InputIsNull) return DefaultValue?.ToString() ?? "null";
        if (!HasDefault
         && ((InputType.IsValueType && InputType.IsNullable())
          || !InputType.IsValueType && Equals(DefaultValue, null)))
            return "null";
        if (!HasDefault && InputType.IsValueType && Equals(DefaultValue, default(TInput))) return "0";
        var sb = new MutableString();
        switch (DefaultValue)
        {
            case bool boolDefault:             styleFormatting.Format(boolDefault, sb, FormatString ?? ""); break;
            case ISpanFormattable spanDefault: styleFormatting.Format(spanDefault, sb, FormatString ?? ""); break;
            case string stringDefault:         styleFormatting.Format(stringDefault, 0, sb, FormatString ?? ""); break;
            case char[] charArrayDefault:      styleFormatting.Format(charArrayDefault, 0, sb, FormatString ?? ""); break;
            case ICharSequence charSeqDefault: styleFormatting.Format(charSeqDefault, 0, sb, FormatString ?? ""); break;
            case StringBuilder sbDefault:      styleFormatting.Format(sbDefault, 0, sb, FormatString ?? ""); break;
            default:                           styleFormatting.Format(DefaultValue?.ToString() ?? "null", 0, sb, FormatString ?? ""); break;
        }
        return sb.ToString();
    }


    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (InputType == typeof(string) && createdStringBearer is ISupportsSettingValueFromString supportsSettingValueFromString)
            supportsSettingValueFromString.StringValue = (string?)(object?)Input;
        else if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<TInput?>)createdStringBearer).Value = Input;
        if (FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (FormatString != null && createdStringBearer is ISupportsIndexRangeLimiting indexRangeLimiting)
        {
            indexRangeLimiting.FromIndex = FromIndex;
            indexRangeLimiting.Length    = Length;
        }
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TDefault> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TDefault)!;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsAnyTypeHoldingChars() && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            if (InputType.IsTimeFormattableOrNullable())
            {
                supportsStringDefaultValue.DefaultValue = expectedDefaultString.Replace("\"", "").Replace("\'", "").Trim();
            }
        }
        return createdStringBearer;
    }

    public override string ToString()
    {
        var sb = new MutableString();
        sb.AppendLine(GetType().ShortNameInCSharpFormat());
        sb.Append(base.ToString());
        sb.Append(", ").Append(nameof(HasDefault)).Append(": ").Append(HasDefault ? "true" : "false");
        sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
        if (DefaultValue == null) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(DefaultValue).ToString()).Append(AsStringDelimiterClose); }
        sb.Append(", ").Append(nameof(DefaultAsString)).Append(": ")
          .Append(AsStringDelimiterOpen)
          .Append(new MutableString()
                  .Append(DefaultAsString(new CompactJsonTypeFormatting())).ToString())
          .Append(AsStringDelimiterClose);
        if (FromIndex != 0 || Length != int.MaxValue)
        {
            sb.Append(", ").Append(nameof(FromIndex)).Append(": ").Append(FromIndex).Append(", ");
            sb.Append(nameof(Length)).Append(": ").Append(Length);
        }
        sb.AppendLine();
        sb.AppendLine("ExpectedResults");
        var count = 0;
        foreach (var keyValuePair in ExpectedResults)
        {
            sb.Append(count++).Append(" - ").Append("{ ").Append(keyValuePair.Key).Append(", >").Append(keyValuePair.Value).AppendLine("< }");
        }
        return sb.ToString();
    }
};
