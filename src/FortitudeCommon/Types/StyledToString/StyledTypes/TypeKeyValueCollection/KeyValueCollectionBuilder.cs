// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;


public partial class KeyValueCollectionBuilder : MultiValueTypeBuilder<KeyValueCollectionBuilder>
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    public KeyValueCollectionBuilder InitializeKeyValueCollectionBuilder (IStyleTypeAppenderBuilderAccess owningStyledTypeAppender
      , TypeAppendSettings appendSettings, string typeName, int existingRefId)
    {
        InitializeMultiValueTypeBuilder(owningStyledTypeAppender, appendSettings, typeName, existingRefId);

        stb = CompAccess;

        return this;
    }


    protected override void InheritedStateReset()
    {
        stb  = null!;

        base.InheritedStateReset();
    }
}
