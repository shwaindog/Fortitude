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
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , IStyledTypeFormatting typeFormatting
      , WriteMethodType writeMethodType  
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeName
                 , remainingGraphDepth, moldGraphVisit, typeFormatting, writeMethodType, createFormatFlags);

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
            var sf = msf.StyleFormatter;
            var gb = sf.GraphBuilder;
            if (gb.GraphEncoder.Type != gb.ParentGraphEncoder.Type)
            {
                
                IEncodingTransfer origGraphEncoder  =  gb.GraphEncoder;
                IEncodingTransfer origParentEncoder =  gb.ParentGraphEncoder;
                origParentEncoder.IncrementRefCount(); // changing GraphEncoder replaces parent and decrements
                gb.GraphEncoder       = origParentEncoder;// setting this changes parentGraphEncoder to old value
                gb.ParentGraphEncoder = origParentEncoder;
                sf.AppendComplexTypeClosing(msf);
                gb.GraphEncoder = origGraphEncoder;
            }
            else
            {
                sf.AppendComplexTypeClosing(msf);
            }
        }
        
        msf.StyleFormatter.GraphBuilder.MarkContentEnd();
    }

}
