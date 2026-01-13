// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

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

    public override void StartTypeOpening()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        AppendTypeOpeningToGraphFields();
    }

    public override void AppendTypeOpeningToGraphFields()
    {
        var formatter = MoldStateField!.StyleFormatter;
        if (IsComplexType)
        {
            formatter.StartComplexTypeOpening(MoldStateField);
        }
        else
        {
            formatter.StartContentTypeOpening(MoldStateField);
        }
    }

    public override void CompleteTypeOpeningToTypeFields()
    {
        var formatter = MoldStateField!.StyleFormatter;
        if (IsComplexType)
        {
            formatter.FinishComplexTypeOpening(MoldStateField);
        }
        else
        {
            formatter.FinishContentTypeOpening(MoldStateField);
        }
    }

    public override void AppendClosing()
    {
        var formatter = MoldStateField!.StyleFormatter;
        if (IsComplexType)
        {
            formatter.AppendComplexTypeClosing(MoldStateField);
        }
        else
        {
            formatter.AppendContentTypeClosing(MoldStateField);
        }
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Msf.StartDelimitedStringBuilder();
}
