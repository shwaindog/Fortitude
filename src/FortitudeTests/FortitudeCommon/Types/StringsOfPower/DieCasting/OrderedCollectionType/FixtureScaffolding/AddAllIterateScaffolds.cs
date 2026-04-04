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

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolNullEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool, IEnumerator<bool>>
{
    public StructEnumerator<IEnumerator<bool>, bool>? OrderedCollectionAddAllBoolEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<bool>, bool>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllBoolEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateBool
               (OrderedCollectionAddAllBoolEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? OrderedCollectionAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllBoolEnumerator?.GetType() ?? typeof(IEnumerator<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateBool
               (OrderedCollectionAddAllBoolEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | SupportsValueFormatString)]
public class
    OrderedFromNullableBoolNullEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
{
    public StructEnumerator<IEnumerator<bool?>, bool?>? OrderedCollectionAddAllNullableBoolEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<bool?>, bool?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllNullableBoolEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullableBool
               (OrderedCollectionAddAllNullableBoolEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<bool?, IEnumerator<bool?>>
{
    public IEnumerator<bool?>? OrderedCollectionAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllNullableBoolEnumerator?.GetType() ?? typeof(IEnumerator<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullableBool
               (OrderedCollectionAddAllNullableBoolEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedEnumeratorMoldScaffold<TFmt, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?
{
    public StructEnumerator<IEnumerator<TFmt>, TFmt>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllSpanFormattableEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterate(OrderedCollectionAddAllSpanFormattableEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedEnumeratorMoldScaffold<TFmt, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?
{
    public IEnumerator<TFmt>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllSpanFormattableEnumerator?.GetType() ?? typeof(IEnumerator<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterate(OrderedCollectionAddAllSpanFormattableEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedEnumeratorMoldScaffold<TFmt, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?
{
    public StructEnumerator<IEnumerator<TFmt>, TFmt>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllSpanFormattableEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterate<StructEnumerator<IEnumerator<TFmt>, TFmt>, TFmt>
               (OrderedCollectionAddAllSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedEnumeratorMoldScaffold<TFmt, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?
{
    public IEnumerator<TFmt>? OrderedCollectionAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllSpanFormattableEnumerator?.GetType() ?? typeof(IEnumerator<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterate<IEnumerator<TFmt>?, TFmt>(OrderedCollectionAddAllSpanFormattableEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableAnyNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllNullableSpanFormattableEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullable
               (OrderedCollectionAddAllNullableSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableAnyEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullable
               (OrderedCollectionAddAllNullableSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullable<StructEnumerator<IEnumerator<TFmtStruct?>, TFmtStruct?>, TFmtStruct>
               (OrderedCollectionAddAllNullableSpanFormattableEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumeratorAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedEnumeratorMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateNullable<IEnumerator<TFmtStruct?>?, TFmtStruct>
               (OrderedCollectionAddAllNullableSpanFormattableEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerEnumeratorMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public StructEnumerator<IEnumerator<TCloaked>, TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate
               (OrderedCollectionRevealAllCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerEnumeratorMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerator?.GetType() ?? typeof(IEnumerator<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate
               (OrderedCollectionRevealAllCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerEnumeratorMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public StructEnumerator<IEnumerator<TCloaked>, TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(StructEnumerator<IEnumerator<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate<StructEnumerator<IEnumerator<TCloaked>, TCloaked>, TCloaked, TRevealBase>
               (OrderedCollectionRevealAllCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerEnumeratorMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate<IEnumerator<TCloaked>?, TCloaked, TRevealBase>
               (OrderedCollectionRevealAllCloakedBearerEnumerator, ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public StructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableCloakedBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TCloakedStruct?>, TCloakedStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable
               (OrderedCollectionRevealAllNullableCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerEnumeratorMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableCloakedBearerEnumerator?.GetType() 
                                               ?? typeof(IEnumerator<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable
               (OrderedCollectionRevealAllNullableCloakedBearerEnumerator, ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> :
    FormattedEnumeratorMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer?
{
    public StructEnumerator<IEnumerator<TBearer>, TBearer>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate
               (OrderedCollectionRevealAllStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> :
    FormattedEnumeratorMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer?
{
    public IEnumerator<TBearer>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerEnumerator?.GetType()
                                               ?? typeof(IEnumerator<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate
               (OrderedCollectionRevealAllStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> :
    FormattedEnumeratorMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer?
{
    public StructEnumerator<IEnumerator<TBearer>, TBearer>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerEnumerator?.GetType()
                                               ?? typeof(StructEnumerator<IEnumerator<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate<StructEnumerator<IEnumerator<TBearer>, TBearer>, TBearer>
               (OrderedCollectionRevealAllStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearer> :
    FormattedEnumeratorMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer?
{
    public IEnumerator<TBearer>? OrderedCollectionRevealAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllStringBearerEnumerator?.GetType()
                                               ?? typeof(IEnumerator<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterate<IEnumerator<TBearer>?, TBearer>
               (OrderedCollectionRevealAllStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerAnyNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionRevealAllNullableStringBearerEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable
               (OrderedCollectionRevealAllNullableStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerAnyEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionRevealAllNullableStringBearerEnumerator?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable
               (OrderedCollectionRevealAllNullableStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerNullEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionRevealAllNullableStringBearerEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable<StructEnumerator<IEnumerator<TBearerStruct?>, TBearerStruct?>, TBearerStruct>
               (OrderedCollectionRevealAllNullableStringBearerEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumeratorRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedEnumeratorMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionRevealAllNullableStringBearerEnumerator?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllIterateNullable<IEnumerator<TBearerStruct?>?, TBearerStruct>
               (OrderedCollectionRevealAllNullableStringBearerEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<string?, IEnumerator<string?>>
{
    public StructEnumerator<IEnumerator<string?>, string?>? OrderedCollectionAddAllStringEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<string?>, string?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllStringEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<string?>, string?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateString
               (OrderedCollectionAddAllStringEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<string?, IEnumerator<string?>>
{
    public IEnumerator<string?>? OrderedCollectionAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllStringEnumerator?.GetType() ?? typeof(IEnumerator<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateString
               (OrderedCollectionAddAllStringEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedEnumeratorMoldScaffold<TCharSeq, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllCharSequenceEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateCharSeq
               (OrderedCollectionAddAllCharSequenceEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedEnumeratorMoldScaffold<TCharSeq, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public IEnumerator<TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllCharSequenceEnumerator?.GetType() ?? typeof(IEnumerator<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateCharSeq
               (OrderedCollectionAddAllCharSequenceEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedEnumeratorMoldScaffold<TCharSeq, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllCharSequenceEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateCharSeq<StructEnumerator<IEnumerator<TCharSeq>, TCharSeq>, TCharSeq>
               (OrderedCollectionAddAllCharSequenceEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumeratorAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedEnumeratorMoldScaffold<TCharSeq, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public IEnumerator<TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllCharSequenceEnumerator?.GetType() ?? typeof(IEnumerator<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateCharSeq<IEnumerator<TCharSeq>?, TCharSeq>
               (OrderedCollectionAddAllCharSequenceEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullEnumeratorAddAllSimpleOrderedCollectionStringBearer :
    FormattedEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public StructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllStringBuilderEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<StringBuilder?>, StringBuilder?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateStringBuilder
               (OrderedCollectionAddAllStringBuilderEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumeratorAddAllSimpleOrderedCollectionStringBearer :
    FormattedEnumeratorMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllStringBuilderEnumerator?.GetType() ?? typeof(IEnumerator<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateStringBuilder
               (OrderedCollectionAddAllStringBuilderEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchAnyNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedEnumeratorMoldScaffold<TAny, IEnumerator<TAny>>
{
    public StructEnumerator<IEnumerator<TAny>, TAny>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllMatchEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateMatch
               (OrderedCollectionAddAllMatchEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchAnyEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedEnumeratorMoldScaffold<TAny, IEnumerator<TAny>>
{
    public IEnumerator<TAny>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllMatchEnumerator?.GetType() ?? typeof(IEnumerator<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateMatch
               (OrderedCollectionAddAllMatchEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchNullEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedEnumeratorMoldScaffold<TAny, IEnumerator<TAny>>
{
    public StructEnumerator<IEnumerator<TAny>, TAny>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllMatchEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateMatch<StructEnumerator<IEnumerator<TAny>, TAny>, TAny>
               (OrderedCollectionAddAllMatchEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumeratorAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedEnumeratorMoldScaffold<TAny, IEnumerator<TAny>>
{
    public IEnumerator<TAny>? OrderedCollectionAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllMatchEnumerator?.GetType() ?? typeof(IEnumerator<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateMatch<IEnumerator<TAny>?, TAny>
               (OrderedCollectionAddAllMatchEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectNullEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<object?, IEnumerator<object?>>
{
    public StructEnumerator<IEnumerator<object?>, object?>? OrderedCollectionAddAllObjectEnumerator
    {
        get => Value.ToNullableStructEnumerator<IEnumerator<object?>, object?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllObjectEnumerator?.GetType() ?? typeof(StructEnumerator<IEnumerator<object?>, object?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateObject
               (OrderedCollectionAddAllObjectEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerator | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectEnumeratorAddAllSimpleOrderedCollectionStringBearer : FormattedEnumeratorMoldScaffold<object?, IEnumerator<object?>>
{
    public IEnumerator<object?>? OrderedCollectionAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType =>
        OrderedCollectionAddAllObjectEnumerator?.GetType() ?? typeof(IEnumerator<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllIterateObject
               (OrderedCollectionAddAllObjectEnumerator, ValueFormatString, FormattingFlags)
           .Complete();
}
