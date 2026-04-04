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

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool, IEnumerator<bool>>
{
    public StructEnumerator<IEnumerator<bool>, bool>? OrderedCollectionAddFilteredBoolEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<bool>, bool>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredBoolEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateBool
               (OrderedCollectionAddFilteredBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? OrderedCollectionAddFilteredBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredBoolEnumerator?.GetType() ?? typeof(IEnumerator<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateBool
               (OrderedCollectionAddFilteredBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
{
    public StructEnumerator<IEnumerator<bool?>, bool?>? OrderedCollectionAddFilteredNullableBoolEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<bool?>, bool?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredNullableBoolEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
{
    public IEnumerator<bool?>? OrderedCollectionAddFilteredNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableBoolEnumerator?.GetType() ?? typeof(IEnumerator<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableBoolEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullableBool
               (OrderedCollectionAddFilteredNullableBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public StructEnumerator<IEnumerator<TFmt>, TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredSpanFormattableEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate
               (OrderedCollectionAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerator<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerator?.GetType() ?? typeof(IEnumerator<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate
               (OrderedCollectionAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public StructEnumerator<IEnumerator<TFmt>, TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredSpanFormattableEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate<StructEnumerator<IEnumerator<TFmt>, TFmt>, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmt, TFmtBase> :
    FormattedFilteredEnumeratorMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerator<TFmt>? OrderedCollectionAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredSpanFormattableEnumerator?.GetType() ?? typeof(IEnumerator<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate<IEnumerator<TFmt>?, TFmt, TFmtBase>
               (OrderedCollectionAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableSpanFormattableEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TFmtStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredNullableSpanFormattableEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredNullableSpanFormattableEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateNullable
               (OrderedCollectionAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public StructEnumerator<IEnumerator<TCloaked>, TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate
               (OrderedCollectionRevealFilteredCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate
               (OrderedCollectionRevealFilteredCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public StructEnumerator<IEnumerator<TCloaked>, TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<StructEnumerator<IEnumerator<TCloaked>, TCloaked>, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredEnumeratorMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? OrderedCollectionRevealFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<IEnumerator<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (OrderedCollectionRevealFilteredCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public StructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TCloakedStruct> :
    RevealerFilteredEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public StructEnumerator<IEnumerator<TBearer>, TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredStringBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate
               (OrderedCollectionRevealFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerator<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredStringBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate
               (OrderedCollectionRevealFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public StructEnumerator<IEnumerator<TBearer>, TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredStringBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<StructEnumerator<IEnumerator<TBearer>, TBearer>, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearer, TBearerBase> :
    FormattedFilteredEnumeratorMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerator<TBearer>? OrderedCollectionRevealFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredStringBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterate<IEnumerator<TBearer>?, TBearer, TBearerBase>
               (OrderedCollectionRevealFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerNullEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableStringBearerEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumeratorRevealFilteredSimpleOrderedCollectionStringBearer<TBearerStruct> :
    FormattedFilteredEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealFilteredNullableStringBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealFilteredNullableStringBearerEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealFilteredIterateNullable
               (OrderedCollectionRevealFilteredNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<string?, IEnumerator<string?>>
{
    public StructEnumerator<IEnumerator<string?>, string?>? OrderedCollectionAddFilteredStringEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<string?>, string?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredStringEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<string?>, string?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate
               (OrderedCollectionAddFilteredStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsString | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<string?, IEnumerator<string?>>
{
    public IEnumerator<string?>? OrderedCollectionAddFilteredStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterate
               (OrderedCollectionAddFilteredStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredCharSequenceEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateCharSeq
               (OrderedCollectionAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public IEnumerator<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateCharSeq
               (OrderedCollectionAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredCharSequenceEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateCharSeq<StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TCharSeq, TCharSeqBase> :
    FormattedFilteredEnumeratorMoldScaffold<TCharSeq, TCharSeqBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqBase?
{
    public IEnumerator<TCharSeq>? OrderedCollectionAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredCharSequenceEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredCharSequenceEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateCharSeq<IEnumerator<TCharSeq>?, TCharSeq, TCharSeqBase>
               (OrderedCollectionAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public StructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringBuilderEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | FilterPredicate
                | AcceptsStringBuilder | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddFilteredStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredStringBuilderEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredStringBuilderEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateStringBuilder
               (OrderedCollectionAddFilteredStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchAnyNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public StructEnumerator<IEnumerator<TAny>, TAny>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateMatch
               (OrderedCollectionAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchAnyEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateMatch
               (OrderedCollectionAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public StructEnumerator<IEnumerator<TAny>, TAny>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateMatch<StructEnumerator<IEnumerator<TAny>, TAny>, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | SupportsValueFormatString
                | AcceptsAnyGeneric | FilterPredicate)]
public class OrderedFromMatchEnumeratorAddFilteredSimpleOrderedCollectionStringBearer<TAny, TAnyFilterBase> :
    FormattedFilteredEnumeratorMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny>? OrderedCollectionAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredMatchEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredMatchEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateMatch<IEnumerator<TAny>?, TAny, TAnyFilterBase>
               (OrderedCollectionAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectNullEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<object?, IEnumerator<object?>>
{
    public StructEnumerator<IEnumerator<object?>, object?>? OrderedCollectionAddFilteredObjectEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<object?>, object?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddFilteredObjectEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<object?>, object?>?);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateObject
               (OrderedCollectionAddFilteredObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString
                | FilterPredicate)]
public class OrderedFromObjectEnumeratorAddFilteredSimpleOrderedCollectionStringBearer :
    FormattedFilteredEnumeratorMoldScaffold<object?, IEnumerator<object?>>
{
    public IEnumerator<object?>? OrderedCollectionAddFilteredObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddFilteredObjectEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddFilteredObjectEnumerator);


    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddFilteredIterateObject
               (OrderedCollectionAddFilteredObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}
