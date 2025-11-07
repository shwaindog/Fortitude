using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;


[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerable<bool>>
  , ISupportsOrderedCollectionPredicate<bool>, ISupportsValueFormatString
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList);
    public IEnumerable<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>, ISupportsValueFormatString
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList);
    public IEnumerable<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : IMoldSupportedValue<IEnumerable<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList);
    public IEnumerable<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<IEnumerable<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList);
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddFilteredStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }
    
    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer)]
public class FieldStringBearerEnumerableAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IEnumerable<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?> where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList);
    public IEnumerable<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerable<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList);
    public IEnumerable<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IEnumerable<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<IEnumerable<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                | CallsViaMatch | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList);
    public IEnumerable<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerable<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList);
    public IEnumerable<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsStruct 
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerator<bool>>
  , ISupportsOrderedCollectionPredicate<bool>, ISupportsValueFormatString
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList);
    public IEnumerator<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>, ISupportsValueFormatString
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList);
    public IEnumerator<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : IMoldSupportedValue<IEnumerator<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList);
    public IEnumerator<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList);
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IEnumerator<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddFilteredStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AlwaysWrites | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IEnumerator<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList);
    public IEnumerator<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerator<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList);
    public IEnumerator<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IEnumerator<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<IEnumerator<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList);
    public IEnumerator<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddFilteredStringBearer : IMoldSupportedValue<IEnumerator<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList);
    public IEnumerator<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}