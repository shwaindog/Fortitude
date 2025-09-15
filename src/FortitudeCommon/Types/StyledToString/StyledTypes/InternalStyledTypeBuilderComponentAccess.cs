// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.Options;
using FortitudeCommon.Types.StyledToString.StyledTypes.ComplexType;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public interface IStyleTypeBuilderComponentAccess
{
    StyledTypeBuilder StyleTypeBuilder { get; }

    IStyleTypeAppenderBuilderAccess OwningAppender { get; }

    TypeAppendSettings AppendSettings { get; set; }

    char IndentChar { get; }

    ushort IndentLevel { get; }
    
    bool WriteAsComplex { get; }

    bool IsComplete { get; }

    Type TypeBeingBuilt { get; }
    string TypeName { get; }

    bool SkipBody { get; set; }
    bool SkipFields { get; set; }
    
    IStyledTypeFormatting StyleFormatter { get; }
    
    int RemainingGraphDepth { get; set; }

    StringBuildingStyle Style { get; }
    
    StyleOptions Settings { get; }

    IStringBuilder Sb { get; }

    public int DecrementIndent();
    public int IncrementIndent();

    public void UnSetIgnoreFlag(IgnoreWriteFlags flagToUnset);

    IRecycler Recycler { get; }
}

public interface IStyleTypeBuilderComponentAccess<out T> : IStyleTypeBuilderComponentAccess where T : StyledTypeBuilder
{
    new T StyleTypeBuilder { get; }
}

public class InternalStyledTypeBuilderComponentAccess<TExt> : RecyclableObject, IStyleTypeBuilderComponentAccess<TExt>
    where TExt : StyledTypeBuilder
{
    StyledTypeBuilder IStyleTypeBuilderComponentAccess.StyleTypeBuilder => StyleTypeBuilder;

    private bool hasJsonFields;

    public TExt StyleTypeBuilder { get; private set; } = null!;

    private StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderState = null!;
    private bool                                            skipFields;

    public InternalStyledTypeBuilderComponentAccess<TExt> Initialize
        (TExt externalTypeBuilder, StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderPortableState)
    {
        StyleTypeBuilder    = externalTypeBuilder;
        typeBuilderState    = typeBuilderPortableState;
        RemainingGraphDepth = typeBuilderPortableState.RemainingGraphDepth;

        var typeOfTExt = typeof(TExt);
        hasJsonFields = typeOfTExt == typeof(ComplexTypeBuilder) 
                     || typeOfTExt == typeof(KeyValueCollectionBuilder)
                     || typeof(MultiValueTypeBuilder<TExt>).IsAssignableFrom(typeOfTExt);
        
        SkipBody      = typeBuilderState.ExistingRefId > 0;
        SkipFields    = SkipBody || (Style.IsJson() && !hasJsonFields);

        return this;
    }


    public IStyleTypeAppenderBuilderAccess OwningAppender => typeBuilderState.OwningAppender;

    public string TypeName => typeBuilderState.TypeName;
    
    public Type TypeBeingBuilt => typeBuilderState.TypeBeingBuilt;

    public int RemainingGraphDepth { get; set; }

    public ushort IndentLevel => typeBuilderState.AppenderSettings.IndentLvl;
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

    public StyleOptions Settings => typeBuilderState.OwningAppender.Settings;

    public int DecrementIndent()
    {
        typeBuilderState.AppenderSettings.IndentLvl--;
        return typeBuilderState.AppenderSettings.IndentLvl;
    }

    public int IncrementIndent()
    {
        typeBuilderState.AppenderSettings.IndentLvl++;
        return typeBuilderState.AppenderSettings.IndentLvl;
    }

    public TypeAppendSettings AppendSettings
    {
        get => typeBuilderState.AppenderSettings;
        set => typeBuilderState.AppenderSettings = value;
    }

    public void UnSetIgnoreFlag(IgnoreWriteFlags flagToUnset)
    {
        typeBuilderState.AppenderSettings.IgnoreWriteFlags &= ~flagToUnset;
    }

    public new IRecycler Recycler => base.Recycler ?? typeBuilderState.OwningAppender.Recycler;

    public StringBuildingStyle Style => Settings.Style;

    public IStringBuilder Sb => typeBuilderState.OwningAppender.WriteBuffer;

    public IStyledTypeStringAppender Complete()
    {
        if (!typeBuilderState.IsComplete)
        {
            typeBuilderState.CompleteResult = StyleTypeBuilder.Complete();
            //OwningAppender.AddTypeEnd(StyleTypeBuilder);
        }
        return OwningAppender;
    }

    public override void StateReset()
    {
        StyleTypeBuilder = null!;
        typeBuilderState = null!;
        base.StateReset();
    }

    public void Dispose()
    {
        Complete();
    }
}
