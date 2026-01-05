using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.KeyedCollectionType;

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
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName, remainingGraphDepth
                                      , typeFormatting, existingRefId, createFormatFlags);

        stb = MoldStateField;

        return this;
    }

    public override bool IsComplexType => true;

    public override void AppendOpening()
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!;
        var typeFormattingFlags = stb.AppendSettings.SkipTypeParts.ToFormattingFlags();
        MoldStateField.StyleFormatter.AppendKeyedCollectionStart(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key
                                                           , keyValueTypes.Value.Value, typeFormattingFlags);
    }

    public override void AppendClosing()
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!; 
        MoldStateField.StyleFormatter.AppendKeyedCollectionEnd(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key, keyValueTypes.Value.Value, elementCount);
    }

    protected override void InheritedStateReset()
    {
        stb = null!;

        base.InheritedStateReset();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry(TKey key, TValue? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueFormatString, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : TKey? 
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : struct, TKey 
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : TKey? 
        where TV : struct, TValue, TVRevealBase
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TK : TKey?, TKRevealBase? 
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TK : TKey?, TKRevealBase? 
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TK : struct, TKey, TKRevealBase 
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TK : struct, TKey, TKRevealBase 
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler);
        return this;
    }


    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry(TKey key, TValue? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        AddKeyValueMatchEntry(key, value, valueFormatString, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : TKey? 
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : struct, TKey 
        where TV : TValue?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TK : TKey? 
        where TV : struct, TValue, TVRevealBase
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TK : TKey?, TKRevealBase?
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TK : struct, TKey, TKRevealBase
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TK : TKey?, TKRevealBase?
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TK : struct, TKey, TKRevealBase
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        AddKeyValueMatchEntry(key, value, valueStyler, keyStyler);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AppendNextKeyedCollectionEntrySeparator()
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AddNextFieldSeparatorAndPadding();
        return this;
    }
    
    public StateExtractStringRange AppendCollectionComplete() => Complete();
}
