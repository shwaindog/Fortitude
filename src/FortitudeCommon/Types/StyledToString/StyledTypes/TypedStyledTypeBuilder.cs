// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public abstract class TypedStyledTypeBuilder<T> : StyledTypeBuilder, ITypeBuilderComponentSource<T>
    where T : StyledTypeBuilder
{
    protected IStyleTypeBuilderComponentAccess<T> CompAccess = null!;

    protected virtual string TypeOpeningDelimiter => "{";
    protected virtual string TypeClosingDelimiter => "}";

    protected void InitializeTypedStyledTypeBuilder(
        Type typeBeingBuilt
      , IStyleTypeAppenderBuilderAccess owningAppender
      , TypeAppendSettings typeSettings
      , string typeName
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeStyledTypeBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, typeFormatting,  existingRefId);

        SourceBuilderComponentAccess();
    }

    IStyleTypeBuilderComponentAccess ITypeBuilderComponentSource.ComponentAccess => CompAccess;

    IStyleTypeBuilderComponentAccess<T> ITypeBuilderComponentSource<T>.CompAccess => CompAccess;

    public override void Start()
    {
        if (!PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeStartFlag())
        {
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess, PortableState.TypeName);
            AppendRefIdIfAny();
        }
    }

    protected T Me => (T)(StyledTypeBuilder)this;

    public override StyledTypeBuildResult Complete()
    {
        if (!PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeEndFlag())
        {
            CompAccess.RemoveLastWhiteSpacedCommaIfFound();
            CompAccess.Sb.Append(TypeClosingDelimiter);
            CompAccess.DecrementIndent();
        }
        var currentAppenderIndex = CompAccess.OwningAppender.WriteBuffer.Length;
        var typeWriteRange       = new Range(Index.FromStart(StartIndex), Index.FromStart(currentAppenderIndex));
        var result               = new StyledTypeBuildResult(TypeName, CompAccess.OwningAppender.ToTypeStringAppender, typeWriteRange);
        PortableState.CompleteResult = result;
        CompAccess.OwningAppender.TypeComplete(CompAccess);
        return result;
    }

    protected bool AppendRefIdIfAny()
    {
        if (CompAccess.StyleTypeBuilder.ExistingRefId != 0)
        {
            CompAccess.Sb.Append("\"$ref\":\"").Append(CompAccess.StyleTypeBuilder.ExistingRefId).Append("\" ");
            return true;
        }
        return false;
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
