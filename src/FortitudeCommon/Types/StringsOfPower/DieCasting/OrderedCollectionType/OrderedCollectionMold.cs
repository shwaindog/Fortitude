using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;


public interface IOrderedCollectionExtendFunctionality
{
    public void BeforeFirstElementWriteFieldName(string fieldName);
}

public partial class OrderedCollectionMold<TOCMold> : KnownTypeMolder<TOCMold>, IOrderedCollectionExtendFunctionality
    where TOCMold : TypeMolder
{
    private CollectionMoldWriteState<TOCMold> mws = null!;
    public override MoldType MoldType => MoldType.SimpleOrderedCollectionMold;

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

        mws = WriteStateAsCollectionMoldWriteState;

        return this;
    }

    public override bool IsComplexType => WriteStateAsCollectionMoldWriteState.SupportsMultipleFields;
    public int ResultCount { get; set; }
    public int ItemCount => mws.ItemCount;
    public int TotalCount { get; set; }

    
    public override void AppendClosing(FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
    {
        if (mws.BeforeFirstItemFieldName == null)
        {
            if (WriteStateAsCollectionMoldWriteState.CreateMoldFormatFlags.HasSuppressClosing())
            {
                State.Sf.Gb.RemoveLastSeparatorAndPadding();
                return;
            }
            if (WriteStateAsCollectionMoldWriteState.CurrentWriteMethod.SupportsMultipleFields())
            {
                State.StyleFormatter.AppendComplexTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod, formatFlags);
            }
            else { State.StyleFormatter.AppendSimpleTypeClosing(State.InstanceOrType, State, State.CurrentWriteMethod, formatFlags); }
        }
    }

    public void BeforeFirstElementWriteFieldName(string fieldName)
    {
        mws.BeforeFirstItemFieldName = fieldName;
    }

    protected virtual CollectionMoldWriteState<TOCMold> WriteStateAsCollectionMoldWriteState => (CollectionMoldWriteState<TOCMold>)MoldStateField;
}