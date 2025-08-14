// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using FortitudeCommon.Types.StyledToString.StyledTypes.ComplexType;
using FortitudeCommon.Types.StyledToString.StyledTypes.SimpleType;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

#endregion

namespace FortitudeCommon.Types.StyledToString;

public interface IStyledTypeStringAppender : IReusableObject<IStyledTypeStringAppender>
{
    const string DefaultIndentString   = "  ";
    const string WhenNullString        = "null";
    const string DefaultString         = "";
    const string DefaultStringAsNumber = "0";
    // ReSharper disable UnusedMemberInSuper.Global
    StringBuildingStyle Style { get; }

    int IndentLevel { get; }

    string Indent { get; set; }

    string NewLineStyle { get; set; }
    string NullStyle    { get; set; }

    StyledTypeBuilder? CurrentTypeBuilder { get; }

    IStyledTypeStringAppender ClearAndReinitialize
        (StringBuildingStyle stringStyle, int indentLevel = 0, IgnoreWriteFlags ignoreFlags = IgnoreWriteFlags.None);

    KeyValueCollectionBuilder StartKeyedCollectionType(string typeName);

    SimpleOrderedCollectionBuilder StartSimpleCollectionType(string typeName);

    ComplexOrderedCollectionBuilder StartComplexCollectionType(string typeName);

    ComplexTypeBuilder StartComplexType(string typeName);

    SimpleValueTypeBuilder  StartSimpleValueType(string typeName);

    ComplexValueTypeBuilder StartComplexValueType(string typeName);

    // IStyleTypeBuilder AutoType<T>();


    IStringBuilder WriteBuffer { get; }

    bool Equals(string? toCompare);

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public interface IStyleTypeAppenderBuilderAccess : IStyledTypeStringAppender
{
    IgnoreWriteFlags IgnoreWriteFlags { get; }

    void TypeComplete(IStyleTypeBuilderComponentAccess completeType);

    new IRecycler Recycler { get; }

    IStyledTypeStringAppender AddBaseFieldsStart();
    IStyledTypeStringAppender AddBaseFieldsEnd();

    StyledTypeStringAppender ToTypeStringAppender { get; }
}

public class StyledTypeStringAppender : ReusableObject<IStyledTypeStringAppender>, IStyleTypeAppenderBuilderAccess
{
    private static readonly IRecycler AlWaysRecycler = new Recycler();


    public static Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    internal const string Null = "null";

    protected StringBuildingStyle BuildStyle;


    protected Stack<IStyleTypeBuilderComponentAccess>? BuildingTypesStack;

    protected IStringBuilder? Sb;

    private TypeAppendSettings       initialAppendSettings;
    private TypeAppendSettings?      nextTypeAppendSettings;

    public StyledTypeStringAppender()
    {
        BuildStyle = StringBuildingStyle.Default;
    }

    public StyledTypeStringAppender(StringBuildingStyle withStyle) => BuildStyle = withStyle;

    public StyledTypeStringAppender(StyledTypeStringAppender toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        initialAppendSettings  = toClone.initialAppendSettings;
        nextTypeAppendSettings = toClone.nextTypeAppendSettings;
    }

    public StyledTypeStringAppender(IStyledTypeStringAppender toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);
    }

    public int IndentLevel => AppendSettings.IndentLvl;

    public IgnoreWriteFlags IgnoreWriteFlags => AppendSettings.IgnoreWriteFlags;

    public string Indent { get; set; } = IStyledTypeStringAppender.DefaultIndentString;

    public string NewLineStyle { get; set; } = Environment.NewLine;

    public string NullStyle { get; set; } = Null;

    public StringBuildingStyle Style => BuildStyle;

    public IStringBuilder WriteBuffer => Sb ??= BufferFactory();

    public new IRecycler Recycler => base.Recycler ?? AlWaysRecycler;

    public StyledTypeBuilder? CurrentTypeBuilder => CurrentTypeAccess?.StyleTypeBuilder;

    protected TypeAppendSettings AppendSettings => nextTypeAppendSettings ?? CurrentTypeAccess?.AppendSettings ?? initialAppendSettings;

    protected IStyleTypeBuilderComponentAccess? CurrentTypeAccess { get; set; }

    public IStyledTypeStringAppender ClearAndReinitialize(StringBuildingStyle stringStyle, int indentLevel = 0, IgnoreWriteFlags ignoreWrite = IgnoreWriteFlags.None)
    {
        BuildStyle = stringStyle;

        initialAppendSettings = new TypeAppendSettings((ushort)indentLevel, ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        return this;
    }

    public IStyledTypeStringAppender Initialize(IStringBuilder usingStringBuilder, StringBuildingStyle buildStyle = StringBuildingStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb         = usingStringBuilder;
        BuildStyle = buildStyle;

        return this;
    }

    public IStyledTypeStringAppender Initialize(StringBuildingStyle buildStyle = StringBuildingStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        BuildStyle = buildStyle;

        return this;
    }

    protected void TypeStart(StyledTypeBuilder newType)
    {
        if (CurrentTypeAccess is { IsComplete: false })
        {
            PushCurrentSettings(CurrentTypeAccess);
        }
        newType.Start();
        CurrentTypeAccess = ((ITypeBuilderComponentSource)newType).ComponentAccess;
    }

    void IStyleTypeAppenderBuilderAccess.TypeComplete(IStyleTypeBuilderComponentAccess completeType)
    {
        PopCurrentSettings();
        ((IRecyclableObject)completeType).DecrementRefCount();
    }

    protected void PushCurrentSettings(IStyleTypeBuilderComponentAccess callingType)
    {
        BuildingTypesStack ??= new Stack<IStyleTypeBuilderComponentAccess>();
        BuildingTypesStack.Push(callingType);
        nextTypeAppendSettings = new TypeAppendSettings(callingType.IndentLevel, IgnoreWriteFlags.None);
    }

    protected void PopCurrentSettings()
    {
        BuildingTypesStack ??= new Stack<IStyleTypeBuilderComponentAccess>();
        CurrentTypeAccess  =   BuildingTypesStack.TryPop(out var result) ? result : null;
    }

    KeyValueCollectionBuilder IStyledTypeStringAppender.StartKeyedCollectionType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var keyedCollectionBuilder = Recycler.Borrow<KeyValueCollectionBuilder>()
                                             .InitializeKeyValueCollectionBuilder(this, appendSettings, typeName);
        TypeStart(keyedCollectionBuilder);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionBuilder StartSimpleCollectionType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var simpleOrderedCollectionBuilder = Recycler.Borrow<SimpleOrderedCollectionBuilder>()
                                             .InitializeSimpleOrderedCollectionBuilder(this, appendSettings, typeName);
        TypeStart(simpleOrderedCollectionBuilder);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionBuilder StartComplexCollectionType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var complexOrderedCollectionBuilder = Recycler.Borrow<ComplexOrderedCollectionBuilder>()
                                             .InitializeComplexOrderedCollectionBuilder(this, appendSettings, typeName);
        TypeStart(complexOrderedCollectionBuilder);
        return complexOrderedCollectionBuilder;
    }

    public ComplexTypeBuilder StartComplexType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var complexTypeBuilder = Recycler.Borrow<ComplexTypeBuilder>()
                                             .InitializeComplexTypeBuilder(this, appendSettings, typeName);
        TypeStart(complexTypeBuilder);
        return complexTypeBuilder;
    }

    public SimpleValueTypeBuilder StartSimpleValueType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var simpleValueBuilder = Recycler.Borrow<SimpleValueTypeBuilder>()
                                             .InitializeSimpleValueTypeBuilder(this, appendSettings, typeName);
        TypeStart(simpleValueBuilder);
        return simpleValueBuilder;
    }

    public ComplexValueTypeBuilder StartComplexValueType(string typeName)
    {
        var appendSettings         = AppendSettings;
        var keyedCollectionBuilder = Recycler.Borrow<ComplexValueTypeBuilder>()
                                             .InitializeComplexValueTypeBuilder(this, appendSettings, typeName);
        TypeStart(keyedCollectionBuilder);
        return keyedCollectionBuilder;
    }

    StyledTypeStringAppender IStyleTypeAppenderBuilderAccess.ToTypeStringAppender => this;

    IStyledTypeStringAppender IStyleTypeAppenderBuilderAccess.AddBaseFieldsEnd()
    {
        var nextAppender = nextTypeAppendSettings ?? 
                           new TypeAppendSettings(CurrentTypeAccess?.IndentLevel ?? 0, IgnoreWriteFlags.All);
        nextAppender.IgnoreWriteFlags = IgnoreWriteFlags.TypeStart | IgnoreWriteFlags.TypeName | IgnoreWriteFlags.TypeEnd;
        nextTypeAppendSettings        = nextAppender;

        return this;
    }

    IStyledTypeStringAppender IStyleTypeAppenderBuilderAccess.AddBaseFieldsStart()
    {
        PopCurrentSettings();

        return this;
    }

    public override void StateReset()
    {
        ClearStringBuilder();

        base.StateReset();
    }

    protected virtual IStringBuilder SourceStringBuilder() => Sb ??= BufferFactory();

    protected virtual void ClearStringBuilder() => Sb = null!;

    public override StyledTypeStringAppender Clone() =>
        Recycler.Borrow<StyledTypeStringAppender>().CopyFrom(this, CopyMergeFlags.FullReplace);

    public override IStyledTypeStringAppender CopyFrom(IStyledTypeStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => 
        CopyFrom((StyledTypeStringAppender)source, copyMergeFlags);

    public StyledTypeStringAppender CopyFrom
        (StyledTypeStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClearAndReinitialize(source.Style, IndentLevel);
        Sb!.Append(source.WriteBuffer);

        initialAppendSettings  = source.initialAppendSettings;
        nextTypeAppendSettings = source.nextTypeAppendSettings;

        return this;
    }

    protected IStringBuilder RemoveLastWhiteSpacedCommaIfFound()
    {
        var sb = WriteBuffer;
        if (sb[^2] == ',' && sb[^1] == ' ')
        {
            sb.Length -= 2;
            sb.Append(" ");
            return sb;
        }
        for (var i = sb.Length - 1; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (sb[i] == ',')
            {
                sb.Remove(i, 1);
                break;
            }
        return sb;
    }

    public override string ToString() => WriteBuffer.ToString();

    public bool Equals(string? toCompare)
    {
        var sb = WriteBuffer;
        if (toCompare == null) return false;
        if (sb.Length != toCompare.Length) return false;
        for (var i = 0; i < sb.Length; i++)
        {
            if (sb[i] != toCompare[i]) return false;
        }
        return true;
    }
}

