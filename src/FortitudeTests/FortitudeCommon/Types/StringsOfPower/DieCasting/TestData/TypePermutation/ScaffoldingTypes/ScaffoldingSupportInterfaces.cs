// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text.Json.Serialization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class TypeGeneratePartAttribute(ScaffoldingStringBuilderInvokeFlags flags) : Attribute
{
    public ScaffoldingStringBuilderInvokeFlags ScaffoldingFlags { get; } = flags;
}

public interface ISinglePropertyTestStringBearer : IStringBearer
{
    [JsonIgnore] string PropertyName { get; }
}

public interface IMoldSupportedValue<TValue> : ISinglePropertyTestStringBearer
{
    [JsonIgnore] TValue Value { get; set; }
    
    [JsonIgnore] FieldContentHandling ContentHandlingFlags { get; set; }
}

public abstract class MoldScaffoldBase<TValue> : IMoldSupportedValue<TValue>
{
    protected TValue? Val;
    
    public abstract StateExtractStringRange RevealState(ITheOneString tos);

    public abstract string PropertyName { get; }

    public FieldContentHandling ContentHandlingFlags { get; set; }

    public virtual TValue? Value
    {
        get => Val;
        set => Val = value;
    }
    
    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

public interface ISupportsValueFormatString
{
    [JsonIgnore] string? ValueFormatString { get; set; }
}

public abstract class FormattedMoldScaffold<TValue> : MoldScaffoldBase<TValue?>, ISupportsValueFormatString
{
    public string? ValueFormatString { get; set; }
}



public interface ISupportsUnknownValueRevealer
{
    [JsonIgnore] Delegate ValueRevealerDelegate { get; set; }
}

public interface ISupportsValueRevealer<TRevealerType> : ISupportsUnknownValueRevealer
{
    [JsonIgnore] PalantírReveal<TRevealerType> ValueRevealer { get; set; }
}
public abstract class ValueRevealerMoldScaffold<TCloaked, TCloakedRevealBase> : MoldScaffoldBase<TCloaked?>
  , ISupportsValueRevealer<TCloakedRevealBase>
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TCloakedRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TCloakedRevealBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public interface ISupportsKeyFormatString
{
    [JsonIgnore] string? KeyFormatString { get; set; }
}

public interface ISupportsKeyRevealer<TCloaked> : ISupportsUnknownKeyRevealer
{
    [JsonIgnore] PalantírReveal<TCloaked> KeyRevealer { get; set; }
}

public interface IUnknownPalantirRevealerFactory : ISinglePropertyTestStringBearer
{   
    [JsonIgnore] Delegate CreateRevealerDelegate { get; }
}

public interface IPalantirRevealerFactory<TValue> : IUnknownPalantirRevealerFactory
{
    [JsonIgnore] PalantírReveal<TValue> CreateRevealer { get; }
}

public interface IMoldSupportedDefaultValue<TValue>
{
    [JsonIgnore] TValue DefaultValue { get; set; }
}

public interface ISupportsSubsetDisplayKeys<TKeyDerivedType>
{
    [JsonIgnore] IReadOnlyList<TKeyDerivedType> DisplayKeys { get; set; }
}

public interface ISupportsOrderedCollectionPredicate<TElement>
{
    private static OrderedCollectionPredicate<TElement>? cachedNoFilterPredicate;
    public static OrderedCollectionPredicate<TElement> GetNoFilterPredicate => 
        cachedNoFilterPredicate ??= (_, _) => CollectionItemResult.IncludedContinueToNext;

    [JsonIgnore] OrderedCollectionPredicate<TElement> ElementPredicate { get; set; }
}

public interface ISupportsKeyedCollectionPredicate<TKey, TValue>
{
    public static KeyValuePredicate<TKey, TValue> GetNoFilterPredicate => (_, _, _) => CollectionItemResult.IncludedContinueToNext;
    [JsonIgnore] KeyValuePredicate<TKey, TValue> KeyValuePredicate { get; set; }
}

public interface ISupportsUnknownKeyRevealer
{
    [JsonIgnore] Delegate KeyRevealerDelegate { get; set; }
}

public interface ISupportsIndexRangeLimiting
{
    [JsonIgnore] int FromIndex { get; set; }

    [JsonIgnore] int Length { get; set; }
}

public interface ISupportsFieldHandling
{
    [JsonIgnore] FieldContentHandling FieldContentHandling { get; set; }
}

public interface ISupportsSettingValueFromString
{
    [JsonIgnore] string? StringValue { get; set; }
}
