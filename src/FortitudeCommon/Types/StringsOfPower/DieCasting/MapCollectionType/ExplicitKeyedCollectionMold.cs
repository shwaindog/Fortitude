using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public class ExplicitKeyedCollectionMold<TKey, TValue> : MultiValueTypeMolder<ExplicitKeyedCollectionMold<TKey, TValue>>
{
    private ITypeMolderDieCast<ExplicitKeyedCollectionMold<TKey, TValue>> stb = null!;

    protected static readonly Type TypeOfElement = typeof(TKey);

    private int elementCount;

    public ExplicitKeyedCollectionMold<TKey, TValue> InitializeExplicitKeyValueCollectionBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower vesselOfStringOfPower
      , MoldDieCastSettings appendSettings
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , IStyledTypeFormatting typeFormatting
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags )
    {
        InitializeMultiValueTypeBuilder(instanceOrContainer, typeBeingBuilt, vesselOfStringOfPower, appendSettings, typeName, remainingGraphDepth
                                      , moldGraphVisit, typeFormatting, writeMethodType, createFormatFlags);

        stb = MoldStateField;

        return this;
    }

    public override bool IsComplexType => true;

    public override void StartFormattingTypeOpening()
    {
        var keyValueTypes = MoldStateField.TypeBeingBuilt.GetKeyedCollectionTypes()!;
        var typeFormattingFlags = stb.AppendSettings.SkipTypeParts.ToFormattingFlags();
        MoldStateField.StyleFormatter.AppendKeyedCollectionStart(MoldStateField.Sb, MoldStateField.TypeBeingBuilt, keyValueTypes.Value.Key
                                                           , keyValueTypes.Value.Value, typeFormattingFlags);
    }

    public override void CompleteTypeOpeningToTypeFields() { }

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

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry(
        TKey key
      , TValue? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueFormatString, keyFormatString);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
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
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString, valueFormatString, formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
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
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString, valueFormatString, formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVRevealBase>(
        TK? key
      , TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TK : TKey? 
        where TV : struct, TValue, TVRevealBase
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++,  valueStyler, keyFormatString, valueFormatString, formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TK : TKey?, TKRevealBase? 
        where TV : TValue?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler, valueFormatString, formatFlags);
        return this;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKRevealBase, TVRevealBase>(TK? key, TV? value
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TK : TKey?, TKRevealBase? 
        where TV : struct, TValue, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler, valueFormatString, formatFlags);
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
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler, valueFormatString, formatFlags);
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
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AppendKeyValuePair(stb, stb.TypeBeingBuilt, key, value, elementCount++, valueStyler, keyStyler, valueFormatString, formatFlags);
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
        if (stb.SkipBody) return this;
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
        if (stb.SkipBody) return this;
        AddKeyValueMatchEntry(key, value, valueStyler, keyFormatString, valueFormatString, formatFlags);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVRevealBase>(
        TK? key
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
        if (stb.SkipBody) return this;
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
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AddToNextFieldSeparatorAndPadding();
        return this;
    }
    
    public StateExtractStringRange AppendCollectionComplete() => Complete();
}
