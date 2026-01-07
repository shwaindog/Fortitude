// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public interface ISingleFieldExpectation : IFormatExpectation
{
    bool IsNullable { get; }

    bool IsStruct { get; }

    string? DefaultAsString(IStyledTypeFormatting styleFormatting);

    string? FormattedDefault { get; }

    bool HasDefault { get; }
}

public interface IComplexFieldFormatExpectation : IFormatExpectation
{
    BuildExpectedOutput WhenValueExpectedOutput { get; set; }
}

// ReSharper disable twice ExplicitCallerInfoArgument
public class FieldExpect<TInput>
(
    TInput? input
  , string? valueFormatString = null
  , bool hasDefault = false
  , TInput? defaultValue = default
  , FormatFlags formatFlags = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : FieldExpect<TInput, TInput>(input, valueFormatString, hasDefault, defaultValue, formatFlags, name, srcFile, srcLine);

public class FieldExpect<TInput, TDefault> : ExpectBase<TInput?>, ISingleFieldExpectation
{

    // ReSharper disable twice ExplicitCallerInfoArgument
    public FieldExpect(TInput? input, string? valueFormatString = null, bool hasDefault = false, TDefault? defaultValue = default
     , FormatFlags formatFlags = DefaultCallerTypeFlags, string? name = null
      , [CallerFilePath] string srcFile = "", [CallerLineNumber] int srcLine = 0) :
        base(input, valueFormatString, formatFlags, name, srcFile, srcLine)
    {
        HasDefault   = hasDefault;
        DefaultValue = !InputType.IfNullableGetUnderlyingTypeOrThis().ImplementsInterface<IStringBearer>()
            ? (InputType.IsValueType
                ? defaultValue.IfNullableGetNonNullableUnderlyingDefault()
                : defaultValue)
            : defaultValue;
    }
    
    public bool HasDefault { get; init; }

    public TDefault? DefaultValue { get; init; }


    public virtual Type DefaultType { get; } = typeof(TDefault);

    public string? FormattedDefault { get; protected set; }

    public bool IsStruct => InputType.IsValueType;

    public virtual bool IsNullable => InputType.IsNullable() || InputIsNull;

    public override bool InputIsEmpty => Input != null && Equals(Input, default(TInput));

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
          || (!InputType.IsValueType && Equals(DefaultValue, null))))
            return "null";
        if (!HasDefault && InputType.IsValueType && Equals(DefaultValue, default(TInput))) return "0";
        var sb = new MutableString();
        switch (DefaultValue)
        {
            case bool boolDefault:             styleFormatting.Format(boolDefault, sb, ValueFormatString ?? ""); break;
            case ISpanFormattable spanDefault: styleFormatting.Format(spanDefault, sb, ValueFormatString ?? ""); break;
            case string stringDefault:         styleFormatting.Format(stringDefault, 0, sb, ValueFormatString ?? ""); break;
            case char[] charArrayDefault:      styleFormatting.Format(charArrayDefault, 0, sb, ValueFormatString ?? ""); break;
            case ICharSequence charSeqDefault: styleFormatting.Format(charSeqDefault, 0, sb, ValueFormatString ?? ""); break;
            case StringBuilder sbDefault:      styleFormatting.Format(sbDefault, 0, sb, ValueFormatString ?? ""); break;
            default:                           styleFormatting.Format(DefaultValue?.ToString() ?? "null", 0, sb, ValueFormatString ?? ""); break;
        }
        return sb.ToString();
    }


    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
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
        {
            isObjectMold.Value           = Input;
            isObjectMold.FormattingFlags = FormatFlags;
        }
        else
        {
            var moldSupportedValue = (IMoldSupportedValue<TInput?>)createdStringBearer;
            moldSupportedValue.Value           = Input;
            moldSupportedValue.FormattingFlags = FormatFlags;
        }
        if (ValueFormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = ValueFormatString;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TDefault> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TDefault)!;
        var scaffFlags = scaffoldEntry.ScaffoldingFlags;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsAnyTypeHoldingChars() && !InputType.IsSpanFormattableOrNullable() && DefaultType == InputType
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            if (InputType.IsTimeFormattableOrNullable())
            {
                supportsStringDefaultValue.DefaultValue = expectedDefaultString.Replace("\"", "").Replace("\'", "").Trim();
            }
        }
        if (!FormatFlags.HasDisableAddingAutoCallerTypeFlags() && scaffFlags.HasOutputTreatedAsValue())
        {
            createdStringBearer.FormattingFlags = FormatFlags | AsValueContent;
        } 
        else if (!FormatFlags.HasDisableAddingAutoCallerTypeFlags() && scaffFlags.HasOutputTreatedAsString())
        {
            createdStringBearer.FormattingFlags = FormatFlags | EncodeAll;
        }
        else 
        {
            createdStringBearer.FormattingFlags = FormatFlags;
        }
        return createdStringBearer;
    }

    protected override void AdditionalToStringExpectFields(IStringBuilder sb, ScaffoldingStringBuilderInvokeFlags forThisScaffold)
    {
        sb.Append(", ").Append(nameof(HasDefault)).Append(": ").Append(HasDefault ? "true" : "false");
        sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
        if (DefaultValue == null) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(DefaultValue).ToString()).Append(AsStringDelimiterClose); }
        var defaultStringFromInput = forThisScaffold.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
                                  && !InputType.IsAnyTypeHoldingChars() 
                                  && !InputType.IsSpanFormattableOrNullable() 
                                  && DefaultType == InputType ; 
        sb.Append(", ").Append(nameof(DefaultAsString)).Append(": ")
          .Append(AsStringDelimiterOpen)
          .Append(defaultStringFromInput 
                      ? new MutableString() .Append(DefaultAsString(new CompactJsonTypeFormatting())).ToString()
                      : new MutableString().Append(DefaultValue).ToString() )
          .Append(AsStringDelimiterClose);
        AddExpectedResultsList(sb);
    }
};
