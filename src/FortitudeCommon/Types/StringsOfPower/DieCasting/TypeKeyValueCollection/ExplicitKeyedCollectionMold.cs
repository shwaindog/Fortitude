﻿using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public class ExplicitKeyedCollectionMold<TKey, TValue> : MultiValueTypeMolder<ExplicitKeyedCollectionMold<TKey, TValue>>
{
    private ITypeMolderDieCast<ExplicitKeyedCollectionMold<TKey, TValue>> stb = null!;

    protected static readonly Type TypeOfElement = typeof(TKey);

    private int elementCount;

    public ExplicitKeyedCollectionMold<TKey, TValue> InitializeExplicitKeyValueCollectionBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        stb = CompAccess;

        return this;
    }

    public override bool IsComplexType => true;

    public override void AppendOpening()
    {
        var keyValueTypes = CompAccess.TypeBeingBuilt.GetKeyedCollectionTypes()!;
        CompAccess.StyleFormatter.AppendKeyedCollectionStart(CompAccess.Sb, CompAccess.TypeBeingBuilt, keyValueTypes.Value.Key
                                                           , keyValueTypes.Value.Value);
    }

    public override void AppendClosing()
    {
        var keyValueTypes = CompAccess.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        CompAccess.StyleFormatter.AppendKeyedCollectionEnd(CompAccess.Sb, CompAccess.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value, elementCount);
    }

    protected override void InheritedStateReset()
    {
        stb = null!;

        base.InheritedStateReset();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry(TKey key, TValue value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueFormatString, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVBase>(TK key, TV value, PalantírReveal<TVBase> valueStyler
      , string? keyFormatString = null) where TK : TKey where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKBase, TVBase>(TK key, TV value, PalantírReveal<TVBase> valueStyler
      , PalantírReveal<TKBase> keyStyler) where TK : TKey, TKBase where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler);
        return this;
    }


    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry(TKey key, TValue value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        AddKeyValueMatchEntry(key, value, valueFormatString, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVBase>(TK key, TV value, PalantírReveal<TVBase> valueStyler
      , string? keyFormatString = null) where TK : TKey where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKBase, TVBase>(TK key, TV value, PalantírReveal<TVBase> valueStyler
      , PalantírReveal<TKBase> keyStyler) where TK : TKey, TKBase where TV : TValue, TVBase
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AppendNextKeyedCollectionEntrySeparator()
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AddNextFieldSeparator(stb.Sb);
        return this;
    }
    
    public StateExtractStringRange AppendCollectionComplete() => Complete();
}
