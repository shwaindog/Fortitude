// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class RawContentMold : KnownTypeMolder<RawContentMold>
{
    public RawContentMold InitializeRawContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);

        return this;
    }

    protected TypeMolderDieCast<RawContentMold> Msf => (TypeMolderDieCast<RawContentMold>)MoldStateField!;

    public override bool IsComplexType => Msf.SupportsMultipleFields;

    public override void StartFormattingTypeOpening()
    {
        var msf = MoldStateField;
        msf.Sf.Gb.StartNextContentSeparatorPaddingSequence(msf.Sb, msf.CreateMoldFormatFlags);
        if (msf.CurrentWriteMethod.SupportsMultipleFields())
        {
            var formatter = msf.Sf;
            formatter.StartComplexTypeOpening(msf);
        }
    }

    public override void CompleteTypeOpeningToTypeFields()
    {
    }

    public override void AppendClosing()
    {
        var msf         = State;
        var writeMethod = msf.CurrentWriteMethod;
        if (writeMethod.SupportsMultipleFields())
        {
            var sf = msf.StyleFormatter;
            var gb = sf.Gb;
            if (sf.ContentEncoder.Type != sf.LayoutEncoder.Type)
            {
                sf = sf.PreviousContextOrThis;
            }
            sf.AppendComplexTypeClosing(msf);
        }
        
        msf.Sf.Gb.MarkContentEnd();
    }

}
