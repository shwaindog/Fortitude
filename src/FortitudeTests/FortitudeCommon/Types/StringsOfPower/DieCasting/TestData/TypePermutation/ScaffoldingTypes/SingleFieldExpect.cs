// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
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
  , FieldContentHandling contentHandling = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : FieldExpect<TInput, TInput>(input, valueFormatString, hasDefault, defaultValue, contentHandling, name, srcFile, srcLine);

public class FieldExpect<TInput, TDefault> : ExpectBase<TInput?>, ISingleFieldExpectation
{

    // ReSharper disable twice ExplicitCallerInfoArgument
    public FieldExpect(TInput? input, string? valueFormatString = null, bool hasDefault = false, TDefault? defaultValue = default
     , FieldContentHandling contentHandling = DefaultCallerTypeFlags, string? name = null
      , [CallerFilePath] string srcFile = "", [CallerLineNumber] int srcLine = 0) :
        base(input, valueFormatString, contentHandling, name, srcFile, srcLine)
    {
        HasDefault   = hasDefault;
        DefaultValue = !typeof(TInput).IfNullableGetUnderlyingTypeOrThis().ImplementsInterface<IStringBearer>()
            ? defaultValue.IfNullableGetNonNullableUnderlyingDefault()
            : defaultValue;
    }
    
    public bool HasDefault { get; init; }

    public TDefault? DefaultValue { get; init; }

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
          || !InputType.IsValueType && Equals(DefaultValue, null)))
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
        if (ValueFormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = ValueFormatString;
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

    protected override void AdditionalToStringExpectFields(IStringBuilder sb)
    {
        sb.Append(", ").Append(nameof(HasDefault)).Append(": ").Append(HasDefault ? "true" : "false");
        sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
        if (DefaultValue == null) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(DefaultValue).ToString()).Append(AsStringDelimiterClose); }
        sb.Append(", ").Append(nameof(DefaultAsString)).Append(": ")
          .Append(AsStringDelimiterOpen)
          .Append(new MutableString()
                  .Append(DefaultAsString(new CompactJsonTypeFormatting())).ToString())
          .Append(AsStringDelimiterClose);
        AddExpectedResultsList(sb);
    }
};
