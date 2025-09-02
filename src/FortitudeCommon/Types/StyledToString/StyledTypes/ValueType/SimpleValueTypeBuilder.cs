namespace FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

public class SimpleValueTypeBuilder : ValueTypeBuilder<SimpleValueTypeBuilder>
{
    public SimpleValueTypeBuilder InitializeSimpleValueTypeBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName, int existingRefId)
    {
        InitializeValueTypeBuilder(owningAppender, typeSettings, typeName,  existingRefId);

        return this;
    }

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<SimpleValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

}