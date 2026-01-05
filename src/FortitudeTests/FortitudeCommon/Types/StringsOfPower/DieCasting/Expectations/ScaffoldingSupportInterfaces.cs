// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class TypeGeneratePartAttribute(ScaffoldingStringBuilderInvokeFlags flags) : Attribute
{
    public ScaffoldingStringBuilderInvokeFlags ScaffoldingFlags { get; } = flags;
}

public interface ISinglePropertyTestStringBearer : IStringBearer
{
    [JsonIgnore] string PropertyName { get; }
    
    [JsonIgnore] FormatFlags FormattingFlags { get; set; }
}

public interface IMoldSupportedValue<TValue> : ISinglePropertyTestStringBearer
{
    [JsonIgnore] TValue Value { get; set; }
}

public abstract class MoldScaffoldBase<TValue> : IMoldSupportedValue<TValue>
{
    protected TValue Val = default!;
    
    public abstract StateExtractStringRange RevealState(ITheOneString tos);

    public abstract string PropertyName { get; }

    public FormatFlags FormattingFlags { get; set; }

    public virtual TValue Value
    {
        get => Val;
        set => Val = value;
    }
    
    public override string ToString() => $"{GetType().CachedCSharpNameNoConstraints()}({Value})";
}

public interface ISupportsValueFormatString
{
    [JsonIgnore] string? ValueFormatString { get; set; }
}

public interface ISupportsProxiedFormatString
{
    [JsonIgnore] string? ValueFormatString { get; set; }
}

public abstract class FormattedMoldScaffold<TValue> : MoldScaffoldBase<TValue>, ISupportsValueFormatString
{
    public string? ValueFormatString { get; set; }
}

public abstract class ProxyFormattedMoldScaffold<TValue> : MoldScaffoldBase<TValue>, ISupportsProxiedFormatString
{
    public string? ValueFormatString { get; set; }
}



public interface ISupportsUnknownValueRevealer
{
    [JsonIgnore] Delegate ValueRevealerDelegate { get; set; }
}

public interface ISupportsValueRevealer<TRevealBase> : ISupportsUnknownValueRevealer
    where TRevealBase : notnull
{
    [JsonIgnore] PalantírReveal<TRevealBase> ValueRevealer { get; set; }
}
public abstract class ValueRevealerMoldScaffold<TCloaked, TRevealBase> : MoldScaffoldBase<TCloaked?>
  , ISupportsValueRevealer<TRevealBase>
    where TRevealBase : notnull
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TRevealBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
    
    public string? ValueFormatString { get; set; }
}

public interface ISupportsKeyFormatString
{
    [JsonIgnore] string? KeyFormatString { get; set; }
}

public interface ISupportsKeyRevealer<TRevealBase> : ISupportsUnknownKeyRevealer
    where TRevealBase : notnull
{
    [JsonIgnore] PalantírReveal<TRevealBase> KeyRevealer { get; set; }
}

public interface IUnknownPalantirRevealerFactory : ISinglePropertyTestStringBearer
{   
    [JsonIgnore] Delegate CreateRevealerDelegate { get; }
}

public interface IPalantirRevealerFactory<in TRevealBase> : IUnknownPalantirRevealerFactory
{
    #pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    [JsonIgnore] PalantírReveal<TRevealBase> CreateRevealer { get; }
    #pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
}

public interface IMoldSupportedDefaultValue<TValue>
{
    [JsonIgnore] TValue DefaultValue { get; set; }
}

public interface ISupportsSubsetDisplayKeys<TKeyDerivedType>
{
    public static List<TKeyDerivedType>? GetAllKeysSubListPredicate<TKey, TValue>(List<KeyValuePair<TKey, TValue>>? kvPairs) => 
        kvPairs?.Select(kvp => kvp.Key).OfType<TKeyDerivedType>().ToList();
    
    [JsonIgnore] IReadOnlyList<TKeyDerivedType>? DisplayKeys { get; set; }
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
    private static KeyValuePredicate<TKey, TValue>? cachedNoFilterPredicate;
    public static KeyValuePredicate<TKey, TValue> GetNoFilterPredicate => 
        cachedNoFilterPredicate ??= (_, _, _) => CollectionItemResult.IncludedContinueToNext;
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

public interface ISupportsSettingValueFromString
{
    [JsonIgnore] string? StringValue { get; set; }
}
