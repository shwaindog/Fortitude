// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public interface IStyleTypeBuilderComponentAccess
{
    StyledTypeBuilder StyleTypeBuilder { get; }

    IStyleTypeAppenderBuilderAccess OwningAppender   { get; }

    TypeAppendSettings AppendSettings { get; set; }

    string Indent { get; }

    ushort                 IndentLevel { get; }

    bool IsComplete { get; }

    StringBuildingStyle Style       { get; }

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

    public TExt StyleTypeBuilder { get; private set; } = null!;

    private StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderState = null!;

    public InternalStyledTypeBuilderComponentAccess<TExt> Initialize
        (TExt externalTypeBuilder, StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderPortableState)
    {
        StyleTypeBuilder = externalTypeBuilder;
        typeBuilderState = typeBuilderPortableState;

        return this;
    }


    public IStyleTypeAppenderBuilderAccess OwningAppender => typeBuilderState.OwningAppender;

    public string TypeName => typeBuilderState.TypeName;

    public ushort IndentLevel => typeBuilderState.AppenderSettings.IndentLvl;

    public string Indent => typeBuilderState.OwningAppender.Indent;

    public bool IsComplete => StyleTypeBuilder.IsComplete;

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

    public StringBuildingStyle Style => typeBuilderState.OwningAppender.Style;

    public IStringBuilder Sb => typeBuilderState.OwningAppender.WriteBuffer;

    public IStyledTypeStringAppender Complete()
    {
        if (!typeBuilderState.IsComplete)
        {
            typeBuilderState.CompleteResult =  StyleTypeBuilder.Complete();
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
