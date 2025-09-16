using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

public class SimpleValueTypeBuilder : ValueTypeBuilder<SimpleValueTypeBuilder>
{
    public SimpleValueTypeBuilder InitializeSimpleValueTypeBuilder
        (
            Type typeBeingBuilt
          , IStyleTypeAppenderBuilderAccess owningAppender
          , TypeAppendSettings typeSettings
          , string typeName
          , int remainingGraphDepth
          , IStyledTypeFormatting typeFormatting  
          , int existingRefId)
    {
        InitializeValueTypeBuilder(typeBeingBuilt, owningAppender, typeSettings, typeName, remainingGraphDepth, typeFormatting,  existingRefId);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<SimpleValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

}