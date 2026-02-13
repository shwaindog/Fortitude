// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public class RawContentMold : KnownTypeMolder<RawContentMold>
{
    private bool isDefaultSuppressOpenCloseType;
    
    public RawContentMold InitializeRawContentTypeBuilder
    (
        object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , FormatFlags createFormatFlags)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, createFormatFlags);


        return this;
    }

    protected TypeMolderDieCast<RawContentMold> Msf => (TypeMolderDieCast<RawContentMold>)MoldStateField!;

    public override bool IsComplexType => Msf.SupportsMultipleFields;

    public override void StartFormattingTypeOpening(IStyledTypeFormatting usingFormatter)
    {
        var msf = MoldStateField;
        usingFormatter.Gb.StartNextContentSeparatorPaddingSequence(msf.Sb, msf.CreateMoldFormatFlags);
        var typeBeingBuilt = msf.TypeBeingBuilt;
        isDefaultSuppressOpenCloseType = typeBeingBuilt.IsInputConstructionTypeCached();
        if (!isDefaultSuppressOpenCloseType && MoldStateField.Style.IsLog())
        {
            MoldStateField.CurrentWriteMethod = msf.CurrentWriteMethod.SupportsMultipleFields() 
                ?  WrittenAsFlags.AsComplex | WrittenAsFlags.AsContent | WrittenAsFlags.AsRaw
                : WrittenAsFlags.AsSimple | WrittenAsFlags.AsContent | WrittenAsFlags.AsRaw;
        }
        if(msf.CurrentWriteMethod.HasAsComplexFlag())
        {
            usingFormatter.StartComplexTypeOpening(msf);
        }
        if(msf.CurrentWriteMethod.HasAllOf(WrittenAsFlags.AsSimple | WrittenAsFlags.AsContent))
        {
            usingFormatter.StartContentTypeOpening(msf);
        }
    }

    public override void CompleteTypeOpeningToTypeFields(IStyledTypeFormatting usingFormatter)
    {
        var msf         = State;
        if(msf.CurrentWriteMethod.HasAsComplexFlag())
        {
            usingFormatter.FinishComplexTypeOpening(msf);
        }
        if(msf.CurrentWriteMethod.HasAllOf(WrittenAsFlags.AsSimple | WrittenAsFlags.AsContent))
        {
            usingFormatter.FinishContentTypeOpening(msf);
        }
    }

    public override void AppendClosing()
    {
        var msf         = State;
        var writeMethod = msf.CurrentWriteMethod;
        if(writeMethod.HasAsComplexFlag())
        {
            msf.CurrentWriteMethod = WrittenAsFlags.AsComplex;
            var sf = msf.StyleFormatter;
            if (sf.ContentEncoder.Type != sf.LayoutEncoder.Type) { sf = sf.PreviousContextOrThis; }
            sf.AppendComplexTypeClosing(msf);
        }

        msf.Sf.Gb.MarkContentEnd();
    }
}
