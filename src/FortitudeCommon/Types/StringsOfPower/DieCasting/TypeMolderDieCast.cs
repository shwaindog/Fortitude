// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public interface ITypeMolderDieCast
{
    TypeMolder TypeMolder { get; }

    ISecretStringOfPower Master { get; }

    MoldDieCastSettings AppendSettings { get; set; }


    FieldContentHandling CallerContentHandling { get; }
    FieldContentHandling CreateContentHandling { get; }

    char IndentChar { get; }

    ushort IndentLevel { get; }

    bool WriteAsComplex { get; }

    bool IsComplete { get; }

    bool WriteAsAttribute { get; }
    bool WriteAsContent { get; set; }
    
    int LastStartNewLineContentPos { get; }

    Type TypeBeingBuilt { get; }
    string? TypeName { get; }

    bool SkipBody { get; set; }

    bool SkipFields { get; set; }

    bool SkipField<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags);

    IStyledTypeFormatting StyleFormatter { get; }

    int RemainingGraphDepth { get; set; }

    StringStyle Style { get; }

    StyleOptions Settings { get; }

    IStringBuilder Sb { get; }

    public int DecrementIndent();
    public int IncrementIndent();

    public void UnSetIgnoreFlag(SkipTypeParts flagToUnset);

    IRecycler Recycler { get; }
}

public interface ITypeMolderDieCast<out T> : ITypeMolderDieCast where T : TypeMolder
{
    T StyleTypeBuilder { get; }

    T WasSkipped<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags);
}

public class TypeMolderDieCast<TExt> : RecyclableObject, ITypeMolderDieCast<TExt>
    where TExt : TypeMolder
{
    TypeMolder ITypeMolderDieCast.TypeMolder => StyleTypeBuilder;

    private bool hasJsonFields;

    public TExt StyleTypeBuilder { get; private set; } = null!;

    private TypeMolder.StyleTypeBuilderPortableState typeBuilderState = null!;

    private bool skipFields;
    private bool writeAsContent;

    public TypeMolderDieCast<TExt> Initialize
        (TExt externalTypeBuilder, TypeMolder.StyleTypeBuilderPortableState typeBuilderPortableState)
    {
        StyleTypeBuilder    = externalTypeBuilder;
        typeBuilderState    = typeBuilderPortableState;
        RemainingGraphDepth = typeBuilderPortableState.RemainingGraphDepth;

        var typeOfTExt = typeof(TExt);
        hasJsonFields = typeOfTExt == typeof(ComplexTypeMold)
                     || typeOfTExt == typeof(KeyValueCollectionMold)
                     || typeof(MultiValueTypeMolder<TExt>).IsAssignableFrom(typeOfTExt);

        SkipBody   = typeBuilderState.ExistingRefId > 0;
        SkipFields = SkipBody || (Style.IsJson() && !hasJsonFields);

        return this;
    }
    
    public ISecretStringOfPower Master => typeBuilderState.Master;

    public string? TypeName => typeBuilderState.TypeName;

    public Type TypeBeingBuilt => typeBuilderState.TypeBeingBuilt;

    public FieldContentHandling CallerContentHandling => Master.CallerContext.FormatFlags;
    public FieldContentHandling CreateContentHandling => typeBuilderState.CreateFormatFlags;

    public ContentSeparatorRanges LastContentSeparatorPaddingRanges { get; set; }

    public int RemainingGraphDepth { get; set; }

    public bool WriteAsAttribute => WriteAsContent;
    public bool WriteAsContent
    {
        get => writeAsContent;
        set => writeAsContent |= value;
    }

    public ushort IndentLevel => (ushort)typeBuilderState.Master.IndentLevel;
    public bool WriteAsComplex => StyleTypeBuilder.IsComplexType || StyleTypeBuilder.ExistingRefId != 0 || RemainingGraphDepth <= 0;

    public char IndentChar => Settings.IndentChar;

    public IStyledTypeFormatting StyleFormatter => typeBuilderState.TypeFormatting;

    public bool IsComplete => StyleTypeBuilder.IsComplete;

    public bool SkipBody { get; set; }

    public bool SkipFields
    {
        get => skipFields || SkipBody;
        set => skipFields = value;
    }

    public int LastStartNewLineContentPos { get; private set; } = -1;

    public bool SkipField<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags) =>
        SkipBody;

    public TExt WasSkipped<TCallerType>(Type? actualType, ReadOnlySpan<char> fieldName
      , FieldContentHandling formatFlags = FieldContentHandling.DefaultCallerTypeFlags)

    {
        return StyleTypeBuilder;
    }

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

    public MoldDieCastSettings AppendSettings
    {
        get => typeBuilderState.AppenderSettings;
        set => typeBuilderState.AppenderSettings = value;
    }

    public void UnSetIgnoreFlag(SkipTypeParts flagToUnset)
    {
        typeBuilderState.AppenderSettings.SkipTypeParts &= ~flagToUnset;
    }

    public new IRecycler Recycler => base.Recycler ?? typeBuilderState.Master.Recycler;

    public StringStyle Style => Settings.Style;

    public IStringBuilder Sb => typeBuilderState.Master.WriteBuffer;

    public ITheOneString Complete()
    {
        if (!typeBuilderState.IsComplete)
        {
            typeBuilderState.CompleteResult = StyleTypeBuilder.Complete();
            //OwningAppender.AddTypeEnd(StyleTypeBuilder);
        }
        return Master;
    }

    public override void StateReset()
    {
        writeAsContent   = false;
        StyleTypeBuilder = null!;
        typeBuilderState = null!;
        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
