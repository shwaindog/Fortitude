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

public interface ITypeMolderDieCast : IRecyclableObject, ITransferState
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
    bool WroteTypeName { get; set; }
    bool WroteCollectionName { get; set; }
    bool WroteTypeOpen { get; set; }
    bool WroteTypeClose { get; set; }
    bool SuppressTypeOpen { get; set; }
    bool SuppressTypeClose { get; set; }
    bool WroteCollectionOpen { get; set; }
    bool WroteCollectionClose { get; set; }
    bool SuppressCollectionOpen { get; set; }
    bool SuppressCollectionClose { get; set; }

    int LastStartNewLineContentPos { get; }

    Type TypeBeingBuilt { get; }
    Type TypeBeingVisitedAs { get; }
    string? InstanceName { get; }

    bool SkipBody { get; set; }

    bool SkipFields { get; set; }

    bool IsEmpty { get; set; }
    bool WasDepthClipped { get; set; }

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

public interface IMigratableTypeMolderDieCast : ITypeMolderDieCast, ITransferState<ITypeMolderDieCast>
{
    TypeMolder.MoldPortableState PortableState { get; }
}

public interface ITypeMolderDieCast<out T> : IMigratableTypeMolderDieCast where T : TypeMolder
{
    new T Mold { get; }

    T WasSkipped(Type actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);
}

public class TypeMolderDieCast<TExt> : RecyclableObject, ITypeMolderDieCast<TExt>
    where TExt : TypeMolder
{
    private TypeMoldFlags moldFlags = IsEmptyFlag;
    TypeMolder ITypeMolderDieCast.Mold => Mold;

    public TExt Mold { get; private set; } = null!;

    private TypeMolder.MoldPortableState typeBuilderState = null!;

    public TypeMolderDieCast<TExt> Initialize
        (TExt externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod)
    {
        Mold                = externalTypeBuilder;
        typeBuilderState    = typeBuilderPortableState;
        RemainingGraphDepth = typeBuilderPortableState.RemainingGraphDepth;

        var typeOfTExt = typeof(TExt);
        var hasJsonFields = typeOfTExt == typeof(ComplexPocoTypeMold)
                         || typeOfTExt == typeof(KeyedCollectionMold)
                         || typeof(MultiValueTypeMolder<TExt>).IsAssignableFrom(typeOfTExt);

        var fmtFlags             = typeBuilderPortableState.CreateFormatFlags;
        var hasBeenVisitedBefore = MoldGraphVisit.IsARevisit;
        SkipBody   = hasBeenVisitedBefore && fmtFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = hasBeenVisitedBefore || (Style.IsJson() && !hasJsonFields);

        InitialWriteMethod = writeMethod;
        CurrentWriteMethod = writeMethod;
        return this;
    }

    public ISecretStringOfPower Master
    {
        [DebuggerStepThrough] get => typeBuilderState.Master;
    }

    TypeMolder.MoldPortableState IMigratableTypeMolderDieCast.PortableState => typeBuilderState;

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

    public bool WroteTypeName
    {
        get => moldFlags.HasWroteTypeNameFlag();
        set => moldFlags = moldFlags.SetTo(WroteTypeNameFlag, value);
    }

    public bool WroteTypeOpen
    {
        get => moldFlags.HasWroteTypeOpenFlag();
        set => moldFlags = moldFlags.SetTo(WroteTypeOpenFlag, value);
    }

    public bool WroteTypeClose
    {
        get => moldFlags.HasWroteTypeCloseFlag();
        set => moldFlags = moldFlags.SetTo(WroteTypeCloseFlag, value);
    }

    public bool SuppressTypeOpen
    {
        get => moldFlags.HasSuppressedTypeOpenFlag();
        set => moldFlags = moldFlags.SetTo(SuppressedTypeOpenFlag, value);
    }

    public bool SuppressTypeClose
    {
        get => moldFlags.HasSuppressedTypeCloseFlag();
        set => moldFlags = moldFlags.SetTo(SuppressedTypeCloseFlag, value);
    }

    public bool WroteCollectionName
    {
        get => moldFlags.HasWroteCollectionNameFlag();
        set => moldFlags = moldFlags.SetTo(WroteCollectionNameFlag, value);
    }

    public bool WroteCollectionOpen
    {
        get => moldFlags.HasWroteCollectionOpenFlag();
        set => moldFlags = moldFlags.SetTo(WroteCollectionOpenFlag, value);
    }

    public bool WroteCollectionClose
    {
        get => moldFlags.HasWroteCollectionCloseFlag();
        set => moldFlags = moldFlags.SetTo(WroteCollectionCloseFlag, value);
    }

    public bool SuppressCollectionOpen
    {
        get => moldFlags.HasSuppressedCollectionOpenFlag();
        set => moldFlags = moldFlags.SetTo(SuppressedCollectionOpenFlag, value);
    }

    public bool SuppressCollectionClose
    {
        get => moldFlags.HasSuppressedCollectionCloseFlag();
        set => moldFlags = moldFlags.SetTo(SuppressedCollectionCloseFlag, value);
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

    public int LastStartNewLineContentPos { get; private set; } = -1;

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

    protected object InstanceOrContainer
    {
        [DebuggerStepThrough] get => typeBuilderState.InstanceOrContainer;
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
        CopyFrom(source as ITypeMolderDieCast, copyMergeFlags);

    public virtual ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        CreateMoldFormatFlags      = source.CreateMoldFormatFlags;
        CurrentWriteMethod         = source.CurrentWriteMethod;
        MoldGraphVisit             = source.MoldGraphVisit;
        RemainingGraphDepth        = source.RemainingGraphDepth;
        SkipBody                   = source.SkipBody;
        SkipFields                 = source.SkipFields;
        LastStartNewLineContentPos = source.LastStartNewLineContentPos;
        IsEmpty                    = source.IsEmpty;
        WroteRefId                 = source.WroteRefId;
        WasDepthClipped            = source.WasDepthClipped;
        WroteTypeName              = source.WroteTypeName;
        WroteTypeOpen              = source.WroteTypeOpen;
        WroteTypeClose             = source.WroteTypeClose;
        WroteCollectionOpen        = source.WroteCollectionOpen;
        WroteCollectionClose       = source.WroteCollectionClose;

        return this;
    }

    public override void StateReset()
    {
        InitialWriteMethod = WrittenAsFlags.Empty;
        moldFlags          = IsEmptyFlag;
        Mold               = null!;
        typeBuilderState   = null!;

        CloseDepthDecrementBy = 1;

        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
