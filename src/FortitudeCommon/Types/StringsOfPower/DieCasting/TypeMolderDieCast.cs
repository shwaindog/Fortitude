// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
    
    TypeMolder TypeMolder { get; }

    ISecretStringOfPower Master { get; }

    FormatFlags CallerContentHandling { get; }
    FormatFlags CreateMoldFormatFlags { get; }

    int InstanceReferenceId { get; }

    char IndentChar { get; }

    ushort IndentLevel { get; }
    
    WriteMethodType WriteMethod { get; set; }

    bool SupportsMultipleFields { get; }

    bool IsComplete { get; }

    // bool WriteAsAttribute { get; set; }
    // bool WriteAsContent { get; set; }
    bool WroteRefId { get; set; }
    bool WroteTypeName { get; set; }
    
    int LastStartNewLineContentPos { get; }

    Type TypeBeingBuilt { get; }
    string? TypeName { get; }

    bool SkipBody { get; set; }

    bool SkipFields { get; set; }
    
    bool IsEmpty { get; set; }
    bool WasDepthClipped { get; set; }
    
    bool BuildingInstanceEquals<T>(T check);

    bool HasSkipBody<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);

    bool HasSkipField<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);

    IStyledTypeFormatting StyleFormatter { get; }

    int RemainingGraphDepth { get; set; }

    StringStyle Style { get; }

    StyleOptions Settings { get; }

    IStringBuilder Sb { get; }

    public int DecrementIndent();
    public int IncrementIndent();

    public void UnSetIgnoreFlag(FormatFlags flagToUnset);
    void        SetUntrackedVisit();

    new IRecycler Recycler { get; }
}

public interface IMigratableTypeMolderDieCast : ITypeMolderDieCast, ITransferState<ITypeMolderDieCast>
{
    TypeMolder.MoldPortableState PortableState { get; }
}

public interface ITypeMolderDieCast<out T> : IMigratableTypeMolderDieCast where T : TypeMolder
{
    T StyleTypeBuilder { get; }

    T WasSkipped<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags);
}

[Flags]
public enum TypeMoldFlags : ushort
{
    None       = 0x00_00
  , IsEmptyFlag    = 0x00_01  
  , IsCompleteFlag = 0x00_02 
  , SkipBodyFlag   = 0x00_04 
  , SkipFieldsFlag = 0x00_08
    
  // , WriteAsAttributeFlag = 0x00_10 
  // , WriteAsContentFlag   = 0x00_20
  // , WriteAsComplexFlag   = 0x00_40  
  , WroteTypeNameFlag    = 0x00_80
  , WroteRefIdFlag       = 0x01_00    
  , WroteTypeOpenFlag    = 0x02_00 
  , WroteTypeCloseFlag   = 0x04_00 
  , WasDepthClippedFlag   = 0x08_00 
}

public static class TypeMoldFlagsExtensions 
{
    
    public static bool HasIsEmptyFlag(this TypeMoldFlags flags)          => (flags & IsEmptyFlag) > 0;
    public static bool HasIsCompleteFlag(this TypeMoldFlags flags)       => (flags & IsCompleteFlag) > 0;
    public static bool HasSkipBodyFlag(this TypeMoldFlags flags)         => (flags & SkipBodyFlag) > 0;
    public static bool HasSkipFieldsFlag(this TypeMoldFlags flags)        => (flags & SkipFieldsFlag) > 0;

    // public static bool HasWriteAsAttributeFlag(this TypeMoldFlags flags) => (flags & WriteAsAttributeFlag) > 0;
    // public static bool HasWriteAsContentFlag(this TypeMoldFlags flags)   => (flags & WriteAsContentFlag) > 0;
    // public static bool HasWriteAsComplexFlag(this TypeMoldFlags flags)   => (flags & WriteAsComplexFlag) > 0;
    public static bool HasWroteTypeNameFlag(this TypeMoldFlags flags)    => (flags & WroteTypeNameFlag) > 0;
    public static bool HasWroteRefIdFlag(this TypeMoldFlags flags)       => (flags & WroteRefIdFlag) > 0;
    public static bool HasWroteTypeOpenFlag(this TypeMoldFlags flags)    => (flags & WroteTypeOpenFlag) > 0;
    public static bool HasWroteTypeCloseFlag(this TypeMoldFlags flags)    => (flags & WroteTypeCloseFlag) > 0;
    public static bool HasWasDepthClippedFlag(this TypeMoldFlags flags)    => (flags & WasDepthClippedFlag) > 0;
}

public class TypeMolderDieCast<TExt> : RecyclableObject, ITypeMolderDieCast<TExt>
    where TExt : TypeMolder
{
    private TypeMoldFlags moldFlags = IsEmptyFlag;
    TypeMolder ITypeMolderDieCast.TypeMolder => StyleTypeBuilder;

    private bool hasJsonFields;

    public TExt StyleTypeBuilder { get; private set; } = null!;

    private TypeMolder.MoldPortableState typeBuilderState = null!;

    public TypeMolderDieCast<TExt> Initialize
        (TExt externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WriteMethodType writeMethod)
    {
        StyleTypeBuilder    = externalTypeBuilder;
        typeBuilderState    = typeBuilderPortableState;
        RemainingGraphDepth = typeBuilderPortableState.RemainingGraphDepth;

        var typeOfTExt = typeof(TExt);
        hasJsonFields = typeOfTExt == typeof(ComplexPocoTypeMold)
                     || typeOfTExt == typeof(KeyedCollectionMold)
                     || typeof(MultiValueTypeMolder<TExt>).IsAssignableFrom(typeOfTExt);

        var fmtFlags = typeBuilderPortableState.CreateFormatFlags;
        var hasBeenVisitedBefore = typeBuilderState.MoldGraphVisit.HasExistingInstanceId;
        SkipBody   = hasBeenVisitedBefore && fmtFlags.DoesNotHaveIsFieldNameFlag();
        SkipFields = hasBeenVisitedBefore || (Style.IsJson() && !hasJsonFields);

        WriteMethod = writeMethod;
        return this;
    }
    
    public ISecretStringOfPower Master => typeBuilderState.Master;

    TypeMolder.MoldPortableState IMigratableTypeMolderDieCast.PortableState => typeBuilderState;
    
    public string? TypeName => typeBuilderState.TypeName;

    public Type TypeBeingBuilt => typeBuilderState.TypeBeingBuilt;

    public FormatFlags CallerContentHandling => Master.CallerContext.FormatFlags;
    public FormatFlags CreateMoldFormatFlags => typeBuilderState.CreateFormatFlags;

    public int RemainingGraphDepth { get; set; }

    public int InstanceReferenceId => typeBuilderState.MoldGraphVisit.InstanceId;
    //
    // public bool WriteAsAttribute
    // {
    //     get => moldFlags.HasWriteAsAttributeFlag();
    //     set => moldFlags ^= !value && WriteAsAttribute || value && !WriteAsAttribute ? WriteAsAttributeFlag : None;
    // }
    //
    // public bool WriteAsContent
    // {
    //     get => moldFlags.HasWriteAsContentFlag();
    //     set => moldFlags ^= !value && WriteAsContent || value && !WriteAsContent ? WriteAsContentFlag : None;
    // }

    public ushort IndentLevel => (ushort)typeBuilderState.Master.IndentLevel;
    
    public WriteMethodType WriteMethod { get; set; }
    
    public bool SupportsMultipleFields
    {
        get => WriteMethod.SupportsMultipleFields();
    }
    
    public bool IsEmpty
    {
        get => moldFlags.HasIsEmptyFlag();
        set => moldFlags ^= !value && IsEmpty || value && !IsEmpty ? IsEmptyFlag : None;
    }
    
    public bool WroteRefId
    {
        get => moldFlags.HasWroteRefIdFlag();
        set => moldFlags ^= !value && WroteRefId || value && !WroteRefId ? WroteRefIdFlag : None;
    }
    
    public bool WasDepthClipped
    {
        get => moldFlags.HasWasDepthClippedFlag();
        set => moldFlags ^= !value && WasDepthClipped || value && !WasDepthClipped ? WasDepthClippedFlag : None;
    }
    
    public bool WroteTypeName
    {
        get => moldFlags.HasWroteTypeNameFlag();
        set => moldFlags ^= !value && WroteTypeName || value && !WroteTypeName ? WroteTypeNameFlag : None;
    }
    

    public char IndentChar => Settings.IndentChar;

    public IStyledTypeFormatting StyleFormatter => typeBuilderState.TypeFormatting;

    public bool IsComplete => StyleTypeBuilder.IsComplete;

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

    public virtual bool HasSkipBody<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipBody;

    public virtual bool HasSkipField<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags) => SkipFields;

    public TExt WasSkipped<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder;
    }
    
    public bool BuildingInstanceEquals<T>(T check) => StyleTypeBuilder.BuildingInstanceEquals(check);

    public StyleOptions Settings => typeBuilderState.Master.Settings;

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

    public void SetUntrackedVisit()
    {
        typeBuilderState.MoldGraphVisit = VisitResult.Empty;
    }

    public ITheOneString Complete()
    {
        if (!typeBuilderState.IsComplete)
        {
            typeBuilderState.CompleteResult = StyleTypeBuilder.Complete();
            //OwningAppender.AddTypeEnd(StyleTypeBuilder);
        }
        return Master;
    }

    public ITransferState     CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) => 
        CopyFrom(source as ITypeMolderDieCast, copyMergeFlags);

    public virtual ITypeMolderDieCast CopyFrom(ITypeMolderDieCast? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        RemainingGraphDepth = source.RemainingGraphDepth;
        WriteMethod      = source.WriteMethod;
        SkipBody            = source.SkipBody;
        SkipFields          = source.SkipFields;
        LastStartNewLineContentPos = source.LastStartNewLineContentPos;

        return this;
    }

    public override void StateReset()
    {
        moldFlags        = IsEmptyFlag;
        StyleTypeBuilder = null!;
        typeBuilderState = null!;
        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
