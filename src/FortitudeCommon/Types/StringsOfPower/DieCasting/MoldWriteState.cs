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
    FormatFlags CreateMoldFormatFlags { get; }

    int InstanceReferenceId { get; }
    int CloseDepthDecrementBy { get; }

    char IndentChar { get; }

    ushort IndentLevel { get; }

    WrittenAsFlags CurrentWriteMethod { get; set; }

    bool SupportsMultipleFields { get; }

    bool IsComplete { get; }

    // bool WriteAsAttribute { get; set; }
    // bool WriteAsContent { get; set; }
    bool WroteRefId { get; set; }
    bool WroteOuterTypeName { get; set; }
    bool WroteInnerTypeName { get; set; }
    // bool WroteCollectionName { get; set; }
    bool WroteOuterTypeOpen { get; set; }
    bool WroteOuterTypeClose { get; set; }
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

    bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);

    IStyledTypeFormatting StyleFormatter { get; }
    IStyledTypeFormatting Sf { get; }

    int RemainingGraphDepth { get; set; }

    StringStyle Style { get; }

    StyleOptions Settings { get; }

    IStringBuilder Sb { get; }

    VisitResult MoldGraphVisit { get; }

    int VisitNumber { get; }

    int DecrementIndent();
    int IncrementIndent();
    int IncrementCloseDepthDecrementBy(int amountBy);

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

    T WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);
}

public class MoldWriteState<TExt> : RecyclableObject, IMoldWriteState<TExt>
    where TExt : TypeMolder
{
    private TypeMoldFlags moldFlags = IsEmptyFlag;
    TypeMolder IMoldWriteState.Mold => Mold;

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

        var fmtFlags             = typeBuilderPortableState.CreateFormatFlags;
        var hasBeenVisitedBefore = MoldGraphVisit.IsARevisit;
        SkipBody   = hasBeenVisitedBefore && fmtFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = hasBeenVisitedBefore || (!Style.IsLog() && !hasAnyStyleFields);

        InitialWriteMethod = writeMethod;
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

    public FormatFlags CallerFormatFlags => Master.CallerContext.FormatFlags;
    public FormatFlags CreateMoldFormatFlags
    {
        [DebuggerStepThrough] get => typeBuilderState.CreateFormatFlags;
        set
        {
            if (typeBuilderState.CreateFormatFlags == value) return;
            var oldValues = typeBuilderState.CreateFormatFlags;
            typeBuilderState.CreateFormatFlags = value;
            Master.UpdateVisitRemoveFormatFlags(MoldGraphVisit.VisitId, oldValues);
            Master.UpdateVisitAddFormatFlags(MoldGraphVisit.VisitId, value);
        }
    }

    public int RemainingGraphDepth { get; set; }

    public int InstanceReferenceId => typeBuilderState.MoldGraphVisit.InstanceId;

    public ushort IndentLevel => (ushort)typeBuilderState.Master.IndentLevel;

    public WrittenAsFlags InitialWriteMethod { get; private set; }

    public WrittenAsFlags CurrentWriteMethod
    {
        get => typeBuilderState.WrittenAsFlags;
        set
        {
            if (typeBuilderState.WrittenAsFlags == value) return;
            if (value.HasAsSimpleFlag() && value.HasAsComplexFlag())
            {
                value &= ~(typeBuilderState.WrittenAsFlags & (WrittenAsFlags.AsSimple | WrittenAsFlags.AsComplex));
            }
            typeBuilderState.WrittenAsFlags = value;
            Master.UpdateVisitWriteMethod(MoldGraphVisit.VisitId, value);
        }
    }

    public bool SupportsMultipleFields => CurrentWriteMethod.SupportsMultipleFields();

    public int CloseDepthDecrementBy { get; private set; } = 1;

    public int IncrementCloseDepthDecrementBy(int amountBy) => CloseDepthDecrementBy += amountBy;

    public bool IsEmpty
    {
        get => moldFlags.HasIsEmptyFlag();
        set => moldFlags = moldFlags.SetTo(IsEmptyFlag, value);
    }

    public bool WroteRefId
    {
        get => moldFlags.HasWroteRefIdFlag();
        set => moldFlags = moldFlags.SetTo(WroteRefIdFlag, value);
    }

    public bool WasDepthClipped
    {
        get => moldFlags.HasWasDepthClippedFlag();
        set => moldFlags = moldFlags.SetTo(WasDepthClippedFlag, value);
    }

    public bool WroteOuterTypeName
    {
        get => moldFlags.HasWroteOuterTypeNameFlag();
        set => moldFlags = moldFlags.SetTo(WroteOuterTypeNameFlag, value);
    }

    public bool WroteOuterTypeOpen
    {
        get => moldFlags.HasWroteOuterTypeOpenFlag();
        set => moldFlags = moldFlags.SetTo(WroteOuterTypeOpenFlag, value);
    }

    public bool WroteOuterTypeClose
    {
        get => moldFlags.HasWroteOuterTypeCloseFlag();
        set => moldFlags = moldFlags.SetTo(WroteOuterTypeCloseFlag, value);
    }

    public bool InnerSameAsOuterType
    {
        get => moldFlags.HasInnerSameAsOuterTypeFlag();
        set => moldFlags = moldFlags.SetTo(InnerSameAsOuterTypeFlag, value);
    }

    public bool WroteInnerTypeName
    {
        get => moldFlags.HasWroteCInnerTypeNameFlag();
        set => moldFlags = moldFlags.SetTo(WroteCInnerTypeNameFlag, value);
    }

    public bool WroteInnerTypeOpen
    {
        get => moldFlags.HasWroteInnerTypeOpenFlag();
        set => moldFlags = moldFlags.SetTo(WroteInnerTypeOpenFlag, value);
    }

    public bool WroteInnerTypeClose
    {
        get => moldFlags.HasWroteInnerTypeCloseFlag();
        set => moldFlags = moldFlags.SetTo(WroteInnerTypeCloseFlag, value);
    }


    public char IndentChar => Settings.IndentChar;

    public IStyledTypeFormatting StyleFormatter
    {
        [DebuggerStepThrough] get => typeBuilderState.Master.CurrentStyledTypeFormatter;
    }
    public IStyledTypeFormatting Sf => StyleFormatter;

    public bool IsComplete => Mold.IsComplete;

    public bool SkipBody
    {
        get => moldFlags.HasSkipBodyFlag();
        set => moldFlags ^= !value && SkipBody || value && !SkipBody ? SkipBodyFlag : None;
    }

    public bool SkipFields
    {
        get => moldFlags.HasSkipFieldsFlag();
        set => moldFlags ^= !value && SkipFields || value && !SkipFields ? SkipFieldsFlag : None;
    }

    public virtual bool HasSkipBody(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipBody;

    [DebuggerStepThrough]
    public virtual bool HasSkipField(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipFields;

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
        CreateMoldFormatFlags      = source.CreateMoldFormatFlags;
        CurrentWriteMethod         = source.CurrentWriteMethod;
        MoldGraphVisit             = source.MoldGraphVisit;
        RemainingGraphDepth        = source.RemainingGraphDepth;
        SkipBody                   = source.SkipBody;
        SkipFields                 = source.SkipFields;
        IsEmpty                    = source.IsEmpty;
        WroteRefId                 = source.WroteRefId;
        WasDepthClipped            = source.WasDepthClipped;
        WroteOuterTypeName         = source.WroteOuterTypeName;
        WroteOuterTypeOpen         = source.WroteOuterTypeOpen;
        WroteOuterTypeClose        = source.WroteOuterTypeClose;
        WroteInnerTypeOpen         = source.WroteInnerTypeOpen;
        WroteInnerTypeClose        = source.WroteInnerTypeClose;

        return this;
    }

    public override void StateReset()
    {
        InitialWriteMethod = WrittenAsFlags.Empty;
        moldFlags          = None;
        Mold               = null!;
        typeBuilderState   = null!;
        RemainingGraphDepth = int.MaxValue;

        CloseDepthDecrementBy = 1;

        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
