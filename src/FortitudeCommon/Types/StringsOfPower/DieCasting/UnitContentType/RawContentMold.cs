// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class RawContentMold : KnownTypeMolder<RawContentMold>
{
    public RawContentMold InitializeRawContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeSettings, typeName
                 , remainingGraphDepth, typeFormatting, existingRefId, writeMethodType, createFormatFlags);

        return this;
    }

    protected TypeMolderDieCast<RawContentMold> Msf => (TypeMolderDieCast<RawContentMold>)MoldStateField!;

    public override bool IsComplexType => Msf.SupportsMultipleFields;

    public override void StartFormattingTypeOpening()
    {
        var msf = MoldStateField;
        msf.StyleFormatter.GraphBuilder.StartNextContentSeparatorPaddingSequence(msf.Sb, msf.CreateMoldFormatFlags);
        if (msf.WriteMethod.SupportsMultipleFields())
        {
            var formatter = MoldStateField!.StyleFormatter;
            formatter.StartComplexTypeOpening(MoldStateField);
        }
    }

    public override void CompleteTypeOpeningToTypeFields()
    {
        
    }

    public override void AppendClosing()
    {
        var msf         = MoldStateField;
        var writeMethod = msf!.WriteMethod;
        if (writeMethod.SupportsMultipleFields())
        {
            var formatter = msf!.StyleFormatter;
            formatter.AppendComplexTypeClosing(msf);
        }
        
        msf.StyleFormatter.GraphBuilder.MarkContentEnd();
    }

}
