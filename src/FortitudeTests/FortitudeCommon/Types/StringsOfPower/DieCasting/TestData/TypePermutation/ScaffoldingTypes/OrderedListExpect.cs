// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IOrderedListExpect : IFormatExpectation
{
    bool ElementTypeIsNullable { get; }
}

public class OrderedListExpect<TInputElement>
(
    List<TInputElement>? inputList
  , string? formatString = null
  , PalantírReveal<TInputElement>? valueRevealer = null
  , OrderedCollectionPredicate<TInputElement>? elementFilter = null)
    : OrderedListExpect<TInputElement, TInputElement, TInputElement>(inputList, formatString, valueRevealer, elementFilter);

public class OrderedListExpect<TInputElement, TFilterBase, TRevealerBase> : ExpectBase<List<TInputElement>?>, IOrderedListExpect
    where TInputElement : TFilterBase, TRevealerBase
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderedListExpect(List<TInputElement>? inputList, string? formatString = null, PalantírReveal<TRevealerBase>? valueRevealer = null
      , OrderedCollectionPredicate<TFilterBase>? elementFilter = null, FieldContentHandling valueHandlingFlags = DefaultCallerTypeFlags)
        : base(inputList, formatString, valueHandlingFlags)
    {
        ElementPredicate = elementFilter;
        ValueRevealer =  valueRevealer;
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public bool ElementTypeIsNullable =>  typeof(TInputElement).IsNullable() || InputIsNull;

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
    
    public PalantírReveal<TRevealerBase>? ValueRevealer { get; init; }
    
    public OrderedCollectionPredicate<TFilterBase>? ElementPredicate { get; init; }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        
        if (createdStringBearer is IMoldSupportedValue<TInputElement[]?> arrayMold)
            arrayMold.Value = Input?.ToArray();
        if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TInputElement>?> listMold)
            listMold.Value = Input;
        if (createdStringBearer is IMoldSupportedValue<IEnumerable<TInputElement>?> enumerableMold)
            enumerableMold.Value = Input;
        if (createdStringBearer is IMoldSupportedValue<IEnumerator<TInputElement>?> enumeratorMold)
            enumeratorMold.Value = Input?.GetEnumerator();
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
