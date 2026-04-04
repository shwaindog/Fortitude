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

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolNullEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, IEnumerable<bool>>
{
    public StructEnumerable<IEnumerable<bool>, bool>? OrderedCollectionAddAllBoolEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<bool>, bool>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllBoolEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<bool>, bool>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateBool
               (OrderedCollectionAddAllBoolEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStruct | SupportsValueFormatString)]
public class OrderedFromBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool, IEnumerable<bool>>

{
    public IEnumerable<bool>? OrderedCollectionAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllBoolEnumerable?.GetType() ?? typeof(IEnumerable<bool>);

    public override string PropertyName => nameof(OrderedCollectionAddAllBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateBool
               (OrderedCollectionAddAllBoolEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolNullEnumerableAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<bool?, IEnumerable<bool?>>
{
    public StructEnumerable<IEnumerable<bool?>, bool?>? OrderedCollectionAddAllNullableBoolEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<bool?>, bool?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableBoolEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<bool?>, bool?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullableBool
               (OrderedCollectionAddAllNullableBoolEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | SupportsValueFormatString)]
public class OrderedFromNullableBoolEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<bool?, IEnumerable<bool?>>

{
    public IEnumerable<bool?>? OrderedCollectionAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableBoolEnumerable?.GetType() ?? 
                                                  typeof(IEnumerable<bool?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullableBool
               (OrderedCollectionAddAllNullableBoolEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public StructEnumerable<IEnumerable<TFmt>, TFmt>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate
               (OrderedCollectionAddAllSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableAnyEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public IEnumerable<TFmt>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableEnumerable?.GetType() ?? typeof(IEnumerable<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate
               (OrderedCollectionAddAllSpanFormattableEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public StructEnumerable<IEnumerable<TFmt>, TFmt>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmt>, TFmt>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmt>, TFmt>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate<StructEnumerable<IEnumerable<TFmt>, TFmt>, TFmt>
               (OrderedCollectionAddAllSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class OrderedFromSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmt>
    : FormattedCollectionMoldScaffold<TFmt, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?
{
    public IEnumerable<TFmt>? OrderedCollectionAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllSpanFormattableEnumerable?.GetType() ?? typeof(IEnumerable<TFmt>);

    public override string PropertyName => nameof(OrderedCollectionAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerate<IEnumerable<TFmt>?, TFmt>
               (OrderedCollectionAddAllSpanFormattableEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableAnyNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullable
               (OrderedCollectionAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableAnyEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(IEnumerable<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullable
               (OrderedCollectionAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullable<StructEnumerable<IEnumerable<TFmtStruct?>, TFmtStruct?>, TFmtStruct>
               (OrderedCollectionAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class OrderedFromNullableSpanFormattableEnumerableAddAllSimpleOrderedCollectionStringBearer<TFmtStruct>
    : FormattedCollectionMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? OrderedCollectionAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllNullableSpanFormattableEnumerable?.GetType() ?? 
                                                  typeof(IEnumerable<TFmtStruct?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateNullable<IEnumerable<TFmtStruct?>?, TFmtStruct>
               (OrderedCollectionAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public StructEnumerable<IEnumerable<TCloaked>, TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate
               (OrderedCollectionRevealAllCloakedBearerEnumerable
               , ValueRevealer
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerAnyEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(IEnumerable<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate
               (OrderedCollectionRevealAllCloakedBearerEnumerable
               , ValueRevealer
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public StructEnumerable<IEnumerable<TCloaked>, TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloaked>, TCloaked>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloaked>, TCloaked>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate<StructEnumerable<IEnumerable<TCloaked>, TCloaked>, TCloaked, TRevealBase>
               (OrderedCollectionRevealAllCloakedBearerEnumerable
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloaked, TRevealBase>
    : RevealerCollectionMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? OrderedCollectionRevealAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(IEnumerable<TCloaked>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate<IEnumerable<TCloaked>?, TCloaked, TRevealBase>
               (OrderedCollectionRevealAllCloakedBearerEnumerable
               , ValueRevealer
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public StructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableCloakedBearerEnumerable?.GetType() ?? 
                                                  typeof(StructEnumerable<IEnumerable<TCloakedStruct?>, TCloakedStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable
               (OrderedCollectionRevealAllNullableCloakedBearerEnumerable
               , ValueRevealer
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class OrderedFromNullableCloakedBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TCloakedStruct>
    : RevealerCollectionMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? OrderedCollectionRevealAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealAllNullableCloakedBearerEnumerable?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable
               (OrderedCollectionRevealAllNullableCloakedBearerEnumerable
               , ValueRevealer
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : FormattedCollectionMoldScaffold<TBearer,
    IEnumerable<TBearer>>
    where TBearer : IStringBearer?
{
    public StructEnumerable<IEnumerable<TBearer>, TBearer>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealAllStringBearerEnumerable?.GetType() ?? typeof(StructEnumerable<IEnumerable<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerAnyEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : FormattedCollectionMoldScaffold<TBearer,
    IEnumerable<TBearer>>
    where TBearer : IStringBearer?
{
    public IEnumerable<TBearer>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealAllStringBearerEnumerable?.GetType() ?? typeof(IEnumerable<TBearer?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate(OrderedCollectionRevealAllStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : FormattedCollectionMoldScaffold<TBearer,
    IEnumerable<TBearer>>
    where TBearer : IStringBearer?
{
    public StructEnumerable<IEnumerable<TBearer>, TBearer>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearer>, TBearer>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealAllStringBearerEnumerable?.GetType() ?? typeof(StructEnumerable<IEnumerable<TBearer>, TBearer>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate<StructEnumerable<IEnumerable<TBearer>, TBearer>, TBearer>
               (OrderedCollectionRevealAllStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class OrderedFromStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearer> : FormattedCollectionMoldScaffold<TBearer,
    IEnumerable<TBearer>>
    where TBearer : IStringBearer?
{
    public IEnumerable<TBearer>? OrderedCollectionRevealAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => 
        OrderedCollectionRevealAllStringBearerEnumerable?.GetType() ?? typeof(IEnumerable<TBearer>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerate<IEnumerable<TBearer>?, TBearer>
               (OrderedCollectionRevealAllStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerAnyNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable
               (OrderedCollectionRevealAllNullableStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerAnyEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable
               (OrderedCollectionRevealAllNullableStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerNullEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>?);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable<StructEnumerable<IEnumerable<TBearerStruct?>, TBearerStruct?>, TBearerStruct>
               (OrderedCollectionRevealAllNullableStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class OrderedFromNullableStringBearerEnumerableRevealAllSimpleOrderedCollectionStringBearer<TBearerStruct>
    : FormattedCollectionMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? OrderedCollectionRevealAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionRevealAllNullableStringBearerEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TBearerStruct?>);

    public override string PropertyName => nameof(OrderedCollectionRevealAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .RevealAllEnumerateNullable<IEnumerable<TBearerStruct?>?, TBearerStruct>
               (OrderedCollectionRevealAllNullableStringBearerEnumerable, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringNullEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, IEnumerable<string?>>
{
    public StructEnumerable<IEnumerable<string?>, string?>? OrderedCollectionAddAllStringEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<string?>, string?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<string?>, string?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateString(OrderedCollectionAddAllStringEnumerable
                                , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<string?, IEnumerable<string?>>
{
    public IEnumerable<string?>? OrderedCollectionAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringEnumerable?.GetType() ?? typeof(IEnumerable<string?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateString(OrderedCollectionAddAllStringEnumerable
                                , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateCharSeq
               (OrderedCollectionAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceAnyEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public IEnumerable<TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateCharSeq
               (OrderedCollectionAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TCharSeq>, TCharSeq>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateCharSeq<StructEnumerable<IEnumerable<TCharSeq>, TCharSeq>, TCharSeq>
               (OrderedCollectionAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromCharSequenceEnumerableAddAllSimpleOrderedCollectionStringBearer<TCharSeq>
    : FormattedCollectionMoldScaffold<TCharSeq, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?
{
    public IEnumerable<TCharSeq>? OrderedCollectionAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllCharSequenceEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<TCharSeq>);

    public override string PropertyName => nameof(OrderedCollectionAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateCharSeq<IEnumerable<TCharSeq>?, TCharSeq>
               (OrderedCollectionAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderNullEnumerableAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public StructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringBuilderEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<StringBuilder?>, StringBuilder?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateStringBuilder
               (OrderedCollectionAddAllStringBuilderEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class OrderedFromStringBuilderEnumerableAddAllSimpleOrderedCollectionStringBearer :
    FormattedCollectionMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public IEnumerable<StringBuilder?>? OrderedCollectionAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllStringBuilderEnumerable?.GetType() 
                                               ?? typeof(IEnumerable<StringBuilder?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateStringBuilder
               (OrderedCollectionAddAllStringBuilderEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchAnyNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : 
    FormattedCollectionMoldScaffold<TAny, IEnumerable<TAny>>
{
    public StructEnumerable<IEnumerable<TAny>, TAny>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateMatch
               (OrderedCollectionAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchAnyEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, IEnumerable<TAny>>
{
    public IEnumerable<TAny>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchEnumerable?.GetType() ?? typeof(IEnumerable<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateMatch
               (OrderedCollectionAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchNullEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, IEnumerable<TAny>>
{
    public StructEnumerable<IEnumerable<TAny>, TAny>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<TAny>, TAny>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<TAny>, TAny>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateMatch<StructEnumerable<IEnumerable<TAny>, TAny>, TAny>
               (OrderedCollectionAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsAnyGeneric | SupportsValueFormatString)]
public class OrderedFromMatchEnumerableAddAllSimpleOrderedCollectionStringBearer<TAny> : FormattedCollectionMoldScaffold<TAny, IEnumerable<TAny>>
{
    public IEnumerable<TAny>? OrderedCollectionAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllMatchEnumerable?.GetType() ?? typeof(IEnumerable<TAny>);

    public override string PropertyName => nameof(OrderedCollectionAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateMatch<IEnumerable<TAny>?, TAny>
               (OrderedCollectionAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectNullEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, IEnumerable<object?>>
{
    public StructEnumerable<IEnumerable<object?>, object?>? OrderedCollectionAddAllObjectEnumerable
    {
        get => Value.ToNullableStructEnumerable<IEnumerable<object?>, object?>();
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllObjectEnumerable?.GetType() 
                                               ?? typeof(StructEnumerable<IEnumerable<object?>, object?>?);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateObject
               (OrderedCollectionAddAllObjectEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsOrderedCollectionType | CollectionCardinality | AcceptsEnumerable | AcceptsNullableObject | SupportsValueFormatString)]
public class OrderedFromObjectEnumerableAddAllSimpleOrderedCollectionStringBearer : FormattedCollectionMoldScaffold<object?, IEnumerable<object?>>
{
    public IEnumerable<object?>? OrderedCollectionAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override Type OrderedCollectionType => OrderedCollectionAddAllObjectEnumerable?.GetType() ?? typeof(IEnumerable<object?>);

    public override string PropertyName => nameof(OrderedCollectionAddAllObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartSimpleCollectionType(this)
           .AddAllEnumerateObject
               (OrderedCollectionAddAllObjectEnumerable
               , ValueFormatString, FormattingFlags)
           .Complete();
}
