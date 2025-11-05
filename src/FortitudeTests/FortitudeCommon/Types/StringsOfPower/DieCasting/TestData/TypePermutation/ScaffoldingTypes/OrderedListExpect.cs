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
    
    bool HasRestrictingFilter { get; }
}

public class OrderedListExpect<TInputElement> : OrderedListExpect<TInputElement, TInputElement>
{
    public OrderedListExpect(List<TInputElement>? inputList
      , string? formatString = null
      , OrderedCollectionPredicate<TInputElement>? elementFilter = null) : base(inputList, formatString, elementFilter) { }
    
    public OrderedListExpect(List<TInputElement>? inputList, string? formatString
      , Func<OrderedCollectionPredicate<TInputElement>?> elementFilterResolver) : base(inputList, formatString, elementFilterResolver) { }
}

public class OrderedListExpect<TInputElement, TFilterBase> : ExpectBase<List<TInputElement>?>, IOrderedListExpect
    where TInputElement : TFilterBase
{
    private readonly OrderedCollectionPredicate<TFilterBase>? elementPredicate;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderedListExpect(List<TInputElement>? inputList, string? formatString = null
      , OrderedCollectionPredicate<TFilterBase>? elementFilter = null, FieldContentHandling valueHandlingFlags = DefaultCallerTypeFlags)
        : base(inputList, formatString, valueHandlingFlags) =>
        elementPredicate = elementFilter ?? ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public OrderedListExpect(List<TInputElement>? inputList, string? formatString 
      , Func<OrderedCollectionPredicate<TFilterBase>?> elementFilterResolver, FieldContentHandling valueHandlingFlags = DefaultCallerTypeFlags)
        : base(inputList, formatString, valueHandlingFlags) =>
        elementPredicate = elementFilterResolver();

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public bool ElementTypeIsNullable =>  typeof(TInputElement).IsNullable();
    
    public override Type CoreType => typeof(TInputElement).IfNullableGetUnderlyingTypeOrThis();

    public bool HasRestrictingFilter => ElementPredicate != null 
                                     && !Equals(ElementPredicate, ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate);

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

    public OrderedCollectionPredicate<TFilterBase>? ElementPredicate
    {
        get => elementPredicate;
        init => elementPredicate = value;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;
        
        return flags.HasFilterPredicate() 
            ? scaffoldEntry.CreateStringBearerFunc(typeof(TInputElement), typeof(TFilterBase))()
            :  scaffoldEntry.CreateStringBearerFunc(typeof(TInputElement))();
        // if (flags.HasAcceptsArray())
        // {
        //     return flags.HasFilterPredicate() 
        //     ? scaffoldEntry.CreateStringBearerFunc(typeof(TInputElement), typeof(TFilterBase))()
        //     : scaffoldEntry.CreateStringBearerFunc(typeof(TInputElement))();
        // }
        // if (flags.HasAcceptsList())
        // {
        //     return flags.HasFilterPredicate() 
        //         ? scaffoldEntry.CreateStringBearerFunc(typeof(TInputElement), typeof(TFilterBase))()
        //         :  scaffoldEntry.CreateStringBearerFunc(typeof(List<TInputElement>))();
        // }
        // if (flags.HasAcceptsEnumerable())
        // {
        //     return flags.HasFilterPredicate() 
        //         ? scaffoldEntry.CreateStringBearerFunc(typeof(IEnumerable<TInputElement>), typeof(TFilterBase))()
        //         : scaffoldEntry.CreateStringBearerFunc(typeof(IEnumerable<TInputElement>))();
        // }
        // if (flags.HasAcceptsEnumerator())
        // {
        //     return flags.HasFilterPredicate() 
        //         ? scaffoldEntry.CreateStringBearerFunc(typeof(IEnumerator<TInputElement>), typeof(TFilterBase))()
        //         : scaffoldEntry.CreateStringBearerFunc(typeof(IEnumerator<TInputElement>))();
        // }
        // throw new ArgumentException("Unexpected collection scaffolding type");
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();
        
        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TInputElement?[]?> nullArrayMold)
            nullArrayMold.Value = Input?.ToArray();
        else if (createdStringBearer is IMoldSupportedValue<TInputElement[]?> arrayMold)
            arrayMold.Value = Input?.OfType<TInputElement>().ToArray();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TInputElement>?> listMold)
            listMold.Value = Input;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TInputElement>?> enumerableMold)
            enumerableMold.Value = Input;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TInputElement>?> enumeratorMold)
            enumeratorMold.Value = Input?.GetEnumerator();
        else if (acceptsNullables && createdStringBearer is IMoldSupportedValue<object?[]?> nullObjArrayMold)
        {
            nullObjArrayMold.Value = Input?.Select(i => i as object).ToArray();
        }
        else if (createdStringBearer is IMoldSupportedValue<object[]?> objArrayMold)
        {
            objArrayMold.Value = Input?.OfType<object>().ToArray();
        }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<object?>?> objListMold)
            objListMold.Value = Input?.Select(i => i as object).ToList();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<object?>?> objEnumerableMold)
            objEnumerableMold.Value = Input?.Select(i => i as object).ToList();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<object?>?> objEnumeratorMold)
            objEnumeratorMold.Value = Input?.Select(i => i as object).ToList().GetEnumerator();
        
        if ( HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object> supportsSettingObjPredicateFilter)
            supportsSettingObjPredicateFilter.ElementPredicate = ElementPredicate!.ToObjectCastingFilter();
        if ( HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<TFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.ElementPredicate = ElementPredicate!;
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
