using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolEnumerableWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool>?>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable);
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct)]
public class FieldNullableBoolEnumerableWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable);
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerable<TFmt?>?>,
    ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable);
    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable);
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable);
    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringEnumerableWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerable<string?>?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable);
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenPopulatedAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsValueFormatString
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable);
    public IEnumerable<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerable<object?>?>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable);
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolEnumeratorWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool>?>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator);
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct)]
public class FieldNullableBoolEnumeratorWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator);
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator);
    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TStructFmt> : IMoldSupportedValue<IEnumerator<TStructFmt?>?>
  , ISupportsValueFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator);
    public IEnumerator<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator);
    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerator<string?>?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator);
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenPopulatedAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsValueFormatString
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator);
    public IEnumerator<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<IEnumerator<T>?>, ISupportsValueFormatString
    where T : class
{
    public IEnumerator<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator);
    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
