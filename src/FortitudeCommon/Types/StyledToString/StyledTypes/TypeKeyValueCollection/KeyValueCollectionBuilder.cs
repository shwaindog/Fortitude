// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;


public class KeyValueCollectionBuilder : MultiValueTypeBuilder<KeyValueCollectionBuilder>
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    private IAddAllTypeIsKeyValueCollection?        addAll;
    private IAddFilteredTypeIsKeyValueCollection?   addFiltered;
    private IAddSelectKeysTypeIsKeyValueCollection? addSelectKeys;

    public KeyValueCollectionBuilder InitializeKeyValueCollectionBuilder (IStyleTypeAppenderBuilderAccess owningStyledTypeAppender
      , TypeAppendSettings appendSettings, string typeName)
    {
        InitializeMultiValueTypeBuilder(owningStyledTypeAppender, appendSettings, typeName);

        stb = CompAccess;

        return this;
    }

    public IAddAllTypeIsKeyValueCollection AddAll
    {
        get => addAll ??= stb.Recycler.Borrow<AddAllTypeIsKeyValueCollection>().Initialize(stb);
        protected set => addAll = value;
    }

    public IAddFilteredTypeIsKeyValueCollection AddFiltered
    {
        get => addFiltered ??= stb.Recycler.Borrow<AddFilteredTypeIsKeyValueCollection>().Initialize(stb);
        protected set => addFiltered = value;
    }

    public IAddSelectKeysTypeIsKeyValueCollection AddSelectKeys
    {
        get => addSelectKeys ??= stb.Recycler.Borrow<AddSelectKeysTypeIsKeyValueCollection>().Initialize(stb);
        protected set => addSelectKeys = value;
    }

    protected override void InheritedStateReset()
    {
        addAll?.DecrementRefCount();
        addAll = null;
        addFiltered?.DecrementRefCount();
        addFiltered = null;
        addSelectKeys?.DecrementRefCount();
        addSelectKeys = null;

        base.StateReset();
    }
}
