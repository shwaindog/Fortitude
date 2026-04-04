// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeMoldFlags;
using KeyedCollectionMold = FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.KeyedCollectionMold;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface IMoldWriteState : IRecyclableObject, ITransferState
{
    TypeMolder Mold { get; }

    ISecretStringOfPower Master { get; }

    FormatFlags CallerFormatFlags { get; }
    string? CallerFormatString { get; }
    CallerContext Caller { get; }

    FormatFlags CurrentFormatFlags { get; set; }
    FormatFlags CreateMoldFormatFlags { get; set; }

    int InstanceReferenceId { get; }
    int CloseDepthDecrementBy { get; }

    char IndentChar { get; }

    ushort IndentLevel { get; }

    WrittenAsFlags CreateWriteMethod { get; set; }
    WrittenAsFlags CurrentWriteMethod { get; set; }

    TypeMoldFlags MoldWrittenFlags { get; set; }

    bool SupportsMultipleFields { get; }

    bool IsComplete { get; }

    // bool WriteAsAttribute { get; set; }
    // bool WriteAsContent { get; set; }
    bool WroteRefId { get; set; }
    bool WroteTypeName { get; set; }
    bool StartedTypeName { get; set; }
    bool WroteTypeOpen { get; set; }
    bool WroteTypeClose { get; set; }
    bool InnerSameAsOuterType { get; set; }
    bool WroteInnerTypeOpen { get; set; }
    bool WroteInnerTypeClose { get; set; }

    Type TypeBeingBuilt { get; }
    Type TypeBeingVisitedAs { get; }
    string? InstanceName { get; }

    bool SkipBody { get; set; }

    bool SkipFields { get; set; }

    bool IsEmpty { get; set; }
    bool WasDepthClipped { get; set; }
    object InstanceOrContainer { get; }
    object InstanceOrType { get; }

    bool BuildingInstanceEquals<T>(T check);

    bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);
    
    TypeMolder WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);

    bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);

    IStyledTypeFormatting StyleFormatter { get; }
    IStyledTypeFormatting Sf { get; }

    int RemainingGraphDepth { get; set; }

    StringStyle Style { get; }

    StyleOptions Settings { get; }

    IStringBuilder Sb { get; }

    VisitResult MoldGraphVisit { get; set; }
    
    public MoldSnapWriteState SnapshotWriteState { get; set; }

    int VisitNumber { get; }

    int DecrementIndent();
    int IncrementIndent();
    int IncrementCloseDepthDecrementBy(int amountBy);

    public Type GetDisplayType<T>(T instanceContainerOrType);

    void UnSetIgnoreFlag(FormatFlags flagToUnset);
    void SetUntrackedVisit();

    new IRecycler Recycler { get; }
}

public interface IMigratableMoldWriteState : IMoldWriteState, ITransferState<IMoldWriteState>
{
    TypeMolder.MoldPortableState PortableState { get; }
}

public interface IMoldWriteState<out T> : IMigratableMoldWriteState where T : TypeMolder
{
    new T Mold { get; }

    new T WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);
}

public record struct MoldSnapWriteState
(
    int RemainingGraphDepth
  , FormatFlags CurrentFormatFlags
  , WrittenAsFlags CurrentWriteMethod
  , TypeMoldFlags MoldWrittenFlags
)
{
    public MoldSnapWriteState RemoveWrittenFlags(TypeMoldFlags flagsToRemove) => 
        this with { MoldWrittenFlags = MoldWrittenFlags & ~(flagsToRemove) };
};

public class MoldWriteState<TExt> : RecyclableObject, IMoldWriteState<TExt>
    where TExt : TypeMolder
{
    TypeMolder IMoldWriteState.Mold => Mold;

    private TypeMoldFlags unregisteredVisitFlags = None;
    public TExt Mold { get; private set; } = null!;

    private TypeMolder.MoldPortableState typeBuilderState = null!;

    public MoldWriteState<TExt> Initialize
        (TExt externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod)
    {
        Mold                = externalTypeBuilder;
        typeBuilderState    = typeBuilderPortableState;
        RemainingGraphDepth = typeBuilderPortableState.RemainingGraphDepth;

        var typeOfTExt = typeof(TExt);
        var hasAnyStyleFields = typeOfTExt == typeof(ComplexPocoTypeMold)
                             || typeOfTExt == typeof(KeyedCollectionMold)
                             || typeof(MultiValueTypeMolder<TExt>).IsAssignableFrom(typeOfTExt);

        var fmtFlags       = typeBuilderPortableState.CreateFormatFlags;
        var shouldSuppress = MoldGraphVisit.IsARevisit && !writeMethod.HasShowSuppressedContents();
        SkipBody   = shouldSuppress && fmtFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = shouldSuppress || (!Style.IsLog() && !hasAnyStyleFields);

        CreateWriteMethod  = writeMethod;
        CurrentWriteMethod = writeMethod;
        return this;
    }

    public ISecretStringOfPower Master
    {
        [DebuggerStepThrough] get => typeBuilderState.Master;
    }

    TypeMolder.MoldPortableState IMigratableMoldWriteState.PortableState => typeBuilderState;

    public VisitResult MoldGraphVisit
    {
        [DebuggerStepThrough] get => typeBuilderState.MoldGraphVisit;

        set => typeBuilderState.MoldGraphVisit = value;
    }

    public int VisitNumber
    {
        [DebuggerStepThrough] get => MoldGraphVisit.VisitId.VisitIndex;
    }


    public string? InstanceName
    {
        [DebuggerStepThrough] get => typeBuilderState.TypeName;
    }

    public Type TypeBeingBuilt
    {
        [DebuggerStepThrough] get => typeBuilderState.TypeBeingBuilt;
    }

    public Type TypeBeingVisitedAs
    {
        [DebuggerStepThrough] get => typeBuilderState.TypeVisitedAs;
    }

    public FormatFlags CallerFormatFlags => typeBuilderState.CallerContext.FormatFlags;
    public CallerContext Caller => typeBuilderState.CallerContext;

    public string? CallerFormatString => typeBuilderState.CallerContext.FormatString;

    public FormatFlags CreateMoldFormatFlags
    {
        [DebuggerStepThrough] get => typeBuilderState.CreateFormatFlags;
        set => typeBuilderState.CreateFormatFlags = value;
    }

    public FormatFlags CurrentFormatFlags
    {
        [DebuggerStepThrough]
        get
        {
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId))
            {
                return Master.ActiveGraphRegistry[VisitNumber].CurrentFormatFlags;
            }
            return typeBuilderState.CreateFormatFlags;
        }
        set
        {
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId))
            {
                var stateUpdate = Master.ActiveGraphRegistry[VisitNumber];
                Master.ActiveGraphRegistry[VisitNumber] = stateUpdate.UpdateVisitReplaceFormatFlags(value);
            }
        }
    }

    public int RemainingGraphDepth { get; set; }

    public int InstanceReferenceId => typeBuilderState.MoldGraphVisit.InstanceId;

    public ushort IndentLevel => (ushort)typeBuilderState.Master.IndentLevel;

    public WrittenAsFlags CreateWriteMethod { get; set; }

    public WrittenAsFlags CurrentWriteMethod
    {
        get
        {
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId)) { return Master.ActiveGraphRegistry[VisitNumber].WrittenAs; }
            return typeBuilderState.WrittenAsFlags;
        }
        set
        {
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId))
            {
                Master.ActiveGraphRegistry[VisitNumber] =
                    Master.ActiveGraphRegistry[VisitNumber].UpdateVisitWriteType(value);
            }
            typeBuilderState.WrittenAsFlags = value;
            if (!WroteTypeOpen && !WroteTypeName) { CreateWriteMethod = value; }
        }
    }

    public bool SupportsMultipleFields => CreateWriteMethod.SupportsMultipleFields();

    public int CloseDepthDecrementBy { get; private set; } = 1;

    public int IncrementCloseDepthDecrementBy(int amountBy) => CloseDepthDecrementBy += amountBy;

    public bool IsEmpty
    {
        get => MoldWrittenFlags.HasIsEmptyFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(IsEmptyFlag, value);
    }

    public bool WroteRefId
    {
        get => MoldWrittenFlags.HasWroteRefIdFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteRefIdFlag, value);
    }

    public bool WasDepthClipped
    {
        get => MoldWrittenFlags.HasWasDepthClippedFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WasDepthClippedFlag, value);
    }

    public bool WroteTypeName
    {
        get => MoldWrittenFlags.HasWroteTypeNameFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteTypeNameFlag, value);
    }

    public bool WroteTypeOpen
    {
        get => MoldWrittenFlags.HasWroteOuterTypeOpenFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteTypeOpenFlag, value);
    }

    public bool WroteTypeClose
    {
        get => MoldWrittenFlags.HasWroteTypeCloseFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteTypeCloseFlag, value);
    }

    public bool InnerSameAsOuterType
    {
        get => MoldWrittenFlags.HasInnerSameAsOuterTypeFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(InnerSameAsOuterTypeFlag, value);
    }

    public bool StartedTypeName
    {
        get => MoldWrittenFlags.HasStartedTypeNameFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(StartedTypeNameFlag, value);
    }

    public bool WroteInnerTypeOpen
    {
        get => MoldWrittenFlags.HasWroteInnerTypeOpenFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteInnerTypeOpenFlag, value);
    }

    public bool WroteInnerTypeClose
    {
        get => MoldWrittenFlags.HasWroteInnerTypeCloseFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(WroteInnerTypeCloseFlag, value);
    }

    public TypeMoldFlags MoldWrittenFlags
    {
        get
        {
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId))
            {
                var shared = Master.ActiveGraphRegistry[VisitNumber].WrittenFlags;
                var result = shared | unregisteredVisitFlags;
                unregisteredVisitFlags = shared;
                return result;
            }
            return unregisteredVisitFlags;
        }
        set
        {
            unregisteredVisitFlags = value;
            if (Master.ActiveGraphRegistry.HasRegistered(MoldGraphVisit.VisitId))
            {
                Master.ActiveGraphRegistry[VisitNumber] =
                    Master.ActiveGraphRegistry[VisitNumber].UpdateMoldWrittenFlags(unregisteredVisitFlags);
            }
        }
    }

    public char IndentChar => Settings.IndentChar;

    public IStyledTypeFormatting StyleFormatter
    {
        [DebuggerStepThrough] get => typeBuilderState.Master.CurrentStyledTypeFormatter;
    }
    public IStyledTypeFormatting Sf
    {
        [DebuggerStepThrough] get =>  StyleFormatter;
    }

    public bool IsComplete => Mold.IsComplete;

    public bool SkipBody
    {
        get => MoldWrittenFlags.HasSkipBodyFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(SkipBodyFlag, value);
    }

    public bool SkipFields
    {
        get => MoldWrittenFlags.HasSkipFieldsFlag();
        set => MoldWrittenFlags = MoldWrittenFlags.SetTo(SkipFieldsFlag, value);
    }

    public virtual bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipBody;

    [DebuggerStepThrough]
    public virtual bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipFields;

    TypeMolder IMoldWriteState.WasSkipped(Type actualType, ReadOnlySpan<char> fieldName, FormatFlags formatFlags) => 
        WasSkipped(actualType, fieldName, formatFlags);

    public virtual TExt WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)

    {
        return Mold;
    }

    public bool BuildingInstanceEquals<T>(T check) => Mold.BuildingInstanceEquals(check);

    public StyleOptions Settings
    {
        [DebuggerStepThrough] get => typeBuilderState.Master.Settings;
    }

    public Type GetDisplayType<T>(T instanceContainerOrType)
    {
        if (typeBuilderState.CreateContext.DisplayAsType != null)
        {
            var overrideName = typeBuilderState.CreateContext.DisplayAsType;
            typeBuilderState.CreateContext = typeBuilderState.CreateContext with { DisplayAsType = null };
            return overrideName;
        }
        
        return instanceContainerOrType is IRecyclableStructContainer structContainer
            ? structContainer.StoredType
            : (instanceContainerOrType as Type ?? (instanceContainerOrType?.GetType() ?? TypeBeingBuilt));
    }

    public int DecrementIndent()
    {
        typeBuilderState.Master.IndentLevel--;
        return typeBuilderState.Master.IndentLevel;
    }

    public int IncrementIndent()
    {
        typeBuilderState.Master.IndentLevel++;
        return typeBuilderState.Master.IndentLevel;
    }

    public void UnSetIgnoreFlag(FormatFlags flagToUnset)
    {
        typeBuilderState.CreateFormatFlags &= ~flagToUnset;
    }

    public new IRecycler Recycler => base.Recycler ?? typeBuilderState.Master.Recycler;

    public StringStyle Style => Settings.Style;

    public IStringBuilder Sb => typeBuilderState.Master.WriteBuffer;

    public object InstanceOrContainer
    {
        [DebuggerStepThrough] get => typeBuilderState.InstanceOrContainer;
    }

    public object InstanceOrType
    {
        [DebuggerStepThrough]
        get
        {
            var stored = typeBuilderState.InstanceOrContainer;
            return ReferenceEquals(stored, TheOneString.NeverEqual) ? TypeBeingBuilt : stored;
        }
    }

    public MoldSnapWriteState SnapshotWriteState
    {
        get => new(RemainingGraphDepth, CurrentFormatFlags, CurrentWriteMethod, MoldWrittenFlags);
        set
        {
            RemainingGraphDepth = value.RemainingGraphDepth;
            CurrentFormatFlags  = value.CurrentFormatFlags;
            CurrentWriteMethod  = value.CurrentWriteMethod;
            MoldWrittenFlags    = value.MoldWrittenFlags;
        }
    }

    public void SetUntrackedVisit()
    {
        typeBuilderState.MoldGraphVisit = VisitResult.VisitNotChecked;
    }

    public override string ToString() =>
        // ReSharper disable twice ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        $"{Mold.GetType().Name}_{TypeBeingVisitedAs?.Name ?? "NoType"}_{TypeBeingBuilt.Name ?? "NoType"}" +
        $"_{MoldGraphVisit}_[{CurrentWriteMethod}]_[{CreateMoldFormatFlags}]";

    public ITheOneString Complete()
    {
        if (!typeBuilderState.IsComplete)
        {
            typeBuilderState.CompleteResult = Mold.Complete();
            //OwningAppender.AddTypeEnd(StyleTypeBuilder);
        }
        return Master;
    }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source as IMoldWriteState, copyMergeFlags);

    public virtual IMoldWriteState CopyFrom(IMoldWriteState? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        CurrentFormatFlags    = source.CurrentFormatFlags;
        CreateMoldFormatFlags = source.CreateMoldFormatFlags;
        CurrentWriteMethod    = source.CurrentWriteMethod;
        CreateWriteMethod     = source.CreateWriteMethod;
        MoldGraphVisit        = source.MoldGraphVisit;
        RemainingGraphDepth   = source.RemainingGraphDepth;
        MoldWrittenFlags      = source.MoldWrittenFlags;

        return this;
    }

    public override void StateReset()
    {
        CreateWriteMethod      = WrittenAsFlags.Empty;
        unregisteredVisitFlags = None;
        Mold                   = null!;
        typeBuilderState       = null!;
        RemainingGraphDepth    = int.MaxValue;

        CloseDepthDecrementBy = 1;

        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
