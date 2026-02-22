using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold> : KnownTypeMolder<TOCMold>
    where TOCMold : TypeMolder
{
    private CollectionMoldWriteState<TOCMold> mws = null!;

    public OrderedCollectionMold<TOCMold> InitializeOrderedCollectionBuilder(
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
                 , remainingGraphDepth, moldGraphVisit, writeMethodType
                 , createFormatFlags | FormatFlags.AsCollection);

        mws = CompAsOrderedCollectionMold;

        return this;
    }


    public override bool IsComplexType => false;

    public int ResultCount { get; set; }

    public int TotalCount { get; set; }

    
    public override void StartTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        if (CompAsOrderedCollectionMold.SupportsMultipleFields)
        {
            MoldStateField.StyleFormatter.StartComplexTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CurrentWriteMethod, formatFlags);
        }
        else
        {
            MoldStateField.StyleFormatter.StartSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField
                                                               , MoldStateField.CurrentWriteMethod, formatFlags);
        }
    }
    
    public override void FinishTypeOpening(IStyledTypeFormatting usingFormatter, FormatFlags formatFlags)
    {
        if (CompAsOrderedCollectionMold.SupportsMultipleFields)
        {
            MoldStateField.StyleFormatter.FinishComplexTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CurrentWriteMethod, formatFlags);
        }
        else
        {
            MoldStateField.StyleFormatter.FinishSimpleTypeOpening(MoldStateField.InstanceOrType, MoldStateField, MoldStateField.CurrentWriteMethod, formatFlags);
        }
    }
    
    public override void AppendClosing()
    {
        if (CompAsOrderedCollectionMold.SupportsMultipleFields)
        {
            State.StyleFormatter.AppendComplexTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod);
        }
        else
        {
            State.StyleFormatter.AppendSimpleTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod);
        }
    }

    protected virtual CollectionMoldWriteState<TOCMold> CompAsOrderedCollectionMold => (CollectionMoldWriteState<TOCMold>)MoldStateField;
}