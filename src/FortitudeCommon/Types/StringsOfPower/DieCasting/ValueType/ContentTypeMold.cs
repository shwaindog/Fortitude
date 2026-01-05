// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ContentTypeMold<TContentMold> : TransitioningTypeMolder<TContentMold, ContentJoinTypeMold<TContentMold>>
    where TContentMold : ContentTypeMold<TContentMold>
{
    public ContentTypeMold<TContentMold> InitializeContentTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FormatFlags createFormatFlags)
    {
        Initialize(typeBeingBuilt, master, typeSettings, typeName
                 , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    protected ContentTypeDieCast<TContentMold> Msf => (ContentTypeDieCast<TContentMold>)MoldStateField!;

    public override bool IsComplexType => Msf.ValueInComplexType;

    public override void Start()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        AppendOpening();
    }

    public override void AppendOpening()
    {
        if (IsComplexType)
            MoldStateField!.StyleFormatter.AppendComplexTypeOpening(MoldStateField);
        else
            MoldStateField!.StyleFormatter.AppendValueTypeOpening(MoldStateField);
    }

    public override void AppendClosing()
    {
        if (IsComplexType)
            MoldStateField!.StyleFormatter.AppendTypeClosing(MoldStateField);
        else
            MoldStateField!.StyleFormatter.AppendValueTypeClosing(MoldStateField);
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Msf.StartDelimitedStringBuilder();
}
