using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public class ExplicitKeyedCollectionBuilder<TKey, TValue> : MultiValueTypeBuilder<ExplicitKeyedCollectionBuilder<TKey, TValue>>
{
    private IStyleTypeBuilderComponentAccess<ExplicitKeyedCollectionBuilder<TKey, TValue>> stb = null!;

    protected static readonly Type TypeOfElement = typeof(TKey);

    private int elementCount = -1;

    public ExplicitKeyedCollectionBuilder<TKey, TValue> InitializeExplicitKeyValueCollectionBuilder
    (
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningStyledTypeAppender
      , TypeAppendSettings appendSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeMultiValueTypeBuilder(typeBeingBuilt, owningStyledTypeAppender, appendSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        stb = CompAccess;

        return this;
    }

    public override bool IsComplexType => true;

    public override void AppendOpening()
    {
        CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
    }

    public override void AppendClosing()
    {
        CompAccess.StyleFormatter.AppendTypeClosing(CompAccess);
    }

    protected override void InheritedStateReset()
    {
        stb = null!;

        base.InheritedStateReset();
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchEntry(TKey key, TValue value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        if (stb.SkipBody) return this;
        _ = keyFormatString.IsNotNullOrEmpty()
            ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : stb.AppendMatchOrNull(key, true).FieldEnd();
        _ = valueFormatString.IsNotNullOrEmpty()
            ? stb.AppendMatchFormattedOrNull(value, valueFormatString)
            : stb.AppendMatchOrNull(value);
        return this;
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TVBase>(TK key, TV value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TK : TKey where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        _ = keyFormatString.IsNotNullOrEmpty()
            ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : stb.AppendMatchOrNull(key, true).FieldEnd();
        valueStyler(value, stb.OwningAppender);
        return this;
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchEntry<TK, TV, TKBase, TVBase>(TK key, TV value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TK : TKey, TKBase where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        keyStyler(key, stb.OwningAppender);
        stb.FieldEnd();
        valueStyler(value, stb.OwningAppender);
        return this;
    }


    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchAndGoToNextEntry(TKey key, TValue value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        AddKeyValueMatchEntry(key, value, valueFormatString, keyFormatString);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TVBase>(TK key, TV value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TK : TKey where TV : TValue, TVBase
    {
        if (stb.SkipBody) return this;
        _ = keyFormatString.IsNotNullOrEmpty()
            ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
            : stb.AppendMatchOrNull(key, true).FieldEnd();
        valueStyler(value, stb.OwningAppender);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AddKeyValueMatchAndGoToNextEntry<TK, TV, TKBase, TVBase>(TK key, TV value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TK : TKey, TKBase where TV : TValue, TVBase
    {
        keyStyler(key, stb.OwningAppender);
        stb.FieldEnd();
        valueStyler(value, stb.OwningAppender);
        return AppendNextKeyedCollectionEntrySeparator();
    }

    public ExplicitKeyedCollectionBuilder<TKey, TValue> AppendNextKeyedCollectionEntrySeparator()
    {
        if (stb.SkipBody) return this;
        stb.StyleFormatter.AddNextFieldSeparator(stb);
        return this;
    }
    
    public StyledTypeBuildResult AppendCollectionComplete() => Complete();
}
