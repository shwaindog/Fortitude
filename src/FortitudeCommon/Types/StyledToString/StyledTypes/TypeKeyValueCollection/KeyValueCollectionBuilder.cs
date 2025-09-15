// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;


public partial class KeyValueCollectionBuilder : MultiValueTypeBuilder<KeyValueCollectionBuilder>
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    public KeyValueCollectionBuilder InitializeKeyValueCollectionBuilder 
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
        stb  = null!;

        base.InheritedStateReset();
    }
}
