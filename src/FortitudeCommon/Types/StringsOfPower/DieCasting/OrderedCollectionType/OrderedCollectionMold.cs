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
      , CallerContext callerContext  
      , CreateContext createContext)
    {
        Initialize(instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
                 , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext
                 , createContext with{ FormatFlags = createContext.FormatFlags | FormatFlags.AsCollection });

        mws = CompAsOrderedCollectionMold;

        return this;
    }


    public override bool IsComplexType => CompAsOrderedCollectionMold.SupportsMultipleFields;

    public int ResultCount { get; set; }

    public int TotalCount { get; set; }

    
    public override void AppendClosing(FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
    {
        if (CompAsOrderedCollectionMold.CreateMoldFormatFlags.HasSuppressClosing())
        {
            State.Sf.Gb.RemoveLastSeparatorAndPadding();
            return;
        }
        if (CompAsOrderedCollectionMold.CurrentWriteMethod.SupportsMultipleFields())
        {
            State.StyleFormatter.AppendComplexTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod, formatFlags);
        }
        else
        {
            State.StyleFormatter.AppendSimpleTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod, formatFlags);
        }
    }

    public void BeforeFirstElementWriteFieldName(string fieldName)
    {
        mws.BeforeFirstItemFieldName = fieldName;
    }

    protected virtual CollectionMoldWriteState<TOCMold> CompAsOrderedCollectionMold => (CollectionMoldWriteState<TOCMold>)MoldStateField;
}