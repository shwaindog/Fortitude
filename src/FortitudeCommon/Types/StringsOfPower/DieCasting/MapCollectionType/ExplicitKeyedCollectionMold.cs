using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public class ExplicitKeyedCollectionMold<TKey, TValue> : KeyedCollectionMold
{
    public ExplicitKeyedCollectionMold<TKey, TValue> InitializeExplicitKeyValueCollectionBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext
      , CreateContext createContext)
    {
        InitializeKeyValueCollectionBuilder(instanceOrContainer, typeBeingBuilt, vesselOfStringOfPower, typeVisitedAs, typeName, remainingGraphDepth
                                          , moldGraphVisit, writeMethodType, callerContext, createContext);
        return this;
    }
    
    public override MoldType MoldType => MoldType.KeyedCollectionMold;

    public override bool IsComplexType => true;

    protected override void InheritedStateReset()
    {
        Mws = null!;

        base.InheritedStateReset();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry(
        TKey key
      , TValue? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueFormatString, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
        TK key
      , TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyFormatString, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
        TK? key
      , TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyFormatString, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
        TK key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?
        where TV : struct, TValue, TVRevealBase
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyFormatString, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK key, TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?, TKRevealBase?
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyStyler, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?, TKRevealBase?
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyStyler, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey, TKRevealBase
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyStyler, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey, TKRevealBase
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AppendKeyValuePair(Mws, Mws.TypeBeingBuilt, key, value, ItemCount++, valueStyler, keyStyler, valueFormatString
                                            , formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry(
        TKey key
      , TValue? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        AddKeyValueMatchEntry(key, value, valueFormatString, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(
        TK key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
    )
        where TK : TKey?
        where TV : struct, TValue, TVRevealBase
        where TVRevealBase : notnull
    {
        if (Mws.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?, TKRevealBase?
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(
        TK? key
      , TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey, TKRevealBase
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : TKey?, TKRevealBase?
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TK : struct, TKey, TKRevealBase
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AppendNextKeyedCollectionEntrySeparator()
    {
        if (Mws.SkipBody) return this;
        Mws.StyleFormatter.AddToNextFieldSeparatorAndPadding();
        return this;
    }

    public AppendSummary AppendCollectionComplete() => Complete();
}
