// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public abstract class TypedStyledTypeBuilder<T> : StyledTypeBuilder, ITypeBuilderComponentSource<T>
    where T : StyledTypeBuilder
{
    protected IStyleTypeBuilderComponentAccess<T> CompAccess = null!;

    protected virtual string TypeOpeningDelimiter => "{";
    protected virtual string TypeClosingDelimiter => "{";

    protected void InitializeTypedStyledTypeBuilder(IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings, string typeName)
    {
        InitializeStyledTypeBuilder(owningAppender, typeSettings, typeName);

        SourceBuilderComponentAccess();
    }

    IStyleTypeBuilderComponentAccess ITypeBuilderComponentSource.ComponentAccess => CompAccess;

    IStyleTypeBuilderComponentAccess<T> ITypeBuilderComponentSource<T>.CompAccess      => CompAccess;

    public override void Start()
    {
        if (!PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeStartFlag())
        {
            CompAccess.Sb.Append(TypeOpeningDelimiter);
            CompAccess.IncrementIndent();
        }
    }

    protected T Me => (T)(StyledTypeBuilder)this;

    public override StyledTypeBuildResult Complete()
    {
        if (!PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeEndFlag())
        {
            CompAccess.Sb.Append(TypeClosingDelimiter);
            CompAccess.DecrementIndent();
        }
        var currentAppenderIndex = CompAccess.OwningAppender.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        CompAccess.OwningAppender.TypeComplete(CompAccess);
        return new StyledTypeBuildResult(TypeName, CompAccess.OwningAppender.ToTypeStringAppender, typeWriteRange);
    }


    protected virtual void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<InternalStyledTypeBuilderComponentAccess<T>>()
                             .Initialize((T)(ITypeBuilderComponentSource<T>)this, PortableState);
    }

    protected override void InheritedStateReset()
    {
        CompAccess?.DecrementIndent();
        CompAccess = null!;

        MeRecyclable.StateReset();
    }
}
