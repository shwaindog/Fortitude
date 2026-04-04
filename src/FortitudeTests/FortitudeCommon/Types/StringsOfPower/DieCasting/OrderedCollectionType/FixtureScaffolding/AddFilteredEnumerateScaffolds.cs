// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType.FixtureScaffolding;

[TypeGeneratePart(  IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class OrderedFromBoolNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<bool, IEnumerable<bool>>
  
{
    public StructEnumerable<IEnumerable<bool>, bool>? OrderedCollectionAddFilteredBoolEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<bool>, bool>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredBoolEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateBool
               (OrderedCollectionAddFilteredBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(  IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class OrderedFromBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? OrderedCollectionAddFilteredBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredBoolEnumerable?.GetType() ?? typeof(IEnumerable<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateBool
               (OrderedCollectionAddFilteredBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<bool?, IEnumerable<bool?>>
{
    public StructEnumerable<IEnumerable<bool?>, bool?>? OrderedCollectionAddFilteredNullableBoolEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<bool?>, bool?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableBoolEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<bool?>, bool?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumerableAddFilteredSimpleOrderedCollectionStringBearer
    : FormattedFilteredCollectionMoldScaffold<bool?, IEnumerable<bool?>>
{
    public IEnumerable<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableBoolEnumerable?.GetType() ?? typeof(IEnumerable<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public StructEnumerable<IEnumerable<TFmt>, TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate
               (OrderedCollectionAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerable<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerable?.GetType() ?? typeof(IEnumerable<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate
               (OrderedCollectionAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public StructEnumerable<IEnumerable<TFmt>, TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate<StructEnumerable<IEnumerable<TFmt>, TFmt>, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerable<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerable?.GetType() ?? typeof(IEnumerable<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate<IEnumerable<TFmt>?, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableSpanFormattableEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public StructEnumerable<IEnumerable<TCloaked>, TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloaked>, TCloaked>();
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate
               (OrderedCollectionRevealFilteredCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate
               (OrderedCollectionRevealFilteredCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public StructEnumerable<IEnumerable<TCloaked>, TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloaked>, TCloaked>();
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<StructEnumerable<IEnumerable<TCloaked>, TCloaked>, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<IEnumerable<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public StructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public StructEnumerable<IEnumerable<TBearer>, TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealFilteredStringBearerEnumerable?.GetType() ?? typeof(StructEnumerable<IEnumerable<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealFilteredStringBearerEnumerable?.GetType() ?? typeof(StructEnumerable<IEnumerable<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public StructEnumerable<IEnumerable<TBearer>, TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealFilteredStringBearerEnumerable?.GetType() ?? typeof(StructEnumerable<IEnumerable<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<StructEnumerable<IEnumerable<TBearer>, TBearer>, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredCollectionMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredStringBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerate<IEnumerable<TBearer>?, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerNullEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumerableRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredEnumerateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<string?, IEnumerable<string?>>

{
    public StructEnumerable<IEnumerable<string?>, string?>? OrderedCollectionAddFilteredStringEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<string?>, string?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<string?>, string?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate
               (OrderedCollectionAddFilteredStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<string?, IEnumerable<string?>>

{
    public IEnumerable<string?>? OrderedCollectionAddFilteredStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerate
               (OrderedCollectionAddFilteredStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateCharSeq
               (OrderedCollectionAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerable<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateCharSeq
               (OrderedCollectionAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateCharSeq<StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredCollectionMoldScaffold<TCharSeq, TCharSeqBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqBase
{
    public IEnumerable<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateCharSeq<IEnumerable<TCharSeq>?, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public StructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringBuilderEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringBuilderEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchAnyNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase?
{
    public StructEnumerable<IEnumerable<TAny>, TAny>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateMatch
               (OrderedCollectionAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchAnyEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase?
{
    public IEnumerable<TAny>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerable?.GetType() ?? typeof(IEnumerable<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateMatch
               (OrderedCollectionAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase?
{
    public StructEnumerable<IEnumerable<TAny>, TAny>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateMatch<StructEnumerable<IEnumerable<TAny>, TAny>, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumerableAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredCollectionMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase?
{
    public IEnumerable<TAny>? OrderedCollectionAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerable?.GetType() ?? typeof(IEnumerable<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateMatch<IEnumerable<TAny>?, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectNullEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, IEnumerable<object?>>
{
    public StructEnumerable<IEnumerable<object?>, object?>? OrderedCollectionAddFilteredObjectEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<object?>, object?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredObjectEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<object?>, object?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateObject
               (OrderedCollectionAddFilteredObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectEnumerableAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredCollectionMoldScaffold<object?, IEnumerable<object?>>
{
    public IEnumerable<object?>? OrderedCollectionAddFilteredObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredObjectEnumerable?.GetType() ?? typeof(IEnumerable<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerable);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredEnumerateObject
               (OrderedCollectionAddFilteredObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

