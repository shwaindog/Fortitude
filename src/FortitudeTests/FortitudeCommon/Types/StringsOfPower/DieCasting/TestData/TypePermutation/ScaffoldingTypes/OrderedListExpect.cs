// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class OrderedListExpect<TInput, TInputElement>
(
    TInput? input
  , string? formatString = null
  , PalantírReveal<TInputElement>? valueRevealer = null
  , OrderedCollectionPredicate<TInputElement>? elementFilter = null)
    : OrderedListExpect<TInput, TInputElement, TInputElement, TInputElement>(input, formatString, valueRevealer, elementFilter);

public class OrderedListExpect<TInput, TInputElement, TFilterBase, TRevealerBase> : FieldExpectBase<TInput?>
{

    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderedListExpect(TInput? input, string? formatString = null, PalantírReveal<TRevealerBase>? valueRevealer = null
      , OrderedCollectionPredicate<TFilterBase>? elementFilter = null) : base(input, formatString)
    {
    }

    public override bool InputIsEmpty
    {
        get
        {
            switch (Input)
            {
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
                result.Append(InputType.ShortNameInCSharpFormat());
                if (Input == null) { result.Append("=null"); }
                else
                {
                    result.Append(AsStringDelimiterOpen)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiterClose).Append("_").Append(FormatString);
                }

                return result.ToString();
            }
        }
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
        return createdStringBearer;
    }

    public override string ToString()
    {
        var sb = new MutableString();
        sb.AppendLine(GetType().ShortNameInCSharpFormat());
        sb.Append(base.ToString());
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
