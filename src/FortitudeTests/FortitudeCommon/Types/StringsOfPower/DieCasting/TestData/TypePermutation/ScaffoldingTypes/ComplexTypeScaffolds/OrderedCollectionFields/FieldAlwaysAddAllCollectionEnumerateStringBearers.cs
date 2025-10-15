using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsStruct)]
public class FieldBoolEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool>?>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable);
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct)]
public class FieldNullableBoolEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable);
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerable<TFmt?>?>,
    ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable);
    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable);
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumerableAlwaysAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable);
    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<string?>?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable);
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableAlwaysAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsValueFormatString
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable);
    public IEnumerable<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<object?>?>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable);
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsStruct)]
public class FieldBoolEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool>?>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator);
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct)]
public class FieldNullableBoolEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator);
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator);
    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorAlwaysAddAllStringBearer<TStructFmt> : IMoldSupportedValue<IEnumerator<TStructFmt?>?>
  , ISupportsValueFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator);
    public IEnumerator<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorAlwaysAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator);
    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<string?>?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator);
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorAlwaysAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsValueFormatString
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator);
    public IEnumerator<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddAllStringBearer<T> : IMoldSupportedValue<IEnumerator<T>?>, ISupportsValueFormatString
    where T : class
{
    public IEnumerator<T>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator);
    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
