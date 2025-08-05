using FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.SimpleType;

public class SimpleValueTypeBuilder : ValueTypeBuilder<SimpleValueTypeBuilder>
{
    public SimpleValueTypeBuilder InitializeSimpleValueTypeBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        InitializeValueTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<SimpleValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

}